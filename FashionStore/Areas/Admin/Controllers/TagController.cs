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
    public class TagController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public TagController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Tag
        public IActionResult Index(string id, int? page)
        {
			if (id != null) HttpContext.Session.SetString("_idProrductTag", id);
            var fashionStoreDbContext = _context.Tags.Include(t => t.IdProductNavigation).Where(m=>m.IdProduct == id);
            return View(fashionStoreDbContext.ToPagedList(page ?? 1, 5).ToList());
        }

        // GET: Admin/Tag/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags
                .Include(t => t.IdProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // GET: Admin/Tag/Create
        public IActionResult Create()
        {
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct");
            return View();
        }

        // POST: Admin/Tag/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameTag,NameTagSlug,IdProduct")] Tag tag)
        {
            if (ModelState.IsValid)
            {
				if (HttpContext.Session.GetString("_idProrductTag") != null) tag.IdProduct = HttpContext.Session.GetString("_idProrductTag");
				tag.NameTagSlug = Library.Library.convertToUnSign3(tag.NameTag);
				_context.Add(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),"Product");
            }
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", tag.IdProduct);
            return View(tag);
        }

        // GET: Admin/Tag/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", tag.IdProduct);
            return View(tag);
        }

        // POST: Admin/Tag/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameTag,NameTagSlug,IdProduct")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.Id))
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
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", tag.IdProduct);
            return View(tag);
        }

        // GET: Admin/Tag/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tags == null)
            {
                return NotFound();
            }

            var tag = await _context.Tags
                .Include(t => t.IdProductNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Admin/Tag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tags == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.Tags'  is null.");
            }
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
            }
            await _context.SaveChangesAsync();
			HttpContext.Session.Remove("_idProrductTag");
			return RedirectToAction(nameof(Index), "Product");
        }

        private bool TagExists(int id)
        {
          return _context.Tags.Any(e => e.Id == id);
        }
    }
}
