using System;
using System.Windows.Browser;
using Silverlight.Html;
using Silverlight.Html.WebGL;
using GlMatrix;

#region WebGL types
using GLenum = System.UInt32;
using GLboolean = System.Boolean;
using GLbitfield = System.UInt32;
using GLbyte = System.SByte;         /* 'byte' should be a signed 8 bit type. */
using GLshort = System.Int16;
using GLint = System.Int32;
using GLsizei = System.Int32;
using GLintptr = System.Int64;
using GLsizeiptr = System.Int64;
using GLubyte = System.Byte;        /* 'unsigned byte' should be an unsigned 8 bit type. */
using GLushort = System.Int16;
using GLuint = System.UInt32;
using GLfloat = System.Single;
using GLclampf = System.Single;
using DOMString = System.String;
using WebGLBuffer = System.Windows.Browser.ScriptObject;
using WebGLFramebuffer = System.Windows.Browser.ScriptObject;
using WebGLProgram = System.Windows.Browser.ScriptObject;
using WebGLRenderbuffer = System.Windows.Browser.ScriptObject;
using WebGLShader = System.Windows.Browser.ScriptObject;
//using WebGLTexture = System.Windows.Browser.ScriptObject;
using WebGLUniformLocation = System.Windows.Browser.ScriptObject;
using System.Collections.Generic;
//using WebGLContextAttributes = System.Windows.Browser.ScriptObject;
#endregion

namespace LearningWebGL
{
    [ScriptableType]
    public class Lesson05 : LessonBase
    {

        string shader_fs = @"
    precision mediump float;

    varying vec2 vTextureCoord;

    uniform sampler2D uSampler;

    void main(void) {
        gl_FragColor = texture2D(uSampler, vec2(vTextureCoord.s, vTextureCoord.t));
    }
";

        string shader_vs = @"
    attribute vec3 aVertexPosition;
    attribute vec2 aTextureCoord;

    uniform mat4 uMVMatrix;
    uniform mat4 uPMatrix;

    varying vec2 vTextureCoord;


    void main(void) {
        gl_Position = uPMatrix * uMVMatrix * vec4(aVertexPosition, 1.0);
        vTextureCoord = aTextureCoord;
    }
";

        WebGLRenderingContext gl;
        float viewportWidth;
        float viewportHeight;

        void initGL(HtmlElement canvas) {
            try {
                gl = new WebGLRenderingContext(canvas);
                viewportWidth = (float)canvas.Width();
                viewportHeight = (float)canvas.Height();
            } catch (Exception e) {
            }
            if (gl == null) {
                alert("Could not initialise WebGL, sorry :-(");
            }
        }


        WebGLShader getShader(WebGLRenderingContext gl, string str, GLuint shaderType) {
            var shader = gl.createShader(shaderType);
            gl.shaderSource(shader, str);
            gl.compileShader(shader);

            if (gl.getShaderParameter(shader, GL.COMPILE_STATUS) == null) {
                alert(gl.getShaderInfoLog(shader));
                return null;
            }

            return shader;
        }


        WebGLProgram shaderProgram;
        GLint shaderProgram_vertexPositionAttribute;
        GLint shaderProgram_textureCoordAttribute;
        WebGLUniformLocation shaderProgram_pMatrixUniform;
        WebGLUniformLocation shaderProgram_mvMatrixUniform;
        WebGLUniformLocation shaderProgram_samplerUniform;

        void initShaders() {
            var fragmentShader = getShader(gl, shader_fs, GL.FRAGMENT_SHADER);
            var vertexShader = getShader(gl, shader_vs, GL.VERTEX_SHADER);

            shaderProgram = gl.createProgram();
            gl.attachShader(shaderProgram, vertexShader);
            gl.attachShader(shaderProgram, fragmentShader);
            gl.linkProgram(shaderProgram);

            if (gl.getProgramParameter(shaderProgram, GL.LINK_STATUS) == null) {
                alert("Could not initialise shaders");
            }

            gl.useProgram(shaderProgram);

            shaderProgram_vertexPositionAttribute = gl.getAttribLocation(shaderProgram, "aVertexPosition");
            gl.enableVertexAttribArray((GLuint)shaderProgram_vertexPositionAttribute);

            shaderProgram_textureCoordAttribute = gl.getAttribLocation(shaderProgram, "aTextureCoord");
            gl.enableVertexAttribArray((GLuint)shaderProgram_textureCoordAttribute);

            shaderProgram_pMatrixUniform = gl.getUniformLocation(shaderProgram, "uPMatrix");
            shaderProgram_mvMatrixUniform = gl.getUniformLocation(shaderProgram, "uMVMatrix");
            shaderProgram_samplerUniform = gl.getUniformLocation(shaderProgram, "uSampler");
        }


