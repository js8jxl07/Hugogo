using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using Hugogo.Model;
using Hugogo.Model.ExternalModel;

namespace Hugogo.Common
{
    /// <summary>
    /// 常用的验证和转换方法，包括字符串是否为空，是否是数字等等
    /// </summary>
    public static class ValidateHelper
    {
        #region 字符串验证

        /// <summary>
        /// 验证是否是空或null字符串
        /// 如果是空或null则返回true,strRealString为null或string.Empty
        /// 否则返回false,strRealString为经过Trim操作的String;
        /// </summary>
        /// <param name="strSource">待查看的string</param>
        /// <param name="strRealString">经过Trim操作的string</param>
        /// <returns>
        /// 如果是空或null则返回true,strRealString为null或string.Empty
        /// 否则返回false,strRealString为经过Trim操作的String;
        /// </returns>
        public static bool IsNullOrEmptyString(string strSource, out string strRealString)
        {
            strRealString = null;
            if (strSource == null)
                return true;
            strRealString = strSource.Trim();
            if (strRealString == string.Empty)
                return true;
            return false;
        }

        /// <summary>
        /// 验证是否是空或null字符串
        /// 如果是空或null则返回true,否则返回false
        /// </summary>
        /// <param name="strSource">待查看的string</param>
        /// <returns>
        /// 如果是空或null则返回true,否则返回false
        /// </returns>
        public static bool IsNullOrEmptyString(string strSource)
        {
            if (strSource == null)
                return true;
            if (strSource.Trim() == string.Empty)
                return true;
            return false;
        }

        /// <summary>
        /// DataRow的value或从数据库中取出的Object型数据验证,验证取出的object是否是DBNull,空或null]
        /// 如果是DBNull,null或空字符串则返回true
        /// </summary>
        /// <param name="objSource">待验证的object</param>
        /// <returns>
        /// 如果是DBNull,null或空字符串则返回true
        /// </returns>
        public static bool IsDBNullOrNullOrEmptyString(object objSource)
        {
            if ((objSource == DBNull.Value) || (objSource == null))
                return true;
            string strSource = objSource.ToString();
            if (strSource.Trim() == string.Empty)
                return true;
            return false;
        }

        /// <summary>
        /// Url限制只能小写字母
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public static bool IsUrlEnWords(string words)
        {
            if (words == null)
                return false;
            return Regex.IsMatch(words, "^([a-z0-9]*)$");
        }

        #endregion

        #region 字符串操作

