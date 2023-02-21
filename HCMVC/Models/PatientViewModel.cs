using System;

namespace HCMVC.Models
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Spec { get; set; }
        public DateTime SlotDate { get; set; }
        public string MobileNo { get; set; }
        public string UserName { get; set; }

        public int UserId { get; set; }
    }
}
