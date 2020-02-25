using System;
using System.Collections.Generic;
using QMS.Models;
using QMS.DAL;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QMS.Controllers
{
    [ResponseCache(NoStore = true, Duration = 0)]
    [Authorize]
    public class EncountersController : Controller
    {
        private readonly IPatientRepo _patientRepo;

        public EncountersController(IPatientRepo patientrepo)
        {
            _patientRepo = patientrepo;
        }

        // GET: Encounters
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.showEnd = false;
            var items = _patientRepo.ReadAllEncounters(null, 20, false);
            if (items == null)
            {
                items = new List<Encounter>();
            }
            return View(items);
        }

        [Authorize]
        public ActionResult Search(EncounterSearch searchVals)
        {
            ViewBag.showEnd = true;
            if (String.IsNullOrEmpty(searchVals.last) && String.IsNullOrEmpty(searchVals.first) && String.IsNullOrEmpty(searchVals.email) &&
                searchVals.DOB == null && searchVals.start == null && searchVals.end == null)
            {
                TempData["CurrentPage"] = new EncounterSearch();
                var item = new List<Encounter>();
                return View(item);
            }
            else
            {
                TempData["CurrentPage"] = searchVals;
                var items = _patientRepo.ReadAllEncounters(searchVals);
                if (items == null)
                {
                    items = new List<Encounter>();
                }
                return View(items);
            }
        }

        [AllowAnonymous]
        public ActionResult PublicScreen()
        {
            var items = _patientRepo.ReadAllEncounters(null, 1, true);
            return View(items);
        }

        // GET: Encounters/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            //TempData.Remove("existingPatient");
            if (id == null)
            {
                return new StatusCodeResult(400);
            }
            var encounter = _patientRepo.ReadEncounter(id.Value);
            if (encounter == null)
            {
                return new StatusCodeResult(404);
            }
            return View(encounter);
        }

        // GET: Encounters/Create
        [Authorize]
        public ActionResult Create()
        {
            return RedirectToAction("Start", "Patients");
        }

        // GET: Encounters/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            TempData.Remove("existingPatient");
            if (id == null)
            {
                return new StatusCodeResult(400);
            }

            var item = _patientRepo.ReadEncounter(id.Value);

            if (item == null)
            {
                return new StatusCodeResult(404);
            }
            if (item.end != null && item.patient != null)
            {
                TempData["lastname"] = item.patient.lastname;
                TempData["firstname"] = item.patient.firstname;
                TempData["birthDate"] = item.patient.birthDate;
                TempData["gender"] = item.patient.gender;
                TempData["preferredGender"] = item.patient.preferredGender;
            }
            return View(item);
        }

        // POST: Encounters/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Encounter encounter)
        {
            var newEncounter = _patientRepo.ReadEncounter(encounter.id);
            newEncounter.status = encounter.status;
            if (ModelState.IsValid)
            {
                _patientRepo.UpdateEncounter(newEncounter);
                return RedirectToAction("Index");
            }
            return View(encounter);
        }

        [Authorize]
        public ActionResult Notify(int id)
        {
            var encounter = _patientRepo.ReadEncounter(id);
            if (encounter.status == Status.waiting)
            {
                ViewData["message"] = Helpers.getParam("HostNoticeReady");
            }
            else if (encounter.status == Status.arrived)
            {
                ViewData["message"] = Helpers.getParam("HostNoticeComplete");
            }

            return View(encounter);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Notify(int id, string message)
        {
            var new_encounter = _patientRepo.ReadEncounter(id);
            var messageIn = message ?? "Please contact Hospital ER desk";
            if (new_encounter.id > 0 && !String.IsNullOrEmpty(messageIn) && new_encounter != null)
            {
                var to = new_encounter.patient.email;
                if (new_encounter.status == Status.waiting)
                {
                    new_encounter.status = Status.notified;
                }
                else if (new_encounter.status == Status.arrived)
                {
                    new_encounter.status = Status.finished;
                    var info = System.IO.File.ReadAllText(@".//Pages/Info.html");
                    messageIn = String.Concat(messageIn, info);
                }
                var sent = await Helpers.SendMessage(to, messageIn);
                if (sent)
                {
                    if (new_encounter != null)
                    {
                        _patientRepo.UpdateEncounter(new_encounter);
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.failed = "Message send attempt failed. Please check mail server settings.";
            return View(new_encounter);
        }


        
        // GET: Encounters/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(400);
            }
            var encounter = _patientRepo.ReadEncounter(id.Value);
            if (encounter == null)
            {
                return new StatusCodeResult(404);
            }
            return View(encounter);
        }

        // POST: Encounters/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var encounter = _patientRepo.ReadEncounter(id);
                if (encounter != null && encounter.id > 0)
                {
                    _patientRepo.DeleteEncounter(id);

                }
                if (encounter != null && encounter.id > 0)
                {
                    FHIRRepo.DeleteFHIR(encounter.FHIR_id.Value, "Encounter");

                }
                return RedirectToAction("Search", "Encounters");
            }
            catch (Exception ex)
            {
                Helpers.NotifyAdmin(ex.ToString());
                return RedirectToAction("Search", "Encounters");
            }
        }
    
}
}

