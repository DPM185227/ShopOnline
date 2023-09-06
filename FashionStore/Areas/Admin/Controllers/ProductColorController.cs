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
    public class ProductColorController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public ProductColorController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductColor
        public IActionResult Index(string id, int? page)
        {
			if(id!=null) HttpContext.Session.SetString("_idProductColor", id);
            var fashionStoreDbContext = _context.ProductColors.Include(p => p.IdProductNavigation).Where(m=>m.IdProduct == id);
            return View(fashionStoreDbContext.ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/ProductColor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductColors == null)
            {
                return NotFound();
            }

            var productColor = await _context.ProductColors
                .Include(p => p.IdProductNavigation)
                .FirstOrDefaultAsync(m => m.Idcolor == id);
            if (productColor == null)
            {
                return NotFound();
            }

            return View(productColor);
        }

        // GET: Admin/ProductColor/Create
        public IActionResult Create()
        {
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct");
            return View();
        }

        // POST: Admin/ProductColor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idcolor,ColorName,ImageProductColor,Active,IdProduct,ImageColor,imageColor,imageProductColor")] ProductColor productColor)
        {
            if (ModelState.IsValid)
            {
				if (HttpContext.Session.GetString("_idProductColor") != null) productColor.IdProduct = HttpContext.Session.GetString("_idProductColor");
				// image color
				if (productColor.imageColor != null)
					productColor.ImageColor = Library.Library.UploadImage("ProductColor", productColor.imageColor);
				else
					return View(productColor);
				// image product
				if (productColor.imageProductColor != null)
					productColor.ImageProductColor = Library.Library.UploadImage("ProductColor", productColor.imageProductColor);
				else return View(productColor);
				// active
				productColor.Active = 1;
                _context.Add(productColor);
                await _context.SaveChangesAsync();
				HttpContext.Session.Remove("_IDProduct");
                return RedirectToAction(nameof(Index),"Product");
            }
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", productColor.IdProduct);
            return View(productColor);
        }

        // GET: Admin/ProductColor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductColors == null)
            {
                return NotFound();
            }

            var productColor = await _context.ProductColors.FindAsync(id);
            if (productColor == null)
            {
                return NotFound();
            }
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", productColor.IdProduct);
            return View(productColor);
        }

        // POST: Admin/ProductColor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("Idcolor,ColorName,ImageProductColor,Active,imageProductColor,imageColor,IdProduct,ImageColor")] ProductColor productColor)
        {
            if (id != productColor.Idcolor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					if (productColor.imageProductColor != null)
					{
#pragma warning disable CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
						Library.Library.DeleteImage(productColor.ImageProductColor);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
						productColor.ImageProductColor = Library.Library.UploadImage("ProductColor", productColor.imageProductColor);
					}

					if (productColor.imageColor != null)
					{
#pragma warning disable CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
						Library.Library.DeleteImage(productColor.ImageColor);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
						productColor.ImageColor = Library.Library.UploadImage("ProductColor", productColor.imageColor);
					}

					_context.Update(productColor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductColorExists(productColor.Idcolor))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),"Product");
            }
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", productColor.IdProduct);
            return View(productColor);
        }

        // GET: Admin/ProductColor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductColors == null)
            {
                return NotFound();
            }

            var productColor = await _context.ProductColors
                .Include(p => p.IdProductNavigation)
                .FirstOrDefaultAsync(m => m.Idcolor == id);
            if (productColor == null)
            {
                return NotFound();
            }

            return View(productColor);
        }

        // POST: Admin/ProductColor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductColors == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.ProductColors'  is null.");
            }
            var productColor = await _context.ProductColors.FindAsync(id);
            if (productColor != null)
            {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
				Library.Library.DeleteImage(productColor.ImageColor);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
#pragma warning disable CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
				Library.Library.DeleteImage(productColor.ImageProductColor);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
                _context.ProductColors.Remove(productColor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index),"Product");
        }

        private bool ProductColorExists(int id)
        {
          return _context.ProductColors.Any(e => e.Idcolor == id);
        }

		// update active
		[HttpGet]
		public IActionResult updateCheck(string idColor)
		{
			string success = "";
			var getProduct = _context.ProductColors.Include(m=>m.IdProductNavigation).Where(m => m.Idcolor == int.Parse(idColor)).FirstOrDefault();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			var color = _context.ProductColors.Where(m=>m.IdProduct == getProduct.IdProductNavigation.IdProduct).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			foreach(var item in color)
			{
				// 1 an 0 hien
				if (item.Idcolor == int.Parse(idColor)) {
					item.Active = item.Active == 1 ? item.Active = 0 : item.Active = 1;
					_context.Update(item);
					int resultCheck = _context.SaveChanges();
					if (resultCheck == 1) success = "Thành công";
					else success = "Thất bại";
				}
				else item.Active = 1;
				_context.Update(item);
				int result = _context.SaveChanges();
				if (result == 1) success = "Thành công";
				else success = "Thất bại";
			}
			// biến : không cần khởi tạo, đối tượng: new để khởi tạo mới
			return Json(success);
		}
	}
}
