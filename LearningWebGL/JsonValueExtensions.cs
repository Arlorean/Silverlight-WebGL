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
using System.Json;

namespace LearningWebGL
{
    public static class JsonValueExtensions
    {
        public static T ReadAsType<T>(this JsonValue jValue) {
            return (T) ReadAsType(jValue, typeof(T));
        }

        public static object ReadAsType(this JsonValue jValue, Type type) {
            switch (Type.GetTypeCode(type)) {
            case TypeCode.Boolean: return (Boolean)jValue;
            case TypeCode.Byte: return (Byte)jValue;
            case TypeCode.Char: return (Char)jValue;
            case TypeCode.DateTime: return (DateTime)jValue;
            case TypeCode.Decimal: return (Decimal)jValue;
            case TypeCode.Double: return (Double)jValue;
            case TypeCode.Int16: return (Int16)jValue;
            case TypeCode.Int32: return (Int32)jValue;
            case TypeCode.Int64: return (Int64)jValue;
            case TypeCode.SByte: return (SByte)jValue;
            case TypeCode.Single: return (Single)jValue;
            case TypeCode.String: return (Single)jValue;
            case TypeCode.UInt16: return (UInt16)jValue;
            case TypeCode.UInt32: return (UInt32)jValue;
            case TypeCode.UInt64: return (UInt64)jValue;

            default:
                if (type.IsArray) {
                    var elementType = type.GetElementType();
                    var jArray = (JsonArray)jValue;
                    var tArray = Array.CreateInstance(elementType, jArray.Count); ;
                    for (var i = 0; i < tArray.Length; ++i) {
                        tArray.SetValue(ReadAsType(jArray[i], elementType), i);
                    }
                    return tArray;
                }

                throw new NotSupportedException("JsonValueExtensions.ReadAsType<" + type.Name + "> not supported");
            }
        }
    }
}
