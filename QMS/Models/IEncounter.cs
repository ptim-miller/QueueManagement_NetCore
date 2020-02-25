using System;
using System.Collections.Generic;

namespace QMS.Models
{
    public interface IEncounter
    {
        int id { get; set; }
        string status { get; set; }
        string visitType { get; set; } //inpatient | outpatient | ambulatory | emergency +
        int patient_id { get; set; }
        Patient patient { get; set; }
        DateTime start { get; set; }
        DateTime? end { get; set; }
        int? FHIR_id { get; set; }
        //bool Create(string status = Status.waiting.ToString(), string visitType = VisitType.emergency.ToString(), DateTime? end = null);
        //bool Update();
        //Encounter ReadLocal(int id);
        //List<Encounter> ReadAll(string status = null, int days = 0, bool allowEnded = false);
        //List<Encounter> ReadAll(EncounterSearch searchItem);
        //FHIREncounter ReadFHIR();
        //bool DeleteLocal();
        //bool DeleteFHIR();
    }
}