        /// <summary>
        /// 截取指定字符串长度（自动区分中英文） (add by LiYundong at 2010-1-14)
        /// </summary>
        /// <param name="stringToSub">待截取的字符串</param>
        /// <param name="length">需要截取的长度</param>
        /// <param name="endstring">如果截断则显示的字符（例如：。。。）</param>
        /// <returns></returns>
        public static string GetSubString(string stringToSub, int length, string endstring)
        {
            if (!string.IsNullOrEmpty(stringToSub))
            {
                Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
                Regex regexq = new Regex("[^\x00-\xff]+", RegexOptions.Compiled);
                char[] stringChar = stringToSub.ToCharArray();
                StringBuilder sb = new StringBuilder();
                int nLength = 0;
                bool isCut = false;
                for (int i = 0; i < stringChar.Length; i++)
                {
                    if (regex.IsMatch((stringChar[i]).ToString()) || regexq.IsMatch((stringChar[i]).ToString()))
                    {
                        sb.Append(stringChar[i]);
                        nLength += 2;
                    }
                    else
                    {
                        sb.Append(stringChar[i]);
                        nLength = nLength + 1;
                    }

                    if (nLength > length)
                    {
                        isCut = true;
                        break;
                    }
                }
                if (isCut)
                    return sb.ToString() + endstring;
                else
                    return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 从左边截取字符串 (add by LiYundong at 2010-1-14)
        /// </summary>
        /// <param name="s">待截取的字符串</param>
        /// <param name="len">要截取的长度</param>
        /// <returns></returns>
        public static string Left(string s, int len)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            else
            {
                if (s.Length <= len)
                    return s;
                else
                    return s.Substring(0, len);
            }
        }

        /// <summary>
        /// 获取最终字符串（排除DBNull，null，string.Empty 或空值后的真实值） 
        /// 注：如果为DBNull，null，string.Empty 或空值，则返回string.Empty (add by LiYundong at 2010-1-14)
        /// </summary>
        /// <param name="objString"></param>
        /// <returns></returns>
        public static string FinalString(object objString)
        {
            if (!IsDBNullOrNullOrEmptyString(objString))
                return objString.ToString();
            else
                return string.Empty;
        }

        /// <summary>
        /// 获取最终字符串（排除null，string.Empty 或空值后的真实值）
        /// 注：如果为DBNull，null，string.Empty 或空值，则返回string.Empty (add by LiYundong at 2010-1-14)
        /// </summary>
        /// <param name="objString"></param>
        /// <returns></returns>
        public static string FinalString(string objString)
        {
            if (!IsNullOrEmptyString(objString))
                return objString;
            else
                return string.Empty;
        }

        /// <summary>
        /// 截取客户端指定长度的字符串并对输入的字符串验证 (add by LiYundong at 2010-1-15)
        /// </summary>
        /// <param name="text">客户端输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns>清理后的字符串</returns>
        public static string ClearInputText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            text = Regex.Replace(text, "[\\s]{2,}", " "); //移除两个以上的空格
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //移除Br
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); //移除&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty); //移除其他一些标志
            text = text.Replace("'", "''"); //防止注入
            return text;
        }

