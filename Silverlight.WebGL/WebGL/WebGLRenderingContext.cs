#region License
//
// Silverlight.WebGL License (MIT License) 
//
// Copyright (c) 2012 Adam Davidson
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the 
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
// sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//
// see http://www.opensource.org/licenses/MIT for more details
//
#endregion

#region Usings
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Browser;
using System.Collections.Generic;
#endregion

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
using WebGLContextAttributes = System.Windows.Browser.ScriptObject;
#endregion

namespace Silverlight.Html.WebGL
{
    public class WebGLRenderingContext
    {
        #region Constructors
        public WebGLRenderingContext() { }
        public WebGLRenderingContext(HtmlElement canvas) : this(canvas, null) {}
        public WebGLRenderingContext(HtmlElement canvas, WebGLContextAttributes attrs) {
            // TODO: Process attrs (e.g. {preserveDrawingBuffer: true})
            context = canvas.Invoke("getContext", new object[] { "webgl" /*, attrs*/ }) as ScriptObject;
            if (context == null) {
                context = canvas.Invoke("getContext", new object[] { "experimental-webgl" }) as ScriptObject;
            }
        }
        public WebGLRenderingContext(ScriptObject context) {
            this.context = context;
        }
        #endregion

        #region WebGL context
        ScriptObject context;
        public ScriptObject Context {
            get { return context; }
            set { context = value; }
        }
        public object Invoke(string name, params object[] args) {
            try {
                return context.Invoke(name, args);
            }
            catch {
                // Ignore errors, such as CORS, textures via file URLs
                return null;
            }
        }
        #endregion

        #region WebGL object mapping
        Dictionary<ScriptObject, int> Ids = new Dictionary<ScriptObject, int>();
        List<ScriptObject> Objects = new List<ScriptObject>();
        public int GetId(ScriptObject obj) {
            var id = 0;
            if (!Ids.TryGetValue(obj, out id)) {
                Objects.Add(obj);
                Ids[obj] = id = Objects.Count;
            }
            else {
            }
            return id;
        }
        public ScriptObject GetObject(int id) {
            return Objects[id-1];
        }
        public ScriptObject GetObject(uint id) {
            return Objects[(int)id-1];
        }
        #endregion

        #region WebGL type conversions
        static GLint GLint(object any) {
            return Convert.ToInt32(any);
        }
        static GLenum GLenum(object any) {
            return Convert.ToUInt32(any);
        }
        static GLboolean GLboolean(object any) {
            return Convert.ToBoolean(any);
        }
        #endregion

        public WebGLContextAttributes getContextAttributes() {
            return (WebGLContextAttributes) Invoke("getContextAttributes", null);
        }
        public bool isContextLost() {
            return (bool)Invoke("isContextLost", null);
        }
        public DOMString[] getSupportedExtensions() {
            return (DOMString[]) Invoke("getSupportedExtensions", null);
        }
        public object getExtension(DOMString name) {
            return Invoke("getExtension", name);
        }

        public void activeTexture(GLenum texture) {
            Invoke("activeTexture", texture);
        }
        public void attachShader(WebGLProgram program, WebGLShader shader) {
            Invoke("attachShader", program, shader);
        }
        public void bindAttribLocation(WebGLProgram program, GLuint index, DOMString name) {
            Invoke("bindAttribLocation", program, index, name);
        }
        public void bindBuffer(GLenum target, WebGLBuffer buffer) {
            Invoke("bindBuffer", target, buffer);
        }
            // void bindFramebuffer(GLenum target, WebGLFramebuffer framebuffer);
            // void bindRenderbuffer(GLenum target, WebGLRenderbuffer renderbuffer);
        public void bindTexture(GLenum target, WebGLTexture texture) {
            Invoke("bindTexture", target, texture != null ? texture.Object : null);
        }
            // void blendColor(GLclampf red, GLclampf green, GLclampf blue, GLclampf alpha);
            // void blendEquation(GLenum mode);
            // void blendEquationSeparate(GLenum modeRGB, GLenum modeAlpha);
            // void blendFunc(GLenum sfactor, GLenum dfactor);
            // void blendFuncSeparate(GLenum srcRGB, GLenum dstRGB, 
            //           GLenum srcAlpha, GLenum dstAlpha);

