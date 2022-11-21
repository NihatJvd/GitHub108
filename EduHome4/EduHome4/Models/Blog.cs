using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome4.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Tittle { get; set; }
        public string By { get; set; }
        public string Image { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsDeactive { get; internal set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
