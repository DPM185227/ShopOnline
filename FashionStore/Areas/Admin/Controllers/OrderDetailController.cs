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
    public class OrderDetailController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public OrderDetailController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/OrderDetail
        public IActionResult Index(string id,int? page)
        {
            var fashionStoreDbContext = _context.OrderDetails.Include(o => o.IdProductNavigation).Include(o => o.IdcolorNavigation).Include(o => o.IdorderNavigation).Include(o => o.IdsizeNavigation);
			ViewBag.orderInfo = _context.Orders.Where(m => m.Idorder == id).FirstOrDefault();
			return View(fashionStoreDbContext.ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/OrderDetail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.IdProductNavigation)
                .Include(o => o.IdcolorNavigation)
                .Include(o => o.IdorderNavigation)
                .Include(o => o.IdsizeNavigation)
                .FirstOrDefaultAsync(m => m.IdOrderDetail == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: Admin/OrderDetail/Create
        public IActionResult Create()
        {
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct");
            ViewData["Idcolor"] = new SelectList(_context.ProductColors, "Idcolor", "Idcolor");
            ViewData["Idorder"] = new SelectList(_context.Orders, "Idorder", "Idorder");
            ViewData["Idsize"] = new SelectList(_context.ProductSizes, "Idsize", "Idsize");
            return View();
        }

        // POST: Admin/OrderDetail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdOrderDetail,Idorder,IdProduct,Idcolor,Idsize,QuantityBuy,Total")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", orderDetail.IdProduct);
            ViewData["Idcolor"] = new SelectList(_context.ProductColors, "Idcolor", "Idcolor", orderDetail.Idcolor);
            ViewData["Idorder"] = new SelectList(_context.Orders, "Idorder", "Idorder", orderDetail.Idorder);
            ViewData["Idsize"] = new SelectList(_context.ProductSizes, "Idsize", "Idsize", orderDetail.Idsize);
            return View(orderDetail);
        }

        // GET: Admin/OrderDetail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", orderDetail.IdProduct);
            ViewData["Idcolor"] = new SelectList(_context.ProductColors, "Idcolor", "Idcolor", orderDetail.Idcolor);
            ViewData["Idorder"] = new SelectList(_context.Orders, "Idorder", "Idorder", orderDetail.Idorder);
            ViewData["Idsize"] = new SelectList(_context.ProductSizes, "Idsize", "Idsize", orderDetail.Idsize);
            return View(orderDetail);
        }

        // POST: Admin/OrderDetail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdOrderDetail,Idorder,IdProduct,Idcolor,Idsize,QuantityBuy,Total")] OrderDetail orderDetail)
        {
            if (id != orderDetail.IdOrderDetail)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.IdOrderDetail))
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
            ViewData["IdProduct"] = new SelectList(_context.Products, "IdProduct", "IdProduct", orderDetail.IdProduct);
            ViewData["Idcolor"] = new SelectList(_context.ProductColors, "Idcolor", "Idcolor", orderDetail.Idcolor);
            ViewData["Idorder"] = new SelectList(_context.Orders, "Idorder", "Idorder", orderDetail.Idorder);
            ViewData["Idsize"] = new SelectList(_context.ProductSizes, "Idsize", "Idsize", orderDetail.Idsize);
            return View(orderDetail);
        }

        // GET: Admin/OrderDetail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderDetails == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.IdProductNavigation)
                .Include(o => o.IdcolorNavigation)
                .Include(o => o.IdorderNavigation)
                .Include(o => o.IdsizeNavigation)
                .FirstOrDefaultAsync(m => m.IdOrderDetail == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: Admin/OrderDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderDetails == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.OrderDetails'  is null.");
            }
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
          return _context.OrderDetails.Any(e => e.IdOrderDetail == id);
        }
    }
}
