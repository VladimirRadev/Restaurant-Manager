using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ToDoManager.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly RestaurantManagerContext _context;

        public ReservationsController()
        {
            _context = new RestaurantManagerContext();

       
            
        }
        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            var toDoManagerContext = _context.Reservations.Include(t => t.ReservationHolder);
            toDoManagerContext.Include(t => t.ServiceWaiter);
            return View(await toDoManagerContext.ToListAsync());
        }
        // GET: Reservations/Create
        public IActionResult Create()
        {
            ViewData["ReservationHolderId"] = new SelectList(_context.Users, "Id", "Username");
            ViewData["WaiterId"] = new SelectList(_context.Waiters, "Id", "Name");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationHolderId,WaiterId,Date,Time")] Reservation reservation)
        {
            var reservationsOwnByThisWaiter = _context.Reservations
                .Where(r => r.WaiterId == reservation.WaiterId).Count();
            var classOfWaiter = _context.Waiters.FirstOrDefault(w => w.Id == reservation.WaiterId).Class;
            if (reservationsOwnByThisWaiter <= classOfWaiter)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(reservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToPage("ErrorClassOfWaiter", "Reservations");
            }
            ViewData["ReservationHolderId"] = new SelectList(_context.Users, "Id", "Username",reservation.ReservationHolderId);
            ViewData["WaiterId"] = new SelectList(_context.Waiters, "Id", "Name",reservation.WaiterId);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ReservationHolderId"] = new SelectList(_context.Users, "Id", "Username", reservation.ReservationHolderId);
            ViewData["WaiterId"] = new SelectList(_context.Waiters, "Id", "Name", reservation.WaiterId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationHolderId,WaiterId,Date,Time,Id")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReservationHolderId"] = new SelectList(_context.Users, "Id", "Username", reservation.ReservationHolderId);
            ViewData["WaiterId"] = new SelectList(_context.Waiters, "Id", "Name", reservation.WaiterId);
            return View(reservation);
        }

        // GET: Lines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(l => l.ReservationHolder).Include(l=>l.ServiceWaiter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Lines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Lines/Delete/5
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(l => l.ReservationHolder).Include(l => l.ServiceWaiter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Lines/Delete/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(int id)
        {
            var reservationFromDb = await _context.Reservations.FindAsync(id);
            reservationFromDb.IsCanceled=true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Reservations/Create
        public async Task<IActionResult> PayBill(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);    
        }


        // POST: Reservations/Create
        [HttpPost, ActionName("PayBill")]
        public async Task<IActionResult> PayBillConfirmed([Bind("TotalPrice,Tip,Id")] Reservation reservation)
        {
            var reservationFromDb = await _context.Reservations.FindAsync(reservation.Id);
            var pastReservation = new PastReservation();
            pastReservation.ReservationId = reservationFromDb.Id;
            reservationFromDb.TotalPrice = reservation.TotalPrice;
            reservationFromDb.Tip= ((Convert.ToDecimal(reservation.TotalPrice) * 20) / 100).ToString();
            reservationFromDb.IsPayed = true;
            if (ModelState.IsValid)
            {
                _context.Update(reservationFromDb);
                _context.Add(pastReservation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ErrorClassOfWaiter(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ReservationHolderId"] = new SelectList(_context.Users, "Id", "Username", reservation.ReservationHolderId);
            ViewData["WaiterId"] = new SelectList(_context.Waiters, "Id", "Name", reservation.WaiterId);
            return View("ErrorClassOfWaiter");
        }
        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
