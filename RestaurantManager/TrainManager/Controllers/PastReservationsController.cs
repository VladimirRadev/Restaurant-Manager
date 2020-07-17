using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ToDoManager.Controllers
{
    public class PastReservationsController : Controller
    {
        private readonly RestaurantManagerContext _context;

        public PastReservationsController()
        {
            _context = new RestaurantManagerContext();



        }
        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            var toDoManagerContext = _context.PastReservations;
            return View(await toDoManagerContext.ToListAsync());
        }
    }
}
