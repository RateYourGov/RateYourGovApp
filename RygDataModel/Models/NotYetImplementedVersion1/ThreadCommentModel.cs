using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Top-level comments related to any Application Setting controlled data entity.
    /// </summary>
    /// <remarks>
    /// Establishes a comment thread containing replies.  Thread and reply comments are stored in different tables in order to reduce DB storage (Thread comments contain much more info) and
    /// to simplify Dapper data access and reduce DB overhead by not joining a single comments table to itself.
    /// Keep it Simple design/Development principle.
    /// </remarks>
    public class ThreadCommentModel
    {

        public int Id { get; set; }

        /// <summary>
        /// LinkedToDataEntityType: (I)ncident; civil s(E)rvant; instituti(O)n; (C)alendar event; (B)log post; a(L)ert
        /// </summary>
        /// <remarks>
        /// Note that the data entities above will always have a GUID string Id.
        /// </remarks>
        /// <example>
        /// LinkedToDataEntityType == RygDataModel.ModelHelper.StringDbEntity(RygDataModel.DbEntity.Incident)
        /// </example>
        public string LinkedToDataEntityType { get; set; }
        public string LinkedToDataEntityId { get; set; }

        public bool PinnedPost { get; set; }            //Show at the top
        public int ReplyCount { get; set; }             //To allow sort by most active
        public DateTime LastReplyDate { get; set; }     //To allow sort by most recent

        //This is the same data contained in the ReplyCommentModel and could probably be separated into it's own model
        //and inherited, but duplicating it instead to use different field names for ease of maintenance and simpler query mapping
        //Keeping it Simple
        public DateTime TcDate { get; set; }            //Tc=ThreadComment 
        public string TcUserId { get; set; }
        public string TcCommentText { get; set; }
        public int TcLikedCount { get; set; }
        public int TcDislikedCount { get; set; }
        public int TcFlaggedCount { get; set; }
        public string TcHashtags { get; set; }          //space separated 
        public bool TcIsHidden { get; set; }
        public int TcFlagReasonId { get; set; }
        public DateTime TcModeratedDate { get; set; }

        public bool NotificationCreated { get; set; }
        public bool EmailSent { get; set; }

    }
}
