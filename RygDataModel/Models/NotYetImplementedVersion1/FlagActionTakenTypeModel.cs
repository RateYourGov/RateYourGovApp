using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// The type of action taken by moderator to censure a user,  to be loaded from the localized json file
    /// </summary>
    public class FlagActionTakenTypeModel
    {

        public int Id { get; set; }
        public string ActionDescription { get; set; }

    }
}
