using System;
using System.ComponentModel;
namespace Data.Entities
{
    public class Reservation : BaseEntity
    {
        public Reservation()
        {
            
        }
        [DisplayName("Date")]
        public DateTime Date { get; set; }
        [DisplayName("Time")]
        public DateTime Time { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsPayed { get; set; }
        public int WaiterId { get; set; }
        public int ReservationHolderId { get; set; }

        [DisplayName("Total Price")]
        public string TotalPrice { get; set; }
        [DisplayName("Tip")]
        public string Tip { get; set; }
        public virtual Waiter ServiceWaiter { get; set; }
        public virtual PastReservation PastReservation { get; set; }
        public virtual User ReservationHolder { get; set; }
    }
}