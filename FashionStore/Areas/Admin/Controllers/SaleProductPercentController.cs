using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FashionStore.Models;

namespace FashionStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SaleProductPercentController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public SaleProductPercentController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SaleProductPercent
        public async Task<IActionResult> Index(string id)
        {
            if (id != null) HttpContext.Session.SetString("_idProductPercent",id);
            var fashionStoreDbContext = _context.SaleProductPercents.Include(s => s.IdProductNavigation);
            return View(await fashionStoreDbContext.Where(m=>m.IdProduct == id).ToListAsync());
        }

        // GET: Admin/SaleProductPercent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SaleProductPercents == null)
            {
                return NotFound();
            }

            var saleProductPercent = await _context.SaleProductPercents
                .Include(s => s.IdProductNavigation)
                .FirstOrDefaultAsync(m => m.IdSale == id);
            if (saleProductPercent == null)
            {
                return NotFound();
            }

            return View(saleProductPercent);
        }

        // GET: Admin/SaleProductPercent/Create
        public IActionResult Create()
        {
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct");
            return View();
        }

        // POST: Admin/SaleProductPercent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSale,StartDay,EndDay,IdProduct,SalePercent")] SaleProductPercent saleProductPercent)
        {
            if (ModelState.IsValid)
            {
                saleProductPercent.IdProduct = HttpContext.Session.GetString("_idProductPercent");
                _context.Add(saleProductPercent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"Product");
            }
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", saleProductPercent.IdProduct);
            return View(saleProductPercent);
        }

        // GET: Admin/SaleProductPercent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SaleProductPercents == null)
            {
                return NotFound();
            }

            var saleProductPercent = await _context.SaleProductPercents.FindAsync(id);
            if (saleProductPercent == null)
            {
                return NotFound();
            }
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", saleProductPercent.IdProduct);
            return View(saleProductPercent);
        }

        // POST: Admin/SaleProductPercent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSale,StartDay,EndDay,IdProduct,SalePercent")] SaleProductPercent saleProductPercent)
        {
            if (id != saleProductPercent.IdSale)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saleProductPercent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleProductPercentExists(saleProductPercent.IdSale))
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
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", saleProductPercent.IdProduct);
            return View(saleProductPercent);
        }

        // GET: Admin/SaleProductPercent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SaleProductPercents == null)
            {
                return NotFound();
            }

            var saleProductPercent = await _context.SaleProductPercents
                .Include(s => s.IdProductNavigation)
                .FirstOrDefaultAsync(m => m.IdSale == id);
            if (saleProductPercent == null)
            {
                return NotFound();
            }

            return View(saleProductPercent);
        }

        // POST: Admin/SaleProductPercent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SaleProductPercents == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.SaleProductPercents'  is null.");
            }
            var saleProductPercent = await _context.SaleProductPercents.FindAsync(id);
            if (saleProductPercent != null)
            {
                _context.SaleProductPercents.Remove(saleProductPercent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Product");
        }

        private bool SaleProductPercentExists(int id)
        {
          return (_context.SaleProductPercents?.Any(e => e.IdSale == id)).GetValueOrDefault();
        }
    }
}
