using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hugogo.Model
{
    public static class ConvertHelper
    {
        // Methods
        public static T ConvertType<T>(object val)
        {
            if ((val != null) && (val != DBNull.Value))
            {
                Type type = typeof(T);
                if (type.IsGenericType)
                {
                    type = type.GetGenericArguments()[0];
                }
                if (type.Name.Equals("String"))
                {
                    return (T)val;
                }
                ParameterModifier[] modifiers = new ParameterModifier[] { new ParameterModifier(2) };
                MethodInfo info = type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder, new Type[] { typeof(string), type.MakeByRefType() }, modifiers);
                object[] parameters = new object[] { val, Activator.CreateInstance(type) };
                if ((bool)info.Invoke(null, parameters))
                {
                    return (T)parameters[1];
                }
            }
            return default(T);
        }

        public static bool ToBool(object val)
        {
            if ((val == null) || (val == DBNull.Value))
            {
                return false;
            }
            if (val is bool)
            {
                return (bool)val;
            }
            if (!(val.ToString().ToLower() == "true") && !(val.ToString().ToLower() == "1"))
            {
                return false;
            }
            return true;
        }

        public static byte ToByte(object val)
        {
            return ToByte(val, 0);
        }

        public static byte ToByte(object val, byte defaultValue)
        {
            byte num;
            if ((val == null) || (val == DBNull.Value))
            {
                return defaultValue;
            }
            if (val is byte)
            {
                return (byte)val;
            }
            if (!byte.TryParse(val.ToString(), out num))
            {
                return defaultValue;
            }
            return num;
        }

        public static byte? ToByteNullable(object val)
        {
            byte num = ToByte(val);
            if (num.Equals((byte)0))
            {
                return null;
            }
            return new byte?(num);
        }

        public static DateTime ToDateTime(object val)
        {
            DateTime time;
            if ((val == null) || (val == DBNull.Value))
            {
                return new DateTime(0x76c, 1, 1);
            }
            if (val is DateTime)
            {
                return (DateTime)val;
            }
            if (!DateTime.TryParse(val.ToString(), out time))
            {
                return new DateTime(0x76c, 1, 1);
            }
            return time;
        }

        public static DateTime? ToDateTimeNullable(object val)
        {
            DateTime time = ToDateTime(val);
            if (time.Equals(new DateTime(0x76c, 1, 1)))
            {
                return null;
            }
            return new DateTime?(time);
        }

        public static decimal ToDecimal(object val)
        {
            return ToDecimal(val, 0M, 2);
        }

        public static decimal ToDecimal(object val, int decimals)
        {
            return ToDecimal(val, 0M, decimals);
        }

        public static decimal ToDecimal(object val, decimal defaultValue, int decimals)
        {
            decimal num;
            if ((val == null) || (val == DBNull.Value))
            {
                return defaultValue;
            }
            if (val is decimal)
            {
                return Math.Round((decimal)val, decimals);
            }
            if (!decimal.TryParse(val.ToString(), out num))
            {
                return defaultValue;
            }
            return Math.Round(num, decimals);
        }

        public static decimal? ToDecimalNullable(object val)
        {
            decimal num = ToDecimal(val);
            if (num.Equals((decimal)0.0M))
            {
                return null;
            }
            return new decimal?(num);
        }

        public static double ToDouble(object val)
        {
            return ToDouble(val, 0.0, 2);
        }

        public static double ToDouble(object val, int digits)
        {
            return ToDouble(val, 0.0, digits);
        }

        public static double ToDouble(object val, double defaultValue, int digits)
        {
            double num;
            if ((val == null) || (val == DBNull.Value))
            {
                return defaultValue;
            }
            if (val is double)
            {
                return Math.Round((double)val, digits);
            }
            if (!double.TryParse(val.ToString(), out num))
            {
                return defaultValue;
            }
            return Math.Round(num, digits);
        }

        public static double? ToDoubleNullable(object val)
        {
            double num = ToDouble(val);
            if (num.Equals((double)0.0))
            {
                return null;
            }
            return new double?(num);
        }

        public static T ToEnum<T>(byte value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static T ToEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name, true);
        }

        public static float ToFloat(object val)
        {
            return ToFloat(val, 0f);
        }

        public static float ToFloat(object val, float defaultValue)
        {
            float num;
            if ((val == null) || (val == DBNull.Value))
            {
                return defaultValue;
            }
            if (val is float)
            {
                return (float)val;
            }
            if (!float.TryParse(val.ToString(), out num))
            {
                return defaultValue;
            }
            return num;
        }

        public static float? ToFloatNullable(object val)
        {
            float num = ToFloat(val);
            if (num.Equals((float)0f))
            {
                return null;
            }
            return new float?(num);
        }

        public static int ToInt(object val)
        {
            return ToInt(val, 0);
        }

        public static int ToInt(object val, int defaultValue)
        {
            int num;
            if ((val == null) || (val == DBNull.Value))
            {
                return defaultValue;
            }
            if (val is int)
            {
                return (int)val;
            }
            if (!int.TryParse(val.ToString(), out num))
            {
                return defaultValue;
            }
            return num;
        }

        public static int? ToIntNullable(object val)
        {
            int num = ToInt(val);
            if (num.Equals(0))
            {
                return null;
            }
            return new int?(num);
        }

        public static long ToLong(object val)
        {
            return ToLong(val, 0L);
        }

        public static long ToLong(object val, long defaultValue)
        {
            long num;
            if ((val == null) || (val == DBNull.Value))
            {
                return defaultValue;
            }
            if (val is long)
            {
                return (long)val;
            }
            if (!long.TryParse(val.ToString(), out num))
            {
                return defaultValue;
            }
            return num;
        }

        public static long? ToLongNullable(object val)
        {
            long num = ToLong(val);
            if (num.Equals((long)0L))
            {
                return null;
            }
            return new long?(num);
        }

        public static sbyte ToSbyte(object val)
        {
            return ToSbyte(val, 0);
        }

        public static sbyte ToSbyte(object val, sbyte defaultValue)
        {
            sbyte num;
            if ((val == null) || (val == DBNull.Value))
            {
                return defaultValue;
            }
            if (val is sbyte)
            {
                return (sbyte)val;
            }
            if (!sbyte.TryParse(val.ToString(), out num))
            {
                return defaultValue;
            }
            return num;
        }

        public static sbyte? ToSbyteNullable(object val)
        {
            sbyte num = ToSbyte(val);
            if (num.Equals((sbyte)0))
            {
                return null;
            }
            return new sbyte?(num);
        }

        public static short ToShort(object val)
        {
            return ToShort(val, 0);
        }

        public static short ToShort(object val, short defaultValue)
        {
            short num;
            if ((val == null) || (val == DBNull.Value))
            {
                return defaultValue;
            }
            if (val is short)
            {
                return (short)val;
            }
            if (!short.TryParse(val.ToString(), out num))
            {
                return defaultValue;
            }
            return num;
        }

        public static short? ToShortNullable(object val)
        {
            short num = ToShort(val);
            if (num.Equals((short)0))
            {
                return null;
            }
            return new short?(num);
        }

        public static string ToString(object val)
        {
            if ((val == null) || (val == DBNull.Value))
            {
                return string.Empty;
            }
            if (val.GetType() == typeof(byte[]))
            {
                return Encoding.ASCII.GetString((byte[])val, 0, ((byte[])val).Length);
            }
            return val.ToString();
        }

        public static string ToString(object val, string replace)
        {
            string str = ToString(val);
            if (!string.IsNullOrEmpty(str))
            {
                return str;
            }
            return replace;
        }

        public static string ToStringNullable(object val)
        {
            string str = ToString(val);
            if (!string.IsNullOrEmpty(str))
            {
                return str;
            }
            return null;
        }

        public static uint ToUint(object val)
        {
            return ToUint(val, 0);
        }

        public static uint ToUint(object val, uint defaultValue)
        {
            uint num;
            if ((val == null) || (val == DBNull.Value))
            {
                return defaultValue;
            }
            if (val is uint)
            {
                return (uint)val;
            }
            if (!uint.TryParse(val.ToString(), out num))
            {
                return defaultValue;
            }
            return num;
        }

        public static uint? ToUintNullable(object val)
        {
            uint num = ToUint(val);
            if (num.Equals((uint)0))
            {
                return null;
            }
            return new uint?(num);
        }

        public static ulong ToUlong(object val)
        {
            return ToUlong(val, 0L);
        }

        public static ulong ToUlong(object val, ulong defaultValue)
        {
            ulong num;
            if ((val == null) || (val == DBNull.Value))
            {
                return defaultValue;
            }
            if (val is long)
            {
                return (ulong)val;
            }
            if (!ulong.TryParse(val.ToString(), out num))
            {
                return defaultValue;
            }
            return num;
        }

        public static ulong? ToUlongNullable(object val)
        {
            ulong num = ToUlong(val);
            if (num.Equals((ulong)0L))
            {
                return null;
            }
            return new ulong?(num);
        }

        public static ushort ToUshort(object val)
        {
            return ToUshort(val, 0);
        }

        public static ushort ToUshort(object val, ushort defaultValue)
        {
            ushort num;
            if ((val == null) || (val == DBNull.Value))
            {
                return defaultValue;
            }
            if (val is ushort)
            {
                return (ushort)val;
            }
            if (!ushort.TryParse(val.ToString(), out num))
            {
                return defaultValue;
            }
            return num;
        }

        public static ushort? ToUshortNullable(object val)
        {
            ushort num = ToUshort(val);
            if (num.Equals((ushort)0))
            {
                return null;
            }
            return new ushort?(num);
        }

        public static bool TryToEnum<T>(byte value, out T parsed)
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                parsed = (T)Enum.ToObject(typeof(T), value);
                return true;
            }
            parsed = (T)Enum.Parse(typeof(T), Enum.GetNames(typeof(T))[0]);
            return false;
        }

        public static bool TryToEnum<T>(string name, out T parsed)
        {
            if (Enum.IsDefined(typeof(T), name))
            {
                parsed = (T)Enum.Parse(typeof(T), name, true);
                return true;
            }
            parsed = (T)Enum.Parse(typeof(T), Enum.GetNames(typeof(T))[0]);
            return false;
        }
    }
}
