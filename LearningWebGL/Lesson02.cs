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
using WebGLTexture = System.Windows.Browser.ScriptObject;
using WebGLUniformLocation = System.Windows.Browser.ScriptObject;
using WebGLContextAttributes = System.Windows.Browser.ScriptObject;
#endregion

namespace LearningWebGL
{
    [ScriptableType]
    public class Lesson02 : LessonBase
    {
        string shader_fs = @"
    precision mediump float;

    varying vec4 vColor;

    void main(void) {
        gl_FragColor = vColor;
    }
";

        string shader_vs = @"
    attribute vec3 aVertexPosition;
    attribute vec4 aVertexColor;

    uniform mat4 uMVMatrix;
    uniform mat4 uPMatrix;

    varying vec4 vColor;

    void main(void) {
        gl_Position = uPMatrix * uMVMatrix * vec4(aVertexPosition, 1.0);
        vColor = aVertexColor;
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
        GLint shaderProgram_vertexColorAttribute;
        WebGLUniformLocation shaderProgram_pMatrixUniform;
        WebGLUniformLocation shaderProgram_mvMatrixUniform;

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

            shaderProgram_vertexColorAttribute = gl.getAttribLocation(shaderProgram, "aVertexColor");
            gl.enableVertexAttribArray((GLuint)shaderProgram_vertexColorAttribute);

            shaderProgram_pMatrixUniform = gl.getUniformLocation(shaderProgram, "uPMatrix");
            shaderProgram_mvMatrixUniform = gl.getUniformLocation(shaderProgram, "uMVMatrix");
        }


        mat4 mvMatrix = mat4.create();
        mat4 pMatrix = mat4.create();

        void setMatrixUniforms() {
            gl.uniformMatrix4fv((WebGLUniformLocation)shaderProgram_pMatrixUniform, false, pMatrix.ToArray());
            gl.uniformMatrix4fv((WebGLUniformLocation)shaderProgram_mvMatrixUniform, false, mvMatrix.ToArray());
        }


        WebGLBuffer triangleVertexPositionBuffer;
        WebGLBuffer triangleVertexColorBuffer;
        WebGLBuffer squareVertexPositionBuffer;
        WebGLBuffer squareVertexColorBuffer;
        int triangleVertexPositionBuffer_itemSize;
        int triangleVertexPositionBuffer_numItems;
        int triangleVertexColorBuffer_itemSize;
        int triangleVertexColorBuffer_numItems;
        int squareVertexPositionBuffer_itemSize;
        int squareVertexPositionBuffer_numItems;
        int squareVertexColorBuffer_itemSize;
        int squareVertexColorBuffer_numItems;


        void initBuffers() {
            triangleVertexPositionBuffer = gl.createBuffer();
            gl.bindBuffer(GL.ARRAY_BUFFER, triangleVertexPositionBuffer);
            var vertices = new GLfloat[] {
                 0.0f,  1.0f,  0.0f,
                -1.0f, -1.0f,  0.0f,
                 1.0f, -1.0f,  0.0f
            };
            gl.bufferData(GL.ARRAY_BUFFER, new Float32Array(vertices), GL.STATIC_DRAW);
            triangleVertexPositionBuffer_itemSize = 3;
            triangleVertexPositionBuffer_numItems = 3;

            triangleVertexColorBuffer = gl.createBuffer();
            gl.bindBuffer(GL.ARRAY_BUFFER, triangleVertexColorBuffer);
            var colors = new GLfloat[] {
                1.0f, 0.0f, 0.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f, 1.0f
            };
            gl.bufferData(GL.ARRAY_BUFFER, new Float32Array(colors), GL.STATIC_DRAW);
            triangleVertexColorBuffer_itemSize = 4;
            triangleVertexColorBuffer_numItems = 3;

            squareVertexPositionBuffer = gl.createBuffer();
            gl.bindBuffer(GL.ARRAY_BUFFER, squareVertexPositionBuffer);
            vertices = new GLfloat[] {
                 1.0f,  1.0f,  0.0f,
                -1.0f,  1.0f,  0.0f,
                 1.0f, -1.0f,  0.0f,
                -1.0f, -1.0f,  0.0f
            };
            gl.bufferData(GL.ARRAY_BUFFER, new Float32Array(vertices), GL.STATIC_DRAW);
            squareVertexPositionBuffer_itemSize = 3;
            squareVertexPositionBuffer_numItems = 4;

            squareVertexColorBuffer = gl.createBuffer();
            gl.bindBuffer(GL.ARRAY_BUFFER, squareVertexColorBuffer);
            colors = new GLfloat[] {};
            for (var i=0; i < 4; i++) {
                colors = colors.concat(new GLfloat[] { 0.5f, 0.5f, 1.0f, 1.0f });
            }
            gl.bufferData(GL.ARRAY_BUFFER, new Float32Array(colors), GL.STATIC_DRAW);
            squareVertexColorBuffer_itemSize = 4;
            squareVertexColorBuffer_numItems = 4;
        }

        public void drawScene() {
            gl.viewport(0, 0, (int)viewportWidth, (int)viewportHeight);
            gl.clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);

            mat4.perspective(45, viewportWidth / viewportHeight, 0.1f, 100.0f, pMatrix);

            mat4.identity(mvMatrix);

            mat4.translate(mvMatrix, new GLfloat[] { -1.5f, 0.0f, -7.0f });
            gl.bindBuffer(GL.ARRAY_BUFFER, triangleVertexPositionBuffer);
            gl.vertexAttribPointer((GLuint)shaderProgram_vertexPositionAttribute, triangleVertexPositionBuffer_itemSize, GL.FLOAT, false, 0, 0);

            gl.bindBuffer(GL.ARRAY_BUFFER, triangleVertexColorBuffer);
            gl.vertexAttribPointer((GLuint)shaderProgram_vertexColorAttribute, triangleVertexColorBuffer_itemSize, GL.FLOAT, false, 0, 0);

            setMatrixUniforms();
            gl.drawArrays(GL.TRIANGLES, 0, triangleVertexPositionBuffer_numItems);

            mat4.translate(mvMatrix, new GLfloat[] { 3.0f, 0.0f, 0.0f });
            gl.bindBuffer(GL.ARRAY_BUFFER, squareVertexPositionBuffer);
            gl.vertexAttribPointer((GLuint)shaderProgram_vertexPositionAttribute, squareVertexPositionBuffer_itemSize, GL.FLOAT, false, 0, 0);

            gl.bindBuffer(GL.ARRAY_BUFFER, squareVertexColorBuffer);
            gl.vertexAttribPointer((GLuint)shaderProgram_vertexColorAttribute, squareVertexColorBuffer_itemSize, GL.FLOAT, false, 0, 0);
            
            setMatrixUniforms();
            gl.drawArrays(GL.TRIANGLE_STRIP, 0, squareVertexPositionBuffer_numItems);
        }

        public void webGLStart(string canvasId) {
            var canvas = document.GetElementById(canvasId);
            initGL(canvas);
            initShaders();
            initBuffers();

            gl.clearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.enable(GL.DEPTH_TEST);

            drawScene();
        }
    }
}