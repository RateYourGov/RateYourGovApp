using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Website Link Type used to show an icon next to the link to be loaded from the localized json file.
    /// </summary>
    /// <remarks>
    /// NB: If you are adding/removing an item in this list, bear in mind that it may require code changes that depend on the RygDataModel.ModelHelper.StringLinkType function or RygDataModel.LinkType enum.
    /// </remarks>
    public class UrlLinkTypeModel
    {
        /// <summary>
        /// LinkTypeCharacter: (Y)outube; yo(U)now; (N)ewsarticle; (B)log; (Ch)ange.org; (A)vaaz.org; (R)eddit; (T)witch; (O)ther  
        /// </summary>
        /// <example>
        /// addLinkType == RygDataModel.ModelHelper.StringLinkType(RygDataModel.LinkType.YouTube)
        /// </example>
        public string LinkTypeCharacter { get; set; }               //primary unique key
        public string LinkTypeName { get; set; }
        public string LinkTypeTagInfo { get; set; }

        public string UrlSearchText { get; set; }                   //The lowercase text to search for in the URL link to associate the URL entered with this type, can be multiple strings, separated with a semi-colon (;) character 
        public string LinkImageFileName { get; set; }
        public bool RequiresInputOfDescriptiveText { get; set; }
    }
}