        public void bufferData(GLenum target, GLsizeiptr size, GLenum usage) {
            Invoke("bufferData", target, size, usage);
        }
        public void bufferData(GLenum target, ArrayBufferView data, GLenum usage) {
            Invoke("bufferData", target, data.Object, usage);
        }
        public void bufferData(GLenum target, ArrayBuffer data, GLenum usage) {
            Invoke("bufferData", target, data.Object, usage);
        }
            // void bufferData(GLenum target, ArrayBuffer data, GLenum usage) {
            // void bufferSubData(GLenum target, GLintptr offset, ArrayBufferView data);
            // void bufferSubData(GLenum target, GLintptr offset, ArrayBuffer data);

            // GLenum checkFramebufferStatus(GLenum target);

        public void clear(GLbitfield mask) {
            Invoke("clear", mask);
        }
        public void clearColor(GLclampf red, GLclampf green, GLclampf blue, GLclampf alpha) {
            Invoke("clearColor", red, green, blue, alpha);
        }
        public void clearDepth(GLclampf depth) {
            Invoke("clearDepth", depth);
        }
        public void clearStencil(GLint s) {
            Invoke("clearStencil", s);
        }
        public void colorMask(GLboolean red, GLboolean green, GLboolean blue, GLboolean alpha) {
            Invoke("colorMask", red, green, blue, alpha);
        }
        public void compileShader(WebGLShader shader) {
            Invoke("compileShader", shader);
        }

            // void copyTexImage2D(GLenum target, GLint level, GLenum internalformat, 
            //        GLint x, GLint y, GLsizei width, GLsizei height, 
            //        GLint border);
            // void copyTexSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, 
            //           GLint x, GLint y, GLsizei width, GLsizei height);


        public WebGLBuffer createBuffer() {
            return (WebGLBuffer) Invoke("createBuffer", null);
        }
        public WebGLFramebuffer createFramebuffer() {
            return (WebGLFramebuffer) Invoke("createFramebuffer", null);
        }
        public WebGLProgram createProgram() {
            return (WebGLProgram) Invoke("createProgram", null);
        }
        public WebGLRenderbuffer createRenderbuffer() {
            return (WebGLRenderbuffer)Invoke("createRenderbuffer", null);
        }
        public WebGLShader createShader(GLenum type) {
            return (WebGLShader) Invoke("createShader", type);
        }
        public WebGLTexture createTexture() {
            var texture = Invoke("createTexture", null);
            return new WebGLTexture(texture as ScriptObject);
        }
        public void cullFace(GLenum mode) {
            Invoke("cullFace", mode);
        }

        public void deleteBuffer(WebGLBuffer buffer) {
            Invoke("deleteBuffer", buffer);
        }
        public void deleteFramebuffer(WebGLFramebuffer framebuffer) {
            Invoke("deleteFramebuffer", framebuffer);
        }
        public void deleteProgram(WebGLProgram program) {
            Invoke("deleteProgram", program);
        }
        public void deleteRenderbuffer(WebGLRenderbuffer renderbuffer) {
            Invoke("deleteRenderbuffer", renderbuffer);
        }
        public void deleteShader(WebGLShader shader) {
            Invoke("deleteShader", shader);
        }
        public void deleteTexture(WebGLTexture texture) {
            Invoke("deleteTexture", texture);
        }
        public void depthFunc(GLenum func) {
            Invoke("depthFunc", func);
        }
        public void depthMask(GLboolean flag) {
            Invoke("depthMask", flag);
        }
        public void depthRange(GLclampf zNear, GLclampf zFar) {
            Invoke("depthRange", zNear, zFar);
        }
        public void detachShader(WebGLProgram program, WebGLShader shader) {
            Invoke("detachShader", program, shader);
        }
        public void disable(GLenum cap) {
            Invoke("disable", cap);
        }
        public void disableVertexAttribArray(GLuint index) {
            Invoke("disableVertexAttribArray", index);
        }
        public void drawArrays(GLenum mode, GLint first, GLsizei count) {
            Invoke("drawArrays", mode, first, count);
        }
        public void drawElements(GLenum mode, GLsizei count, GLenum type, GLintptr offset) {
            Invoke("drawElements", mode, count, type, offset);
        }
        public void enable(GLenum cap) {
            Invoke("enable", cap);
        }
        public void enableVertexAttribArray(GLuint index) {
            Invoke("enableVertexAttribArray", index);
        }
        public void finish() {
            Invoke("finish", null);
        }
        public void flush() {
            Invoke("flush", null);
        }
        public void framebufferRenderbuffer(GLenum target, GLenum attachment, GLenum renderbuffertarget, WebGLRenderbuffer renderbuffer) {
            Invoke("framebufferRenderbuffer", target, attachment, renderbuffertarget, renderbuffer);
        }



