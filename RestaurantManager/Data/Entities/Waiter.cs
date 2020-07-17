using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Entities
{
    public class Waiter : BaseEntity
    {
        public Waiter()
        {
            this.Reservations = new HashSet<Reservation>();
        }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Status { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public int Class { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
       

    }
}
