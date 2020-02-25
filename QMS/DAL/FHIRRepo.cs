using System;
using System.Linq;
using QMS.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace QMS.DAL
{
    internal static class FHIRRepo
    {
        public static int CreateFHIR(dynamic item)
        {
            var jsonObject = JsonConvert.SerializeObject(item);
            var createdItem = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");

            string myReturn = "";
            string FHIRURL = Helpers.getParam("FHIRServer");

            FHIRURL += item.resourceType;
            using (var FHIRClient = new HttpClient { Timeout = TimeSpan.FromMilliseconds(60000) })
            {
                FHIRClient.BaseAddress = new Uri(FHIRURL);
                FHIRClient.DefaultRequestHeaders.Accept.Clear();
                FHIRClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json+fhir"));

                HttpResponseMessage response = FHIRClient.PostAsync(FHIRURL, createdItem).Result;
                if (response.IsSuccessStatusCode)
                {
                    myReturn = response.Headers.Location.ToString();
                    char[] sepChars = { '/' };
                    int id = 0;
                    string[] strID = myReturn.Split(sepChars, System.StringSplitOptions.RemoveEmptyEntries);
                    if (strID != null)
                    {
                        bool found = false;
                        foreach(string strItem in strID)
                        {
                            if (found)
                            {
                                id = Convert.ToInt32(strItem);
                                return id;
                            }
                            if (strItem.Equals(item.resourceType))
                            {
                                found = true;
                            }
                        }
                    }
                    return id;
                }
            }
            return 0;
        }

        public static bool DeleteFHIR(int FHIR_id, string resourceType)
        {
            try
            {
                string FHIRURL = Helpers.getParam("FHIRServer");
                FHIRURL += resourceType + "/" + Convert.ToString(FHIR_id);
                using (var FHIRClient = new HttpClient { Timeout = TimeSpan.FromMilliseconds(60000) })
                {
                    FHIRClient.BaseAddress = new Uri(FHIRURL);
                    FHIRClient.DefaultRequestHeaders.Accept.Clear();
                    FHIRClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json+fhir"));

                    var response = FHIRClient.DeleteAsync(FHIRURL).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false; ;
        }
    }
}








//public static void ReadFHIR(int FHIR_id, string resourceType, out dynamic item)
//{
//    try
//    {
//        string FHIRURL = Helpers.getParam("FHIRServer");
//        FHIRURL += resourceType + "/" + Convert.ToString(FHIR_id);
//        using (var FHIRClient = new HttpClient { Timeout = TimeSpan.FromMilliseconds(60000) })
//        {
//            FHIRClient.BaseAddress = new Uri(FHIRURL);
//            FHIRClient.DefaultRequestHeaders.Accept.Clear();
//            FHIRClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json+fhir"));

//            var response = FHIRClient.GetAsync(FHIRURL).Result;
//            //if (response.IsSuccessStatusCode)
//            //{
//            //    item = response.Content.ReadAsAsync<dynamic>().Result;
//            //}
//        }
//    }
//    catch (Exception ex)
//    {
//        Helpers.NotifyAdmin(ex.ToString());
//    }
//    item = null;
//}

//public static bool UpdateFHIR(int FHIR_id, dynamic item)
//{
//    try
//    {
//        var FHIRItem = new StringContent(JsonConvert.SerializeObject(item).ToString(), Encoding.UTF8, "application/json");
//        string FHIRURL = Helpers.getParam("FHIRServer");
//        FHIRURL += item.resourceType + "/" + Convert.ToString(FHIR_id);
//        using (var FHIRClient = new HttpClient { Timeout = TimeSpan.FromMilliseconds(60000) })
//        {
//            FHIRClient.BaseAddress = new Uri(FHIRURL);
//            FHIRClient.DefaultRequestHeaders.Accept.Clear();
//            FHIRClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json+fhir"));

//            var response = FHIRClient.PutAsync(FHIRURL, FHIRItem).Result;
//            return response.IsSuccessStatusCode;
//        }
//    }
//    catch(Exception ex)
//    {
//        return false;
//    }
//}