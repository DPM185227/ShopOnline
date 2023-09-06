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
    public class CustomerController : Controller
    {
        private readonly FashionStoreDbContext _context;

        public CustomerController(FashionStoreDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Customer
        public IActionResult Index(int? page)
        {
              return View(_context.Customers.ToList().ToPagedList(page ?? 1, 5));
        }

        // GET: Admin/Customer/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.IdCustomer == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Admin/Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCustomer,CustomerName,CustomerAddress,CustomerPhone,CurstomerBirtday,CurstomerIdentification,CustomerImage,UserName,Pass,CustomerAccountType,Authentication_code")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Admin/Customer/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Admin/Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdCustomer,CustomerName,CustomerAddress,CustomerPhone,CurstomerBirtday,CurstomerIdentification,CustomerImage,UserName,Pass,CustomerAccountType,Authentication_code")] Customer customer)
        {
            if (id != customer.IdCustomer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.IdCustomer))
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
            return View(customer);
        }

        // GET: Admin/Customer/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.IdCustomer == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Admin/Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'FashionStoreDbContext.Customers'  is null.");
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(string id)
        {
          return _context.Customers.Any(e => e.IdCustomer == id);
        }

		// update active
		[HttpGet]
		public IActionResult updateCheck(string idCustomer)
		{
			string success = "";
			var customer = _context.Customers.Where(m => m.IdCustomer == idCustomer).FirstOrDefault();
			// hidden category
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			customer.CustomerAccountType = customer.CustomerAccountType == 1 ? customer.CustomerAccountType = 0 : customer.CustomerAccountType = 1;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			customer.Authentication_code = Guid.NewGuid().ToString();
			_context.Update(customer);
			int result = _context.SaveChanges();
			string body = "Mã kích hoạt là " + customer.Authentication_code;
			if (result == 1)
			{
				success = "Thành công";
#pragma warning disable CS8604 // Possible null reference argument for parameter 'strTo' in 'bool Library.sendMail_UseGmail(string strTo, string strSubject, string strBody)'.
				Library.Library.sendMail_UseGmail(customer.Gmail, "Mã xác nhận", body);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'strTo' in 'bool Library.sendMail_UseGmail(string strTo, string strSubject, string strBody)'.
			}
			else success = "Thất bại";
			// biến : không cần khởi tạo, đối tượng: new để khởi tạo mới
			return Json(success);
		}
	}
}
