using System;

namespace QMS.Models
{
    public interface IPatient
    {
        int id { get; set; }
        bool active { get; set; }
        string lastname { get; set; }
        string firstname { get; set; }
        Gender gender { get; set; }
        Gender preferredGender { get; set; }
        string birthDate { get; set; }
        string line1 { get; set; }
        string line2 { get; set; }
        string city { get; set; }
        string state { get; set; }
        string postalCode { get; set; }
        string telecom { get; set; }
        string email { get; set; }
        string prefgender { get; set; }
        string complaint { get; set; }
        Boolean lifethreatening { get; set; }
        string provider { get; set; }
        string policy { get; set; }
        Boolean primary { get; set; }
        string primaryName { get; set; }
        Boolean travel { get; set; }
        Boolean HIVtest { get; set; }
        Boolean vaccines { get; set; }
        Boolean abused { get; set; }
        Boolean primaryPhysician { get; set; }
        //int? FHIR_id { get; set; }
        //bool Create();
        //bool Update();
        //FHIRPatient ReadFHIR();
        //bool DeleteLocal();
        //bool DeleteFHIR();
    }
}