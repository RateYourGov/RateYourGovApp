using System;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel
{
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

    public enum SubsType
    {
        DataEntity,
        Regional
    }

    public enum RegistrationDenyReason
    {
        LimitExceeded,
        RegisterByInvitationOnly,
        AllowOutOfCountryUserRegistration,
        UnderConstruction
    }

    public enum ApiAuthType
    {
        AuthenticationToken,
        UserPassword
    }

    public enum ReadDataType
    {
        Json,
        XML,
        Text
    }

    public enum BlogType
    {
        Blog,
        WebsiteFrontpageArticle,
        Newsletter
    }

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
    /// </summary>
    /// <see cref="https://en.wikipedia.org/wiki/Base64#Base64_table"/>
    /// <see cref="https://en.wikipedia.org/wiki/Base64#RFC_4648"/>
    public enum StringEncodingType
    {
        HexString,
        HexStringShort,
        Base64,
        Base64url
    }


    public static class ModelHelper
    {

        /// <summary>
        ///(I)ncident [guid]; civil s(E)rvant [guid]; instituti(O)n [guid]; (C)alendar event [guid]; (B)log post [guid]; (S)tateprovinceregion [int]; 
        ///county(A)rea [int]; ci(T)ty [int]; comme(N)tthread [int]; (R)eply [int]; (M)ention [int]; o(F)ficial register [guid];  official re[G]ister entry [guid]; 
        ///a(L)ert [guid]; incidentcategor(Y) [guid]; co(U)ntry [iso]; u(Z)er [guid]; incident u(P)date [guid]
        /// </summary>
        public static string StringDbEntity(DbEntity dbEntityEnum)
        {
            switch (dbEntityEnum)
            {
                case DbEntity.Incident: return "I";                 //guid
                case DbEntity.CivilServant: return "E";             //guid
                case DbEntity.Institution: return "O";              //guid
                case DbEntity.CalendarEvent: return "C";            //guid
                case DbEntity.BlogPost: return "B";                 //guid
                case DbEntity.StateProvinceRegion: return "S";      //int
                case DbEntity.CountyArea: return "A";               //int
                case DbEntity.City: return "T";                     //int
                case DbEntity.CommentInThread: return "N";          //int
                case DbEntity.Reply: return "R";                    //int
                case DbEntity.CommentMention: return "M";           //int
                case DbEntity.OfficialRegister: return "F";         //guid
                case DbEntity.Alert: return "L";                    //guid
                case DbEntity.IncidentCategory: return "Y";         //guid
                case DbEntity.Country: return "U";                  //iso
                case DbEntity.User: return "Z";                     //guid
                case DbEntity.OfficialRegisterEntry: return "G";    //guid
                case DbEntity.IncidentUpdate: return "P";           //guid
                default: return String.Empty;
            }
        }

        //(D)ata Entity; (R)egional 
        public static string StringSubsType(SubsType subsTypeEnum)
        {
            switch (subsTypeEnum)
            {
                case SubsType.DataEntity: return "D";
                case SubsType.Regional: return "R";
                default: return String.Empty;
            }
        }

        //(L)imit exceeded; registerby(I)nvitationonly; allowoutof(C)ountryuserregistration; (U)nder construction
        public static string StringRegDenyType(RegistrationDenyReason denyTypeEnum)
        {
            switch (denyTypeEnum)
            {
                case RegistrationDenyReason.LimitExceeded: return "L";
                case RegistrationDenyReason.RegisterByInvitationOnly: return "I";
                case RegistrationDenyReason.AllowOutOfCountryUserRegistration: return "C";
                case RegistrationDenyReason.UnderConstruction: return "U";
                default: return String.Empty;
            }
        }

        //authentication(T)oken; (U)serpassword
        public static string StringApiAuthType(ApiAuthType apiAuthTypeEnum)
        {
            switch (apiAuthTypeEnum)
            {
                case ApiAuthType.AuthenticationToken: return "T";
                case ApiAuthType.UserPassword: return "U";
                default: return String.Empty;
            }
        }

        //(J)son; (X)ml; (T)ext
        public static string StringReadDataType(ReadDataType readDataTypeEnum)
        {
            switch (readDataTypeEnum)
            {
                case ReadDataType.Json: return "J";
                case ReadDataType.XML: return "X";
                case ReadDataType.Text: return "T";
                default: return String.Empty;
            }
        }

        //(B)log; (W)ebsite front page article; (N)ewsletter
        public static string StringBlogType(BlogType blogTypeEnum)
        {
            switch (blogTypeEnum)
            {
                case BlogType.Blog: return "B";
                case BlogType.WebsiteFrontpageArticle: return "W";
                case BlogType.Newsletter: return "N";
                default: return String.Empty;
            }
        }

        //(Y)outube; yo(U)now; (N)ewsarticle; (B)log; (Ch)ange.org; (A)vaaz.org; (R)eddit; (T)witch; (O)ther  
        public static string StringLinkType(LinkType linkTypeEnum)
        {
            switch (linkTypeEnum)
            {
                case LinkType.YouTube: return "Y";
                case LinkType.YouNow: return "U";
                case LinkType.NewsArticle: return "N";
                case LinkType.Blog: return "B";
                case LinkType.ChangeOrg: return "C";
                case LinkType.AvaazOrg: return "A";
                case LinkType.Reddit: return "R";
                case LinkType.Twitch: return "T";
                case LinkType.Other: return "O";
                default: return String.Empty;
            }
        }

        //(M)eetup; (D)emonstration; Online meeting; online (S)tream
        public static string StringCalendarEventType(CalendarEventType calendarEventTypeEnum)
        {
            switch (calendarEventTypeEnum)
            {
                case CalendarEventType.Meetup: return "M";
                case CalendarEventType.Demonstration: return "D";
                case CalendarEventType.OnlineMeeting: return "O";
                case CalendarEventType.OnlineStream: return "S";
                default: return String.Empty;
            }
        }

        /// <summary>
        /// Convert a byte array to a string in the StringEncodingType format requested.
        /// </summary>
        /// <param name="sourceByteArrayData">The byte array to convert.</param>
        /// <param name="stringOutputType">The StringEncodingType enum format in which to return the string.</param>
        /// <returns>A string encoded in the requested StringEncodingType enum format.</returns>
        public static string ConvertBytesToStringEncodingType(byte[] sourceByteArrayData,
                                                                  StringEncodingType stringOutputType = StringEncodingType.Base64url)
        {

            switch (stringOutputType)
            {
                case StringEncodingType.Base64: return Convert.ToBase64String(sourceByteArrayData);
                case StringEncodingType.Base64url: return Base64UrlTextEncoder.Encode(sourceByteArrayData);
                case StringEncodingType.HexString: return ConvertByteArrayToHexString(sourceByteArrayData);
                case StringEncodingType.HexStringShort: return ConvertByteArrayToHexStringShort(sourceByteArrayData);
                default: return String.Empty;
            }

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

            switch (stringInputType)
            {
                case StringEncodingType.Base64: return Convert.FromBase64String(sourceStringData);
                case StringEncodingType.Base64url: return Base64UrlTextEncoder.Decode(sourceStringData);
                case StringEncodingType.HexString: return ConvertByteArrayFromHexString(sourceStringData);
                case StringEncodingType.HexStringShort: return ConvertByteArrayFromHexStringShort(sourceStringData);
                default: return null;
            }

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
                        if (sourceStringData[i].ToString().Length != 2)
                        {
                            throw new ApplicationException($"Hex string value elements must be 2 characters in length.  Invalid value '{sourceStringData[i]}' encountered at array position {i} of string value {sourceStringData} and can not be converted.");
                        }
                        _returnBytes[i] = Convert.ToByte(sourceStringData[i].ToString(), 16);
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

    }

}
