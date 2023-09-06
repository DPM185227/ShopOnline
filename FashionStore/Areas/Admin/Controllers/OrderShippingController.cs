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
    public class OrderShippingController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public OrderShippingController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/OrderShipping
        public IActionResult Index(int? page)
        {
            var fashionStoreDbContext = _context.OrderShippings.Include(o => o.CodeProvinceNavigation);
            return View(fashionStoreDbContext.ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/OrderShipping/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderShippings == null)
            {
                return NotFound();
            }

            var orderShipping = await _context.OrderShippings
                .Include(o => o.CodeProvinceNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderShipping == null)
            {
                return NotFound();
            }

            return View(orderShipping);
        }

        // GET: Admin/OrderShipping/Create
        public IActionResult Create()
        {
            ViewData["CodeProvince"] = new SelectList(_context.Provinces, "CodeProvince", "NameProvince");
            return View();
        }

        // POST: Admin/OrderShipping/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodeProvince,Price")] OrderShipping orderShipping)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderShipping);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodeProvince"] = new SelectList(_context.Provinces, "CodeProvince", "CodeProvince", orderShipping.CodeProvince);
            return View(orderShipping);
        }

        // GET: Admin/OrderShipping/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderShippings == null)
            {
                return NotFound();
            }

            var orderShipping = await _context.OrderShippings.FindAsync(id);
            if (orderShipping == null)
            {
                return NotFound();
            }
            ViewData["CodeProvince"] = new SelectList(_context.Provinces, "CodeProvince", "NameProvince", orderShipping.CodeProvince);
            return View(orderShipping);
        }

        // POST: Admin/OrderShipping/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodeProvince,Price")] OrderShipping orderShipping)
        {
            if (id != orderShipping.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderShipping);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderShippingExists(orderShipping.Id))
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
            ViewData["CodeProvince"] = new SelectList(_context.Provinces, "CodeProvince", "CodeProvince", orderShipping.CodeProvince);
            return View(orderShipping);
        }

        // GET: Admin/OrderShipping/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderShippings == null)
            {
                return NotFound();
            }

            var orderShipping = await _context.OrderShippings
                .Include(o => o.CodeProvinceNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderShipping == null)
            {
                return NotFound();
            }

            return View(orderShipping);
        }

        // POST: Admin/OrderShipping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderShippings == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.OrderShippings'  is null.");
            }
            var orderShipping = await _context.OrderShippings.FindAsync(id);
            if (orderShipping != null)
            {
                _context.OrderShippings.Remove(orderShipping);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderShippingExists(int id)
        {
          return _context.OrderShippings.Any(e => e.Id == id);
        }
    }
}
