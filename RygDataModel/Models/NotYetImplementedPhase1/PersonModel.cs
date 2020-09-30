using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Civil Servant 
    /// </summary>
    public class PersonModel
    {

        /// <summary>
        /// GUID unique to the civil servant 
        /// </summary>
        public string Id { get; set; }

        public int PersonTitleId { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string LastName { get; set; }
        public int GenderId { get; set; }

        public string ProfessionalEmail { get; set; }
        public string ProfessionalWebsite { get; set; }
        public string ProfessionalFacebookPage { get; set; }
        public string ProfessionalLinkedin { get; set; }
        public string ProfessionalTwitterHandle { get; set; }
        public string ProfessionalTwitterUrl { get; set; }

        public int Rating { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }

        public int TotalPositiveIncidentCount { get; set; }
        public int LastPositiveIncidentYear { get; set; }
        public int LastPositiveIncidentMonth { get; set; }
        public int LastPositiveIncidentDay { get; set; }

        public int TotalNegativeIncidentCount { get; set; }
        public int LastNegativeIncidentYear { get; set; }
        public int LastNegativeIncidentMonth { get; set; }
        public int LastNegativeIncidentDay { get; set; }

        public string AdditionalInfoText { get; set; }

        public int TotalMisconductRegisterEntryCount { get; set; }
        public int TotalCommendationRegisterEntryCount { get; set; }

        public int LastKnownEmploymentStatusId { get; set; }
        public bool LastKnownActiveService { get; set; }
        public string LastKnownInstitutionId { get; set; }
        public string LastKnownSupervisorId { get; set; }
        public string LastKnownComplaintCommendationInstitutionId { get; set; }
        public string LastKnownWorkPhoneNumber { get; set; }
        public int LastKnownStateOrProvinceOrRegionId { get; set; }
        public int LastKnownCountyOrAreaId { get; set; }
        public int LastKnownCityId { get; set; }
        public string LastKnownCountryIsoCode { get; set; }
        public int LastKnownJobTitleId { get; set; }
        public bool LastKnownPositionIsElected { get; set; }
        public string LastKnownGovernmentIdNumber { get; set; }
        public int LastKnownInfoYear { get; set; }
        public int LastKnownInfoMonth { get; set; }
        public int LastKnownInfoDay { get; set; }
        public decimal LastKnownSalary { get; set; }
        public int LastKnownSalaryYear { get; set; }

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
