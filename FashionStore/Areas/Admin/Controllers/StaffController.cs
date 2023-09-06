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
    public class StaffController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public StaffController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Staff
        public IActionResult Index(int? page)
        {
            var fashionStoreDbContext = _context.Staff.Include(s => s.IdBranchNavigation);
            return View(fashionStoreDbContext.ToList().ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/Staff/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Staff == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.IdBranchNavigation)
                .FirstOrDefaultAsync(m => m.IdStaff == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Admin/Staff/Create
        public IActionResult Create()
        {
            ViewData["IdBranch"] = new SelectList(_context.Branches, "IdBranch", "LocationName");
            return View();
        }

        // POST: Admin/Staff/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStaff,StaffName,StaffAddress,StaffPhone,StaffBirthday,StaffIdentification,StaffImage,UserName,Pass,AccountType,StaffAccountType,IdBranch,imageStaff")] Staff staff)
        {
			staff.IdStaff = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'file' in 'string Library.UploadImage(string folderName, IFormFile file)'.
				staff.StaffImage = Library.Library.UploadImage("Staff", staff.imageStaff);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'file' in 'string Library.UploadImage(string folderName, IFormFile file)'.
#pragma warning disable CS8604 // Possible null reference argument for parameter 's' in 'string Library.MD5(string s)'.
				staff.Pass = Library.Library.MD5(staff.Pass);
#pragma warning restore CS8604 // Possible null reference argument for parameter 's' in 'string Library.MD5(string s)'.
				// đang dùng
				staff.AccountType = 2;
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdBranch"] = new SelectList(_context.Branches, "IdBranch", "IdBranch", staff.IdBranch);
            return View(staff);
        }

        // GET: Admin/Staff/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
			if (id == null || _context.Staff == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            ViewData["IdBranch"] = new SelectList(_context.Branches, "IdBranch", "LocationName", staff.IdBranch);
            return View(staff);
        }

        // POST: Admin/Staff/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,string passOld,[Bind("IdStaff,StaffName,StaffAddress,StaffPhone,StaffBirthday,StaffIdentification,StaffImage,UserName,Pass,AccountType,StaffAccountType,IdBranch,imageStaff")] Staff staff)
        {
            if (id != staff.IdStaff)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					if(staff.imageStaff != null)
					{
#pragma warning disable CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
						Library.Library.DeleteImage(staff.StaffImage);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
						staff.StaffImage =	Library.Library.UploadImage("Staff", staff.imageStaff);
					}
					if (staff.Pass != null) staff.Pass = Library.Library.MD5(staff.Pass);
					else staff.Pass = passOld;
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.IdStaff))
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
            ViewData["IdBranch"] = new SelectList(_context.Branches, "IdBranch", "IdBranch", staff.IdBranch);
            return View(staff);
        }

        // GET: Admin/Staff/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Staff == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.IdBranchNavigation)
                .FirstOrDefaultAsync(m => m.IdStaff == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Admin/Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Staff == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.Staff'  is null.");
            }
            var staff = await _context.Staff.FindAsync(id);
            if (staff != null)
            {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
				Library.Library.DeleteImage(staff.StaffImage);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
                _context.Staff.Remove(staff);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(string id)
        {
          return _context.Staff.Any(e => e.IdStaff == id);
        }
    }
}
