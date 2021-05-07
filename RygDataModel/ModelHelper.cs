using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RygDataModel
{
    #region Enums
    /// <summary>
    /// The Database Entity to which the data relates.
    /// </summary>
    public enum DbEntity
    {
        Incident,
        CivilServant,
        Institution,
        CalendarEvent,
        BlogPost,
        StateProvinceRegion,
        CountyArea,
        City,
        CommentInThread,
        Reply,
        CommentMention,
        OfficialRegister,
        Alert,
        IncidentCategory,
        Country,
        User,
        OfficialRegisterEntry,
        IncidentUpdate
    }

    /// <summary>
    /// The type of the subscription.
    /// </summary>
    public enum SubsType
    {
        DataEntity,
        Regional
    }

    /// <summary>
    /// The reason that registration was denied.
    /// </summary>
    public enum RegistrationDenyReason
    {
        LimitExceeded,
        RegisterByInvitationOnly,
        AllowOutOfCountryUserRegistration,
        UnderConstruction
    }

    /// <summary>
    /// The API authentication type to use.
    /// </summary>
    public enum ApiAuthType
    {
        AuthenticationToken,
        UserPassword
    }

    /// <summary>
    /// The data input format.
    /// </summary>
    public enum ReadDataType
    {
        Json,
        XML,
        Text
    }

    /// <summary>
    /// The type of Blog entity data.
    /// </summary>
    public enum BlogType
    {
        Blog,
        WebsiteFrontpageArticle,
        Newsletter
    }

    /// <summary>
    /// The online service to which the link points.
    /// </summary>
    public enum LinkType
    {
        YouTube,
        YouNow,
        NewsArticle,
        Blog,
        ChangeOrg,
        AvaazOrg,
        Reddit,
        Twitch,
        Other
    }

    /// <summary>
    /// The type of the calendar event.
    /// </summary>
    public enum CalendarEventType
    {
        Meetup,
        Demonstration,
        OnlineMeeting,
        OnlineStream
    }

    /// <summary>
    /// HexString: The hex character string separated by dashes, e.g. "42-2F-51-4F-79-32-50-2F-63...",  
    /// HexStringShort: The shortened hex character string, NOT separated by dashes, e.g. "422F514F7932502F63...",  
    /// Base64: The usual flavor of Base64,  
    /// Base64url: Base64 with URL/FileName safe characters [+ -> -, / -> _, no padding (=)]. 
    /// UTF8: The standard Windows encoding type.
    /// UTF16: Unicode.
    /// UTF32: UTF32.
    /// ASCII: The basic old fasioned ASCII character set.
    /// Unicode: Synonym for UTF16.
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Base64#Base64_table"/>
    /// <see cref="https://en.wikipedia.org/wiki/Base64#RFC_4648"/>
    public enum StringEncodingType
    {
        HexString,
        HexStringShort,
        Base64,
        Base64url,
        UTF8,
        UTF16,
        UTF32,
        ASCII,
        Unicode
    }
    #endregion

    public static class ModelHelper
    {

        /// <summary>
        ///(I)ncident [guid]; civil s(E)rvant [guid]; instituti(O)n [guid]; (C)alendar event [guid]; (B)log post [guid]; (S)tateprovinceregion [int]; 
        ///county(A)rea [int]; ci(T)ty [int]; comme(N)tthread [int]; (R)eply [int]; (M)ention [int]; o(F)ficial register [guid];  official re[G]ister entry [guid]; 
        ///a(L)ert [guid]; incidentcategor(Y) [guid]; co(U)ntry [iso]; u(Z)er [guid]; incident u(P)date [guid]
        /// </summary>
        public static string StringDbEntity(DbEntity dbEntityEnum)
        {
            return dbEntityEnum switch
            {
                DbEntity.Incident => "I",//guid
                DbEntity.CivilServant => "E",//guid
                DbEntity.Institution => "O",//guid
                DbEntity.CalendarEvent => "C",//guid
                DbEntity.BlogPost => "B",//guid
                DbEntity.StateProvinceRegion => "S",//int
                DbEntity.CountyArea => "A",//int
                DbEntity.City => "T",//int
                DbEntity.CommentInThread => "N",//int
                DbEntity.Reply => "R",//int
                DbEntity.CommentMention => "M",//int
                DbEntity.OfficialRegister => "F",//guid
                DbEntity.Alert => "L",//guid
                DbEntity.IncidentCategory => "Y",//guid
                DbEntity.Country => "U",//iso
                DbEntity.User => "Z",//guid
                DbEntity.OfficialRegisterEntry => "G",//guid
                DbEntity.IncidentUpdate => "P",//guid
                _ => String.Empty,
            };
        }

        //(D)ata Entity; (R)egional 
        public static string StringSubsType(SubsType subsTypeEnum)
        {
            return subsTypeEnum switch
            {
                SubsType.DataEntity => "D",
                SubsType.Regional => "R",
                _ => String.Empty,
            };
        }

        //(L)imit exceeded; registerby(I)nvitationonly; allowoutof(C)ountryuserregistration; (U)nder construction
        public static string StringRegDenyType(RegistrationDenyReason denyTypeEnum)
        {
            return denyTypeEnum switch
            {
                RegistrationDenyReason.LimitExceeded => "L",
                RegistrationDenyReason.RegisterByInvitationOnly => "I",
                RegistrationDenyReason.AllowOutOfCountryUserRegistration => "C",
                RegistrationDenyReason.UnderConstruction => "U",
                _ => String.Empty,
            };
        }

        //authentication(T)oken; (U)serpassword
        public static string StringApiAuthType(ApiAuthType apiAuthTypeEnum)
        {
            return apiAuthTypeEnum switch
            {
                ApiAuthType.AuthenticationToken => "T",
                ApiAuthType.UserPassword => "U",
                _ => String.Empty,
            };
        }

        //(J)son; (X)ml; (T)ext
        public static string StringReadDataType(ReadDataType readDataTypeEnum)
        {
            return readDataTypeEnum switch
            {
                ReadDataType.Json => "J",
                ReadDataType.XML => "X",
                ReadDataType.Text => "T",
                _ => String.Empty,
            };
        }

        //(B)log; (W)ebsite front page article; (N)ewsletter
        public static string StringBlogType(BlogType blogTypeEnum)
        {
            return blogTypeEnum switch
            {
                BlogType.Blog => "B",
                BlogType.WebsiteFrontpageArticle => "W",
                BlogType.Newsletter => "N",
                _ => String.Empty,
            };
        }

        //(Y)outube; yo(U)now; (N)ewsarticle; (B)log; (Ch)ange.org; (A)vaaz.org; (R)eddit; (T)witch; (O)ther  
        public static string StringLinkType(LinkType linkTypeEnum)
        {
            return linkTypeEnum switch
            {
                LinkType.YouTube => "Y",
                LinkType.YouNow => "U",
                LinkType.NewsArticle => "N",
                LinkType.Blog => "B",
                LinkType.ChangeOrg => "C",
                LinkType.AvaazOrg => "A",
                LinkType.Reddit => "R",
                LinkType.Twitch => "T",
                LinkType.Other => "O",
                _ => String.Empty,
            };
        }

        //(M)eetup; (D)emonstration; Online meeting; online (S)tream
        public static string StringCalendarEventType(CalendarEventType calendarEventTypeEnum)
        {
            return calendarEventTypeEnum switch
            {
                CalendarEventType.Meetup => "M",
                CalendarEventType.Demonstration => "D",
                CalendarEventType.OnlineMeeting => "O",
                CalendarEventType.OnlineStream => "S",
                _ => String.Empty,
            };
        }

        #region String Manipulation Helpers
        /// <summary>
        /// Trim or Pad a given string using the requested method.
        /// </summary>
        /// <param name="textToTrimOrPad">The string to pad or trim.</param>
        /// <param name="maxLength">The maximum length of the output string, the string will be trimmed (or padded depending on the minLength value).</param>
        /// <param name="minLength">The minimum length of the output string, the string will be padded if the lengthis less than this value.</param>
        /// <param name="trimOrPadInString">The method for trimming or Padding.  Inserts or trims the value from inside the string.</param>
        /// <param name="trimOrPadLeft">The method for trimming or Padding.  Inserts or trims the value at the beginning of the string.</param>
        /// <param name="trimOrPadRight">The method for trimming or Padding.  Inserts or trims the value at the end of the string.</param>
        /// <param name="padRandom">The method for trimming or Padding.  Inserts or trims the characters randomly in the string.  DO NOT use for long term symmetric encryption as it can't be replicated for decryption..</param>
        /// <param name="padCharacters">If provided, use this set of characters for padding.  If omitted, a predefined set of characters will be used.</param>
        /// <returns></returns>
        public static string TrimOrPadText(string textToTrimOrPad,
                                           int maxLength,
                                           int minLength = 0,
                                           bool trimOrPadInString = true,
                                           bool trimOrPadLeft = false,
                                           bool trimOrPadRight = false,
                                           bool padRandom = false,
                                           string padCharacters = "")
        {
            string _resStr = textToTrimOrPad;

            StringBuilder _sb = new(textToTrimOrPad);
            if (_sb.Length > maxLength)
            {
                //Trim
                if (trimOrPadInString)
                {
                    int _rStr = (int)Math.Floor(((double)_sb.Length - (double)maxLength) / Math.Sqrt((double)_sb.Length - (double)maxLength));
                    _sb.Remove((_rStr < 0 ? _sb.Length - maxLength - 1 : _rStr), _sb.Length - maxLength);
                    _resStr = _sb.ToString();
                }
                else if (trimOrPadLeft)
                {
                    _resStr = textToTrimOrPad.Substring(0, maxLength);
                }
                else if (trimOrPadRight)
                {
                    _resStr = textToTrimOrPad.Substring(textToTrimOrPad.Length - maxLength - 1, maxLength);
                }
            }
            else if (_sb.Length < minLength)
            {
                //Pad
                if (padCharacters.Length == 0)
                {
                    if (textToTrimOrPad.Length < 4)
                    {
                        padCharacters = @"OBB¤cxHléLpBrßHVnj¼HeIV~F6Aq®EzP";
                    }
                    else
                    {
                        padCharacters = textToTrimOrPad;
                    }
                }

                if (padRandom)
                {
                    //Random padding insertion: DO NOT use for symetric long term encryption as it can't be replicated when decrypting
                    if (trimOrPadInString)
                    {
                        Random _rndFrom = new();
                        Random _rndTo = new((int)Math.Sqrt(int.MaxValue) + (DateTime.Now.Millisecond * 1000) + (DateTime.Now.Year - DateTime.Now.Minute) - (DateTime.Now.Hour * 10));
                        do
                        {
                            _sb.Insert(_rndTo.Next(0, _sb.Length - 1),
                                       padCharacters.Substring(_rndFrom.Next(0, padCharacters.Length - 1), 1));
                        } while (_sb.Length < maxLength);
                    }
                    else
                    {
                        Random _rndFrom = new();
                        do
                        {
                            if (trimOrPadRight)
                            {
                                _sb.Append(padCharacters.Substring(_rndFrom.Next(0, padCharacters.Length - 1), 1));
                            }
                            else
                            {
                                _sb.Insert(0, padCharacters.Substring(_rndFrom.Next(0, padCharacters.Length - 1), 1));
                            }
                        } while (_sb.Length < maxLength);
                    }
                }
                else
                {
                    //Fixed padding insertion: DO use for symetric long term encryption as it can be replicated when decrypting
                    StringBuilder _sbInsert = new();
                    do
                    {
                        _sbInsert.Append(padCharacters);
                    } while (_sbInsert.Length < maxLength - _sb.Length + 1);
                    if (_sb.Length > maxLength)
                    {
                        _sb.Remove(0, _sb.Length - maxLength);
                    }
                    if (trimOrPadInString)
                    {
                        int _rStr = (int)Math.Floor(((double)_sb.Length - (double)maxLength) / Math.Sqrt((double)_sb.Length - (double)maxLength));
                        _sb.Insert((_rStr < 0 ? _sb.Length - maxLength - 1 : _rStr), _sbInsert);
                    }
                    else
                    {
                        if (trimOrPadRight)
                        {
                            _sb.Append(_sbInsert);
                        }
                        else
                        {
                            _sb.Insert(0, _sbInsert);
                        }
                    }
                }
            }

            return _resStr;
        }
        #endregion

        #region Data Type Conversion Helpers
        /// <summary>
        /// Convert a byte array to a string in the StringEncodingType format requested.
        /// </summary>
        /// <param name="sourceByteArrayData">The byte array to convert.</param>
        /// <param name="stringOutputType">The StringEncodingType enum format in which to return the string.</param>
        /// <returns>A string encoded in the requested StringEncodingType enum format.</returns>
        public static string ConvertBytesToStringEncodingType(byte[] sourceByteArrayData,
                                                              StringEncodingType stringOutputType = StringEncodingType.Base64url)
        {

            return stringOutputType switch
            {
                StringEncodingType.Base64 => Convert.ToBase64String(sourceByteArrayData),
                StringEncodingType.Base64url => Base64UrlTextEncoder.Encode(sourceByteArrayData),
                StringEncodingType.HexString => ConvertByteArrayToHexString(sourceByteArrayData),
                StringEncodingType.HexStringShort => ConvertByteArrayToHexStringShort(sourceByteArrayData),
                StringEncodingType.UTF8 => ConvertByteArrayToUTF8(sourceByteArrayData),
                StringEncodingType.UTF16 => ConvertByteArrayToUnicode(sourceByteArrayData),
                StringEncodingType.UTF32 => ConvertByteArrayToUTF32(sourceByteArrayData),
                StringEncodingType.Unicode => ConvertByteArrayToUnicode(sourceByteArrayData),
                StringEncodingType.ASCII => ConvertByteArrayToASCII(sourceByteArrayData),
                _ => String.Empty,
            };
        }
        /// <summary>
        /// Convert a string in the specified StringEncodingType format to a byte array.
        /// </summary>
        /// <param name="sourceStringData">The string value to convert.</param>
        /// <param name="stringInputType">The StringEncodingType enum format in which the sourceStringData string is encoded.</param>
        /// <returns>The converted byte array.</returns>
        public static byte[] ConvertStringEncodingTypeToBytes(string sourceStringData,
                                                              StringEncodingType stringInputType = StringEncodingType.Base64url)
        {

            return stringInputType switch
            {
                StringEncodingType.Base64 => Convert.FromBase64String(sourceStringData),
                StringEncodingType.Base64url => Base64UrlTextEncoder.Decode(sourceStringData),
                StringEncodingType.HexString => ConvertByteArrayFromHexString(sourceStringData),
                StringEncodingType.HexStringShort => ConvertByteArrayFromHexStringShort(sourceStringData),
                StringEncodingType.UTF8 => ConvertByteArrayFromUTF8(sourceStringData),
                StringEncodingType.UTF16 => ConvertByteArrayFromUnicode(sourceStringData),
                StringEncodingType.UTF32 => ConvertByteArrayFromUTF32(sourceStringData),
                StringEncodingType.Unicode => ConvertByteArrayFromUnicode(sourceStringData),
                StringEncodingType.ASCII => ConvertByteArrayFromASCII(sourceStringData),
                _ => null,
            };
        }
        /// <summary>
        /// Convert string Encoding type.
        /// </summary>
        /// <param name="StringToConvert">String to voncert.</param>
        /// <param name="ConvertFromType">The StringEncodingType of the StringToConvert parameter string.</param>
        /// <param name="ConvertToType">The StringEncodingType to convert the StringToConvert parameter string to.</param>
        /// <returns>
        /// The converted string in the ConvertToType parameter encoding.
        /// </returns>
        public static string ConvertStringEncodingType(string StringToConvert,
                                                StringEncodingType ConvertFromType,
                                                StringEncodingType ConvertToType)
        {
            byte[] _bytes = ConvertStringEncodingTypeToBytes(StringToConvert, ConvertFromType);
            return ConvertBytesToStringEncodingType(_bytes, ConvertToType);
        }

        /// <summary>
        /// Converts a byte array to it's string hex representation.
        /// </summary>
        /// <remarks>
        /// Shown as two hex characters per byte (each pair represents the byte value of 0-255) separated by a dash (-) character. 
        /// </remarks>
        /// <param name="sourceByteArrayData">The byte array to convert.</param>
        /// <returns>The hex character string separated by dashes.  E.g. "42-2F-51-4F-79-32-50-2F-63..."</returns>
        public static string ConvertByteArrayToHexString(byte[] sourceByteArrayData)
        {

            if ((sourceByteArrayData == null) || (sourceByteArrayData?.Length == 0))
            {
                return string.Empty;
            }
            else
            {
                return BitConverter.ToString(sourceByteArrayData);
            }

        }
        /// <summary>
        /// Converts hex string to a byte array.
        /// </summary>
        /// <param name="sourceStringData">The BitConverter.ToString StringEncodingType.HexString value to convert back to bytes.  E.g. "42-2F-51-4F-79-32-50-2F-63..."</param>
        /// <returns>The converted byte array</returns>
        public static byte[] ConvertByteArrayFromHexString(string sourceStringData)
        {

            //if ((sourceStringData == null) || (sourceStringData.Length == 0) || (sourceStringData.Length % 2 > 0))
            if ((sourceStringData == null) || (sourceStringData?.Length < 2))
            {
                if ((sourceStringData?.Length > 0) && (sourceStringData?.Length != 2))
                {
                    throw new ApplicationException($"Hex string values must be 2 characters in length.  Invalid string value {sourceStringData} can not be converted.");
                }
                return null;
            }
            else
            {

                string[] _strArray = sourceStringData.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                if ((_strArray == null) || (_strArray?.Length == 0))
                {
                    return null;
                }
                else
                {

                    int strLen = _strArray.Length;
                    byte[] _returnBytes = new byte[strLen];
                    for (int i = 0; i < strLen; i++)
                    {
                        if (_strArray[i].ToString().Length != 2)
                        {
                            throw new ApplicationException($"Hex string value elements must be 2 characters in length.  Invalid value '{sourceStringData[i]}' encountered at array position {i} of string value {sourceStringData} and can not be converted.");
                        }
                        _returnBytes[i] = Convert.ToByte(_strArray[i].ToString(), 16);
                    }
                    return _returnBytes;

                }

            }

        }

        /// <summary>
        /// Converts a byte array to a shortened version of it's string hex representation.  
        /// </summary>
        /// <remarks>
        /// Shown as two hex characters per byte (each pair represents the byte value of 0-255), shortened by removal of the dash (-) separation character. 
        /// </remarks>
        /// <param name="sourceByteArrayData">The byte array to convert.</param>
        /// <returns>The string shortened hex character string, NOT separated by dashes.  E.g. "422F514F7932502F63..."</returns>
        public static string ConvertByteArrayToHexStringShort(byte[] sourceByteArrayData)
        {

            if ((sourceByteArrayData == null) || (sourceByteArrayData?.Length == 0))
            {
                return string.Empty;
            }
            else
            {
                return BitConverter.ToString(sourceByteArrayData).Replace("-", "");
            }

        }
        /// <summary>
        /// Converts short hex string to a byte array.
        /// </summary>
        /// <param name="sourceStringData">The StringEncodingType.HexStringShort value to convert back to bytes.  E.g. "422F514F7932502F63..."</param>
        /// <returns>The converted byte array</returns>
        public static byte[] ConvertByteArrayFromHexStringShort(string sourceStringData)
        {
            if ((sourceStringData == null) || (sourceStringData?.Length == 0) || (sourceStringData?.Length % 2 > 0))
            {
                if ((sourceStringData?.Length > 0) && (sourceStringData?.Length % 2 > 0))
                {
                    throw new ApplicationException($"Hex string values must be multiples of 2 characters in length.  Invalid string value {sourceStringData} can not be converted.");
                }
                return null;
            }
            else
            {
                int byteCount = sourceStringData.Length / 2;
                byte[] _returnBytes = new byte[byteCount];
                for (int i = 0; i < byteCount; i++)
                {
                    _returnBytes[i] = Convert.ToByte(sourceStringData.Substring(i * 2, 2), 16);
                }
                return _returnBytes;
            }
        }

        /// <summary>
        /// Converts a byte array to it's UTF8 string representation.
        /// </summary>
        /// <param name="sourceByteArrayData">The byte array to convert.</param>
        /// <returns>The UTF8 Encoded string.
        public static string ConvertByteArrayToUTF8(byte[] sourceByteArrayData)
        {

            if ((sourceByteArrayData == null) || (sourceByteArrayData?.Length == 0))
            {
                return string.Empty;
            }
            else
            {
                return Encoding.UTF8.GetString(sourceByteArrayData);
            }

        }
        /// <summary>
        /// Converts a UTF8 string to a byte array.
        /// </summary>
        /// <param name="sourceStringData">The StringEncodingType.UTF8 value to convert to bytes.</param>
        /// <returns>The converted byte array</returns>
        public static byte[] ConvertByteArrayFromUTF8(string sourceStringData)
        {

            if ((sourceStringData == null) || (sourceStringData?.Length == 0))
            {
                return null;
            }
            else
            {
                return Encoding.UTF8.GetBytes(sourceStringData);
            }

        }

        /// <summary>
        /// Converts a byte array to it's UTF32 string representation.
        /// </summary>
        /// <param name="sourceByteArrayData">The byte array to convert.</param>
        /// <returns>The UTF32 Encoded string.
        public static string ConvertByteArrayToUTF32(byte[] sourceByteArrayData)
        {

            if ((sourceByteArrayData == null) || (sourceByteArrayData?.Length == 0))
            {
                return string.Empty;
            }
            else
            {
                return Encoding.UTF32.GetString(sourceByteArrayData);
            }

        }
        /// <summary>
        /// Converts an UTF32 string to a byte array.
        /// </summary>
        /// <param name="sourceStringData">The StringEncodingType.UTF32 value to convert to bytes.</param>
        /// <returns>The converted byte array</returns>
        public static byte[] ConvertByteArrayFromUTF32(string sourceStringData)
        {

            if ((sourceStringData == null) || (sourceStringData?.Length == 0))
            {
                return null;
            }
            else
            {
                return Encoding.UTF32.GetBytes(sourceStringData);
            }

        }

        /// <summary>
        /// Converts a byte array to it's Unicode (UTF16) string representation.
        /// </summary>
        /// <param name="sourceByteArrayData">The byte array to convert.</param>
        /// <returns>The Unicode (UTF16) Encoded string.
        public static string ConvertByteArrayToUnicode(byte[] sourceByteArrayData)
        {

            if ((sourceByteArrayData == null) || (sourceByteArrayData?.Length == 0))
            {
                return string.Empty;
            }
            else
            {
                return Encoding.Unicode.GetString(sourceByteArrayData);
            }

        }
        /// <summary>
        /// Converts a Unicode (UTF16) string to a byte array.
        /// </summary>
        /// <param name="sourceStringData">The StringEncodingType.Unicode value to convert to bytes.</param>
        /// <returns>The converted byte array</returns>
        public static byte[] ConvertByteArrayFromUnicode(string sourceStringData)
        {

            if ((sourceStringData == null) || (sourceStringData?.Length == 0))
            {
                return null;
            }
            else
            {
                return Encoding.Unicode.GetBytes(sourceStringData);
            }

        }

        /// <summary>
        /// Converts a byte array to it's ASCII string representation.
        /// </summary>
        /// <param name="sourceByteArrayData">The byte array to convert.</param>
        /// <returns>The UTF8 Encoded string.
        public static string ConvertByteArrayToASCII(byte[] sourceByteArrayData)
        {

            if ((sourceByteArrayData == null) || (sourceByteArrayData?.Length == 0))
            {
                return string.Empty;
            }
            else
            {
                return Encoding.ASCII.GetString(sourceByteArrayData);
            }

        }
        /// <summary>
        /// Converts an ASCII string to a byte array.
        /// </summary>
        /// <param name="sourceStringData">The StringEncodingType.ASCII value to convert to bytes.</param>
        /// <returns>The converted byte array</returns>
        public static byte[] ConvertByteArrayFromASCII(string sourceStringData)
        {

            if ((sourceStringData == null) || (sourceStringData?.Length == 0))
            {
                return null;
            }
            else
            {
                return Encoding.ASCII.GetBytes(sourceStringData);
            }

        }
        #endregion

    }
}
