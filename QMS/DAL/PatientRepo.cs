using System;
using System.Collections.Generic;
using System.Linq;
using QMS.Models;
using Microsoft.EntityFrameworkCore;
using QMS.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QMS.DAL
{
    internal class PatientRepo : IPatientRepo, IDisposable
    {
        private readonly PatientDbContext _context;

        public PatientRepo(PatientDbContext context)
        {
            _context = context;
        }

        #region patient
        public Boolean CreatePatient(Patient item)
        {
            try
            {
                var existing = this.ReadPatientbyFHIR_ID(item.FHIR_id.Value);
                // checks if FHIR ID in system, if so it updates, otherwise add new
                if (existing != null && existing.id > 0 && item.birthDate.Equals(existing.birthDate))
                {
                    this.PatientBlender(existing, item);
                    _context.Entry(existing).State = EntityState.Modified;
                }
                else
                {
                    _context.patients.Add(item);
                }

                var count = _context.SaveChanges();
                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Helpers.NotifyAdmin(ex.ToString());
                return false;
            }
        }

        public Boolean UpdatePatient(Patient item)
        {
            try
            {
                _context.Entry(item).State = EntityState.Modified;
                var count = _context.SaveChanges();
                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Helpers.NotifyAdmin(ex.ToString());
                return false;
            }
        }

        public Patient ReadPatient(int id)
        {
            try
            {
                var model = _context.patients.Find(id);
                return model;
            }
            catch
            {

                return null;
            }
        }

        public List<Patient> ReadAllPatients()
        {
            try
            {
                var model = from u in _context.patients
                            orderby u.id ascending
                            select u;
                if (model != null)
                {
                    return model.ToList();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public Boolean DeletePatient(int id)
        {
            try
            {
                var item = this.ReadPatient(id);
                _context.patients.Remove(item);
                var count = _context.SaveChanges();
                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region encounter
        public Boolean CreateEncounter(Encounter item)
        {
            try
            {
                var existing = this.ReadPatientbyFHIR_ID(item.patient.FHIR_id.Value);
                // checks if patient's FHIR ID in system, if so it updates, otherwise add new
                if (existing != null && existing.id > 0 && item.patient.birthDate.Equals(existing.birthDate))
                {
                    this.PatientBlender(existing, item.patient);
                    item.patient = existing;
                    item.patient_id = existing.id;
                }

                if (item.id > 0)
                {
                    _context.Entry(item).State = EntityState.Modified;
                }
                else
                {
                    _context.encounters.Add(item);
                }

                var count = _context.SaveChanges();
                if (count > 0)
                {

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public Boolean UpdateEncounter(Encounter item)
        {
            try
            {
                if (item.status == Status.cancelled || item.status == Status.finished)
                {
                    item.end = DateTime.Now;
                }
                else
                {
                    item.end = null;
                }
                _context.Entry(item).State = EntityState.Modified;
                var count = _context.SaveChanges();
                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public Encounter ReadEncounter(int id)
        {
            try
            {
                var model = from u in _context.encounters
                            orderby u.start descending
                            join r in _context.patients on u.patient_id equals r.id
                            where u.id.Equals(id)
                            select u;
                model.Include(p => p.patient).FirstOrDefault();

                return model.FirstOrDefault();
            }
            catch
            {

                return null;
            }
        }

        public List<Encounter> ReadAllEncounters(string status = null, int days = 0, bool allowEnded = false)
        {
            try
            {
                DateTime checkdate = DateTime.Now.AddDays(-(days));
                var model = from u in _context.encounters
                            orderby u.start descending
                            join r in _context.patients on u.patient_id equals r.id
                            where (status == null || u.status.Equals(status))
                            where (allowEnded == true || u.end.HasValue == false)
                            where (days == 0 || u.start >= checkdate)
                            select u;
                model.Include(p => p.patient).ToList();

                if (model != null)
                {
                    var items = model.ToList();
                    return items;
                }
                return null;
            }
            catch (Exception ex)
            {
                Helpers.NotifyAdmin(ex.ToString());
                return null;
            }
        }

        public List<Encounter> ReadAllEncounters(EncounterSearch item)
        {
            try
            {
                DateTime checkdate = new DateTime();
                if (item.end != null)
                {
                    checkdate = item.end.Value.AddDays(1);
                }

                var model = from u in _context.encounters
                            orderby u.start descending
                            join r in _context.patients on u.patient_id equals r.id
                            where (item.last == null || r.lastname.ToLower().StartsWith(item.last.Trim().ToLower()))
                            where (item.first == null || r.firstname.ToLower().StartsWith(item.first.Trim().ToLower()))
                            where (item.email == null || r.email.ToLower().StartsWith(item.email.Trim().ToLower()))
                            where (item.DOB == null || r.birthDate.Equals(item.DOB))
                            where (item.start == null || u.start >= item.start)
                            where (item.end == null || u.start <= checkdate)
                            select u;
                model.Include(p => p.patient).ToList();

                if (model != null)
                {
                    return model.ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                Helpers.NotifyAdmin(ex.ToString());
                return null;
            }
        }

        public Patient ReadPatientbyFHIR_ID(int FHIR_id)
        {
            try
            {
                var model = from u in _context.patients
                            where (u.FHIR_id.Equals(FHIR_id))
                            orderby u.id descending
                            select u;

                var result = model.FirstOrDefault();
                if (result != null)
                {
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                Helpers.NotifyAdmin(ex.ToString());
                return null;
            }
        }

        public Boolean DeleteEncounter(int id)
        {
            try
            {
                var item = this.ReadEncounter(id);
                _context.encounters.Remove(item);
                var count = _context.SaveChanges();
                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {

                return false;
            }
        }
        #endregion

        #region helpers
        /// <summary>
        /// Creates the Encounter and Patient records
        /// in FHIR and stores them in the database.
        /// 
        /// TODO - This write process should be set to retry and/or roll
        /// back the entire process if any step fails along the way.
        /// 
        /// Note - Original FHIR server did not create duplicate Patient records
        /// if one already existed in FHIR. HAPI FHIR V3.0.0 appears to create
        /// duplicates based on same info. This may be a FHIR configuration issue.
        /// 
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public Boolean CreateVisit(Patient patient)
        {
            {
                try
                {
                    //Create FHIR patient
                    FHIRPatient fhirPatient = new FHIRPatient(patient);
                    patient.FHIR_id = FHIRRepo.CreateFHIR(fhirPatient);
                    bool Pcreated = false;
                    Encounter encounter = new Encounter
                    {
                        patient = patient,
                        end = null,
                        status = Status.waiting,
                        visitType = VisitType.emergency,
                        start = DateTime.Now
                    };
                    //Create FHIR encounter
                    FHIREncounter fhirEncounter = new FHIREncounter(encounter);
                    encounter.FHIR_id = FHIRRepo.CreateFHIR(fhirEncounter);
                    Pcreated = this.CreateEncounter(encounter);
                    if (Pcreated)
                    {
                        return true;
                    }
                }

                catch (Exception ex)
                {
                    Helpers.NotifyAdmin(ex.ToString());
                    return false;
                }
                return false;
            }
        }

        private void PatientBlender(Patient oldInfo, Patient newInfo)
        {
            oldInfo.lastname = newInfo.lastname;
            oldInfo.firstname = newInfo.firstname;
            oldInfo.gender = newInfo.gender;
            oldInfo.preferredGender = newInfo.preferredGender;
            oldInfo.birthDate = newInfo.birthDate;
            oldInfo.line1 = newInfo.line1;
            oldInfo.line2 = newInfo.line2;
            oldInfo.city = newInfo.city;
            oldInfo.state = newInfo.state;
            oldInfo.postalCode = newInfo.postalCode;
            oldInfo.telecom = newInfo.telecom ?? oldInfo.telecom;
            oldInfo.email = newInfo.email;
            oldInfo.complaint = newInfo.complaint;
            oldInfo.lifethreatening = newInfo.lifethreatening;
            oldInfo.provider = newInfo.provider;
            oldInfo.policy = newInfo.policy ?? oldInfo.policy;
            oldInfo.primary = newInfo.primary;
            oldInfo.primaryName = newInfo.primaryName ?? oldInfo.primaryName;
            oldInfo.travel = newInfo.travel;
            oldInfo.HIVtest = newInfo.HIVtest;
            oldInfo.vaccines = newInfo.vaccines;
            oldInfo.abused = newInfo.abused;
            oldInfo.primaryPhysician = newInfo.primaryPhysician;
        }

        public List<SelectListItem> BuildStateList()
        {
            List<SelectListItem> States = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Alabama", Value="AL"},
                new SelectListItem() { Text="Alaska", Value="AK"},
                new SelectListItem() { Text="Arizona", Value="AZ"},
                new SelectListItem() { Text="Arkansas", Value="AR"},
                new SelectListItem() { Text="California", Value="CA"},
                new SelectListItem() { Text="Colorado", Value="CO"},
                new SelectListItem() { Text="Connecticut", Value="CT"},
                new SelectListItem() { Text="District of Columbia", Value="DC"},
                new SelectListItem() { Text="Delaware", Value="DE"},
                new SelectListItem() { Text="Florida", Value="FL"},
                new SelectListItem() { Text="Georgia", Value="GA"},
                new SelectListItem() { Text="Hawaii", Value="HI"},
                new SelectListItem() { Text="Idaho", Value="ID"},
                new SelectListItem() { Text="Illinois", Value="IL"},
                new SelectListItem() { Text="Indiana", Value="IN"},
                new SelectListItem() { Text="Iowa", Value="IA"},
                new SelectListItem() { Text="Kansas", Value="KS"},
                new SelectListItem() { Text="Kentucky", Value="KY"},
                new SelectListItem() { Text="Louisiana", Value="LA"},
                new SelectListItem() { Text="Maine", Value="ME"},
                new SelectListItem() { Text="Maryland", Value="MD"},
                new SelectListItem() { Text="Massachusetts", Value="MA"},
                new SelectListItem() { Text="Michigan", Value="MI"},
                new SelectListItem() { Text="Minnesota", Value="MN"},
                new SelectListItem() { Text="Mississippi", Value="MS"},
                new SelectListItem() { Text="Missouri", Value="MO"},
                new SelectListItem() { Text="Montana", Value="MT"},
                new SelectListItem() { Text="Nebraska", Value="NE"},
                new SelectListItem() { Text="Nevada", Value="NV"},
                new SelectListItem() { Text="New Hampshire", Value="NH"},
                new SelectListItem() { Text="New Jersey", Value="NJ"},
                new SelectListItem() { Text="New Mexico", Value="NM"},
                new SelectListItem() { Text="New York", Value="NY"},
                new SelectListItem() { Text="North Carolina", Value="NC"},
                new SelectListItem() { Text="North Dakota", Value="ND"},
                new SelectListItem() { Text="Ohio", Value="OH"},
                new SelectListItem() { Text="Oklahoma", Value="OK"},
                new SelectListItem() { Text="Oregon", Value="OR"},
                new SelectListItem() { Text="Pennsylvania", Value="PA"},
                new SelectListItem() { Text="Rhode Island", Value="RI"},
                new SelectListItem() { Text="South Carolina", Value="SC"},
                new SelectListItem() { Text="South Dakota", Value="SD"},
                new SelectListItem() { Text="Tennessee", Value="TN"},
                new SelectListItem() { Text="Texas", Value="TX"},
                new SelectListItem() { Text="Utah", Value="UT"},
                new SelectListItem() { Text="Vermont", Value="VT"},
                new SelectListItem() { Text="Virginia", Value="VA"},
                new SelectListItem() { Text="Washington", Value="WA"},
                new SelectListItem() { Text="West Virginia", Value="WV"},
                new SelectListItem() { Text="Wisconsin", Value="WI"},
                new SelectListItem() { Text="Wyoming", Value="WY"}
            };
            return States;
        }

        # endregion

        #region disposal
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}