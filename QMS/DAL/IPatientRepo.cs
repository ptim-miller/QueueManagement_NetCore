using Microsoft.AspNetCore.Mvc.Rendering;
using QMS.Models;
using System;
using System.Collections.Generic;

namespace QMS.DAL
{
    public interface IPatientRepo
    {
        Boolean CreatePatient(Patient item);
        Boolean UpdatePatient(Patient item);
        Patient ReadPatient(int id);
        List<Patient> ReadAllPatients();
        Boolean DeletePatient(int id);

        Boolean CreateEncounter(Encounter item);
        Boolean UpdateEncounter(Encounter item);
        Encounter ReadEncounter(int id);
        List<Encounter> ReadAllEncounters(string status = null, int days = 0, bool allowEnded = false);
        List<Encounter> ReadAllEncounters(EncounterSearch item);
        Boolean DeleteEncounter(int id);
        Boolean CreateVisit(Patient patient);
        List<SelectListItem> BuildStateList();
    }
}