        void handleLoadedTexture(WebGLTexture texture, Image image) {
            gl.bindTexture(GL.TEXTURE_2D, texture);
            gl.pixelStorei(GL.UNPACK_FLIP_Y_WEBGL, 1/*true*/);
            gl.texImage2D(GL.TEXTURE_2D, 0, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, image);
            gl.texParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.NEAREST);
            gl.texParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.NEAREST);
            gl.bindTexture(GL.TEXTURE_2D, null);
        }

        WebGLTexture neheTexture;

        void initTexture() {
            neheTexture = gl.createTexture();
            var neheTexture_image = new Image();
            neheTexture_image.onload += delegate {
                handleLoadedTexture(neheTexture, neheTexture_image);
            };

            neheTexture_image.src = "nehe.gif";
        }


        mat4 mvMatrix = mat4.create();
        Stack<mat4> mvMatrixStack = new Stack<mat4>();
        mat4 pMatrix = mat4.create();

        void mvPushMatrix() {
            var copy = mat4.create();
            mat4.set(mvMatrix, copy);
            mvMatrixStack.Push(copy);
        }

        void mvPopMatrix() {
            if (mvMatrixStack.Count == 0) {
                throw new Exception("Invalid popMatrix!");
            }
            mvMatrix = mvMatrixStack.Pop();
        }


        void setMatrixUniforms() {
            gl.uniformMatrix4fv((WebGLUniformLocation)shaderProgram_pMatrixUniform, false, pMatrix.ToArray());
            gl.uniformMatrix4fv((WebGLUniformLocation)shaderProgram_mvMatrixUniform, false, mvMatrix.ToArray());
        }

        double degToRad(double degrees) {
            return degrees * Math.PI / 180;
        }

        WebGLBuffer cubeVertexPositionBuffer;
        WebGLBuffer cubeVertexTextureCoordBuffer;
        WebGLBuffer cubeVertexIndexBuffer;
        int cubeVertexPositionBuffer_itemSize;
        int cubeVertexPositionBuffer_numItems;
        int cubeVertexTextureCoordBuffer_itemSize;
        int cubeVertexTextureCoordBuffer_numItems;
        int cubeVertexIndexBuffer_itemSize;
        int cubeVertexIndexBuffer_numItems;


        void initBuffers() {
            cubeVertexPositionBuffer = gl.createBuffer();
            gl.bindBuffer(GL.ARRAY_BUFFER, cubeVertexPositionBuffer);
            var vertices = new GLfloat[] {
                // Front face
                -1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,

                // Back face
                -1.0f, -1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,

                // Top face
                -1.0f,  1.0f, -1.0f,
                -1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f, -1.0f,

                // Bottom face
                -1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f,  1.0f,
                -1.0f, -1.0f,  1.0f,

                // Right face
                 1.0f, -1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,

                // Left face
                -1.0f, -1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f, -1.0f,
            };
            gl.bufferData(GL.ARRAY_BUFFER, new Float32Array(vertices), GL.STATIC_DRAW);
            cubeVertexPositionBuffer_itemSize = 3;
            cubeVertexPositionBuffer_numItems = 24;

            cubeVertexTextureCoordBuffer = gl.createBuffer();
            gl.bindBuffer(GL.ARRAY_BUFFER, cubeVertexTextureCoordBuffer);
            var textureCoords = new GLfloat[] {
              // Front face
              0.0f, 0.0f,
              1.0f, 0.0f,
              1.0f, 1.0f,
              0.0f, 1.0f,

              // Back face
              1.0f, 0.0f,
              1.0f, 1.0f,
              0.0f, 1.0f,
              0.0f, 0.0f,

              // Top face
              0.0f, 1.0f,
              0.0f, 0.0f,
              1.0f, 0.0f,
              1.0f, 1.0f,

              // Bottom face
              1.0f, 1.0f,
              0.0f, 1.0f,
              0.0f, 0.0f,
              1.0f, 0.0f,

              // Right face
              1.0f, 0.0f,
              1.0f, 1.0f,
              0.0f, 1.0f,
              0.0f, 0.0f,

              // Left face
              0.0f, 0.0f,
              1.0f, 0.0f,
              1.0f, 1.0f,
              0.0f, 1.0f,
            };
            gl.bufferData(GL.ARRAY_BUFFER, new Float32Array(textureCoords), GL.STATIC_DRAW);
            cubeVertexTextureCoordBuffer_itemSize = 2;
            cubeVertexTextureCoordBuffer_numItems = 24;

            cubeVertexIndexBuffer = gl.createBuffer();
            gl.bindBuffer(GL.ELEMENT_ARRAY_BUFFER, cubeVertexIndexBuffer);
            var cubeVertexIndices = new UInt16 [] {
                0, 1, 2,      0, 2, 3,    // Front face
                4, 5, 6,      4, 6, 7,    // Back face
                8, 9, 10,     8, 10, 11,  // Top face
                12, 13, 14,   12, 14, 15, // Bottom face
                16, 17, 18,   16, 18, 19, // Right face
                20, 21, 22,   20, 22, 23  // Left face
            };
            gl.bufferData(GL.ELEMENT_ARRAY_BUFFER, new Uint16Array(cubeVertexIndices), GL.STATIC_DRAW);
            cubeVertexIndexBuffer_itemSize = 1;
            cubeVertexIndexBuffer_numItems = 36;
        }


        double xRot = 0;
        double yRot = 0;
        double zRot = 0;

        public void drawScene() {
            gl.viewport(0, 0, (int)viewportWidth, (int)viewportHeight);
            gl.clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);

            mat4.perspective(45, viewportWidth / viewportHeight, 0.1f, 100.0f, pMatrix);

            mat4.identity(mvMatrix);

            mat4.translate(mvMatrix, new GLfloat[] { -0.0f, 0.0f, -5.0f });

            mat4.rotate(mvMatrix, degToRad(xRot), new GLfloat[] { 1, 0, 0 });
            mat4.rotate(mvMatrix, degToRad(yRot), new GLfloat[] { 0, 1, 0 });
            mat4.rotate(mvMatrix, degToRad(zRot), new GLfloat[] { 0, 0, 1 });

            gl.bindBuffer(GL.ARRAY_BUFFER, cubeVertexPositionBuffer);
            gl.vertexAttribPointer((GLuint)shaderProgram_vertexPositionAttribute, cubeVertexPositionBuffer_itemSize, GL.FLOAT, false, 0, 0);

            gl.bindBuffer(GL.ARRAY_BUFFER, cubeVertexTextureCoordBuffer);
            gl.vertexAttribPointer((GLuint)shaderProgram_textureCoordAttribute, cubeVertexTextureCoordBuffer_itemSize, GL.FLOAT, false, 0, 0);

            gl.activeTexture(GL.TEXTURE0);
            gl.bindTexture(GL.TEXTURE_2D, neheTexture);
            gl.uniform1i(shaderProgram_samplerUniform, 0);

            gl.bindBuffer(GL.ELEMENT_ARRAY_BUFFER, cubeVertexIndexBuffer);
            setMatrixUniforms();
            gl.drawElements(GL.TRIANGLES, cubeVertexIndexBuffer_numItems, GL.UNSIGNED_SHORT, 0);
        }

        DateTime lastTime;

        void animate() {
            var timeNow = DateTime.Now;
            if (lastTime != default(DateTime)) {
                var elapsed = (timeNow - lastTime).TotalMilliseconds;

                xRot += (90 * elapsed) / 1000.0;
                yRot += (90 * elapsed) / 1000.0;
                zRot += (90 * elapsed) / 1000.0;
            }
            lastTime = timeNow;
        }


        void tick() {
            requestAnimFrame(tick);
            drawScene();
            animate();
        }


        public void webGLStart(string canvasId) {
            var canvas = document.GetElementById(canvasId);
            initGL(canvas);
            initShaders();
            initBuffers();
            initTexture();

            gl.clearColor(0.0f, 0.0f, 1.0f, 1.0f);
            gl.enable(GL.DEPTH_TEST);

            tick();
        }
    }
}