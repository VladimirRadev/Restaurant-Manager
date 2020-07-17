using System.Collections.Generic;

namespace Data.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
