using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Keywords to use to flag comments automatically for potential violation of terms in lieu of a real AI.  
    /// Just a placeholder for now, to be fleshed out if/when the functionality is added.
    /// </summary>
    public class CommentAutoFlagKeywordModel
    {

        public int Id { get; set; }

        public int SearchSequence { get; set; }     //Perhaps use SeverityLevel instead or in junction with to sort?

        public string SearchKeywordPhrase { get; set; }
        public int SeverityLevel { get; set; }
        public int FlagReasonId { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsActive { get; set; }

    }
}
