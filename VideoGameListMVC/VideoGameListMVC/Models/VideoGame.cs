using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VideoGameListMVC.Models
{
    public class VideoGame
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Company { get; set; }
        public string Director { get; set; }
        public string System { get; set; }
        public int YearReleased { get; set; }


    }
}
