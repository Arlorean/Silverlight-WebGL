/**
 * @fileoverview gl-matrix - High performance matrix and vector operations for WebGL
 * @author Brandon Jones
 * @version 1.2.4
 */

/*
 * Copyright (c) 2011 Brandon Jones
 *
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 *    1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 *
 *    2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 *
 *    3. This notice may not be removed or altered from any source
 *    distribution.
 */    

/*
 *  Partially ported from original "gl-matrix.js" JavaScript to C#
 *     by Adam Davidson 12/March/2012
 *     
 * see https://github.com/toji/gl-matrix for original source
 */

using System;

using GLfloat = System.Single;

namespace GlMatrix
{
    public class mat4
    {
        GLfloat[] m = new GLfloat[16];

        public GLfloat this[int i] {
            get { return m[i]; }
            set { m[i] = value;  }
        }

        public GLfloat[] ToArray() { return m; }

        public static mat4 create() {
            return new mat4();
        }

        public static mat4 identity(mat4 dest=null) {
            if (dest == null) { dest = mat4.create(); }
            dest[0] = 1;
            dest[1] = 0;
            dest[2] = 0;
            dest[3] = 0;
            dest[4] = 0;
            dest[5] = 1;
            dest[6] = 0;
            dest[7] = 0;
            dest[8] = 0;
            dest[9] = 0;
            dest[10] = 1;
            dest[11] = 0;
            dest[12] = 0;
            dest[13] = 0;
            dest[14] = 0;
            dest[15] = 1;
            return dest;
        }

        public static mat4 frustum(GLfloat left, GLfloat right, GLfloat bottom, GLfloat top, GLfloat near, GLfloat far, mat4 dest=null) {
            if (dest == null) { dest = mat4.create(); }
            var rl = (right - left);
            var tb = (top - bottom);
            var fn = (far - near);
            dest[0] = (near * 2) / rl;
            dest[1] = 0;
            dest[2] = 0;
            dest[3] = 0;
            dest[4] = 0;
            dest[5] = (near * 2) / tb;
            dest[6] = 0;
            dest[7] = 0;
            dest[8] = (right + left) / rl;
            dest[9] = (top + bottom) / tb;
            dest[10] = -(far + near) / fn;
            dest[11] = -1;
            dest[12] = 0;
            dest[13] = 0;
            dest[14] = -(far * near * 2) / fn;
            dest[15] = 0;
            return dest;
        }

        public static mat4 perspective(GLfloat fovy, GLfloat aspect, GLfloat near, GLfloat far, mat4 dest=null) {
            var top = near * (GLfloat)Math.Tan(fovy * Math.PI / 360.0);
            var right = top * aspect;
            return mat4.frustum(-right, right, -top, top, near, far, dest);
        }

        public static mat4 translate(mat4 mat, GLfloat[] vec, mat4 dest=null) {
            var x = vec[0]; var y = vec[1]; var z = vec[2];
            GLfloat a00; GLfloat a01; GLfloat a02; GLfloat a03;
            GLfloat a10; GLfloat a11; GLfloat a12; GLfloat a13;
            GLfloat a20; GLfloat a21; GLfloat a22; GLfloat a23;
             
            if (dest == null || ReferenceEquals(mat, dest)) {
                mat[12] = mat[0] * x + mat[4] * y + mat[8] * z + mat[12];
                mat[13] = mat[1] * x + mat[5] * y + mat[9] * z + mat[13];
                mat[14] = mat[2] * x + mat[6] * y + mat[10] * z + mat[14];
                mat[15] = mat[3] * x + mat[7] * y + mat[11] * z + mat[15];
                return mat;
            }
             
            a00 = mat[0]; a01 = mat[1]; a02 = mat[2]; a03 = mat[3];
            a10 = mat[4]; a11 = mat[5]; a12 = mat[6]; a13 = mat[7];
            a20 = mat[8]; a21 = mat[9]; a22 = mat[10]; a23 = mat[11];
             
            dest[0] = a00; dest[1] = a01; dest[2] = a02; dest[3] = a03;
            dest[4] = a10; dest[5] = a11; dest[6] = a12; dest[7] = a13;
            dest[8] = a20; dest[9] = a21; dest[10] = a22; dest[11] = a23;
             
            dest[12] = a00 * x + a10 * y + a20 * z + mat[12];
            dest[13] = a01 * x + a11 * y + a21 * z + mat[13];
            dest[14] = a02 * x + a12 * y + a22 * z + mat[14];
            dest[15] = a03 * x + a13 * y + a23 * z + mat[15];
            return dest;
        }

