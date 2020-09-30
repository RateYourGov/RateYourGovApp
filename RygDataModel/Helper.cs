using System;
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

    public class Helper
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

    }

}
