using System;
using System.Collections.Generic;
using System.Text;

namespace RygDataModel.Models
{
    /// <summary>
    /// Public institutions related to Civil Servants and Incidents/Official Register entries or Calendar Entries, etc.
    /// </summary>
    /// <remarks>
    /// Includes generic entries (to be used sparingly) where the specific details of the iinstitution is unknown, but we can capture regional information etc.  
    /// Note that for Civil Servants (PeopleModel) and/or Incidents this is a required field, hence the need for generic.  
    /// Generic entries may require moderation depenant on application settings.
    /// </remarks>
    public class InstitutionModel
    {

        /// <summary>
        /// GUID unique to the Institution 
        /// </summary>
        public string Id { get; set; }

        public string InstitutionName { get; set; }
        public string InstitutionTypeId { get; set; }
        public bool IsGeneric { get; set; }

        public string Address1 { get; set; }                //not applicable if generic
        public string Address2 { get; set; }                //not applicable if generic
        public string Address3 { get; set; }                //not applicable if generic
        public string Address4 { get; set; }                //not applicable if generic
        public string Address5 { get; set; }                //not applicable if generic
        public int CityId { get; set; }                     //null if not applicable to the institution or can be null if generic institution
        public int CountyAreaId { get; set; }               //null if not applicable to the institution or can be null if generic institution
        public int StateProvinceRegionId { get; set; }      //null if not applicable to the institution or can be null if generic institution
        public string CountryIsoCode { get; set; }          //null if not applicable to the institution or can be null if generic institution, dependent on application settings
        public string GoogleSearchName { get; set; }        //the exact Google search name that shows the organization on the right and gives access to ratings, etc.
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        //TODO: Create separate models for these fields to allow for multiple entries, at which time InstitutionTollFreePhoneNumber can be moved to it's own entry??  Perhaps Person phone details can be stored there as well??
        public string InstitutionPhoneNumber { get; set; }          //not applicable if generic
        public string InstitutionTollFreePhoneNumber { get; set; }  //not applicable if generic

        //TODO: Create separate models for these fields to allow for multiple entries??  Perhaps Person email details can be stored there as well??
        public string InstitutionEmailDescription { get; set; } //not applicable if generic
        public string InstitutionEmail { get; set; }            //not applicable if generic

        public string InstitutionWebsite { get; set; }          //not applicable if generic
        public string InstitutionFacebookPage { get; set; }     //not applicable if generic
        public string InstitutionLinkedin { get; set; }         //not applicable if generic
        public string InstitutionTwitterHandle { get; set; }    //not applicable if generic
        public string InstitutionTwitterUrl { get; set; }       //not applicable if generic

        public string SupervisingInstitutionId { get; set; }    //not applicable if generic
        public string ComplaintsToInstitutionId { get; set; }   //not applicable if generic

        public int DirectIncidentCount { get; set; }
        public int DirectLikesCount { get; set; }               //not applicable if generic
        public int DirectDislikesCount { get; set; }            //not applicable if generic
        public int DirectRating { get; set; }                   //not applicable if generic

        public int NumberOfEmployees { get; set; }              //not applicable if generic
        public decimal LastKnownAnnualBudget { get; set; }      //not applicable if generic
        public decimal LastKnownAnnualBudgetYear { get; set; }  //not applicable if generic

        public int AssociatedPersonCount { get; set; }          
        public int TotalPersonIncidentCount { get; set; }
        public int TotalPersonLikesCount { get; set; }
        public int TotalPersonDislikesCount { get; set; }
        public int AveragePersonRating { get; set; }
        public int TotalPersonMisconductRegisterEntryCount { get; set; }    //not applicable if generic
        public int TotalPersonCommendationRegisterEntryCount { get; set; }  //not applicable if generic

        public int LastPositiveIncidentYear { get; set; }
        public int LastPositiveIncidentMonth { get; set; }
        public int LastPositiveIncidentDay { get; set; }

        public int LastNegativeIncidentYear { get; set; }
        public int LastNegativeIncidentMonth { get; set; }
        public int LastNegativeIncidentDay { get; set; }

        public bool IsFlaggedForModeration { get; set; }
        public string FlaggedByUserId { get; set; }
        public string FlaggedForModerationText { get; set; }
        public DateTime FlaggedDate { get; set; }
        public DateTime ModeratorReviewDate { get; set; }
        public string ModeratorUserId { get; set; }

        public string AddedByUserId { get; set; }
        public DateTime AddedDate { get; set; }
        public string ChangedByUserId { get; set; }
        public DateTime ChangedDate { get; set; }

    }
}
