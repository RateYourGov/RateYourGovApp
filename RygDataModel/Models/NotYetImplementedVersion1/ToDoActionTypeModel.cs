using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// The type of to do list action entry,  to be loaded from the localized json file
    /// NB: InternalActionType Should NOT be modified in json localization file, it is used by the application to recognize associated functionality 
    /// </summary>
    public class ToDoActionTypeModel
    {

        public int Id { get; set; }
        public string ActionTypeDescription { get; set; }

        public string AInternalActionType { get; set; }   //Should NOT be modified in json localization file, this is the keyword used internally to trigger functionality 

    }
}