        internal static mat4 rotate(mat4 mat, double angle, GLfloat[] axis, mat4 dest=null) {
            GLfloat x = axis[0]; GLfloat y = axis[1]; GLfloat z = axis[2];
            GLfloat len = (GLfloat)Math.Sqrt(x * x + y * y + z * z);
            GLfloat s; GLfloat c; GLfloat t;
            GLfloat a00; GLfloat a01; GLfloat a02; GLfloat a03;
            GLfloat a10; GLfloat a11; GLfloat a12; GLfloat a13;
            GLfloat a20; GLfloat a21; GLfloat a22; GLfloat a23;
            GLfloat b00; GLfloat b01; GLfloat b02;
            GLfloat b10; GLfloat b11; GLfloat b12;
            GLfloat b20; GLfloat b21; GLfloat b22;

            if (len == 0) { return null; }
            if (len != 1) {
                len = 1 / len;
                x *= len;
                y *= len;
                z *= len;
            }

            s = (GLfloat)Math.Sin(angle);
            c = (GLfloat)Math.Cos(angle);
            t = 1 - c;

            a00 = mat[0]; a01 = mat[1]; a02 = mat[2]; a03 = mat[3];
            a10 = mat[4]; a11 = mat[5]; a12 = mat[6]; a13 = mat[7];
            a20 = mat[8]; a21 = mat[9]; a22 = mat[10]; a23 = mat[11];

            // Construct the elements of the rotation matrix
            b00 = x * x * t + c; b01 = y * x * t + z * s; b02 = z * x * t - y * s;
            b10 = x * y * t - z * s; b11 = y * y * t + c; b12 = z * y * t + x * s;
            b20 = x * z * t + y * s; b21 = y * z * t - x * s; b22 = z * z * t + c;

            if (dest == null) {
                dest = mat;
            } else if (!ReferenceEquals(mat, dest)) { // If the source and destination differ, copy the unchanged last row
                dest[12] = mat[12];
                dest[13] = mat[13];
                dest[14] = mat[14];
                dest[15] = mat[15];
            }

            // Perform rotation-specific matrix multiplication
            dest[0] = a00 * b00 + a10 * b01 + a20 * b02;
            dest[1] = a01 * b00 + a11 * b01 + a21 * b02;
            dest[2] = a02 * b00 + a12 * b01 + a22 * b02;
            dest[3] = a03 * b00 + a13 * b01 + a23 * b02;

            dest[4] = a00 * b10 + a10 * b11 + a20 * b12;
            dest[5] = a01 * b10 + a11 * b11 + a21 * b12;
            dest[6] = a02 * b10 + a12 * b11 + a22 * b12;
            dest[7] = a03 * b10 + a13 * b11 + a23 * b12;

            dest[8] = a00 * b20 + a10 * b21 + a20 * b22;
            dest[9] = a01 * b20 + a11 * b21 + a21 * b22;
            dest[10] = a02 * b20 + a12 * b21 + a22 * b22;
            dest[11] = a03 * b20 + a13 * b21 + a23 * b22;
            return dest;
        }

        internal static mat4 set(mat4 mat, mat4 dest) {
            dest[0] = mat[0];
            dest[1] = mat[1];
            dest[2] = mat[2];
            dest[3] = mat[3];
            dest[4] = mat[4];
            dest[5] = mat[5];
            dest[6] = mat[6];
            dest[7] = mat[7];
            dest[8] = mat[8];
            dest[9] = mat[9];
            dest[10] = mat[10];
            dest[11] = mat[11];
            dest[12] = mat[12];
            dest[13] = mat[13];
            dest[14] = mat[14];
            dest[15] = mat[15];
            return dest;
        }

        /**
         * Calculates the inverse of the upper 3x3 elements of a mat4 and copies the result into a mat3
         * The resulting matrix is useful for calculating transformed normals
         *
         * Params:
         * @param {mat4} mat mat4 containing values to invert and copy
         * @param {mat3} [dest] mat3 receiving values
         *
         * @returns {mat3} dest is specified, a new mat3 otherwise, null if the matrix cannot be inverted
         */
        public static mat3 toInverseMat3(mat4 mat, mat3 dest=null) {
            // Cache the matrix values (makes for huge speed increases!)
            var a00 = mat[0]; var a01 = mat[1]; var a02 = mat[2];
            var a10 = mat[4]; var a11 = mat[5]; var a12 = mat[6];
            var a20 = mat[8]; var a21 = mat[9]; var a22 = mat[10];

            var b01 = a22 * a11 - a12 * a21;
            var b11 = -a22 * a10 + a12 * a20;
            var b21 = a21 * a10 - a11 * a20;

            var d = a00 * b01 + a01 * b11 + a02 * b21;
            GLfloat id;

            if (d == 0) { return null; }
            id = 1 / d;

            if (dest == null) { dest = mat3.create(); }

            dest[0] = b01 * id;
            dest[1] = (-a22 * a01 + a02 * a21) * id;
            dest[2] = (a12 * a01 - a02 * a11) * id;
            dest[3] = b11 * id;
            dest[4] = (a22 * a00 - a02 * a20) * id;
            dest[5] = (-a12 * a00 + a02 * a10) * id;
            dest[6] = b21 * id;
            dest[7] = (-a21 * a00 + a01 * a20) * id;
            dest[8] = (a11 * a00 - a01 * a10) * id;

            return dest;
        }
    }