            // void framebufferRenderbuffer(GLenum target, GLenum attachment, 
            //                 GLenum renderbuffertarget, 
            //                 WebGLRenderbuffer renderbuffer);
            // void framebufferTexture2D(GLenum target, GLenum attachment, GLenum textarget, 
            //              WebGLTexture texture, GLint level);
        public void frontFace(GLenum mode) {
            Invoke("frontFace", mode);
        }

        public void generateMipmap(GLenum target) {
            Invoke("generateMipmap", target);
        }

            // WebGLActiveInfo getActiveAttrib(WebGLProgram program, GLuint index);
            // WebGLActiveInfo getActiveUniform(WebGLProgram program, GLuint index);
            // WebGLShader[ ] getAttachedShaders(WebGLProgram program);

            // GLint getAttribLocation(WebGLProgram program, DOMString name);
        public GLint getAttribLocation(WebGLProgram program, DOMString name) {
            return GLint(Invoke("getAttribLocation", program, name));
        }

            // any getParameter(GLenum pname);
            // any getBufferParameter(GLenum target, GLenum pname);

        public GLenum getError() {
            return GLenum(Invoke("getError", null));
        }

            // any getFramebufferAttachmentParameter(GLenum target, GLenum attachment, 
            //                          GLenum pname);
        public object getProgramParameter(WebGLProgram program, GLenum pname) {
            return Invoke("getProgramParameter", program, pname);
        }
            // DOMString getProgramInfoLog(WebGLProgram program);
            // any getRenderbufferParameter(GLenum target, GLenum pname);

        public object getShaderParameter(WebGLShader shader, GLenum pname) {
            return Invoke("getShaderParameter", shader, pname);
        }
        public DOMString getShaderInfoLog(WebGLShader shader) {
            return (DOMString) Invoke("getShaderInfoLog", shader);
        }
            // DOMString getShaderInfoLog(WebGLShader shader);

            // DOMString getShaderSource(WebGLShader shader);

            // any getTexParameter(GLenum target, GLenum pname);

            // any getUniform(WebGLProgram program, WebGLUniformLocation location);

        public WebGLUniformLocation getUniformLocation(WebGLProgram program, DOMString name) {
            return (WebGLUniformLocation)Invoke("getUniformLocation", program, name);
        }

            // any getVertexAttrib(GLuint index, GLenum pname);

            // GLsizeiptr getVertexAttribOffset(GLuint index, GLenum pname);

        public void hint(GLenum target, GLenum mode) {
            Invoke("hint", target, mode);
        }
        public GLboolean isBuffer(WebGLBuffer buffer) {
            return (GLboolean)Invoke("isBuffer", buffer);
        }
        public GLboolean isEnabled(GLenum cap) {
            return (GLboolean)Invoke("isEnabled", cap);
        }
        public GLboolean isFramebuffer(WebGLFramebuffer framebuffer) {
            return (GLboolean)Invoke("isFramebuffer", framebuffer);
        }
        public GLboolean isProgram(WebGLProgram program) {
            return (GLboolean)Invoke("isProgram", program);
        }
        public GLboolean isRenderbuffer(WebGLRenderbuffer renderbuffer) {
            return (GLboolean)Invoke("isRenderbuffer", renderbuffer);
        }
        public GLboolean isShader(WebGLShader shader) {
            return (GLboolean)Invoke("isShader", shader);
        }
        public GLboolean isTexture(WebGLTexture texture) {
            return (GLboolean)Invoke("isTexture", texture);
        }
        public void lineWidth(GLfloat width) {
            Invoke("lineWidth", width);
        }
        public void linkProgram(WebGLProgram program) {
            Invoke("linkProgram", program);
        }
        public void pixelStorei(GLenum pname, GLint param) {
            Invoke("pixelStorei", pname, param);
        }
        public void polygonOffset(GLfloat factor, GLfloat units) {
            Invoke("polygonOffset", factor, units);
        }
        public void readPixels(GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, ArrayBufferView pixels) {
            Invoke("readPixels", x, y, width, height, format, type, pixels);
        }
        public void renderbufferStorage(GLenum target, GLenum internalformat, GLsizei width, GLsizei height) {
            Invoke("renderbufferStorage", target, internalformat, width, height);
        }
        public void sampleCoverage(GLclampf value, GLboolean invert) {
            Invoke("sampleCoverage", value, invert);
        }
        public void scissor(GLint x, GLint y, GLsizei width, GLsizei height) {
            Invoke("shaderSource", x, y, width, height);
        }

