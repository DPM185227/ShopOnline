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
    public class ProductSizeController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public ProductSizeController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductSize
        public IActionResult Index(string id, int? page)
        {
			if (id != null) HttpContext.Session.SetString("_idProductColorSize", id);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var fashionStoreDbContext = _context.ProductSizes.Include(p => p.IdcolorNavigation).Where(m=>m.IdcolorNavigation.IdProduct == id);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return View(fashionStoreDbContext.ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/ProductSize/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductSizes == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSizes
                .Include(p => p.IdcolorNavigation)
                .FirstOrDefaultAsync(m => m.Idsize == id);
            if (productSize == null)
            {
                return NotFound();
            }

            return View(productSize);
        }

        // GET: Admin/ProductSize/Create
        public IActionResult Create()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            ViewData["Idcolor"] = new SelectList(_context.ProductColors.Where(m=>m.IdProductNavigation.IdProduct == HttpContext.Session.GetString("_idProductColorSize")), "Idcolor", "ColorName");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return View();
        }

        // POST: Admin/ProductSize/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idsize,SizeName,Delta,QuanityProduct,Idcolor")] ProductSize productSize)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productSize);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"Product");
            }
            ViewData["Idcolor"] = new SelectList(_context.ProductColors, "Idcolor", "Idcolor", productSize.Idcolor);
            return View(productSize);
        }

        // GET: Admin/ProductSize/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductSizes == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSizes.FindAsync(id);
            if (productSize == null)
            {
                return NotFound();
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            ViewData["Idcolor"] = new SelectList(_context.ProductColors.Where(m=>m.IdProductNavigation.IdProduct == HttpContext.Session.GetString("_ProductID")), "Idcolor", "ColorName", productSize.Idcolor);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return View(productSize);
        }

        // POST: Admin/ProductSize/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idsize,SizeName,Delta,QuanityProduct,Idcolor")] ProductSize productSize)
        {
            if (id != productSize.Idsize)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productSize);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSizeExists(productSize.Idsize))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "Product");
            }
            ViewData["Idcolor"] = new SelectList(_context.ProductColors, "Idcolor", "ColorName", productSize.Idcolor);
            return View(productSize);
        }

        // GET: Admin/ProductSize/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductSizes == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSizes
                .Include(p => p.IdcolorNavigation)
                .FirstOrDefaultAsync(m => m.Idsize == id);
            if (productSize == null)
            {
                return NotFound();
            }

            return View(productSize);
        }

        // POST: Admin/ProductSize/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductSizes == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.ProductSizes'  is null.");
            }
            var productSize = await _context.ProductSizes.FindAsync(id);
            if (productSize != null)
            {
                _context.ProductSizes.Remove(productSize);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Product");
        }

        private bool ProductSizeExists(int id)
        {
          return _context.ProductSizes.Any(e => e.Idsize == id);
        }
    }
}
