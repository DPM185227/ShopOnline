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
    public class CategoryController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public CategoryController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Category
        public IActionResult Index(int? page)
        {
            var fashionStoreDbContext = _context.Categories.Include(c => c.IdtypeNavigation);
            return View(fashionStoreDbContext.ToList().ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/Category/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.IdtypeNavigation)
                .FirstOrDefaultAsync(m => m.IdCategory == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            ViewData["Idtype"] = new SelectList(_context.ProductTypes.Where(m=>m.ViewInFrontEnd == 0), "Idtype", "TypeName");
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategory,CategoryName,CategoryNameSlug,ImageCategory,ViewInFrontEnd,Idtype,File")] Category category)
        {
            if (ModelState.IsValid)
            {
				// set id cate
				category.IdCategory = Guid.NewGuid().ToString();	
				// upload image and save path
#pragma warning disable CS8604 // Possible null reference argument for parameter 'file' in 'string Library.UploadImage(string folderName, IFormFile file)'.
				category.ImageCategory = Library.Library.UploadImage("Category", category.File);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'file' in 'string Library.UploadImage(string folderName, IFormFile file)'.
				// set slug name
				category.CategoryNameSlug = Library.Library.convertToUnSign3(category.CategoryName);
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idtype"] = new SelectList(_context.ProductTypes, "Idtype", "Idtype", category.Idtype);
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["Idtype"] = new SelectList(_context.ProductTypes.Where(m=>m.ViewInFrontEnd == 0), "Idtype", "TypeName", category.Idtype);
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,[Bind("IdCategory,CategoryName,CategoryNameSlug,ImageCategory,ViewInFrontEnd,Idtype,File")] Category category)
        {
            if (id != category.IdCategory)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					if(category.File != null)
					{
						//delete img old
#pragma warning disable CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
						Library.Library.DeleteImage(category.ImageCategory);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
						// upload and update path image
						category.ImageCategory = Library.Library.UploadImage("Category", category.File);
					}
					category.CategoryNameSlug = Library.Library.convertToUnSign3(category.CategoryName);
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.IdCategory))
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
            ViewData["Idtype"] = new SelectList(_context.ProductTypes, "Idtype", "Idtype", category.Idtype);
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.IdtypeNavigation)
                .FirstOrDefaultAsync(m => m.IdCategory == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(string id)
        {
          return _context.Categories.Any(e => e.IdCategory == id);
        }

		// update active
		[HttpGet]
		public IActionResult updateCheck(string idCatgory)
		{
			string success = "";
			var category = _context.Categories.Where(m => m.IdCategory == idCatgory).FirstOrDefault();
			// hidden category
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			category.ViewInFrontEnd = category.ViewInFrontEnd == 1 ? category.ViewInFrontEnd = 0 : category.ViewInFrontEnd = 1;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			_context.Update(category);
			int result = _context.SaveChanges();
			if (result == 1) success = "Thành công";
			else success = "Thất bại";
			// biến : không cần khởi tạo, đối tượng: new để khởi tạo mới
			return Json(success);
		}
	}
}
