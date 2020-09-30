using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// civil servant employment history
    /// </summary>
    public class PersonPositionHistoryModel
    {

        public int Id { get; set; }
        public string PersonId { get; set; }

        public int EmploymentStatusId { get; set; }
        public string InstitutionId { get; set; }
        public string SupervisorId { get; set; }
        public string ComplaintCommendationInstitutionId { get; set; }
        public int StateOrProvinceOrRegionId { get; set; }
        public int CountyOrAreaId { get; set; }
        public int CityId { get; set; }
        public string CountryIsoCode { get; set; }
        public int JobTitleId { get; set; }
        public bool PositionIsElected { get; set; }
        public string GovernmentIdNumber { get; set; }
        public int FromYear { get; set; }
        public int FromMonth { get; set; }
        public int FromDay { get; set; }
        public int ToYear { get; set; }
        public int ToMonth { get; set; }
        public int ToDay { get; set; }
        public decimal KnownSalary { get; set; }
        public int KnownSalaryYear { get; set; }
        public string AdditionalInfoText { get; set; }

        public int PositiveIncidentCount { get; set; }
        public int LastPositiveIncidentYear { get; set; }
        public int LastPositiveIncidentMonth { get; set; }
        public int LastPositiveIncidentDay { get; set; }

        public int NegativeIncidentCount { get; set; }
        public int LastNegativeIncidentYear { get; set; }
        public int LastNegativeIncidentMonth { get; set; }
        public int LastNegativeIncidentDay { get; set; }

        public int MisconductRegisterEntryCount { get; set; }
        public int CommendationRegisterEntryCount { get; set; }

        public string AddedByUserId { get; set; }
        public DateTime AddedDate { get; set; }
        public string ChangedByUserId { get; set; }
        public DateTime ChangedDate { get; set; }

        public bool FlaggedForModeration { get; set; }
        public DateTime FlaggedForModerationDate { get; set; }
        public string FlaggedForModerationText { get; set; }
        public string FlaggedByUserId { get; set; }
        public DateTime ModeratorReviewDate { get; set; }
        public string ModeratorUserId { get; set; }

    }
}
