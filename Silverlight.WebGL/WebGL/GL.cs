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

using GLenum = System.UInt32;

namespace Silverlight.Html.WebGL
{
    /// <summary>
    /// WebGL contants taken from:
    /// http://www.khronos.org/registry/webgl/specs/latest/webgl.idl
    /// </summary>
    public static class GL
    {
        #region Constants
        /* ClearBufferMask */
        public const GLenum DEPTH_BUFFER_BIT = 0x00000100;
        public const GLenum STENCIL_BUFFER_BIT = 0x00000400;
        public const GLenum COLOR_BUFFER_BIT = 0x00004000;

        /* BeginMode */
        public const GLenum POINTS = 0x0000;
        public const GLenum LINES = 0x0001;
        public const GLenum LINE_LOOP = 0x0002;
        public const GLenum LINE_STRIP = 0x0003;
        public const GLenum TRIANGLES = 0x0004;
        public const GLenum TRIANGLE_STRIP = 0x0005;
        public const GLenum TRIANGLE_FAN = 0x0006;

        /* AlphaFunction (not supported in ES20) */
        /*      NEVER */
        /*      LESS */
        /*      EQUAL */
        /*      LEQUAL */
        /*      GREATER */
        /*      NOTEQUAL */
        /*      GEQUAL */
        /*      ALWAYS */

        /* BlendingFactorDest */
        public const GLenum ZERO = 0;
        public const GLenum ONE = 1;
        public const GLenum SRC_COLOR = 0x0300;
        public const GLenum ONE_MINUS_SRC_COLOR = 0x0301;
        public const GLenum SRC_ALPHA = 0x0302;
        public const GLenum ONE_MINUS_SRC_ALPHA = 0x0303;
        public const GLenum DST_ALPHA = 0x0304;
        public const GLenum ONE_MINUS_DST_ALPHA = 0x0305;

        /* BlendingFactorSrc */
        /*      ZERO */
        /*      ONE */
        public const GLenum DST_COLOR = 0x0306;
        public const GLenum ONE_MINUS_DST_COLOR = 0x0307;
        public const GLenum SRC_ALPHA_SATURATE = 0x0308;
        /*      SRC_ALPHA */
        /*      ONE_MINUS_SRC_ALPHA */
        /*      DST_ALPHA */
        /*      ONE_MINUS_DST_ALPHA */

        /* BlendEquationSeparate */
        public const GLenum FUNC_ADD = 0x8006;
        public const GLenum BLEND_EQUATION = 0x8009;
        public const GLenum BLEND_EQUATION_RGB = 0x8009;   /* same as BLEND_EQUATION */
        public const GLenum BLEND_EQUATION_ALPHA = 0x883D;

        /* BlendSubtract */
        public const GLenum FUNC_SUBTRACT = 0x800A;
        public const GLenum FUNC_REVERSE_SUBTRACT = 0x800B;

        /* Separate Blend Functions */
        public const GLenum BLEND_DST_RGB = 0x80C8;
        public const GLenum BLEND_SRC_RGB = 0x80C9;
        public const GLenum BLEND_DST_ALPHA = 0x80CA;
        public const GLenum BLEND_SRC_ALPHA = 0x80CB;
        public const GLenum CONSTANT_COLOR = 0x8001;
        public const GLenum ONE_MINUS_CONSTANT_COLOR = 0x8002;
        public const GLenum CONSTANT_ALPHA = 0x8003;
        public const GLenum ONE_MINUS_CONSTANT_ALPHA = 0x8004;
        public const GLenum BLEND_COLOR = 0x8005;

        /* Buffer Objects */
        public const GLenum ARRAY_BUFFER = 0x8892;
        public const GLenum ELEMENT_ARRAY_BUFFER = 0x8893;
        public const GLenum ARRAY_BUFFER_BINDING = 0x8894;
        public const GLenum ELEMENT_ARRAY_BUFFER_BINDING = 0x8895;

        public const GLenum STREAM_DRAW = 0x88E0;
        public const GLenum STATIC_DRAW = 0x88E4;
        public const GLenum DYNAMIC_DRAW = 0x88E8;

        public const GLenum BUFFER_SIZE = 0x8764;
        public const GLenum BUFFER_USAGE = 0x8765;

        public const GLenum CURRENT_VERTEX_ATTRIB = 0x8626;

        /* CullFaceMode */
        public const GLenum FRONT = 0x0404;
        public const GLenum BACK = 0x0405;
        public const GLenum FRONT_AND_BACK = 0x0408;