    public class mat3
    {
        GLfloat[] m = new GLfloat[9];

        public GLfloat this[int i] {
            get { return m[i]; }
            set { m[i] = value;  }
        }

        public GLfloat[] ToArray() { return m; }

        /**
         * Creates a new instance of a mat3 using the default array type
         * Any javascript array-like object containing at least 9 numeric elements can serve as a mat3
         *
         * @param {mat3} [mat] mat3 containing values to initialize with
         *
         * @returns {mat3} New mat3
         */
        public static mat3 create(mat3 mat = null) {
            var dest = new mat3();

            if (mat != null) {
                dest[0] = mat[0];
                dest[1] = mat[1];
                dest[2] = mat[2];
                dest[3] = mat[3];
                dest[4] = mat[4];
                dest[5] = mat[5];
                dest[6] = mat[6];
                dest[7] = mat[7];
                dest[8] = mat[8];
            }

            return dest;
        }

        /**
         * Copies the values of one mat3 to another
         *
         * @param {mat3} mat mat3 containing values to copy
         * @param {mat3} dest mat3 receiving copied values
         *
         * @returns {mat3} dest
         */
        public static mat3 set(mat3 mat, mat3 dest) {
            dest[0] = mat[0];
            dest[1] = mat[1];
            dest[2] = mat[2];
            dest[3] = mat[3];
            dest[4] = mat[4];
            dest[5] = mat[5];
            dest[6] = mat[6];
            dest[7] = mat[7];
            dest[8] = mat[8];
            return dest;
        }

        /**
         * Sets a mat3 to an identity matrix
         *
         * @param {mat3} dest mat3 to set
         *
         * @returns dest if specified, otherwise a new mat3
         */
        public static mat3 identity(mat3 dest) {
            if (dest == null) { dest = mat3.create(); }
            dest[0] = 1;
            dest[1] = 0;
            dest[2] = 0;
            dest[3] = 0;
            dest[4] = 1;
            dest[5] = 0;
            dest[6] = 0;
            dest[7] = 0;
            dest[8] = 1;
            return dest;
        }

        /**
         * Transposes a mat3 (flips the values over the diagonal)
         *
         * Params:
         * @param {mat3} mat mat3 to transpose
         * @param {mat3} [dest] mat3 receiving transposed values. If not specified result is written to mat
         *
         * @returns {mat3} dest is specified, mat otherwise
         */
        public static mat3 transpose(mat3 mat, mat3 dest=null) {
            // If we are transposing ourselves we can skip a few steps but have to cache some values
            if (dest == null || ReferenceEquals(mat, dest)) {
                var a01 = mat[1]; var a02 = mat[2];
                var a12 = mat[5];

                mat[1] = mat[3];
                mat[2] = mat[6];
                mat[3] = a01;
                mat[5] = mat[7];
                mat[6] = a02;
                mat[7] = a12;
                return mat;
            }

            dest[0] = mat[0];
            dest[1] = mat[3];
            dest[2] = mat[6];
            dest[3] = mat[1];
            dest[4] = mat[4];
            dest[5] = mat[7];
            dest[6] = mat[2];
            dest[7] = mat[5];
            dest[8] = mat[8];
            return dest;
        }

        /**
         * Copies the elements of a mat3 into the upper 3x3 elements of a mat4
         *
         * @param {mat3} mat mat3 containing values to copy
         * @param {mat4} [dest] mat4 receiving copied values
         *
         * @returns {mat4} dest if specified, a new mat4 otherwise
         */
        mat4 toMat4(mat3 mat, mat4 dest) {
            if (dest == null) { dest = mat4.create(); }

            dest[15] = 1;
            dest[14] = 0;
            dest[13] = 0;
            dest[12] = 0;

            dest[11] = 0;
            dest[10] = mat[8];
            dest[9] = mat[7];
            dest[8] = mat[6];

            dest[7] = 0;
            dest[6] = mat[5];
            dest[5] = mat[4];
            dest[4] = mat[3];

            dest[3] = 0;
            dest[2] = mat[2];
            dest[1] = mat[1];
            dest[0] = mat[0];

            return dest;
        }
    }
}
