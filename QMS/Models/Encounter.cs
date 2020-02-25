using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Models
{
    [Serializable]
    [Table("Encounter", Schema = "public")]
    public class Encounter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Display(Name = "Latest Status")]
        public Status status { get; set; }
        [Display(Name = "Visit Type")]
        public VisitType visitType { get; set; } //inpatient | outpatient | ambulatory | emergency +
        public int patient_id { get; set; }
        [ForeignKey("patient_id")]
        public virtual Patient patient { get; set; }
        [Display(Name = "Status Time")]
        public DateTime start { get; set; }
        [Display(Name = "Finalized")]
        public DateTime? end { get; set; }
        [Display(Name = "Patient ID")]
        public int? FHIR_id { get; set; }
    }

    public class PatientSummary
    {
        public string reference { get; set; } //Patient/<id>
        public string display { get; set; } //<full name>
    }

    public class ParticipantSummary
    {
        public ParticipantSummary()
        {
            this.individual = new Individual();
        }
        public Individual individual { get; set; }
    }

    public class Individual
    {
        public string reference { get; set; }// Practitioner/4658
        public string display { get; set; }// ER
    }

    public class Period
    {
        public string start { get; set; } //"2016-11-09T15:37:00+00:00"
        public string end { get; set; } //"2016-11-09T15:37:00+00:00"
    }

    public class ServiceProviderSummary
    {
        public string reference { get; set; } //Organization/424
        public string display { get; set; } 
    }

    public enum VisitType
    {
        inpatient = 1,
        outpatient = 2,
        ambulatory = 3,
        emergency = 4
    }

    public enum Status
    {
        [Display(Name = "waiting")]
        waiting = 1,
        [Display(Name = "notified")]
        notified = 2,
        [Display(Name = "arrived")]
        arrived = 3,
        [Display(Name = "onleave")]
        onleave = 4,
        [Display(Name = "finished")]
        finished = 5,
        [Display(Name = "cancelled")]
        cancelled = 6
    }

    public class FHIREncounter : IDisposable
    {
        public FHIREncounter()
        {
            this.resourceType = "Encounter";
        }

        public FHIREncounter(Encounter encounter, int practitionerID = 0, int organizationID = 0)
        {
            this.resourceType = "Encounter";
            this.status = encounter.status == Status.notified ? "in-progress" : encounter.status == Status.waiting ? "planned" : encounter.status.ToString();
            //special handling for keyword (in) conflict with model
            this.@class = encounter.visitType.ToString();
            this.patient = new PatientSummary();
            this.patient.reference = "Patient/" + encounter.patient.FHIR_id;
            this.patient.display = encounter.patient.firstname + " " + encounter.patient.lastname;
            this.participant = new List<ParticipantSummary>();
            var participant = new ParticipantSummary();
            //grab default practitionerID if not specified
            //if (practitionerID == 0)
            //{
            //    participant.individual.reference = "Practitioner/" + Helpers.getParam("FHIRServerPractitionerID");
            //}
            //else
            //{
            //    participant.individual.reference = "Practitioner/" + practitionerID.ToString();
            //}

            participant.individual.display = Helpers.getParam("ServiceProvider");
            this.participant.Add(participant);
            this.period = new Period();
            this.period.start = String.Format("{0:s}", encounter.start);
            if (encounter.end != null)
            {
                this.period.end = String.Format("{0:s}", encounter.end);
            }
            else
            {
                // start 24 hour clock, bug in FHIR forces status to finished on null date
                this.period.end = String.Format("{0:s}", DateTime.Now.AddHours(24));
            }

            this.serviceProvider = new ServiceProviderSummary();
            //grab default organizationID if not specified
            if (organizationID == 0)
            {
                this.serviceProvider.reference = "Organization/" + Helpers.getParam("FHIRServerHospitalID");
            }else
            {
                this.serviceProvider.reference = "Organization/" + organizationID.ToString();
            }
            
            this.serviceProvider.display = Helpers.getParam("ServiceProvider");
        }
        public string resourceType { get; set; }
        public string status { get; set; }
        public string @class { get; set; } //inpatient | outpatient | ambulatory | emergency +
        public PatientSummary patient { get; set; }
        public List<ParticipantSummary> participant { get; set; }
        public Period period { get; set; }
        public ServiceProviderSummary serviceProvider { get; set; }

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

    public class EncounterSearch
    {
        public string last { get; set; }
        public string first { get; set; }
        public string email { get; set; }
        public string DOB { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
    }

}
