using QMS.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QMS.Models
{

    [Serializable]
    [Table("Patient", Schema = "public")]
    public class Patient: IDisposable  
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public bool active { get; set; }
        [Required(ErrorMessage ="Please enter last name")]
        [StringLength(30, ErrorMessage = "Last Name cannot be longer than 30 characters.")]
        [Display(Name = "Last Name")]
        public string lastname { get; set; }
        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(30, ErrorMessage = "First Name cannot be longer than 30 characters.")]
        [Display(Name = "First Name")]
        public string firstname { get; set; }
        [Display(Name = "Biological Gender")]
        public Gender gender { get; set; }  // m | f
        [Display(Name = "Preferred Gender")]
        public Gender preferredGender { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please enter date of birth: yyyy-mm-dd")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public string birthDate { get; set; }
        [Required(ErrorMessage = "Please enter street address")]
        [StringLength(40, ErrorMessage = "Address cannot be longer than 40 characters.")]
        [Display(Name = "Address")]
        public string line1 { get; set; }
        [StringLength(40, ErrorMessage = "Address cannot be longer than 40 characters.")]
        [Display(Name = "Line2")]
        public string line2 { get; set; }
        [Required(ErrorMessage = "Please enter city name")]
        [StringLength(40, ErrorMessage = "City cannot be longer than 40 characters.")]
        [Display(Name = "City")]
        public string city { get; set; }
        [Required(ErrorMessage = "Please enter state name")]
        [Display(Name = "State")]
        public string state { get; set; }
        [Required(ErrorMessage = "Please enter zip code")]
        [RegularExpression(@"^(\d{5})$", ErrorMessage = "Please enter a valid 5 digit zip code.")]
        [Display(Name = "Zip")]
        public string postalCode { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid number ###-###-####")]
        public string telecom { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter an email address")]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Are you pregnant?")]
        public Boolean IsPregnant { get; set; }

        [StringLength(400, ErrorMessage = "Complaint cannot be longer than 400 characters.")]
        [Display(Name = "What is your main complaint / reason for your visit today?")]
        public string complaint { get; set; }
        [Display(Name = "Is your complaint life threatening?")]
        public Boolean lifethreatening { get; set; }

        [Display(Name = "Insurance Provider")]
        [StringLength(50, ErrorMessage = "Field cannot be longer than 50 characters.")]
        public string provider { get; set; }
        [Display(Name = "Policy Number")]
        [StringLength(50, ErrorMessage = "Field cannot be longer than 50 characters.")]
        public string policy { get; set; }
        [Display(Name = "Are you the primary insured?")]
        public Boolean primary { get; set; }
        [Display(Name = "Primary Insured")]
        [StringLength(50, ErrorMessage = "Field cannot be longer than 50 characters.")]
        public string primaryName { get; set; }

        [Display(Name = "Have you traveled outside of the country in the last 30 days?")]
        public Boolean travel { get; set; }
        [Display(Name = "Would you like an HIV test today?")]
        public Boolean HIVtest { get; set; }
        [Display(Name = "Are you up-to-date on your vacinations? (e.g. tetanus)")]
        public Boolean vaccines { get; set; }
        [Display(Name = "Have you been abused?")]
        public Boolean abused { get; set; }
        [Display(Name = "Do you have a primary physician?")]
        public Boolean primaryPhysician { get; set; }
        [Display(Name = "Patient ID")]
        public int? FHIR_id { get; set; }


        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //    // free managed resources example
                //    if (managedResource != null)
                //    {
                //        managedResource.Dispose();
                //        managedResource = null;
                //    }
                //}
                //// free native resources if there are any.
                //if (nativeResource != IntPtr.Zero)
                //{
                //    Marshal.FreeHGlobal(nativeResource);
                //    nativeResource = IntPtr.Zero;
            }
        }
        #endregion

    }

    public class IdentityView
    {
        [Required(ErrorMessage = "Please enter last name")]
        [StringLength(30, ErrorMessage = "Last Name cannot be longer than 30 characters.")]
        [Display(Name = "Last Name")]
        public string lastname { get; set; }
        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(30, ErrorMessage = "First Name cannot be longer than 30 characters.")]
        [Display(Name = "First Name")]
        public string firstname { get; set; }
        [Display(Name = "Biological Gender")]
        public Gender gender { get; set; }  // m | f
        [Display(Name = "Preferred Gender")]
        public Gender preferredGender { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please enter date of birth: yyyy-mm-dd")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public string birthDate { get; set; }
    }

    public class AddressView
    {
        [Required(ErrorMessage = "Please enter street address")]
        [StringLength(40, ErrorMessage = "Address cannot be longer than 40 characters.")]
        [Display(Name = "Address")]
        public string line1 { get; set; }
        [StringLength(40, ErrorMessage = "Address cannot be longer than 40 characters.")]
        [Display(Name = "Line2")]
        public string line2 { get; set; }
        [Required(ErrorMessage = "Please enter city name")]
        [StringLength(40, ErrorMessage = "City cannot be longer than 40 characters.")]
        [Display(Name = "City")]
        public string city { get; set; }
        [Required(ErrorMessage = "Please enter state name")]
        //[StringLength(2, MinimumLength = 2, ErrorMessage = "State must be 2 characters.")]
        [Display(Name = "State")]
        public string state { get; set; }
        [Required(ErrorMessage = "Please enter zip code")]
        [RegularExpression(@"^(\d{5})$", ErrorMessage = "Please enter a valid 5 digit zip code.")]
        [Display(Name = "Zip")]
        public string postalCode { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid number ###-###-####")]
        public string telecom { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter an email address")]
        [Display(Name = "Email")]
        public string email { get; set; }
    }

    public class ComplaintView
    {
        [StringLength(400, ErrorMessage = "Complaint cannot be longer than 400 characters.")]
        [Display(Name = "What is your main complaint / reason for your visit today?")]
        public string complaint { get; set; }
        [Display(Name = "Is your complaint life threatening?")]
        public Boolean lifethreatening { get; set; }
    }

    public class InsuranceView
    {
        [Display(Name = "Insurance Provider")]
        [StringLength(50, ErrorMessage = "Field cannot be longer than 50 characters.")]
        public string provider { get; set; }
        [Display(Name = "Policy Number")]
        [StringLength(50, ErrorMessage = "Field cannot be longer than 50 characters.")]
        public string policy { get; set; }
        [Display(Name = "Are you the primary insured?")]
        public Boolean primary { get; set; }
        [Display(Name = "Primary Insured")]
        [StringLength(50, ErrorMessage = "Field cannot be longer than 50 characters.")]
        public string primaryName { get; set; }
    }

    public class QuestionsView
    {
        [Display(Name = "Have you traveled outside of the country in the last 30 days?")]
        public Boolean travel { get; set; }
        [Display(Name = "Would you like an HIV test today?")]
        public Boolean HIVtest { get; set; }
        [Display(Name = "Are you up-to-date on your vacinations? (e.g. tetanus)")]
        public Boolean vaccines { get; set; }
        [Display(Name = "Have you been abused?")]
        public Boolean abused { get; set; }
        [Display(Name = "Do you have a primary physician?")]
        public Boolean primaryPhysician { get; set; }
        [Display(Name = "Patient ID")]
        public int? FHIR_id { get; set; }
    }

    public class Name
    {
        public Name()
        {
            this.family = new List<string>();
            this.given = new List<string>();
        }
        [StringLength(100, ErrorMessage = " {0} cannot be longer than {1} characters.")]
        [Display(Name = "Last Name")]
        public List<string> family { get; set; }
        [StringLength(100, ErrorMessage = " {0} cannot be longer than {1} characters.")]
        [Display(Name = "First Name")]
        public List<string> given { get; set; }
    }

    public class Address
    {
        public Address()
        {
            this.line = new List<string>();
        }
        [Display(Name = "Address")]
        public List<string> line { get; set; }
        [StringLength(100, ErrorMessage = " {0} cannot be longer than {1} characters.")]
        [Display(Name = "City")]
        public string city { get; set; }
        [StringLength(100, ErrorMessage = " {0} cannot be longer than {1} characters.")]
        [Display(Name = "State")]
        public string state { get; set; }
        [StringLength(10, ErrorMessage = " {0} cannot be longer than {1} characters.")]
        [Display(Name = "Zip")]
        public string postalCode { get; set; }
    }

    public class Telecom
    {
        public string system { get; set; }  //phone | fax | email | pager | other
        public string value { get; set; }
        [Display(Name = "Type (home or work)")]
        public string use { get; set; }     //home | work | temp | old | mobile
    }

    public class FHIRPatient: IDisposable
    {
        public FHIRPatient()
        {
            this.resourceType = "Patient";
        }

        public FHIRPatient(Patient patient)
        {
            this.active = true; ;
            this.resourceType = "Patient";
            var name = new Name();
            name.given.Add(patient.firstname);
            name.family.Add(patient.lastname);
            this.name = new List<Name>();
            this.name.Add(name);
            this.gender = patient.gender.ToString() == "Female" ? "female" : "male";
            this.birthDate = patient.birthDate.ToString();
            var address = new Address();
            address.line.Add(!String.IsNullOrEmpty(patient.line1) ? patient.line1 : "No Specified");
            if (!String.IsNullOrEmpty(patient.line2))
            {
                address.line.Add(patient.line2);
            }

            address.city = patient.city;
            address.state = patient.state;
            address.postalCode = patient.postalCode;
            this.address = new List<Address>();
            this.address.Add(address);
            this.telecom = new List<Telecom>();
            if (!String.IsNullOrEmpty(patient.email))
            {
                var tel1 = new Telecom();
                tel1.system = "phone";
                tel1.use = "home";
                tel1.value = patient.telecom;
                this.telecom.Add(tel1);
            }
            if (!String.IsNullOrEmpty(patient.email))
            {
                var tel2 = new Telecom();
                tel2.system = "email";
                tel2.use = "home";
                tel2.value = patient.email;
                this.telecom.Add(tel2);
            }
        }
        public bool active { get; set; }
        public string resourceType { get; set; }
        public List<Name> name { get; set; }
        public string gender { get; set; }
        public string birthDate { get; set; }
        public List<Address> address { get; set; }
        public List<Telecom> telecom { get; set; }

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //    // free managed resources example
                //    if (managedResource != null)
                //    {
                //        managedResource.Dispose();
                //        managedResource = null;
                //    }
                //}
                //// free native resources if there are any.
                //if (nativeResource != IntPtr.Zero)
                //{
                //    Marshal.FreeHGlobal(nativeResource);
                //    nativeResource = IntPtr.Zero;
            }
        }
        #endregion
    }

}