using Microsoft.AspNetCore.Mvc;
using QMS.Models;
using QMS.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using QMS.DAL;
using System.Threading.Tasks;

namespace QMS.Controllers
{
    [AllowAnonymous]
    [Route("[controller]/[action]")]
    public class PatientsController : Controller
    {
        private readonly IPatientRepo _patientRepo;

        public PatientsController(IEmailSender emailSender, IPatientRepo patientrepo)
        {
            _patientRepo = patientrepo;
        }

        // GET: Patients/Start
        public ActionResult Start()
        {
            TempData.Clear();
            return View();
        }

        // GET: Patients/CreateIdentity
        public ActionResult CreateIdentity(bool newPatient = true)
        {

            IdentityView identityView = new IdentityView();
            if (newPatient)
            {
                TempData.Clear();
                return View(identityView);
            }
            else
            {
                string tmpName = (string)TempData["lastname"];
                if (string.IsNullOrEmpty(tmpName))
                {
                    TempData.Clear();
                    return RedirectToAction("Start");
                }

                identityView.lastname = tmpName;
                identityView.firstname = (string)TempData["firstname"];
                identityView.birthDate = (string)TempData["birthDate"];
                identityView.gender = (Gender)TempData["gender"];
                identityView.preferredGender = (Gender)TempData["preferredGender"];
                return View(identityView);
            }
        }

        // POST: Patients/CreateIdentity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIdentity(IdentityView identityView)
        {
            if (ModelState.IsValid)
            {
                TempData["lastname"] = identityView.lastname;
                TempData["firstname"] = identityView.firstname;
                TempData["birthDate"] = identityView.birthDate;
                TempData["gender"] = identityView.gender;
                TempData["preferredGender"] = identityView.preferredGender;

                if (identityView.gender == Gender.Female)
                {
                    return RedirectToAction("PregnancyStatus");
                }
                else
                {
                    return RedirectToAction("Contact");
                }
            }
            return View(identityView);
        }

        // GET: Patients/Contact
        public ActionResult Contact()
        {
            AddressView addressView = new AddressView();
            string line1 = (string)TempData["line1"];
            if (!string.IsNullOrEmpty(line1))
            {
                TempData["line1"] = line1;
                addressView = this.getAddress(addressView);
            }
            ViewData["StatesList"] = _patientRepo.BuildStateList();
            return View(addressView);
        }

        // POST: Patients/Contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(AddressView addressView)
        {
            if (ModelState.IsValid)
            {
                this.setAddress(addressView);
                return RedirectToAction("Complaint");
            }

            return View(addressView);
        }

        public AddressView getAddress(AddressView addressView)
        {
            addressView.line1 = (string)TempData["line1"];
            addressView.line2 = (string)TempData["line2"];
            addressView.city = (string)TempData["city"];
            addressView.state = (string)TempData["state"];
            addressView.postalCode = (string)TempData["postalCode"];
            addressView.email = (string)TempData["email"];
            addressView.telecom = (string)TempData["telecom"];
            setAddress(addressView);
            return addressView;
        }

        private void setAddress(AddressView addressView)
        {
            TempData["line1"] = addressView.line1;
            TempData["line2"] = addressView.line2;
            TempData["city"] = addressView.city;
            TempData["state"] = addressView.state;
            TempData["postalCode"] = addressView.postalCode;
            TempData["email"] = addressView.email;
            TempData["telecom"] = addressView.telecom;
        }

        // GET: Patients/PregnancyStatus/5
        public ActionResult PregnancyStatus()
        {
            return View();
        }

        // GET: Patients/PregnancyInfo
        public ActionResult PregnancyInfo()
        {
            return View();
        }

        // GET: Patients/Complaint
        public ActionResult Complaint()
        {
            ComplaintView complaintView = new ComplaintView();
            string tmpComplaint = (string)TempData["complaint"];
            if (!string.IsNullOrEmpty(tmpComplaint))
            {
                complaintView.complaint = tmpComplaint;
                TempData["complaint"] = tmpComplaint;
                complaintView.lifethreatening = (bool)TempData["lifethreatening"];
                TempData["lifethreatening"] = complaintView.lifethreatening;
            }

            return View(complaintView);
        }

        // POST: Patients/Complaint
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Complaint(ComplaintView complaintView)
        {
            if (ModelState.IsValid)
            {
                TempData["complaint"] = complaintView.complaint;
                TempData["lifethreatening"] = complaintView.lifethreatening;
                return RedirectToAction("Walkin");
            }
            return View(complaintView);
        }

