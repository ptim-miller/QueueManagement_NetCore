using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Local FHIR server id is 424

namespace QMS.Models
{
    public class Organization
    {
        public Organization()
        {
            this.resourceType = "Organization";
        }
        public string resourceType { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }
}