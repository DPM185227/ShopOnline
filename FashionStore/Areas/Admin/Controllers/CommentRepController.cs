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
    public class CommentRepController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public CommentRepController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CommentRep
        public IActionResult Index(int idComment, int? page)
        {
#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
			if (idComment != null) HttpContext.Session.SetInt32("_idComment", idComment);
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
            var fashionStoreDbContext = _context.CommentReps.Include(c => c.IdCommentNavigation).Where(m=>m.IdComment == idComment).ToList();
            return View(fashionStoreDbContext.ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/CommentRep/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CommentReps == null)
            {
                return NotFound();
            }

            var commentRep = await _context.CommentReps
                .Include(c => c.IdCommentNavigation)
                .FirstOrDefaultAsync(m => m.IdcommentRep == id);
            if (commentRep == null)
            {
                return NotFound();
            }

            return View(commentRep);
        }

        // GET: Admin/CommentRep/Create
        public IActionResult Create()
        {
            ViewData["IdComment"] = new SelectList(_context.Comments, "IdComment", "IdComment");
            return View();
        }

        // POST: Admin/CommentRep/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdcommentRep,Content,IDStaff,StatusComment,IdComment")] CommentRep commentRep)
        {
            if (ModelState.IsValid)
            {
				if (HttpContext.Session.GetInt32("_idComment") != null) commentRep.IdComment = HttpContext.Session.GetInt32("_idComment");
				if (HttpContext.Session.GetString("_IDStaff") != null) commentRep.ID_Staff = HttpContext.Session.GetString("_IDStaff");
				commentRep.StatusComment = 0;
				_context.Add(commentRep);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),new {commentRep.IdComment});
            }
            ViewData["IdComment"] = new SelectList(_context.Comments, "IdComment", "IdComment", commentRep.IdComment);
            return View(commentRep);
        }

        // GET: Admin/CommentRep/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CommentReps == null)
            {
                return NotFound();
            }

            var commentRep = await _context.CommentReps.FindAsync(id);
            if (commentRep == null)
            {
                return NotFound();
            }
            ViewData["IdComment"] = new SelectList(_context.Comments, "IdComment", "IdComment", commentRep.IdComment);
            return View(commentRep);
        }

        // POST: Admin/CommentRep/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		/*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdcommentRep,Content,IDStaff,StatusComment,IdComment")] CommentRep commentRep)
        {
            if (id != commentRep.IdcommentRep)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentRep);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentRepExists(commentRep.IdcommentRep))
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
            ViewData["IdComment"] = new SelectList(_context.Comments, "IdComment", "IdComment", commentRep.IdComment);
            return View(commentRep);
        }*/

        // GET: Admin/CommentRep/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CommentReps == null)
            {
                return NotFound();
            }

            var commentRep = await _context.CommentReps
                .Include(c => c.IdCommentNavigation)
                .FirstOrDefaultAsync(m => m.IdcommentRep == id);
            if (commentRep == null)
            {
                return NotFound();
            }

            return View(commentRep);
        }

        // POST: Admin/CommentRep/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CommentReps == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.CommentReps'  is null.");
            }
            var commentRep = await _context.CommentReps.FindAsync(id);
            if (commentRep != null)
            {
                _context.CommentReps.Remove(commentRep);
            }
            
            await _context.SaveChangesAsync();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return RedirectToAction(nameof(Index), new { commentRep.IdComment });
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private bool CommentRepExists(int id)
        {
          return _context.CommentReps.Any(e => e.IdcommentRep == id);
        }
    }
}
