using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Calendar Event Type to be loaded from the localized json file.
    /// </summary>
    /// <remarks>
    /// NB: If you are adding/removing an item in this list, bear in mind that it may require code changes that depend on the RygDataModel.Helper.StringCalendarEventType function or RygDataModel.CalendarEventType enum.
    /// </remarks>
    public class CalendarEventTypeModel
    {

        /// <summary>
        /// CalendarEventType: (M)eetup; (D)emonstration; (O)nline meeting; online (S)tream 
        /// </summary>
        /// <example>
        /// calendarEventType == RygDataModel.Helper.StringCalendarEventType(RygDataModel.Helper.CalendarEventType.Meetup)
        /// </example>
        public string CalendarEventTypeCharacter { get; set; }               //primary unique key
        public string CalendarEventTypeName { get; set; }
        public string CalendarEventTypeTagInfo { get; set; }

        public string CalendarEventImageFileName { get; set; }
        public bool RequiresInputOfDescriptiveText { get; set; }

    }
}