        /* DepthFunction */
        /*      NEVER */
        /*      LESS */
        /*      EQUAL */
        /*      LEQUAL */
        /*      GREATER */
        /*      NOTEQUAL */
        /*      GEQUAL */
        /*      ALWAYS */

        /* EnableCap */
        public const GLenum TEXTURE_2D = 0x0DE1;
        public const GLenum CULL_FACE = 0x0B44;
        public const GLenum BLEND = 0x0BE2;
        public const GLenum DITHER = 0x0BD0;
        public const GLenum STENCIL_TEST = 0x0B90;
        public const GLenum DEPTH_TEST = 0x0B71;
        public const GLenum SCISSOR_TEST = 0x0C11;
        public const GLenum POLYGON_OFFSET_FILL = 0x8037;
        public const GLenum SAMPLE_ALPHA_TO_COVERAGE = 0x809E;
        public const GLenum SAMPLE_COVERAGE = 0x80A0;

        /* ErrorCode */
        public const GLenum NO_ERROR = 0;
        public const GLenum INVALID_ENUM = 0x0500;
        public const GLenum INVALID_VALUE = 0x0501;
        public const GLenum INVALID_OPERATION = 0x0502;
        public const GLenum OUT_OF_MEMORY = 0x0505;

        /* FrontFaceDirection */
        public const GLenum CW = 0x0900;
        public const GLenum CCW = 0x0901;

        /* GetPName */
        public const GLenum LINE_WIDTH = 0x0B21;
        public const GLenum ALIASED_POINT_SIZE_RANGE = 0x846D;
        public const GLenum ALIASED_LINE_WIDTH_RANGE = 0x846E;
        public const GLenum CULL_FACE_MODE = 0x0B45;
        public const GLenum FRONT_FACE = 0x0B46;
        public const GLenum DEPTH_RANGE = 0x0B70;
        public const GLenum DEPTH_WRITEMASK = 0x0B72;
        public const GLenum DEPTH_CLEAR_VALUE = 0x0B73;
        public const GLenum DEPTH_FUNC = 0x0B74;
        public const GLenum STENCIL_CLEAR_VALUE = 0x0B91;
        public const GLenum STENCIL_FUNC = 0x0B92;
        public const GLenum STENCIL_FAIL = 0x0B94;
        public const GLenum STENCIL_PASS_DEPTH_FAIL = 0x0B95;
        public const GLenum STENCIL_PASS_DEPTH_PASS = 0x0B96;
        public const GLenum STENCIL_REF = 0x0B97;
        public const GLenum STENCIL_VALUE_MASK = 0x0B93;
        public const GLenum STENCIL_WRITEMASK = 0x0B98;
        public const GLenum STENCIL_BACK_FUNC = 0x8800;
        public const GLenum STENCIL_BACK_FAIL = 0x8801;
        public const GLenum STENCIL_BACK_PASS_DEPTH_FAIL = 0x8802;
        public const GLenum STENCIL_BACK_PASS_DEPTH_PASS = 0x8803;
        public const GLenum STENCIL_BACK_REF = 0x8CA3;
        public const GLenum STENCIL_BACK_VALUE_MASK = 0x8CA4;
        public const GLenum STENCIL_BACK_WRITEMASK = 0x8CA5;
        public const GLenum VIEWPORT = 0x0BA2;
        public const GLenum SCISSOR_BOX = 0x0C10;
        /*      SCISSOR_TEST */
        public const GLenum COLOR_CLEAR_VALUE = 0x0C22;
        public const GLenum COLOR_WRITEMASK = 0x0C23;
        public const GLenum UNPACK_ALIGNMENT = 0x0CF5;
        public const GLenum PACK_ALIGNMENT = 0x0D05;
        public const GLenum MAX_TEXTURE_SIZE = 0x0D33;
        public const GLenum MAX_VIEWPORT_DIMS = 0x0D3A;
        public const GLenum SUBPIXEL_BITS = 0x0D50;
        public const GLenum RED_BITS = 0x0D52;
        public const GLenum GREEN_BITS = 0x0D53;
        public const GLenum BLUE_BITS = 0x0D54;
        public const GLenum ALPHA_BITS = 0x0D55;
        public const GLenum DEPTH_BITS = 0x0D56;
        public const GLenum STENCIL_BITS = 0x0D57;
        public const GLenum POLYGON_OFFSET_UNITS = 0x2A00;
        /*      POLYGON_OFFSET_FILL */
        public const GLenum POLYGON_OFFSET_FACTOR = 0x8038;
        public const GLenum TEXTURE_BINDING_2D = 0x8069;
        public const GLenum SAMPLE_BUFFERS = 0x80A8;
        public const GLenum SAMPLES = 0x80A9;
        public const GLenum SAMPLE_COVERAGE_VALUE = 0x80AA;
        public const GLenum SAMPLE_COVERAGE_INVERT = 0x80AB;

