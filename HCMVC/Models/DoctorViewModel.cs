using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
namespace HCMVC.Models
{
    public class DoctorViewModel
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
    }
}
