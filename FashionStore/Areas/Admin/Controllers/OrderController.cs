using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FashionStore.Models;
using X.PagedList;

namespace FashionStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public OrderController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Order
        public IActionResult Index(int? page)
        {
            var fashionStoreDbContext = _context.Orders.Include(o => o.IdCustomerNavigation).Include(o => o.IdStaffNavigation);
            return View(fashionStoreDbContext.ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.IdCustomerNavigation)
                .Include(o => o.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.Idorder == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Admin/Order/Create
        public IActionResult Create()
        {
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer");
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "IdStaff");
            return View();
        }

        // POST: Admin/Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idorder,IdCustomer,IdStaff,ShippingPrice,DiscountPrice,FullName,Phone,AddressSend,TotalAll,TypeOrder,CreateAt,UpdateAt")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer", order.IdCustomer);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "IdStaff", order.IdStaff);
            return View(order);
        }

        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer", order.IdCustomer);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "IdStaff", order.IdStaff);
            return View(order);
        }

        // POST: Admin/Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Idorder,IdCustomer,IdStaff,ShippingPrice,DiscountPrice,FullName,Phone,AddressSend,TotalAll,TypeOrder,CreateAt,UpdateAt")] Order order)
        {
            if (id != order.Idorder)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Idorder))
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
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer", order.IdCustomer);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "IdStaff", order.IdStaff);
            return View(order);
        }

        // GET: Admin/Order/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.IdCustomerNavigation)
                .Include(o => o.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.Idorder == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
          return _context.Orders.Any(e => e.Idorder == id);
        }

		[HttpPost]
		public IActionResult updateType(string idOrder, int type)
		{
			if(idOrder != null)
			{
				var getOrder = _context.Orders.Where(m => m.Idorder == idOrder).FirstOrDefault();
				if(getOrder != null)
				{
					getOrder.TypeOrder = type;
					_context.Update(getOrder);
					if (_context.SaveChanges() > 0) return Json("Cập nhật thành công");
					else Json("Cập nhật thất bại");
				}
			}
			return Json(null);
		}

	}
}
