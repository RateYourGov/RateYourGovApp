using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Create a set of rules to apply to flag comments automatically for potential violation of terms in lieu of a real AI.  
    /// Just a placeholder for now, to be fleshed out if/when the functionality is added.
    /// </summary>
    public class CommentAutoFlagRuleModel
    {

        public int Id { get; set; }

        public string RuleName { get; set; }
        public string RuleDescription { get; set; }

        public int RunSequence { get; set; }

        public string RuleActionType { get; set; }  //??

        //TODO: Flesh out field names to use

        public int FlagReasonId { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsActive { get; set; }

    }
}