        // GET: Patients/Walkin/5
        public ActionResult Walkin()
        {
            return View();
        }

        // GET: Patients/WalkinDirections
        public ActionResult WalkinDirections()
        {
            return View();
        }

        // GET: Patients/Insurance/5
        public ActionResult Insurance()
        {
            InsuranceView insuranceView = new InsuranceView();
            string tmpProvider = (string)TempData["provider"];
            if (!string.IsNullOrEmpty(tmpProvider))
            {
                insuranceView.provider = tmpProvider;
                insuranceView.policy = (string)TempData["policy"];
                insuranceView.primary = (bool)TempData["primary"];
                insuranceView.primaryName = (string)TempData["primaryName"];
            }

            return View(insuranceView);
        }

        // POST: Patients/Insurance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insurance(InsuranceView insuranceView)
        {
            if (ModelState.IsValid)
            {
                TempData["provider"] = insuranceView.provider;
                TempData["policy"] = insuranceView.policy;
                TempData["primary"] = insuranceView.primary;
                TempData["primaryName"] = insuranceView.primaryName;
                return RedirectToAction("Questions");
            }
            return View(insuranceView);
        }

        // GET: Patients/Questions/5
        public ActionResult Questions()
        {
            QuestionsView questionsView = new QuestionsView();
            return View(questionsView);
        }

        // POST: Patients/Questions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Questions(QuestionsView questionsView)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (Patient patient = new Patient())
                    {
                        patient.lastname = (string)TempData["lastname"];
                        patient.firstname = (string)TempData["firstname"];
                        patient.gender = (Gender)TempData["gender"];
                        patient.preferredGender = (Gender)TempData["preferredGender"];
                        patient.birthDate = (string)TempData["birthDate"];
                        patient.line1 = (string)TempData["line1"];
                        patient.line2 = (string)TempData["line2"];
                        patient.city = (string)TempData["city"];
                        patient.state = (string)TempData["state"];
                        patient.postalCode = (string)TempData["postalCode"];
                        patient.telecom = (string)TempData["telecom"];
                        patient.email = (string)TempData["email"];
                        patient.complaint = (string)TempData["complaint"];
                        patient.lifethreatening = (bool)TempData["lifethreatening"];
                        patient.provider = (string)TempData["provider"];
                        patient.policy = (string)TempData["policy"];
                        patient.primary = (bool)TempData["primary"];
                        patient.primaryName = (string)TempData["primaryName"];
                        patient.travel = questionsView.travel;
                        patient.HIVtest = questionsView.HIVtest;
                        patient.vaccines = questionsView.vaccines;
                        patient.abused = questionsView.abused;
                        patient.primaryPhysician = questionsView.primaryPhysician;
                        if (!string.IsNullOrEmpty(patient.lastname) && !string.IsNullOrEmpty(patient.firstname))
                        {
                            var created = true;
                            if (patient.FHIR_id == null)
                            {
                                created = _patientRepo.CreateVisit(patient);
                            }
                            if (created)
                            {
                                if (created)
                                {
                                    bool sent = false;
                                    var to = patient.email;
                                    var from = Helpers.getParam("HostServerLogin");
                                    var msg = Helpers.getParam("HostNoticeAdded");
                                    try
                                    {
                                        sent = await Helpers.SendMessage(to, msg);
                                    }
                                    catch (Exception ex)
                                    {
                                        Helpers.NotifyAdmin(ex.ToString());
                                    }
                                    TempData.Clear();
                                    return RedirectToAction("Confirmation", new { id = patient.FHIR_id, msg = sent });
                                }
                            }
                        }
                        else
                        {
                            TempData.Clear();
                            return RedirectToAction("Start", "Patients");
                        }
                    }

                }
                catch (Exception ex)
                {
                    Helpers.NotifyAdmin(ex.ToString());
                    TempData.Clear();
                    return RedirectToAction("Start", "Patients");
                }
            }
            return View(questionsView);
        }

        // GET: Patients/Confirmation
        public ActionResult Confirmation(int? id, bool msg = false)
        {
            if (id == null)
            {
                return new StatusCodeResult(400);
            }
            ViewBag.Patient_ID = id;
            ViewBag.Notified = msg == true ? "Welcome to the Queue System. A welcome message has been sent to your smart device." : "";
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