        /// <summary>
        /// 客户端输入字符串验证  (add by LiYundong at 2010-1-15)
        /// </summary>
        /// <param name="text">客户端输入字符串</param>
        /// <returns>清理后的字符串</returns>
        public static string ClearInputText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = Regex.Replace(text, "[\\s]{2,}", " "); //移除两个以上的空格
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //移除Br
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); //移除&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty); //移除其他一些标志
            text = text.Replace("'", "''"); //防止注入
            return text;
        }

        /// <summary>
        /// 检测是否有Sql危险字符 (add by LiYundong at 2010-1-15)
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 替换SQL危险字符 (add by LiYundong at 2010-1-15)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string SafeSql(string s)
        {
            string str = string.Empty;
            if (s != null)
            {
                str = s.Replace("'", "''");
            }
            return str.Trim();
        }

        /// <summary>
        /// 将字符串转换成List<T>
        /// </summary>
        /// <typeparam name="T">类型，目前只支持int,string</typeparam>
        /// <param name="str">要转换的字符串</param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string str, string split)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] arr = str.Split(new char[] { Convert.ToChar(split) });
                if (typeof(T) == typeof(int))
                {
                    //int[] arri = Array.ConvertAll(arr, new Converter<string, int>(StrToInt));
                    int[] arrInt = Array.ConvertAll<string, int>(arr, int.Parse);
                    List<int> list = new List<int>(arrInt);
                    return list as List<T>;
                }
                else if (typeof(T) == typeof(string))
                {
                    List<string> list = new List<string>(arr);
                    return list as List<T>;
                }
                else
                {
                    return new List<T>();
                }
            }
            else
            {
                return new List<T>();
            }
        }

        /// <summary>
        /// 截取指定字符的左/右边部分
        /// </summary>
        /// <param name="str">待截取字符串</param>
        /// <param name="ident">指定的字符标识</param>
        /// <param name="position">字符右边/左边    true左边  false 右边</param>
        /// <returns></returns>
        public static string AppointStr(string str, char ident, bool position)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            var index = str.IndexOf(ident);
            var len = str.Length;
            if (index <= -1) return string.Empty;
            if (position)
            {
                return str.Substring(0, index);
            }
            else
            {
                return str.Substring(index + 1, len - index - 1);
            }
        }

        #endregion

        #region 数字验证

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="strNum">待测试的字符串</param>
        /// <returns>是则返回true,否则返回false</returns>
        public static bool IsNumber(string strNum)
        {
            if (strNum == null)
                return false;
            return Regex.IsMatch(strNum.Trim(), "^(0|[1-9][0-9]*)$");
        }

        #endregion

        #region 汉字验证

        /// <summary>
        /// 是否是中文
        /// </summary>
        /// <param name="strWords">待测试的字符串</param>
        /// <returns>是则返回true;否则返回false</returns>
        public static bool IsChineseWord(string strWords)
        {
            if (strWords == null)
                return false;
            bool bResult = true;
            foreach (char charWord in strWords)
            {
                if (!Regex.IsMatch(charWord.ToString(), "[\u4e00-\u9fa5]+"))
                {
                    bResult = false;
                    break;
                }
            }

            return bResult;
        }

        #endregion

        #region 整型验证和转换方法

        /// <summary>
        /// 是否是整数,是则返回true,否则返回false
        /// </summary>
        /// <param name="strNum">待测试的字符串</param>
        /// <returns></returns>
        public static bool IsInteger(string strNum)
        {
            if (strNum == null)
                return false;
            return Regex.IsMatch(strNum.Trim(), @"^-?\d+$");
        }

        /// <summary>
        /// 是否是正整数
        /// </summary>
        /// <param name="strNum">待测试的字符串</param>
        /// <param name="bIncludeZero">true:含0；false:不含0</param>
        /// <returns></returns>
        public static bool IsPositiveInteger(string strNum, bool bIncludeZero)
        {
            if (strNum == null)
                return false;
            if (bIncludeZero) //0也认为是正整数
                return Regex.IsMatch(strNum.Trim(), @"^\d+$");
            else
                return Regex.IsMatch(strNum.Trim(), "^[0-9]*[1-9][0-9]*$");
        }

        /// <summary>
        /// 是否是负整数
        /// </summary>
        /// <param name="strNum">待测试的字符串</param>
        /// <param name="bIncludeZero">true:含0；false:不含0</param>
        /// <returns></returns>
        public static bool IsNegativeInteger(string strNum, bool bIncludeZero)
        {
            if (strNum == null)
                return false;
            if (bIncludeZero) //0也认为是负整数
                return Regex.IsMatch(strNum.Trim(), @"^((-\d+)|(0+))$");
            else
                return Regex.IsMatch(strNum.Trim(), "^-[0-9]*[1-9][0-9]*$");
        }

        /// <summary>
        /// 转换成整型数字
        /// 转换失败则返回由nDefault指定的数字
        /// 转换成功则返回真实转换的数字
        /// </summary>
        /// <param name="objInfo">待转换的对象</param>
        /// <param name="nDefault">指定传回的默认值</param>
        /// <returns>转换失败则返回由nDefault指定的数字;转换成功则返回真实转换的数字</returns>
        public static int ToInt(object objInfo, int nDefault)
        {
            return ToInt(objInfo, nDefault, NumberStyles.Number);
        }

        /// <summary>
        /// 转换成整型数字,样式由numStyle指定
        /// 转换失败则返回由nDefault指定的数字
        /// 转换成功则返回真实转换的数字
        /// </summary>
        /// <param name="objInfo">待转换的对象</param>
        /// <param name="nDefault">指定传回的默认值</param>
        /// <returns>转换失败则返回由nDefault指定的数字;转换成功则返回真实转换的数字</returns>
        public static int ToInt(object objInfo, int nDefault, NumberStyles numStyle)
        {
            if (objInfo == null)
                return nDefault;
            return ToInt(objInfo.ToString(), nDefault, numStyle);
        }

        /// <summary>
        /// 转换成整型数字，
        /// 转换失败则返回由nDefault指定的数字
        /// 转换成功则返回真实转换的数字
        /// </summary>
        /// <param name="strInfo">待转换的字符串</param>
        /// <param name="nDefault">指定传回的默认值</param>
        /// <returns>转换失败则返回由nDefault指定的数字;转换成功则返回真实转换的数字</returns>
        /// <remarks>
        /// edited by 王纪虎 at 2011-8-16 for 修改转换类型为Number，避免负数转换失败
        /// </remarks>
        public static int ToInt(string strInfo, int nDefault)
        {
            return ToInt(strInfo, nDefault, NumberStyles.Number);
        }

        /// <summary>
        /// 转换成整型数字，数字样式由numStyle指定
        /// 转换失败则返回由nDefault指定的数字
        /// 转换成功则返回真实转换的数字
        /// </summary>
        /// <param name="strInfo">待转换的字符串</param>
        /// <param name="nDefault">指定传回的默认值</param>
        /// <returns>转换失败则返回由nDefault指定的数字;转换成功则返回真实转换的数字</returns>
        public static int ToInt(string strInfo, int nDefault, NumberStyles numStyle)
        {
            string strRealInfo = null;
            if (IsNullOrEmptyString(strInfo, out strRealInfo))
                return nDefault;
            int nResult = 0;
            if (int.TryParse(strRealInfo, numStyle, null, out nResult))
                return nResult;
            else
                return nDefault;
        }

        #endregion

        #region 浮点验证和转换方法

        /// <summary>
        /// 是否是浮点数
        /// 如果满足浮点数格式则返回true;否则返回false
        /// </summary>
        /// <returns></returns>
        public static bool IsDecimal(string strFloatNum)
        {
            if (strFloatNum == null)
                return false;
            return Regex.IsMatch(strFloatNum.Trim(), @"^(-?\d+)(\.\d+)?$");
        }

        /// <summary>
        /// 是否是正浮点数
        /// </summary>
        /// <param name="strFloatNum">待测试字符串</param>
        /// <param name="bInculudeZero">true:含0；false:不含0</param>
        /// <returns></returns>
        public static bool IsPositiveDecimal(string strFloatNum, bool bInculudeZero)
        {
            if (strFloatNum == null)
                return false;
            if (bInculudeZero) //0也认为是正浮点数
                return Regex.IsMatch(strFloatNum.Trim(), @"^\d+(\.\d+)?$");
            else
                return Regex.IsMatch(strFloatNum.Trim(), @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$");
        }

        /// <summary>
        /// 是否是负浮点数
        /// </summary>
        /// <param name="strFloatNum">待测试字符串</param>
        /// <param name="bIncludeZero">true:含0；false:不含0</param>
        /// <returns>如果是</returns>
        public static bool IsNegativeDecimal(string strFloatNum, bool bIncludeZero)
        {
            if (strFloatNum == null)
                return false;
            if (bIncludeZero) //0也认为是负浮点数
                return Regex.IsMatch(strFloatNum.Trim(), @"^((-\d+(\.\d+)?)|(0+(\.0+)?))$");
            else
                return Regex.IsMatch(strFloatNum.Trim(), @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$");
        }

        /// <summary>
        /// 转换成浮点数，
        /// 转换失败则返回由dDefault指定的数字
        /// 转换成功则返回真实转换的数字
        /// </summary>
        /// <param name="objFloat">待转换的对象</param>
        /// <param name="dDefault">指定传回的默认值</param>
        /// <returns>转换失败则返回由dDefault指定的数字;转换成功则返回真实转换的数字</returns>
        public static decimal ToDecimal(object objFloat, decimal dDefault)
        {
            return ToDecimal(objFloat, dDefault, NumberStyles.None);
        }

        /// <summary>
        /// 转换成浮点数,样式由numStyle指定
        /// 转换失败则返回由dDefault指定的数字
        /// 转换成功则返回真实转换的数字
        /// </summary>
        /// <param name="objFloat"></param>
        /// <param name="dDefault"></param>
        /// <param name="numStyle"></param>
        /// <returns></returns>
        public static decimal ToDecimal(object objFloat, decimal dDefault, NumberStyles numStyle)
        {
            if (objFloat == null)
                return dDefault;
            return ToDecimal(objFloat.ToString(), dDefault, numStyle);
        }

        /// <summary>
        /// 转换成浮点数,样式由numStyle指定
        /// 转换失败则返回由dDefault指定的数字
        /// 转换成功则返回真实转换的数字 
        /// </summary>
        /// <param name="strFloat"></param>
        /// <param name="dDefault"></param>
        /// <returns></returns>
        public static decimal ToDecimal(string strFloat, decimal dDefault)
        {
            return ToDecimal(strFloat, dDefault, NumberStyles.Float);
        }

        /// <summary>
        /// 转换成浮点数,样式由numStyle指定
        /// 转换失败则返回由dDefault指定的数字
        /// 转换成功则返回真实转换的数字
        /// </summary>
        /// <param name="strFloat"></param>
        /// <param name="dDefault"></param>
        /// <param name="numStyle"></param>
        /// <returns></returns>
        public static decimal ToDecimal(string strFloat, decimal dDefault, NumberStyles numStyle)
        {
            string strRealFloat = null;
            if (IsNullOrEmptyString(strFloat, out strRealFloat))
                return dDefault;
            decimal dResult = 0.0m;
            if (decimal.TryParse(strRealFloat, numStyle, null, out dResult))
                return dResult;
            else

                return dDefault;
        }

        #endregion

        #region 日期验证和转换方法

        /// <summary>
        /// 是否是日期类型
        /// </summary>
        /// <returns></returns>
        public static bool IsDateTime(string strDateValue)
        {
            string strRealValue = null;
            if (IsNullOrEmptyString(strDateValue, out strRealValue))
                return false;
            DateTime dtDate = DateTime.MinValue;
            return DateTime.TryParse(strRealValue, out dtDate);
        }

        /// <summary>
        /// 转换成日期类型
        /// 转换失败则返回由dtDefault指定的日期
        /// 转换成功则返回真实日期
        /// </summary>
        /// <param name="objDateValue">待转换的对象</param>
        /// <param name="dtDefault">指定传回的默认值</param>
        /// <returns>转换失败则返回由dtDefault指定的日期;转换成功则返回真实日期</returns>
        public static DateTime ToDateTime(object objDateValue, DateTime dtDefault)
        {
            if (objDateValue == null)
                return dtDefault;
            return ToDateTime(objDateValue.ToString(), dtDefault);
        }

        /// <summary>
        /// 转换成日期类型
        /// 转换失败则返回由dtDefault指定的日期
        /// 转换成功则返回真实日期 
        /// </summary>
        /// <param name="strDateValue">待转换的字符串</param>
        /// <param name="dtDefault">指定传回的默认值</param>
        /// <returns>转换失败则返回由dtDefault指定的日期;转换成功则返回真实日期 </returns>
        public static DateTime ToDateTime(string strDateValue, DateTime dtDefault)
        {
            string strRealDate;
            if (IsNullOrEmptyString(strDateValue, out strRealDate))
                return dtDefault;
            DateTime dtResult;
            return DateTime.TryParse(strRealDate, out dtResult) ? dtResult : dtDefault;
        }

        /// <summary>
        /// 将一个日期集合转换成日期段集合key为开始/Value为结束
        /// </summary>
        /// <param name="dateList"></param>
        /// <param name="isAddEnd">是否在结束日期上加1，默认是要加（主要是应用于酒店）</param>
        /// <returns></returns>
        public static Dictionary<DateTime, DateTime> ConvertDateListToSegment(List<DateTime> dateList, bool isAddEnd = true)
        {
            Dictionary<DateTime, DateTime> dateSegment = new Dictionary<DateTime, DateTime>();
            int i = 0;
            DateTime startDate = DateTime.MinValue;
            dateList = dateList.Select(time => time.Date).Distinct().OrderBy(time => time).ToList();
            foreach (var dateTime in dateList)
            {
                if (startDate.AddDays(i) != dateTime)
                {
                    i = 0;
                }
                if (i == 0)
                {
                    startDate = dateTime;
                }
                dateSegment[startDate] = dateTime;
                i++;
            }
            if (isAddEnd)
            {
                dateSegment = dateSegment.ToDictionary(pair => pair.Key, pair => pair.Value.AddDays(1));
            }
            return dateSegment;
        }

        #endregion
        #region 值类型泛型数据转换方法

        /// <summary>
        /// 值类型泛型数据转换方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="defaultValue">若转换失败，则返回该默认值</param>
        /// <returns></returns>
        public static T ConvertType<T>(object val, T defaultValue = default (T))
        {
            try
            {
                return (T)Convert.ChangeType(val, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region EMail验证

        /// <summary>
        /// 是否是电子邮件
        /// 如果满足电邮格式则返回tue,否则返回false
        /// </summary>
        /// <param name="strMail">邮件地址</param>
        /// <returns>如果满足电邮格式则返回tue,否则返回false</returns>
        public static bool IsEMail(string strMail)
        {
            if (strMail == null)
                return false;
            return Regex.IsMatch(strMail.Trim(), @"^([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\-|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$");
        }

        #endregion

        #region 手机验证

        /// <summary>
        /// 是否是手机号
        /// </summary>
        /// <param name="strMobile">待测试手机号字符串</param>
        /// <returns>是手机格式就返回true;否则返回false</returns>
        public static bool IsMobile(string strMobile)
        {
            if (strMobile == null)
                return false;
            return Regex.IsMatch(strMobile.Trim(), @"^(((\(\d{2,3}\))|(\d{3}\-))?(13\d{9})|(15\d{9})|(17\d{9})|(18\d{9})|(147\d{8}))$");
        }

        #endregion

        #region 电话号码验证

        /// <summary>
        /// 是否是电话号码
        /// </summary>
        /// <param name="strPhone"></param>
        /// <returns>是电话格式就返回true;否则返回false</returns>
        public static bool IsPhone(string strPhone)
        {
            if (strPhone == null)
                return false;
            return Regex.IsMatch(strPhone.Trim(), @"^(\+86\s{1,1})?((\d{3,4}\-)\d{7,8})$");
        }

        #endregion

        #region 身份证验证

        /// <summary>
        /// 是否是身份证
        /// </summary>
        /// <param name="strCardNo"></param>
        /// <returns></returns>
        public static bool IsIdentityCard(string strCardNo)
        {
            if (strCardNo == null)
                return false;
            return Regex.IsMatch(strCardNo.Trim(), @"^d{15}|d{}18$");
        }

        /// <summary>
        /// 验证身份证号码
        /// </summary>
        /// <param name="Id">身份证号码</param>
        /// <returns>验证成功为True，否则为False</returns>
        public static bool CheckIDCard(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                return false;
            }
            if (Id.Length == 18)
            {
                bool check = CheckIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = CheckIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 验证15位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false; //数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false; //省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false; //生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false; //校验码验证
            }
            return true; //符合GB11643-1999标准
        }

        /// <summary>
        /// 验证18位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false; //数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false; //省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false; //生日验证
            }
            return true; //符合15位身份证标准
        }

        #endregion

        #region 从身份证中获取生日

        /// <summary>
        ///从身份证中获取生日
        /// </summary>
        /// <param name="strCardNo">身份证号</param>
        /// <returns>无效返回DateTime.MinValue</returns>
        /// 创建人：顾李忠
        /// 2010-11-15 19:11
        public static DateTime GetBirthDayFromIDCard(string strCardNo)
        {
            //身份证号码已经包含了每个人的出生年月日及性别等方面的信息
            //    （对于老式的15位身份证而言，7-12位即个人的出生年月日，而最后一位奇数或偶数则分别表示男性或女性。
            //如某人的身份证号码为130226760904098，它的7-12位为760904，这就表示此人是1976年9月4日出生的，
            //身份证的最后一位为偶数8，这就表示此人为女性；
            //对于新式的18位身份证而言，7-14位代表个人的出身年月日，而倒数第二位的奇数或偶数则分别表示男性或女性

            strCardNo = strCardNo.Trim();
            if (strCardNo.Length == 18)
            {
                try
                {
                    var birthDay = DateTime.ParseExact(strCardNo.Substring(6, 8), "yyyyMMdd", CultureInfo.CurrentCulture);
                    return DateTime.Compare(birthDay, ConvertHelper.ToDateTime("1900-01-01")) < 0 ? ConvertHelper.ToDateTime("1900-01-01") : birthDay;
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            else if (strCardNo.Length == 15)
            {
                try
                {
                    return DateTime.ParseExact("19" + strCardNo.Substring(6, 6).ToString(), "yyyyMMdd", CultureInfo.CurrentCulture);
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            else
            {
                return DateTime.MinValue;
            }


        }

        #endregion

        #region 从身份证中获取性别

        /// <summary>
        /// 从身份证中获取性别 0：女 1：男 -1：获取错误
        /// </summary>
        /// <param name="strCardNo">身份证号</param>
        /// <returns></returns>
        /// 创建人：顾李忠
        /// 2010-12-3 11:16
        public static int GetSexFromIDCard(string strCardNo)
        {
            strCardNo = strCardNo.Trim();
            string sexCode = string.Empty;
            ////性别代码为偶数是女性奇数为男性
            if (strCardNo.Length == 18)
            {
                sexCode = strCardNo.Substring(16, 1);
            }
            else if (strCardNo.Length == 15)
            {
                sexCode = strCardNo.Substring(14, 1);
            }
            if (string.IsNullOrEmpty(sexCode))
            {
                return -1;
            }
            if (Common.ValidateHelper.ToInt(sexCode, -1) % 2 == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }

        }

        #endregion

        #region 从身份证中获取其他信息(性别、属性、生日等)

        /// <summary>
        /// 从身份证中获取其他信息(性别、属性、生日等)
        /// </summary>
        /// <param name="strCardNo">身份证号</param>
        /// <returns>GetIDCardDetailModel</returns>
        public static GetIDCardDetailModel GetDetailInfoFromIDCard(string strCardNo)
        {
            //先初始化，然后分别取各项的值
            var returnModel = new GetIDCardDetailModel();
            //请求参数验证
            if (string.IsNullOrWhiteSpace(strCardNo)) return returnModel;

            //是否是身份证
            returnModel.IsIDCard = CheckIDCard(strCardNo);
            if (!returnModel.IsIDCard) return returnModel;
            //获取生日
            DateTime birthDay = GetBirthDayFromIDCard(strCardNo);
            if (birthDay != DateTime.MinValue) returnModel.BirthDay = birthDay;
            //获取性别
            int customerSex = GetSexFromIDCard(strCardNo);
            if (customerSex > -1) returnModel.CustomerSex = (byte)customerSex;
            //获取游客属性
            returnModel.CustomerType = (byte)(birthDay != ConvertHelper.ToDateTime("1900-01-01") ? ((DateTime.Now.AddYears(-18) - birthDay.Date).Days >= 0 ? 1 : 2) : 0);
            return returnModel;
        }

        #endregion

        #region 是否是传真号

        /// <summary>
        /// 是否是传真号
        /// </summary>
        /// <param name="fax">传真号码</param>
        /// <returns></returns>
        public static bool IsFax(string fax)
        {



            if (string.IsNullOrEmpty(fax))
                return false;
            return Regex.IsMatch(fax.Trim(), @"^(\d{3,4}-)?\d{7,8}$");


        }

        #endregion

        #region 是否是邮箱

        /// <summary>
        /// 是否是邮箱
        /// </summary>
        /// <param name="mail">邮箱地址</param>
        /// <returns></returns>
        public static bool IsMail(string mail)
        {

            //string pattern = @"^[a-zA-Z0-9_\-]{1,}@[a-zA-Z0-9_\-]{1,}\.[a-zA-Z0-9_\-.]{1,}$";

            if (string.IsNullOrEmpty(mail))
                return false;
            return Regex.IsMatch(mail.Trim(), @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");


        }

        #endregion

        #region 获取两个时间差，相差不到一个月

        /// <summary>
        /// 获取两个时间差，相差不到一个月
        /// </summary>
        /// <param name="d1">结束时间</param>
        /// <param name="d2">开始时间</param>
        /// <returns>间隔描述</returns>
        public static int GetTimeSpan(DateTime d1, DateTime d2)
        {
            int seconds = 0;
            TimeSpan ts = d1.Subtract(d2).Duration();
            seconds = ts.Seconds + ts.Minutes * 60 + ts.Hours * 60 * 60 + ts.Days * 24 * 60 * 60;
            return seconds;
        }

        #endregion

        #region 验证以逗号隔开的字符串中是否有某一个字符

        /// <summary>
        /// 验证以逗号隔开的字符串中是否有某一个字符
        /// </summary>
        /// <param name="s">以逗号隔开的字符串</param>
        /// <param name="special">特殊字符</param>
        /// <returns></returns>
        public static bool IsContains(string s, string special)
        {
            bool isContain = false;
            string[] array = s.Split(',');
            for (int i = 0; i <= array.Length - 1; i++)
            {
                if (special == array[i])
                {
                    isContain = true;
                }
            }
            return isContain;
        }

        #endregion

        #region ROW_NUMBER()方式的分页ＳＱＬ

        /// <summary>
        /// ROW_NUMBER()方式的分页ＳＱＬ
        /// </summary>
        /// <param name="mainSql">分页主查询ＳＱＬ(必传不能为空)</param>
        /// <param name="orderBy">排序字段（可带ASC、DESC　如：Id DESC）(必传不能为空)</param>
        /// <param name="nMinId">最小值(必须大于等于０的)</param>
        /// <param name="nMaxId">最大值(必须大于０的)</param>
        /// <param name="otherSql">其它查询ＳＱＬ（如：查总记录数　SELECT COUNT(1) FROM table1 WITH(NOLOCK)）</param>
        /// <param name="sqlSummary">sql注释(必传不能为空)</param>
        /// <returns></returns>
        public static string GetSelectByPagsSql(string mainSql, string orderBy, int nMinId, int nMaxId, string otherSql, string sqlSummary)
        {
            if (string.IsNullOrEmpty(mainSql) || string.IsNullOrEmpty(orderBy) || string.IsNullOrEmpty(sqlSummary)
                || nMinId < 0 || nMaxId < 1)
                return "";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT  *
                            FROM    ( SELECT ROW_NUMBER() OVER ( ORDER BY temTab.{0} ) AS rowid,*
                                FROM    ( 
                                        {1}
                                        ) temTab
                                    ) tb
                            WHERE   tb.rowid > {2} AND tb.rowid < {3};", orderBy, mainSql, nMinId, nMaxId);
            if (!string.IsNullOrEmpty(otherSql))
            {
                sb.AppendFormat(" {0}; ", otherSql);
            }
            sb.AppendFormat(" {0}; ", sqlSummary);
            return sb.ToString();
        }

        #endregion

        #region 获取xml节点值

        /// <summary>
        /// 获取xml节点值,处理不存在节点,返回string.Empty
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetXmlNodeText(XmlNode node)
        {
            if (node != null)
                return node.InnerText;
            else
                return string.Empty;
        }

        #endregion

        #region 判断是否是儿童

        /// <summary>
        /// 判断是否是儿童  如果是180天-17周岁，属于儿童
        /// </summary>
        /// <param name="idCard">身份证号</param>
        /// <returns>1:是儿童，0：是成人，-1：身份证号验证失败</returns>
        public static int CheckIsChildren(string idCard)
        {
            if (string.IsNullOrEmpty(idCard) || !CheckIDCard(idCard))
            {
                return -1;
            }
            DateTime birth = GetBirthDayFromIDCard(idCard);
            if (DateTime.Now > birth.AddDays(180) && DateTime.Now <= birth.AddYears(17))
            {
                return 1;
            }
            return 0;
        }

        #endregion

    }
}
