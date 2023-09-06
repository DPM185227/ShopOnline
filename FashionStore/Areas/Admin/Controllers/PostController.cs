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
    public class PostController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public PostController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Post
        public IActionResult Index(int? page)
        {
            var fashionStoreDbContext = _context.Posts.Include(p => p.IdStaffNavigation).ToList();
            return View(fashionStoreDbContext.ToPagedList(page ?? 1,5));
        }

        // GET: Admin/Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.IdBaiViet == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Admin/Post/Create
        public IActionResult Create()
        {
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "IdStaff");
            return View();
        }

        // POST: Admin/Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("IdBaiViet,IdStaff,Title,DescriptionPost,ViewPost,Content,Banner,StatusPost, banner")] Post post)
        {
            if (ModelState.IsValid)
            {
				if (HttpContext.Session.GetString("_IDStaff") != null) post.IdStaff = HttpContext.Session.GetString("_IDStaff");
				if (post.banner != null)
				{
					post.Banner = Library.Library.UploadImage("Post",post.banner);
				}
				else
				{
					ViewBag.Error = "Vui lòng chọn hình ảnh";
					return View(post);
				}
				post.ViewPost = 0;
				post.StatusPost = 0;
				_context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "IdStaff", post.IdStaff);
            return View(post);
        }

        // GET: Admin/Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "IdStaff", post.IdStaff);
            return View(post);
        }

        // POST: Admin/Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBaiViet,IdStaff,Title,DescriptionPost,ViewPost,Content,Banner,StatusPost")] Post post)
        {
            if (id != post.IdBaiViet)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.IdBaiViet))
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
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "IdStaff", post.IdStaff);
            return View(post);
        }

        // GET: Admin/Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.IdBaiViet == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return _context.Posts.Any(e => e.IdBaiViet == id);
        }

		[HttpGet]
		public IActionResult updateCheck(int idPost)
		{
			string success = "";
			var post = _context.Posts.Where(m => m.IdBaiViet == idPost).FirstOrDefault();
			post.StatusPost = post.StatusPost == 1 ? post.StatusPost = 0 : post.StatusPost = 1;
			_context.Update(post);
			int result = _context.SaveChanges();
			if (result == 1) success = "Thành công";
			else success = "Thất bại";
			// biến : không cần khởi tạo, đối tượng: new để khởi tạo mới
			return Json(success);
		}
	}
}
