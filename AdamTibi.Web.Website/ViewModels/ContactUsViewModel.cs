using System;
using System.ComponentModel.DataAnnotations;

namespace AdamTibi.Web.Website.ViewModels
{
    public class ContactUsViewModel
    {
        public int ResponseTime { get; set; }

        public DateTime CurrentTime { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
        
    }
}
