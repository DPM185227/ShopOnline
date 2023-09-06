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
    public class CommentController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public CommentController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Comment
        public IActionResult Index(int? page)
        {
            var fashionStoreDbContext = _context.Comments.Include(c => c.IdCustomerNavigation).Include(c => c.IdProductNavigation);
            return View(fashionStoreDbContext.ToList().ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/Comment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.IdCustomerNavigation)
                .Include(c => c.IdProductNavigation)
                .FirstOrDefaultAsync(m => m.IdComment == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Admin/Comment/Create
        public IActionResult Create()
        {
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer");
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct");
            return View();
        }

        // POST: Admin/Comment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdComment,IdCustomer,IdProduct,IdPost,Content,Folder,StatusComment,CreateAt")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer", comment.IdCustomer);
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", comment.IdProduct);
            return View(comment);
        }

        // GET: Admin/Comment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer", comment.IdCustomer);
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", comment.IdProduct);
            return View(comment);
        }

        // POST: Admin/Comment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdComment,IdCustomer,IdProduct,IdPost,Content,Folder,StatusComment,CreateAt")] Comment comment)
        {
            if (id != comment.IdComment)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.IdComment))
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
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "IdCustomer", "IdCustomer", comment.IdCustomer);
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", comment.IdProduct);
            return View(comment);
        }

        // GET: Admin/Comment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.IdCustomerNavigation)
                .Include(c => c.IdProductNavigation)
                .FirstOrDefaultAsync(m => m.IdComment == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Admin/Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comments == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.Comments'  is null.");
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {

#pragma warning disable CS8604 // Possible null reference argument for parameter 'path' in 'string[] Directory.GetFiles(string path)'.
				foreach (var file in Directory.GetFiles(comment.Folder))
				{
					Library.Library.DeleteImageFolder(file);
				}
#pragma warning restore CS8604 // Possible null reference argument for parameter 'path' in 'string[] Directory.GetFiles(string path)'.
				Directory.Delete(comment.Folder);
				_context.Comments.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
          return _context.Comments.Any(e => e.IdComment == id);
        }

		[HttpPost]
		public IActionResult updateCheck(int idComment)
		{
			var getComment = _context.Comments.Where(m=>m.IdComment == idComment).FirstOrDefault();
			if(getComment != null)
			{
				if (getComment.StatusComment == 1) getComment.StatusComment = 0;
				else getComment.StatusComment = 1;
				_context.Update(getComment);
				if (_context.SaveChanges() > 0) return Json("Cập nhật thành công");
				else Json("Cập nhật thất bại");
			}
			return Json(null);
		}

	}
}
