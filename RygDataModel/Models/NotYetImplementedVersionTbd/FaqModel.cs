using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Frequently asked question, created by anyone authorized to do so as defined in application settings.
    /// </summary>
    public class FaqModel
    {

        public string Id { get; set; }

        public String FaqGroupHeading { get; set; }
        public string FaqShortDescription { get; set; }
        public string FaqDetailedText { get; set; }
        public int DisplayPrioritySeq { get; set; }         //Ascending sort within ResourceGroupHeading, Lower numbers will be shown first

        public string AddedByUserId { get; set; }
        public DateTime AddedDate { get; set; }

        public bool IsVisible { get; set; }

    }
}