        public void shaderSource(WebGLShader shader, DOMString source) {
            Invoke("shaderSource", shader, source);
        }
        public void stencilFunc(GLenum func, GLint @ref, GLuint mask) {
            Invoke("stencilFunc", func, @ref, mask);
        }
        public void stencilFuncSeparate(GLenum face, GLenum func, GLint @ref, GLuint mask) {
            Invoke("stencilFunc", face, func, @ref, mask);
        }
        public void stencilMask(GLuint mask) {
            Invoke("stencilFunc", mask);
        }
        public void stencilMaskSeparate(GLenum face, GLuint mask) {
            Invoke("stencilFunc", face, mask);
        }
        public void stencilOp(GLenum fail, GLenum zfail, GLenum zpass) {
            Invoke("stencilFunc", fail, zfail, zpass);
        }
        public void stencilOpSeparate(GLenum face, GLenum fail, GLenum zfail, GLenum zpass) {
            Invoke("stencilFunc", face, fail, zfail, zpass);
        }

            // void texImage2D(GLenum target, GLint level, GLenum internalformat, 
            //    GLsizei width, GLsizei height, GLint border, GLenum format, 
            //    GLenum type, ArrayBufferView pixels);
            // void texImage2D(GLenum target, GLint level, GLenum internalformat,
            //    GLenum format, GLenum type, ImageData pixels);
        public void texImage2D(GLenum target, GLint level, GLenum internalformat, GLenum format, GLenum type, Image image) {
            Invoke("texImage2D", target, level, internalformat, format, type, image.Object);
        }
            // void texImage2D(GLenum target, GLint level, GLenum internalformat,
            //    GLenum format, GLenum type, HTMLCanvasElement canvas) raises (DOMException);
            // void texImage2D(GLenum target, GLint level, GLenum internalformat,
            //    GLenum format, GLenum type, HTMLVideoElement video) raises (DOMException);

        public void texParameterf(GLenum target, GLenum pname, GLfloat param) {
            Invoke("texParameterf", target, pname, param);
        }
        public void texParameteri(GLenum target, GLenum pname, GLint param) {
            Invoke("texParameteri", target, pname, param);
        }
        public void texParameteri(GLenum target, GLenum pname, GLuint param) {
            Invoke("texParameteri", target, pname, param);
        }

            // void texSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, 
            //       GLsizei width, GLsizei height, 
            //       GLenum format, GLenum type, ArrayBufferView pixels);
            // void texSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, 
            //       GLenum format, GLenum type, ImageData pixels);
            // void texSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, 
            //       GLenum format, GLenum type, HTMLImageElement image) raises (DOMException);
            // void texSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, 
            //       GLenum format, GLenum type, HTMLCanvasElement canvas) raises (DOMException);
            // void texSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, 
            //       GLenum format, GLenum type, HTMLVideoElement video) raises (DOMException);

        public void uniform1f(WebGLUniformLocation location, GLfloat x) {
            Invoke("uniform1f", location, x);
        }
            // void uniform1fv(WebGLUniformLocation location, FloatArray v);
            // void uniform1fv(WebGLUniformLocation location, sequence<float> v);
        public void uniform1i(WebGLUniformLocation location, GLint x) {
            Invoke("uniform1i", location, x);
        }
            // void uniform1iv(WebGLUniformLocation location, Int32Array v);
            // void uniform1iv(WebGLUniformLocation location, sequence<long> v);
            // void uniform2f(WebGLUniformLocation location, GLfloat x, GLfloat y);
            // void uniform2fv(WebGLUniformLocation location, FloatArray v);
            // void uniform2fv(WebGLUniformLocation location, sequence<float> v);
            // void uniform2i(WebGLUniformLocation location, GLint x, GLint y);
            // void uniform2iv(WebGLUniformLocation location, Int32Array v);
            // void uniform2iv(WebGLUniformLocation location, sequence<long> v);

