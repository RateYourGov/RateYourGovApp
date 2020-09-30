using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Person Salutation Title (Mr, Mrs, Ms, Dr, etc)  to be loaded from the localized json file
    /// </summary>
    public class PersonTitleModel
    {

        public int Id { get; set; }
        public string PersonTitleName { get; set; }

    }
}
