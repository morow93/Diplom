using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Entities
{
    public class Place
    {
        public Place() { }

        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public String PlaceId { get; set; }

        public String Name { get; set; }
        public String About { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }
}
