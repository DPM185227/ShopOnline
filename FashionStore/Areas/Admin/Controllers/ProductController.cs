using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FashionStore.Models;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using X.PagedList;

namespace FashionStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public ProductController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Product
        public IActionResult Index(int? page)
        {
			HttpContext.Session.Remove("_idProductColor");
			HttpContext.Session.Remove("_idProrductTag");
			HttpContext.Session.Remove("_idProductColorSize");
            HttpContext.Session.Remove("_idProductPercent");
			ViewBag.listColor = _context.ProductColors.Include(p => p.IdProductNavigation).ToList();
			ViewBag.listSize = _context.ProductSizes.Include(p => p.IdcolorNavigation).ToList();
			ViewBag.listTag = _context.Tags.Include(p => p.IdProductNavigation).ToList();
            ViewBag.listPrecent = _context.SaleProductPercents.Include(m => m.IdProductNavigation).ToList();
			var fashionStoreDbContext = _context.Products.Include(p => p.IdBranchNavigation).Include(p => p.IdCategoryNavigation).Include(p => p.IdStaffNavigation);
            return View(fashionStoreDbContext.ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.IdBranchNavigation)
                .Include(p => p.IdCategoryNavigation)
                .Include(p => p.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.IdProduct == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Product/Create
        public IActionResult Create()
        {
            ViewData["IdBranch"] = new SelectList(_context.Branches, "IdBranch", "LocationName");
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "CategoryName");
            return View();
        }

		// POST: Admin/Product/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string ckEditor, [Bind("IdProduct,ProductName,ProductPrice,ProductDescription,ProductNameSlug,IdCategory,IdStaff,IdBranch")] Product product)
        {
            if (ModelState.IsValid)
            {
				if (product.ProductPrice < 0)
					return View(product);
				product.IdProduct = Guid.NewGuid().ToString();
#pragma warning disable CS8604 // Possible null reference argument for parameter 's' in 'string Library.convertToUnSign3(string s)'.
				product.ProductNameSlug = Library.Library.convertToUnSign3(product.ProductName);
#pragma warning restore CS8604 // Possible null reference argument for parameter 's' in 'string Library.convertToUnSign3(string s)'.
				product.ProductDescription = ckEditor;
				product.IdStaff = HttpContext.Session.GetString("_IDStaff");
				product.ViewInFrontEnd = 1;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdBranch"] = new SelectList(_context.Branches, "IdBranch", "IdBranch", product.IdBranch);
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "IdCategory", product.IdCategory);
            //ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "IdStaff", product.IdStaff);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["IdBranch"] = new SelectList(_context.Branches, "IdBranch", "LocationName", product.IdBranch);
            ViewData["IdCategory"] = new SelectList(_context.Categories.Where(m=>m.ViewInFrontEnd == 0), "IdCategory", "CategoryName", product.IdCategory);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,string ckEditor,[Bind("IdProduct,ProductName,ProductPrice,ProductDescription,ProductNameSlug,IdCategory,IdStaff,IdBranch")] Product product)
        {
            if (id != product.IdProduct)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
#pragma warning disable CS8604 // Possible null reference argument for parameter 's' in 'string Library.convertToUnSign3(string s)'.
					product.ProductNameSlug = Library.Library.convertToUnSign3(product.ProductName);
#pragma warning restore CS8604 // Possible null reference argument for parameter 's' in 'string Library.convertToUnSign3(string s)'.
					product.ProductDescription = ckEditor;
					_context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.IdProduct))
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
            ViewData["IdBranch"] = new SelectList(_context.Branches, "IdBranch", "IdBranch", product.IdBranch);
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "IdCategory", product.IdCategory);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "IdStaff", product.IdStaff);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.IdBranchNavigation)
                .Include(p => p.IdCategoryNavigation)
                .Include(p => p.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.IdProduct == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
          return _context.Products.Any(e => e.IdProduct == id);
        }

		// update active
		[HttpGet]
		public IActionResult updateCheck(string idProduct)
		{
			string success = "";
			var type = _context.Products.Where(m => m.IdProduct == idProduct).FirstOrDefault();
			// hidden category
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			type.ViewInFrontEnd = type.ViewInFrontEnd == 1 ? type.ViewInFrontEnd = 0 : type.ViewInFrontEnd = 1;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			_context.Update(type);
			int result = _context.SaveChanges();
			if (result == 1) success = "Thành công";
			else success = "Thất bại";
			// biến : không cần khởi tạo, đối tượng: new để khởi tạo mới
			return Json(success);
		}
	}
}
