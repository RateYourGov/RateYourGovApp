using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Simple used hashtag to facilitate combo box lookups, show hottest topics, etc.
    /// </summary>
    public class HashtagModel
    {

        public string Hashtag { get; set; }

        public DateTime FirstUsedDate { get; set; }
        public int UsedCount { get; set; }
        public DateTime LastUsedDate { get; set; }
        public int UsedSinceDateCount { get; set; }     //dependent on application settings, recalculated by system maintenance, used to track "trending topics" 
        public DateTime UsedSinceDate { get; set; }     //dependent on application settings, recalculated by system maintenance, used to track "trending topics"  

        public bool IsVisible { get; set; }

    }
}
