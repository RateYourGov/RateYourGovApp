using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Reply to a Top-level ThreadComponentModel comment, possibly to another comment in the same thread.
    /// </summary>
    /// <remarks>
    /// Whilst the Id of a comment within the same thread and therefore the same table is being recorded, the general idea is that we would prefer
    /// to avoid joining the table to itself in most scenarios, instead, the information can be used to match the comments for highlighting once the data has been 
    /// loaded into the view model ThreadAndReplies.  The original comment may also have been deleted or flagged, so this scenario should be considered when using the data.
    /// Thread and reply comments are stored in different tables in order to reduce DB storage (Thread comments contain much more info) and
    /// to simplify Dapper data access and reduce DB overhead by not joining a single comments table to itself.
    /// Keep it Simple design/Development principle.
    /// </remarks>
    public class ReplyCommentModel
    {

        public int Id { get; set; }
        public int ThreadCommentId { get; set; }
        public int ReplyToReplyId { get; set; } //mainly intended for use once the data is loaded in the data list or complex thread-replies model
        public string ReplyToUserName { get; set; } //note that this is the username, not userid, which is all we really need and is a smaller field, so much less db space and bandwidth usage

        //This is the same data contained in the ThreadCommentModel and could probably be separated into it's own model
        //and inherited, but duplicating it instead to use different field names for ease of maintenance and simpler query mapping
        //Keeping it Simple
        public DateTime RcDate { get; set; }            //Rc=ReplyComment 
        public string RcUserId { get; set; }
        public string RcCommentText { get; set; }
        public int RcLikedCount { get; set; }
        public int RcDislikedCount { get; set; }
        public int RcFlaggedCount { get; set; }
        public string RcHashtags { get; set; }          //space separated 
        public bool RcIsHidden { get; set; }
        public int RcFlagReasonId { get; set; }
        public DateTime RcModeratedDate { get; set; }

        public bool NotificationCreated { get; set; }
        public bool EmailSent { get; set; }

    }
}
