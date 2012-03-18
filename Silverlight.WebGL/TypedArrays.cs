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
            var elementType = typeof(T);
            var elementSize = elementType.SizeOf();
            var length = array.Length * elementSize;
            var memory = new byte[length];
            using (var bw = new BinaryWriter(new MemoryStream(memory))) {
                foreach (var element in array) {
                    elementType.WriteTo(bw, element);
                }
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

    public static class TypeExtensionMethod
    {
        public static int SizeOf(this Type type) {
            switch (Type.GetTypeCode(type)) {
            case TypeCode.Boolean:  return 1;
            case TypeCode.Byte:     return 1;
            case TypeCode.Char:     return 2;
            case TypeCode.Decimal:  return 16;
            case TypeCode.Double:   return 8;
            case TypeCode.Int16:    return 2;
            case TypeCode.Int32:    return 4;
            case TypeCode.Int64:    return 8;
            case TypeCode.Single:   return 4;
            case TypeCode.UInt16:   return 2;
            case TypeCode.UInt32:   return 4;
            case TypeCode.UInt64:   return 8;
            case TypeCode.Object:
                return type.GetFields().Sum(x => x.FieldType.SizeOf());
            }
            return 0;
        }

        public static void WriteTo(this Type type, BinaryWriter bw, object v) {
            switch (Type.GetTypeCode(type)) {
            case TypeCode.Boolean: bw.Write((bool)v); break;
            case TypeCode.Byte: bw.Write((byte)v); break;
            case TypeCode.Char: bw.Write((char)v); break;
            case TypeCode.Decimal: // BinaryWriter.Write(Decimal) not supported in Silverlight
                foreach (var i in Decimal.GetBits((decimal)v)) {
                    bw.Write(i);
                }
                break;
            case TypeCode.Double: bw.Write((double)v); break;
            case TypeCode.Int16: bw.Write((Int16)v); break;
            case TypeCode.Int32: bw.Write((Int32)v); break;
            case TypeCode.Int64: bw.Write((Int64)v); break;
            case TypeCode.Single: bw.Write((Single)v); break;
            case TypeCode.UInt16: bw.Write((UInt16)v); break;
            case TypeCode.UInt32: bw.Write((UInt32)v); break;
            case TypeCode.UInt64: bw.Write((UInt64)v); break;
            case TypeCode.Object:
                foreach (var field in type.GetFields()) {
                    var fv = field.GetValue(v);
                    field.GetType().WriteTo(bw, fv);
                }
                break;
            }
        }
    }
}