        /* GetTextureParameter */
        /*      TEXTURE_MAG_FILTER */
        /*      TEXTURE_MIN_FILTER */
        /*      TEXTURE_WRAP_S */
        /*      TEXTURE_WRAP_T */

        public const GLenum NUM_COMPRESSED_TEXTURE_FORMATS = 0x86A2;
        public const GLenum COMPRESSED_TEXTURE_FORMATS = 0x86A3;

        /* HintMode */
        public const GLenum DONT_CARE = 0x1100;
        public const GLenum FASTEST = 0x1101;
        public const GLenum NICEST = 0x1102;

        /* HintTarget */
        public const GLenum GENERATE_MIPMAP_HINT = 0x8192;

        /* DataType */
        public const GLenum BYTE = 0x1400;
        public const GLenum UNSIGNED_BYTE = 0x1401;
        public const GLenum SHORT = 0x1402;
        public const GLenum UNSIGNED_SHORT = 0x1403;
        public const GLenum INT = 0x1404;
        public const GLenum UNSIGNED_INT = 0x1405;
        public const GLenum FLOAT = 0x1406;

        /* PixelFormat */
        public const GLenum DEPTH_COMPONENT = 0x1902;
        public const GLenum ALPHA = 0x1906;
        public const GLenum RGB = 0x1907;
        public const GLenum RGBA = 0x1908;
        public const GLenum LUMINANCE = 0x1909;
        public const GLenum LUMINANCE_ALPHA = 0x190A;

        /* PixelType */
        /*      UNSIGNED_BYTE */
        public const GLenum UNSIGNED_SHORT_4_4_4_4 = 0x8033;
        public const GLenum UNSIGNED_SHORT_5_5_5_1 = 0x8034;
        public const GLenum UNSIGNED_SHORT_5_6_5 = 0x8363;

        /* Shaders */
        public const GLenum FRAGMENT_SHADER = 0x8B30;
        public const GLenum VERTEX_SHADER = 0x8B31;
        public const GLenum MAX_VERTEX_ATTRIBS = 0x8869;
        public const GLenum MAX_VERTEX_UNIFORM_VECTORS = 0x8DFB;
        public const GLenum MAX_VARYING_VECTORS = 0x8DFC;
        public const GLenum MAX_COMBINED_TEXTURE_IMAGE_UNITS = 0x8B4D;
        public const GLenum MAX_VERTEX_TEXTURE_IMAGE_UNITS = 0x8B4C;
        public const GLenum MAX_TEXTURE_IMAGE_UNITS = 0x8872;
        public const GLenum MAX_FRAGMENT_UNIFORM_VECTORS = 0x8DFD;
        public const GLenum SHADER_TYPE = 0x8B4F;
        public const GLenum DELETE_STATUS = 0x8B80;
        public const GLenum LINK_STATUS = 0x8B82;
        public const GLenum VALIDATE_STATUS = 0x8B83;
        public const GLenum ATTACHED_SHADERS = 0x8B85;
        public const GLenum ACTIVE_UNIFORMS = 0x8B86;
        public const GLenum ACTIVE_UNIFORM_MAX_LENGTH = 0x8B87;
        public const GLenum ACTIVE_ATTRIBUTES = 0x8B89;
        public const GLenum ACTIVE_ATTRIBUTE_MAX_LENGTH = 0x8B8A;
        public const GLenum SHADING_LANGUAGE_VERSION = 0x8B8C;
        public const GLenum CURRENT_PROGRAM = 0x8B8D;

        /* StencilFunction */
        public const GLenum NEVER = 0x0200;
        public const GLenum LESS = 0x0201;
        public const GLenum EQUAL = 0x0202;
        public const GLenum LEQUAL = 0x0203;
        public const GLenum GREATER = 0x0204;
        public const GLenum NOTEQUAL = 0x0205;
        public const GLenum GEQUAL = 0x0206;
        public const GLenum ALWAYS = 0x0207;

        /* StencilOp */
        /*      ZERO */
        public const GLenum KEEP = 0x1E00;
        public const GLenum REPLACE = 0x1E01;
        public const GLenum INCR = 0x1E02;
        public const GLenum DECR = 0x1E03;
        public const GLenum INVERT = 0x150A;
        public const GLenum INCR_WRAP = 0x8507;
        public const GLenum DECR_WRAP = 0x8508;

        /* StringName */
        public const GLenum VENDOR = 0x1F00;
        public const GLenum RENDERER = 0x1F01;
        public const GLenum VERSION = 0x1F02;

