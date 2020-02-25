using System;
using System.ComponentModel.DataAnnotations;

namespace QMS.Models
{
    //[Table("EmailHostServer", Schema = "public")]
    public class EmailHostServer
    {
        [StringLength(100, ErrorMessage = " {0} cannot be longer than {1} characters.")]
        public string HostServer { get; set; }

        [StringLength(5, ErrorMessage = " {0} cannot be longer than {1} characters.")]
        public string HostServerPort { get; set; }

        [Display(Name = "Enable SSL?")]
        public Boolean HostServerEnableSSL { get; set; }

        [Display(Name = "Email Host Username")]
        [StringLength(50, ErrorMessage = " {0} cannot be longer than {1} characters.")]
        public string HostServerLogin { get; set; }

        [Display(Name = "Email Host Password")]
        [DataType(DataType.Password)]
        [StringLength(25, ErrorMessage = " {0} cannot be longer than {1} characters.")]
        public string HostServerCredential { get; set; }
    }
}

