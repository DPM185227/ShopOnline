using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FashionStore.Models;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using X.PagedList;

namespace FashionStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypeController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public ProductTypeController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductType
        public IActionResult Index(int? page)
        {
			ViewData["checkExitCategory"] = _context.Categories.Include(m=>m.IdtypeNavigation).ToList();
              return View(_context.ProductTypes.ToList().ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/ProductType/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ProductTypes == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes
                .FirstOrDefaultAsync(m => m.Idtype == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // GET: Admin/ProductType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idtype,TypeName,TypeNameSlug,ImageType,ViewInFrontEnd,File")] ProductType productType)
        {
            if (ModelState.IsValid)
            {
				// id type
				productType.Idtype = Guid.NewGuid().ToString();
				productType.TypeNameSlug = Library.Library.convertToUnSign3(productType.TypeName);
				// upload image and save path
#pragma warning disable CS8604 // Possible null reference argument for parameter 'file' in 'string Library.UploadImage(string folderName, IFormFile file)'.
				productType.ImageType = Library.Library.UploadImage("ProductType", productType.File);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'file' in 'string Library.UploadImage(string folderName, IFormFile file)'.
                _context.Add(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
			return View(productType);
        }

		// GET: Admin/ProductType/Edit/5
		public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ProductTypes == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        // POST: Admin/ProductType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,[Bind("Idtype,TypeName,TypeNameSlug,ImageType,ViewInFrontEnd,File")] ProductType productType)
        {
            if (id != productType.Idtype)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					productType.TypeNameSlug = Library.Library.convertToUnSign3(productType.TypeName);
					if (productType.File != null)
					{
#pragma warning disable CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
						Library.Library.DeleteImage(productType.ImageType);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
						productType.ImageType = Library.Library.UploadImage("ProductType", productType.File);
					}
                    _context.Update(productType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTypeExists(productType.Idtype))
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
            return View(productType);
        }

        // GET: Admin/ProductType/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ProductTypes == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes
                .FirstOrDefaultAsync(m => m.Idtype == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        // POST: Admin/ProductType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ProductTypes == null)
            {
				return NotFound("Entity set 'FashionStoreDbContext.ProductTypes'  is null.");
            }
            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType != null)
            {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
				Library.Library.DeleteImage(productType.ImageType);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
                _context.ProductTypes.Remove(productType);
			}
            await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
        }

        private bool ProductTypeExists(string id)
        {
          return _context.ProductTypes.Any(e => e.Idtype == id);
        }

		// update active
		[HttpGet]
		public IActionResult updateCheck(string idType)
		{
			string success = "";
			var type = _context.ProductTypes.Where(m => m.Idtype == idType).FirstOrDefault();
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

		[HttpDelete]
		public IActionResult deleteAll()
		{
			var listType = _context.ProductTypes.ToList();
			var listCate = _context.Categories.ToList();
			// variable save result
			string sucess = "Thất bại";
			int deleteSuccessCount = 0;
			foreach (var item in listType)
			{
				// kiểm tra tồn tại category
				if(listCate.Where(m=>m.Idtype == item.Idtype).Count() < 1)
				{
					// xóa từng phần tử
					_context.Remove(item);
					_context.SaveChanges();
					++deleteSuccessCount;
					sucess = "Thành công " + deleteSuccessCount + " dòng dữ liệu";
				}
			}
			return Json(sucess);
		}
	}
}
