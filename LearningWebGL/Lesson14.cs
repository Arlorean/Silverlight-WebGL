using System;
using System.Windows;
using System.Collections.Generic;
using System.Json;
using System.Windows.Browser;
using System.Windows.Resources;
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
//using WebGLContextAttributes = System.Windows.Browser.ScriptObject;
#endregion

namespace LearningWebGL
{
    [ScriptableType]
    public class Lesson14 : LessonBase
    {

        string per_fragment_lighting_fs = @"
    precision mediump float;

    varying vec2 vTextureCoord;
    varying vec3 vTransformedNormal;
    varying vec4 vPosition;

    uniform float uMaterialShininess;

    uniform bool uShowSpecularHighlights;
    uniform bool uUseLighting;
    uniform bool uUseTextures;

    uniform vec3 uAmbientColor;

    uniform vec3 uPointLightingLocation;
    uniform vec3 uPointLightingSpecularColor;
    uniform vec3 uPointLightingDiffuseColor;

    uniform sampler2D uSampler;


    void main(void) {
        vec3 lightWeighting;
        if (!uUseLighting) {
            lightWeighting = vec3(1.0, 1.0, 1.0);
        } else {
            vec3 lightDirection = normalize(uPointLightingLocation - vPosition.xyz);
            vec3 normal = normalize(vTransformedNormal);

            float specularLightWeighting = 0.0;
            if (uShowSpecularHighlights) {
                vec3 eyeDirection = normalize(-vPosition.xyz);
                vec3 reflectionDirection = reflect(-lightDirection, normal);

                specularLightWeighting = pow(max(dot(reflectionDirection, eyeDirection), 0.0), uMaterialShininess);
            }

            float diffuseLightWeighting = max(dot(normal, lightDirection), 0.0);
            lightWeighting = uAmbientColor
                + uPointLightingSpecularColor * specularLightWeighting
                + uPointLightingDiffuseColor * diffuseLightWeighting;
        }

        vec4 fragmentColor;
        if (uUseTextures) {
            fragmentColor = texture2D(uSampler, vec2(vTextureCoord.s, vTextureCoord.t));
        } else {
            fragmentColor = vec4(1.0, 1.0, 1.0, 1.0);
        }
        gl_FragColor = vec4(fragmentColor.rgb * lightWeighting, fragmentColor.a);
    }";

        string per_fragment_lighting_vs = @"
    attribute vec3 aVertexPosition;
    attribute vec3 aVertexNormal;
    attribute vec2 aTextureCoord;

    uniform mat4 uMVMatrix;
    uniform mat4 uPMatrix;
    uniform mat3 uNMatrix;

    varying vec2 vTextureCoord;
    varying vec3 vTransformedNormal;
    varying vec4 vPosition;


    void main(void) {
        vPosition = uMVMatrix * vec4(aVertexPosition, 1.0);
        gl_Position = uPMatrix * vPosition;
        vTextureCoord = aTextureCoord;
        vTransformedNormal = uNMatrix * aVertexNormal;
    }";

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
        GLint shaderProgram_vertexNormalAttribute;
        GLint shaderProgram_textureCoordAttribute;
        WebGLUniformLocation shaderProgram_pMatrixUniform;
        WebGLUniformLocation shaderProgram_mvMatrixUniform;
        WebGLUniformLocation shaderProgram_nMatrixUniform;
        WebGLUniformLocation shaderProgram_samplerUniform;
        WebGLUniformLocation shaderProgram_materialShininessUniform;
        WebGLUniformLocation shaderProgram_showSpecularHighlightsUniform;
        WebGLUniformLocation shaderProgram_useTexturesUniform;
        WebGLUniformLocation shaderProgram_useLightingUniform;
        WebGLUniformLocation shaderProgram_ambientColorUniform;
        WebGLUniformLocation shaderProgram_pointLightingLocationUniform;
        WebGLUniformLocation shaderProgram_pointLightingSpecularColorUniform;
        WebGLUniformLocation shaderProgram_pointLightingDiffuseColorUniform;

