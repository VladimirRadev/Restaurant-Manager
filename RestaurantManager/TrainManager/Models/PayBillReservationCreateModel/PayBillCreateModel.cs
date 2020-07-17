using System.ComponentModel.DataAnnotations;

namespace ToDoManager.Models.PayBillReservationCreateModel
{
    public class PayBillCreateModel
    {
        public int Id { get; set; }
        public string TotalPrice { get; set; }
    }
}
