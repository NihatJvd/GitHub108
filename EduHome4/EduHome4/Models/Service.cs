using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome4.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Xaiş Olunur Servis Ad-ı Daxil Edin")]

        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDeactive { get; set; }
    }
}