        void initShaders() {
            var fragmentShader = getShader(gl, per_fragment_lighting_fs, GL.FRAGMENT_SHADER);
            var vertexShader = getShader(gl, per_fragment_lighting_vs, GL.VERTEX_SHADER);

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

            shaderProgram_vertexNormalAttribute = gl.getAttribLocation(shaderProgram, "aVertexNormal");
            gl.enableVertexAttribArray((GLuint)shaderProgram_vertexNormalAttribute);

            shaderProgram_textureCoordAttribute = gl.getAttribLocation(shaderProgram, "aTextureCoord");
            gl.enableVertexAttribArray((GLuint)shaderProgram_textureCoordAttribute);

            shaderProgram_pMatrixUniform = gl.getUniformLocation(shaderProgram, "uPMatrix");
            shaderProgram_mvMatrixUniform = gl.getUniformLocation(shaderProgram, "uMVMatrix");
            shaderProgram_nMatrixUniform = gl.getUniformLocation(shaderProgram, "uNMatrix");
            shaderProgram_samplerUniform = gl.getUniformLocation(shaderProgram, "uSampler");
            shaderProgram_materialShininessUniform = gl.getUniformLocation(shaderProgram, "uMaterialShininess");
            shaderProgram_showSpecularHighlightsUniform = gl.getUniformLocation(shaderProgram, "uShowSpecularHighlights");
            shaderProgram_useTexturesUniform = gl.getUniformLocation(shaderProgram, "uUseTextures");
            shaderProgram_useLightingUniform = gl.getUniformLocation(shaderProgram, "uUseLighting");
            shaderProgram_ambientColorUniform = gl.getUniformLocation(shaderProgram, "uAmbientColor");
            shaderProgram_pointLightingLocationUniform = gl.getUniformLocation(shaderProgram, "uPointLightingLocation");
            shaderProgram_pointLightingSpecularColorUniform = gl.getUniformLocation(shaderProgram, "uPointLightingSpecularColor");
            shaderProgram_pointLightingDiffuseColorUniform = gl.getUniformLocation(shaderProgram, "uPointLightingDiffuseColor");
        }


        void handleLoadedTexture(WebGLTexture texture, Image image) {
            gl.bindTexture(GL.TEXTURE_2D, texture);
            gl.pixelStorei(GL.UNPACK_FLIP_Y_WEBGL, 1/*true*/);
            gl.texImage2D(GL.TEXTURE_2D, 0, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, image);
            gl.texParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.NEAREST);
            gl.texParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.NEAREST);
            gl.generateMipmap(GL.TEXTURE_2D);

