﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Entities
{
    public   class PastReservation:BaseEntity
    {
        public PastReservation()
        {

        }
        public int ReservationId { get; set; }
        public virtual Reservation Reservation { get; set; }
    }
}
