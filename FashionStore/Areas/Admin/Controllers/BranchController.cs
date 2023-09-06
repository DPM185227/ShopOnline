using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using FashionStore.Models;
using X.PagedList;

namespace FashionStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public BranchController(FashionStoreDbContext context)
        {
            _context = context;
        }

        public IActionResult getDanhSach()
        {
            var FashionStoreDbContext = _context.Branches.Include(b => b.CodeDistrictNavigation).Include(b => b.CodeProvinceNavigation).Select(s => new
            {
                idBranch = s.IdBranch,
                locationName = s.LocationName,
                nameDistrict = s.CodeDistrictNavigation.NameDistrict,
                nameProvince = s.CodeProvinceNavigation.NameProvince,
                phoneBranch = s.PhoneBrand,
            }).ToList();
            var objectJson = JsonConvert.SerializeObject(FashionStoreDbContext);
            return Json(objectJson);
        }

        // GET: Admin/Branch
        public IActionResult Index(int? page)
        {
            var FashionStoreDbContext = _context.Branches.Include(b => b.CodeDistrictNavigation).Include(b => b.CodeProvinceNavigation).ToList();
            return View(FashionStoreDbContext.ToPagedList(page ?? 1,5));
        }

        // GET: Admin/Branch/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Branches == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .Include(b => b.CodeDistrictNavigation)
                .Include(b => b.CodeProvinceNavigation)
                .FirstOrDefaultAsync(m => m.IdBranch == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // GET: Admin/Branch/Create
        public IActionResult Create()
        {
            ViewData["CodeDistrict"] = new SelectList(_context.Districts, "CodeDistrict", "NameDistrict");
            ViewData["CodeProvince"] = new SelectList(_context.Provinces, "CodeProvince", "NameProvince");
            return View();
        }

        // POST: Admin/Branch/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBranch,LocationName,LoactionMap,PhoneBrand,CodeProvince,CodeDistrict")] Branch branch)
        {
            if (!ModelState.IsValid)
            {
				branch.IdBranch = Guid.NewGuid().ToString();
                _context.Add(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodeDistrict"] = new SelectList(_context.Districts, "CodeDistrict", "CodeDistrict", branch.CodeDistrict);
            ViewData["CodeProvince"] = new SelectList(_context.Provinces, "CodeProvince", "CodeProvince", branch.CodeProvince);
            return View(branch);
        }

        // GET: Admin/Branch/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Branches == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            ViewData["CodeDistrict"] = new SelectList(_context.Districts, "CodeDistrict", "NameDistrict", branch.CodeDistrict);
            ViewData["CodeProvince"] = new SelectList(_context.Provinces, "CodeProvince", "NameProvince", branch.CodeProvince);
            return View(branch);
        }

        // POST: Admin/Branch/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string codeProvinceOld, string codeDistrictOld, string id, [Bind("IdBranch,LocationName,LoactionMap,PhoneBrand,CodeProvince,CodeDistrict")] Branch branch)
        {
            if (id != branch.IdBranch)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
					if (branch.CodeDistrict == "0" )
					{
						branch.CodeProvince = codeProvinceOld;
						branch.CodeDistrict = codeDistrictOld;
					}
                    _context.Update(branch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.IdBranch))
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
            ViewData["CodeDistrict"] = new SelectList(_context.Districts, "CodeDistrict", "CodeDistrict", branch.CodeDistrict);
            ViewData["CodeProvince"] = new SelectList(_context.Provinces, "CodeProvince", "CodeProvince", branch.CodeProvince);
            return View(branch);
        }

        // GET: Admin/Branch/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Branches == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .Include(b => b.CodeDistrictNavigation)
                .Include(b => b.CodeProvinceNavigation)
                .FirstOrDefaultAsync(m => m.IdBranch == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Admin/Branch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Branches == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.Branches'  is null.");
            }
            var branch = await _context.Branches.FindAsync(id);
            if (branch != null)
            {
                _context.Branches.Remove(branch);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(string id)
        {
          return _context.Branches.Any(e => e.IdBranch == id);
        }

		[HttpGet]
		public JsonResult getHuyen(string id)
		{
			// get list districts in array
			var listHuyen = _context.Districts.Where(m => m.ProvinceId == id).Select(m => new
			{
				CodeDistrict = m.CodeDistrict,
				NameDistrict = m.NameDistrict
			}).ToList();
			// convert List<District> To Json
			var resultJson = JsonConvert.SerializeObject(listHuyen);

			return Json(resultJson);
		}

		[HttpPost]
		public IActionResult deleteAll()
		{
			var listBrach = _context.Branches.ToList();
			// variable save result
			string sucess = "Thất bại";
			foreach (var item in listBrach)
			{
				// xóa từng phần tử
				_context.RemoveRange(item);
				_context.SaveChanges();
				sucess = "Thành công";
			}
			return Json(sucess);
		}

	}
}