        /* TextureMagFilter */
        public const GLenum NEAREST = 0x2600;
        public const GLenum LINEAR = 0x2601;

        /* TextureMinFilter */
        /*      NEAREST */
        /*      LINEAR */
        public const GLenum NEAREST_MIPMAP_NEAREST = 0x2700;
        public const GLenum LINEAR_MIPMAP_NEAREST = 0x2701;
        public const GLenum NEAREST_MIPMAP_LINEAR = 0x2702;
        public const GLenum LINEAR_MIPMAP_LINEAR = 0x2703;

        /* TextureParameterName */
        public const GLenum TEXTURE_MAG_FILTER = 0x2800;
        public const GLenum TEXTURE_MIN_FILTER = 0x2801;
        public const GLenum TEXTURE_WRAP_S = 0x2802;
        public const GLenum TEXTURE_WRAP_T = 0x2803;

        /* TextureTarget */
        /*      TEXTURE_2D */
        public const GLenum TEXTURE = 0x1702;

        public const GLenum TEXTURE_CUBE_MAP = 0x8513;
        public const GLenum TEXTURE_BINDING_CUBE_MAP = 0x8514;
        public const GLenum TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515;
        public const GLenum TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516;
        public const GLenum TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517;
        public const GLenum TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518;
        public const GLenum TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519;
        public const GLenum TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A;
        public const GLenum MAX_CUBE_MAP_TEXTURE_SIZE = 0x851C;

        /* TextureUnit */
        public const GLenum TEXTURE0 = 0x84C0;
        public const GLenum TEXTURE1 = 0x84C1;
        public const GLenum TEXTURE2 = 0x84C2;
        public const GLenum TEXTURE3 = 0x84C3;
        public const GLenum TEXTURE4 = 0x84C4;
        public const GLenum TEXTURE5 = 0x84C5;
        public const GLenum TEXTURE6 = 0x84C6;
        public const GLenum TEXTURE7 = 0x84C7;
        public const GLenum TEXTURE8 = 0x84C8;
        public const GLenum TEXTURE9 = 0x84C9;
        public const GLenum TEXTURE10 = 0x84CA;
        public const GLenum TEXTURE11 = 0x84CB;
        public const GLenum TEXTURE12 = 0x84CC;
        public const GLenum TEXTURE13 = 0x84CD;
        public const GLenum TEXTURE14 = 0x84CE;
        public const GLenum TEXTURE15 = 0x84CF;
        public const GLenum TEXTURE16 = 0x84D0;
        public const GLenum TEXTURE17 = 0x84D1;
        public const GLenum TEXTURE18 = 0x84D2;
        public const GLenum TEXTURE19 = 0x84D3;
        public const GLenum TEXTURE20 = 0x84D4;
        public const GLenum TEXTURE21 = 0x84D5;
        public const GLenum TEXTURE22 = 0x84D6;
        public const GLenum TEXTURE23 = 0x84D7;
        public const GLenum TEXTURE24 = 0x84D8;
        public const GLenum TEXTURE25 = 0x84D9;
        public const GLenum TEXTURE26 = 0x84DA;
        public const GLenum TEXTURE27 = 0x84DB;
        public const GLenum TEXTURE28 = 0x84DC;
        public const GLenum TEXTURE29 = 0x84DD;
        public const GLenum TEXTURE30 = 0x84DE;
        public const GLenum TEXTURE31 = 0x84DF;
        public const GLenum ACTIVE_TEXTURE = 0x84E0;

        /* TextureWrapMode */
        public const GLenum REPEAT = 0x2901;
        public const GLenum CLAMP_TO_EDGE = 0x812F;
        public const GLenum MIRRORED_REPEAT = 0x8370;

        /* Uniform Types */
        public const GLenum FLOAT_VEC2 = 0x8B50;
        public const GLenum FLOAT_VEC3 = 0x8B51;
        public const GLenum FLOAT_VEC4 = 0x8B52;
        public const GLenum INT_VEC2 = 0x8B53;
        public const GLenum INT_VEC3 = 0x8B54;
        public const GLenum INT_VEC4 = 0x8B55;
        public const GLenum BOOL = 0x8B56;
        public const GLenum BOOL_VEC2 = 0x8B57;
        public const GLenum BOOL_VEC3 = 0x8B58;
        public const GLenum BOOL_VEC4 = 0x8B59;
        public const GLenum FLOAT_MAT2 = 0x8B5A;
        public const GLenum FLOAT_MAT3 = 0x8B5B;
        public const GLenum FLOAT_MAT4 = 0x8B5C;
        public const GLenum SAMPLER_2D = 0x8B5E;
        public const GLenum SAMPLER_CUBE = 0x8B60;