        public void uniform3f(WebGLUniformLocation location, GLfloat x, GLfloat y, GLfloat z) {
            Invoke("uniform3f", location, x, y, z);
        }

            // void uniform3fv(WebGLUniformLocation location, FloatArray v);
            // void uniform3fv(WebGLUniformLocation location, sequence<float> v);
            // void uniform3i(WebGLUniformLocation location, GLint x, GLint y, GLint z);
            // void uniform3iv(WebGLUniformLocation location, Int32Array v);
            // void uniform3iv(WebGLUniformLocation location, sequence<long> v);
            // void uniform4f(WebGLUniformLocation location, GLfloat x, GLfloat y, GLfloat z, GLfloat w);
            // void uniform4fv(WebGLUniformLocation location, FloatArray v);
            // void uniform4fv(WebGLUniformLocation location, sequence<float> v);
            // void uniform4i(WebGLUniformLocation location, GLint x, GLint y, GLint z, GLint w);
            // void uniform4iv(WebGLUniformLocation location, Int32Array v);
            // void uniform4iv(WebGLUniformLocation location, sequence<long> v);

            // void uniformMatrix2fv(WebGLUniformLocation location, GLboolean transpose, 
            //          FloatArray value);
            // void uniformMatrix2fv(WebGLUniformLocation location, GLboolean transpose, 
            //          sequence<float> value);
        public void uniformMatrix3fv(WebGLUniformLocation location, GLboolean transpose, Float32Array value) {
            Invoke("uniformMatrix3fv", location, transpose, value.Object);
        }
        public void uniformMatrix3fv(WebGLUniformLocation location, GLboolean transpose, GLfloat[] value) {
            // FIXME: Passing float array as value into IE9 works but Firefox/Chrome gives an error
            Invoke("uniformMatrix3fv", location, transpose, new Float32Array(value).Object);
        }
        public void uniformMatrix4fv(WebGLUniformLocation location, GLboolean transpose, Float32Array value) {
            Invoke("uniformMatrix4fv", location, transpose, value.Object);
        }
        public void uniformMatrix4fv(WebGLUniformLocation location, GLboolean transpose, GLfloat[] value) {
            // FIXME: Passing float array as value into IE9 works but Firefox/Chrome gives an error
            Invoke("uniformMatrix4fv", location, transpose, new Float32Array(value).Object);
        }

        public void useProgram(WebGLProgram program) {
            Invoke("useProgram", program);
        }
        public void validateProgram(WebGLProgram program) {
            Invoke("validateProgram", program);
        }

            // void vertexAttrib1f(GLuint indx, GLfloat x);
            // void vertexAttrib1fv(GLuint indx, FloatArray values);
            // void vertexAttrib1fv(GLuint indx, sequence<float> values);
            // void vertexAttrib2f(GLuint indx, GLfloat x, GLfloat y);
            // void vertexAttrib2fv(GLuint indx, FloatArray values);
            // void vertexAttrib2fv(GLuint indx, sequence<float> values);
            // void vertexAttrib3f(GLuint indx, GLfloat x, GLfloat y, GLfloat z);
            // void vertexAttrib3fv(GLuint indx, FloatArray values);
            // void vertexAttrib3fv(GLuint indx, sequence<float> values);
            // void vertexAttrib4f(GLuint indx, GLfloat x, GLfloat y, GLfloat z, GLfloat w);
            // void vertexAttrib4fv(GLuint indx, FloatArray values);
            // void vertexAttrib4fv(GLuint indx, sequence<float> values);

        public void vertexAttribPointer(GLuint indx, GLint size, GLenum type, GLboolean normalized, GLsizei stride, GLintptr offset) {
            Invoke("vertexAttribPointer", indx, size, type, normalized, stride, offset);   
        }
        public void viewport(GLint x, GLint y, GLsizei width, GLsizei height) {
            Invoke("viewport", x, y, width, height);
        }
    }
}
