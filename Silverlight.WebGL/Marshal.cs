using System;
using System.Linq;

namespace System.Runtime.InteropServices
{
    internal class SafeMarshal
    {
#if SILVERLIGHT
        public static int SizeOf(Type type) {
            switch (Type.GetTypeCode(type)) {
            case TypeCode.Boolean: return 1;
            case TypeCode.Byte: return 1;
            case TypeCode.Char: return 2;
            case TypeCode.Decimal: return 16;
            case TypeCode.Double: return 8;
            case TypeCode.Int16: return 2;
            case TypeCode.Int32: return 4;
            case TypeCode.Int64: return 8;
            case TypeCode.Single: return 4;
            case TypeCode.UInt16: return 2;
            case TypeCode.UInt32: return 4;
            case TypeCode.UInt64: return 8;
            case TypeCode.Object:
                return type.GetFields().Sum(x => SizeOf(x.FieldType));
            }
            return 0;
        }
#else
        public static int SizeOf(Type type) {
            return Marshal.SizeOf(type);
        }
#endif
    }
}