            gl.bindTexture(GL.TEXTURE_2D, null);
        }

        WebGLTexture earthTexture;
        WebGLTexture galvanizedTexture;

        void initTexture() {
            earthTexture = gl.createTexture();
            var earthTexture_image = new Image();
            earthTexture_image.onload += delegate {
                handleLoadedTexture(earthTexture, earthTexture_image);
            };
            earthTexture_image.src = "earth.jpg";

            galvanizedTexture = gl.createTexture();
            var galvanizedTexture_image = new Image();
            galvanizedTexture_image.onload += delegate {
                handleLoadedTexture(galvanizedTexture, galvanizedTexture_image);
            };
            galvanizedTexture_image.src = "arroway.de_metal+structure+06_d100_flat.jpg";
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

            var normalMatrix = mat3.create();
            mat4.toInverseMat3(mvMatrix, normalMatrix);
            mat3.transpose(normalMatrix);
            gl.uniformMatrix3fv((WebGLUniformLocation)shaderProgram_nMatrixUniform, false, normalMatrix.ToArray());
        }

        double degToRad(double degrees) {
            return degrees * Math.PI / 180;
        }

        WebGLBuffer teapotVertexPositionBuffer;
        WebGLBuffer teapotVertexNormalBuffer;
        WebGLBuffer teapotVertexTextureCoordBuffer;
        WebGLBuffer teapotVertexIndexBuffer;
        int teapotVertexPositionBuffer_itemSize;
        int teapotVertexPositionBuffer_numItems;
        int teapotVertexNormalBuffer_itemSize;
        int teapotVertexNormalBuffer_numItems;
        int teapotVertexTextureCoordBuffer_itemSize;
        int teapotVertexTextureCoordBuffer_numItems;
        int teapotVertexIndexBuffer_itemSize;
        int teapotVertexIndexBuffer_numItems;

        void handleLoadedTeapot(JsonValue teapotData) {
            teapotVertexNormalBuffer = gl.createBuffer();
            gl.bindBuffer(GL.ARRAY_BUFFER, teapotVertexNormalBuffer);
            gl.bufferData(GL.ARRAY_BUFFER, new Float32Array(teapotData["vertexNormals"].ReadAsType<float[]>()), GL.STATIC_DRAW);
            teapotVertexNormalBuffer_itemSize = 3;
            teapotVertexNormalBuffer_numItems = teapotData["vertexNormals"].Count / 3;

            teapotVertexTextureCoordBuffer = gl.createBuffer();
            gl.bindBuffer(GL.ARRAY_BUFFER, teapotVertexTextureCoordBuffer);
            gl.bufferData(GL.ARRAY_BUFFER, new Float32Array(teapotData["vertexTextureCoords"].ReadAsType<float[]>()), GL.STATIC_DRAW);
            teapotVertexTextureCoordBuffer_itemSize = 2;
            teapotVertexTextureCoordBuffer_numItems = teapotData["vertexTextureCoords"].Count / 2;

            teapotVertexPositionBuffer = gl.createBuffer();
            gl.bindBuffer(GL.ARRAY_BUFFER, teapotVertexPositionBuffer);
            gl.bufferData(GL.ARRAY_BUFFER, new Float32Array(teapotData["vertexPositions"].ReadAsType<float[]>()), GL.STATIC_DRAW);
            teapotVertexPositionBuffer_itemSize = 3;
            teapotVertexPositionBuffer_numItems = teapotData["vertexPositions"].Count / 3;

            teapotVertexIndexBuffer = gl.createBuffer();
            gl.bindBuffer(GL.ELEMENT_ARRAY_BUFFER, teapotVertexIndexBuffer);
            gl.bufferData(GL.ELEMENT_ARRAY_BUFFER, new Uint16Array(teapotData["indices"].ReadAsType<UInt16[]>()), GL.STATIC_DRAW);
            teapotVertexIndexBuffer_itemSize = 1;
            teapotVertexIndexBuffer_numItems = teapotData["indices"].Count;

            //document.GetElementById("loadingtext").textContent = "";
        }

        void loadTeapot() {
            var teapotData = JsonValue.Load(Application.GetResourceStream(new Uri("Teapot.json", UriKind.Relative)).Stream);
            handleLoadedTeapot(teapotData);
            //var request = new XmlHttpRequest();
            //request.open("GET", "Teapot.json");
            //request.onreadystatechange = function () {
            //    if (request.readyState == 4) {
            //        handleLoadedTeapot(JSON.parse(request.responseText));
            //    }
            //}
            //
            //request.send();
        }

        double teapotAngle = 180;

        public void drawScene() {
            gl.viewport(0, 0, (int)viewportWidth, (int)viewportHeight);
            gl.clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);

            if (teapotVertexPositionBuffer == null || teapotVertexNormalBuffer == null || teapotVertexTextureCoordBuffer == null || teapotVertexIndexBuffer == null) {
                return;
            }

            // Don't render until the options html has been loaded
            var document = optionsDocument;
            if (document == null) { return; }

            mat4.perspective(45, viewportWidth / viewportHeight, 0.1f, 100.0f, pMatrix);

            var specularHighlights = document.GetElementById("specular").Checked();
            gl.uniform1i(shaderProgram_showSpecularHighlightsUniform, specularHighlights?1:0);

            var lighting = document.GetElementById("lighting").Checked();
            gl.uniform1i(shaderProgram_useLightingUniform, lighting?1:0);
            if (lighting) {
                gl.uniform3f(
                    shaderProgram_ambientColorUniform,
                    parseFloat(document.GetElementById("ambientR").Value()),
                    parseFloat(document.GetElementById("ambientG").Value()),
                    parseFloat(document.GetElementById("ambientB").Value())
                );

                gl.uniform3f(
                    shaderProgram_pointLightingLocationUniform,
                    parseFloat(document.GetElementById("lightPositionX").Value()),
                    parseFloat(document.GetElementById("lightPositionY").Value()),
                    parseFloat(document.GetElementById("lightPositionZ").Value())
                );

                gl.uniform3f(
                    shaderProgram_pointLightingSpecularColorUniform,
                    parseFloat(document.GetElementById("specularR").Value()),
                    parseFloat(document.GetElementById("specularG").Value()),
                    parseFloat(document.GetElementById("specularB").Value())
                );

                gl.uniform3f(
                    shaderProgram_pointLightingDiffuseColorUniform,
                    parseFloat(document.GetElementById("diffuseR").Value()),
                    parseFloat(document.GetElementById("diffuseG").Value()),
                    parseFloat(document.GetElementById("diffuseB").Value())
                );
            }

            var texture = document.GetElementById("texture").Value();
            gl.uniform1i(shaderProgram_useTexturesUniform, (texture != "none")?1:0);

            mat4.identity(mvMatrix);

            mat4.translate(mvMatrix, new GLfloat[] { 0, 0, -40 });
            mat4.rotate(mvMatrix, degToRad(23.4), new GLfloat[] { 1, 0, -1 });
            mat4.rotate(mvMatrix, degToRad(teapotAngle), new GLfloat[] { 0, 1, 0 });

            gl.activeTexture(GL.TEXTURE0);
            if (texture == "earth") {
                gl.bindTexture(GL.TEXTURE_2D, earthTexture);
            } else if (texture == "galvanized") {
                gl.bindTexture(GL.TEXTURE_2D, galvanizedTexture);
            }
            gl.uniform1i(shaderProgram_samplerUniform, 0);

            gl.uniform1f(shaderProgram_materialShininessUniform, parseFloat(document.GetElementById("shininess").Value()));

            gl.bindBuffer(GL.ARRAY_BUFFER, teapotVertexPositionBuffer);
            gl.vertexAttribPointer((GLuint)shaderProgram_vertexPositionAttribute, teapotVertexPositionBuffer_itemSize, GL.FLOAT, false, 0, 0);

            gl.bindBuffer(GL.ARRAY_BUFFER, teapotVertexTextureCoordBuffer);
            gl.vertexAttribPointer((GLuint)shaderProgram_textureCoordAttribute, teapotVertexTextureCoordBuffer_itemSize, GL.FLOAT, false, 0, 0);

            gl.bindBuffer(GL.ARRAY_BUFFER, teapotVertexNormalBuffer);
            gl.vertexAttribPointer((GLuint)shaderProgram_vertexNormalAttribute, teapotVertexNormalBuffer_itemSize, GL.FLOAT, false, 0, 0);

            gl.bindBuffer(GL.ELEMENT_ARRAY_BUFFER, teapotVertexIndexBuffer);
            setMatrixUniforms();
            gl.drawElements(GL.TRIANGLES, teapotVertexIndexBuffer_numItems, GL.UNSIGNED_SHORT, 0);

        }

        DateTime lastTime;

        void animate() {
            var timeNow = DateTime.Now;
            if (lastTime != default(DateTime)) {
                var elapsed = (timeNow - lastTime).TotalMilliseconds;

                teapotAngle += 0.05 * elapsed;
            }
            lastTime = timeNow;
        }


        void tick() {
            requestAnimFrame(tick);
            drawScene();
            animate();
        }

        HtmlDocument optionsDocument;

        public void webGLStart(string canvasId) {
            var canvas = document.GetElementById(canvasId);
            initGL(canvas);
            initShaders();
            initTexture();
            loadTeapot();
            initOptions();

            gl.clearColor(0.0f, 0.0f, 1.0f, 1.0f);
            gl.enable(GL.DEPTH_TEST);

            tick();
        }

        void initOptions() {
            // load the options html into the known iframe src
            var parent = window.GetProperty("parent") as ScriptObject;
            var parentDocument = parent.GetProperty("document") as HtmlDocument;
            var optionsIFrame = parentDocument.GetElementById("options");
            optionsIFrame.SetAttribute("src", "Lesson14.html");
            optionsIFrame.AttachEvent("onload", delegate(object sender, EventArgs args) {
                optionsDocument = optionsIFrame.GetProperty("contentDocument") as HtmlDocument;

                // Resize the options iframe height to the options html height
                var optionsBody = optionsDocument.GetProperty("body") as HtmlElement;
                var optionsHeight = optionsBody.GetProperty("scrollHeight");
                optionsIFrame.SetAttribute("height", optionsHeight + "px");
            });
        }
    }
}