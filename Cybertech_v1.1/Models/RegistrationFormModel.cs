using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cybertech_v1._1.Models
{
    public class RegistrationFormModel : ExperianceModel
    {
        [Required]
        public int RegisterId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        //[Required]
        //public string FathersName { get; set; }
        //[Required]
        //public string MothersName { get; set; }
        //[Required]
        //public string Nationality { get; set; }
        [Required]
        public string HighestDegree { get; set; }
        [Required]
        public string  CollegeDegree { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime PassoutYear { get; set; }
        
        public ExperianceModel Experiance { get; set; }

        public string OfficeTelephone { get; set; }

        public string OfficeMobile { get; set; }

        public string OfficialEmail { get; set; }
        [Required]
        public string PersonalMobile{ get; set; }
        [Required]
        public string PersonalEmail{ get; set; }
        [Required]
        public string HomePhone { get; set; }

        public string FacebookAccount { get; set; }

        public string TwitterAccount { get; set; }

        public string SkypeAccount { get; set; }

        public string LinkedInAccount { get; set; }
        [Required]
        public string CurrentAddressLine1  { get; set; }

        internal string GetType(string field_name)
        {
            throw new NotImplementedException();
        }

        [Required]
        public string CurrentAddressLine2 { get; set; }
        [Required]
        public string CurrentAddressCity { get; set; }
        [Required]
        public string CurrentAddressState { get; set; }
        [Required]
        public string CurrentAddressPinCode { get; set; }
        [Required]
        public string CurrentAddressCountry { get; set; }
        [Required]
        public string PermanentAddressLine1 { get; set; }
        [Required]
        public string PermanentAddressLine2 { get; set; }
        [Required]
        public string PermanentAddressCity { get; set; }
        [Required]
        public string PermanentAddressState { get; set; }
        [Required]
        public string PermanentAddressPinCode { get; set; }
        [Required]
        public string PermanentAddressCountry { get; set; }
        
    }

    public class ExperianceModel 
    {
        public string Employer { get; set; }

        public string Position { get; set; }

        public string PrincipleActivity { get; set; }

        public string KeyAchievement { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }

}