using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Available HTML color codes for user first letter "picture" to be randomly assigned by the system or eventually chosen by the user, adding as DB so that we can choose only colors that work well with the letter color.  This is probably not the best approach but going with it for now, should be easy to refactor when needed.
    /// </summary>
    public class ColorCodeModel
    {

        public int Id { get; set; }
        public string HtmlColorCode { get; set; }

        public bool IsVisible { get; set; }

    }
}
