using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Blog; Website Front Page Article; Newsletter.  Who can contribute is determined by Application Settings.
    /// </summary>
    public class BlogPostModel
    {

        public string Id { get; set; }

        /// <summary>
        /// (B)log; (W)ebsite front page article; (N)ewsletter
        /// </summary>
        /// <example>
        /// postType == RygDataModel.ModelHelper.StringBlogType(RygDataModel.BlogType.Blog)
        /// </example>
        public string PostType { get; set; }

        //null=not applicable to area, if all are blank it is global to the entire site/all registered users
        public int StateOrProvinceOrRegionId { get; set; }      
        public int CountyOrAreaId { get; set; }
        public int CityId { get; set; }
        public string CountryIsoCode { get; set; }

        public string SubjectText { get; set; }
        public string BlogText { get; set; }
        public string MainLinkUrl { get; set; }             //optional
        public string PostedByNameToShow { get; set; }
        public string Hashtags { get; set; }

        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }

        public string PostedByUserId { get; set; }
        public DateTime PostedDate { get; set; }

        public bool PinnedPost { get; set; }

        public bool NotificationCreated { get; set; }
        public DateTime NotificationCreateDate { get; set; }
        public bool EmailSent { get; set; }
        public DateTime EmailSentDate { get; set; }

        public bool FlaggedForModeration { get; set; }
        public DateTime FlaggedForModerationDate { get; set; }
        public string FlaggedForModerationText { get; set; }
        public string FlaggedByUserId { get; set; }
        public DateTime ModeratorReviewDate { get; set; }
        public string ModeratorUserId { get; set; }

        public bool IsVisible { get; set; }

    }
}