        /* Vertex Arrays */
        public const GLenum VERTEX_ATTRIB_ARRAY_ENABLED = 0x8622;
        public const GLenum VERTEX_ATTRIB_ARRAY_SIZE = 0x8623;
        public const GLenum VERTEX_ATTRIB_ARRAY_STRIDE = 0x8624;
        public const GLenum VERTEX_ATTRIB_ARRAY_TYPE = 0x8625;
        public const GLenum VERTEX_ATTRIB_ARRAY_NORMALIZED = 0x886A;
        public const GLenum VERTEX_ATTRIB_ARRAY_POINTER = 0x8645;
        public const GLenum VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = 0x889F;

        /* Shader Source */
        public const GLenum COMPILE_STATUS = 0x8B81;
        public const GLenum INFO_LOG_LENGTH = 0x8B84;
        public const GLenum SHADER_SOURCE_LENGTH = 0x8B88;

        /* Shader Precision-Specified Types */
        public const GLenum LOW_FLOAT = 0x8DF0;
        public const GLenum MEDIUM_FLOAT = 0x8DF1;
        public const GLenum HIGH_FLOAT = 0x8DF2;
        public const GLenum LOW_INT = 0x8DF3;
        public const GLenum MEDIUM_INT = 0x8DF4;
        public const GLenum HIGH_INT = 0x8DF5;

        /* Framebuffer Object. */
        public const GLenum FRAMEBUFFER = 0x8D40;
        public const GLenum RENDERBUFFER = 0x8D41;

        public const GLenum RGBA4 = 0x8056;
        public const GLenum RGB5_A1 = 0x8057;
        public const GLenum RGB565 = 0x8D62;
        public const GLenum DEPTH_COMPONENT16 = 0x81A5;
        public const GLenum STENCIL_INDEX = 0x1901;
        public const GLenum STENCIL_INDEX8 = 0x8D48;
        public const GLenum DEPTH_STENCIL = 0x84F9;

        public const GLenum RENDERBUFFER_WIDTH = 0x8D42;
        public const GLenum RENDERBUFFER_HEIGHT = 0x8D43;
        public const GLenum RENDERBUFFER_INTERNAL_FORMAT = 0x8D44;
        public const GLenum RENDERBUFFER_RED_SIZE = 0x8D50;
        public const GLenum RENDERBUFFER_GREEN_SIZE = 0x8D51;
        public const GLenum RENDERBUFFER_BLUE_SIZE = 0x8D52;
        public const GLenum RENDERBUFFER_ALPHA_SIZE = 0x8D53;
        public const GLenum RENDERBUFFER_DEPTH_SIZE = 0x8D54;
        public const GLenum RENDERBUFFER_STENCIL_SIZE = 0x8D55;

        public const GLenum FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x8CD0;
        public const GLenum FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x8CD1;
        public const GLenum FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x8CD2;
        public const GLenum FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x8CD3;

        public const GLenum COLOR_ATTACHMENT0 = 0x8CE0;
        public const GLenum DEPTH_ATTACHMENT = 0x8D00;
        public const GLenum STENCIL_ATTACHMENT = 0x8D20;
        public const GLenum DEPTH_STENCIL_ATTACHMENT = 0x821A;

        public const GLenum NONE = 0;

        public const GLenum FRAMEBUFFER_COMPLETE = 0x8CD5;
        public const GLenum FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x8CD6;
        public const GLenum FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x8CD7;
        public const GLenum FRAMEBUFFER_INCOMPLETE_DIMENSIONS = 0x8CD9;
        public const GLenum FRAMEBUFFER_UNSUPPORTED = 0x8CDD;

        public const GLenum FRAMEBUFFER_BINDING = 0x8CA6;
        public const GLenum RENDERBUFFER_BINDING = 0x8CA7;
        public const GLenum MAX_RENDERBUFFER_SIZE = 0x84E8;

        public const GLenum INVALID_FRAMEBUFFER_OPERATION = 0x0506;

        /* WebGL-specific enums */
        public const GLenum UNPACK_FLIP_Y_WEBGL = 0x9240;
        public const GLenum UNPACK_PREMULTIPLY_ALPHA_WEBGL = 0x9241;
        public const GLenum CONTEXT_LOST_WEBGL = 0x9242;
        public const GLenum UNPACK_COLORSPACE_CONVERSION_WEBGL = 0x9243;
        public const GLenum BROWSER_DEFAULT_WEBGL = 0x9244;
        #endregion
    }
}
