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

using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.IO;
using System.Runtime.InteropServices;

namespace Silverlight.Html
{
    public sealed class Uint8Array : TypedArray<byte> {
        public Uint8Array(ulong length) : base(length) { }
        public Uint8Array(byte[] array) : base(array) { }
        public const int BYTES_PER_ELEMENT = 1;
    }

    public sealed class Uint16Array : TypedArray<UInt16> {
        public Uint16Array(ulong length) : base(length) { }
        public Uint16Array(UInt16[] array) : base(array) { }
        public const int BYTES_PER_ELEMENT = 2;
    }

    public sealed class Float32Array : TypedArray<float>
    {
        public Float32Array(ulong length) : base(length) { }
        public Float32Array(float[] array) : base(array) { }
        public const int BYTES_PER_ELEMENT = 4;
    }

    public sealed class Float64Array : TypedArray<double>
    {
        public Float64Array(ulong length) : base(length) { }
        public Float64Array(double[] array) : base(array) { }
        public const int BYTES_PER_ELEMENT = 4;
    }

    public class TypedArray<T> : ArrayBufferView where T : struct
    {
        public TypedArray(ulong length) {
            Object = HtmlPage.Window.CreateInstance(GetType().Name, length);
        }

        public TypedArray(T[] array) {
            Object = HtmlPage.Window.CreateInstance(GetType().Name, array);
        }

        public TypedArray(ArrayBuffer arrayBuffer, ulong byteOffset=0, ulong length=0) {
            Object = HtmlPage.Window.CreateInstance(GetType().Name, arrayBuffer.Object, byteOffset, length);
        }

        public T get(ulong index) {
            return (T)Object.Invoke("get", index);
        }
    }

    public class ArrayBufferView
    {
        internal ScriptObject Object { get; set; }

        public ArrayBuffer buffer {
            get { return new ArrayBuffer(Object.GetProperty("buffer") as ScriptObject); }
        }

        public static ArrayBufferView FromArray<T>(T[] array) where T : struct {
            byte[] memory;
  
            var elementType = typeof(T);
            if (elementType == typeof(byte)) {
                memory = array as byte[];
            }
            else {
                var elementSize = SafeMarshal.SizeOf(elementType);
                var length = array.Length * elementSize;
                memory = new byte[length];
                Buffer.BlockCopy(array, 0, memory, 0, length);
            }
            return new Uint8Array(memory);
        }
    }

    public class ArrayBuffer
    {
        internal ScriptObject Object { get; set; }

        internal ArrayBuffer(ScriptObject arrayBuffer) {
            Object = arrayBuffer;
        }

        public ArrayBuffer(ulong length) {
            Object = HtmlPage.Window.CreateInstance(GetType().Name, length);
        }

        public static ArrayBuffer FromArray<T>(T[] array) where T : struct {
            var view = ArrayBufferView.FromArray(array);
            return view.buffer;
        }
    }
}
