using FashionStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;

namespace FashionStore.Areas.Admin.Controllers	
{
	[Area("Admin")]
	public class HomeController : Controller
	{
		private readonly FashionStoreDbContext _context;

		public HomeController(FashionStoreDbContext context)
		{
			
			_context = context;
		}

		[HttpGet]
		public IActionResult Dashboard()
		{
			// doanh thu ngày
#pragma warning disable CS8629 // Nullable value type may be null.
			var getOrderToday = _context.Orders.Where(m => m.CreateAt.Value.Date == DateTime.Now.Date).ToList();
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			var getOrderDetailToday = _context.OrderDetails.Include(m=>m.IdProductNavigation).Include(m=>m.IdorderNavigation).Where(m => m.IdorderNavigation.CreateAt.Value.Date == DateTime.Now.Date).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8629 // Nullable value type may be null.
			ViewBag.sumOrderToday = getOrderToday.Count();
			ViewBag.sumSaleToday = getOrderToday.Where(m=>m.TypeOrder == 5).Sum(m => m.TotalAll);
			//ViewBag.productSale = getOrderDetailToday.OrderByDescending(m => m.Total).Take(1).FirstOrDefault();
			// doanh thu tháng
#pragma warning disable CS8629 // Nullable value type may be null.
			var getOrderMonth = _context.Orders.Where(m => m.CreateAt.Value.Month == DateTime.Now.Month).ToList();
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8629 // Nullable value type may be null.
			var getOrderDetailMonth = _context.OrderDetails.Include(m => m.IdProductNavigation).Include(m => m.IdorderNavigation).Where(m => m.IdorderNavigation.CreateAt.Value.Month == DateTime.Now.Month).ToList();
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			ViewBag.sumOrderMonth = getOrderMonth.Count();
			ViewBag.sumSaleMonth = getOrderMonth.Where(m => m.TypeOrder == 5).Sum(m => m.TotalAll);
			// doanh thu năm
#pragma warning disable CS8629 // Nullable value type may be null.
			var getOrderYear = _context.Orders.Where(m => m.CreateAt.Value.Year == DateTime.Now.Year).ToList();
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8629 // Nullable value type may be null.
			var getOrderDetailYear = _context.OrderDetails.Include(m => m.IdProductNavigation).Include(m => m.IdorderNavigation).Where(m => m.IdorderNavigation.CreateAt.Value.Year == DateTime.Now.Year).ToList();
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			ViewBag.sumOrderYear = getOrderYear.Count();
			ViewBag.sumSaleYear = getOrderYear.Where(m => m.TypeOrder == 5).Sum(m => m.TotalAll);
			// thống kê người dùng nhân viên
			ViewBag.sumCustomer = _context.Customers.Count();
			ViewBag.sumStaff = _context.Staff.Count();
			// chart data
			List<DataPoint> dataPoints = new List<DataPoint>();
			foreach (var item in getOrderToday.Where(m=>m.TypeOrder == 5).ToList())
			{
#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8629 // Nullable value type may be null.
				dataPoints.Add(new DataPoint(
					item.CreateAt.Value.ToShortTimeString(),
					Double.Parse(item.TotalAll.Value.ToString())
				));
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8629 // Nullable value type may be null.
			}
			ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
			return View();
		}

		[HttpGet]
		public IActionResult LoginDashboard()
		{
			
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> LoginDashboard(Staff staff)
		{
#pragma warning disable CS0219 // The variable 'loginFail' is assigned but its value is never used
			int loginFail = 0;
#pragma warning restore CS0219 // The variable 'loginFail' is assigned but its value is never used
			if(staff.UserName != null && staff.Pass != null)
			{
				string passMD5 = Library.Library.MD5(staff.Pass);
				var getStaff = await _context.Staff.Where(m => m.UserName == staff.UserName && m.Pass == passMD5 && m.AccountType == 2).FirstOrDefaultAsync();
				if (getStaff != null)
				{
#pragma warning disable CS8604 // Possible null reference argument for parameter 'value' in 'void SessionExtensions.SetString(ISession session, string key, string value)'.
					HttpContext.Session.SetString("_IDStaff", getStaff.IdStaff);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'value' in 'void SessionExtensions.SetString(ISession session, string key, string value)'.
					HttpContext.Session.SetString("_NameStaff", getStaff.StaffName);
#pragma warning disable CS8604 // Possible null reference argument for parameter 'value' in 'void SessionExtensions.SetString(ISession session, string key, string value)'.
					HttpContext.Session.SetString("_ImageStaff", getStaff.StaffImage);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'value' in 'void SessionExtensions.SetString(ISession session, string key, string value)'.
#pragma warning disable CS8629 // Nullable value type may be null.
					HttpContext.Session.SetInt32("_AccountType", (int)getStaff.StaffAccountType);
#pragma warning restore CS8629 // Nullable value type may be null.
					return RedirectToAction(nameof(Dashboard), "Home");
				}
				else
				{
					ViewBag.errorlogin = "Đăng nhập thất bại";
					return View(staff);
				}
			}else
			{
				ViewBag.errorlogin = "Vui lòng nhập tài khoản mật khẩu";
				return View(staff);
			}
#pragma warning disable CS0162 // Unreachable code detected
			return View(staff);
#pragma warning restore CS0162 // Unreachable code detected
		}



#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
		public async Task<IActionResult> Logout()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
		{
			HttpContext.Session.Clear();
			return RedirectToAction(nameof(LoginDashboard), "Home");
		}


		[HttpGet]
		public IActionResult Statistical()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Result(DateTime startDay, DateTime endDay)
		{
			var countOrder = _context.Orders.Where(m=> m.CreateAt >= startDay.Date && m.CreateAt <= endDay.Date).Count();
			var orderSum = _context.Orders.Where(m=>m.CreateAt >= startDay.Date && m.CreateAt <= endDay.Date).Sum(m => m.TotalAll);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			var orderProduct = _context.OrderDetails.Include(m=>m.IdorderNavigation).Where(m=>m.IdorderNavigation.CreateAt >= startDay.Date && m.IdorderNavigation.CreateAt <= endDay.Date).Sum(m => m.QuantityBuy);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			// count order confirm 
			var orderConfirm = _context.Orders.Where(m => m.CreateAt >= startDay.Date && m.CreateAt <= endDay.Date).Count(m => m.TypeOrder == 6);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			var getCustomer = _context.Orders.Include(m=>m.IdCustomerNavigation).Where(m => m.CreateAt >= startDay.Date && m.CreateAt <= endDay.Date).Max(m => m.IdCustomerNavigation.CustomerName);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			// order avg day
			TimeSpan totalDay = endDay.Date - startDay.Date;
			int countDay = (int)totalDay.TotalDays;
			// avg 1 day
			int avg1Day = Convert.ToInt32(orderSum) / countDay;

			return Json(new
			{
				countOrder = countOrder,
                orderSum = orderSum,
                orderProduct = orderProduct,
                orderConfirm = orderConfirm,
				getCustomer = getCustomer,
				avg1Day = avg1Day,
            });

        }
		// 
		public async Task<IActionResult> StaffUpdate()
		{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			string id = HttpContext.Session.GetString("_IDStaff");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
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
		public async Task<IActionResult> StaffUpdate(string id, string passOld, [Bind("IdStaff,StaffName,StaffAddress,StaffPhone,StaffBirthday,StaffIdentification,StaffImage,UserName,Pass,AccountType,StaffAccountType,IdBranch,imageStaff")] Staff staff)
		{
			if (id != staff.IdStaff)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				if (staff.imageStaff != null)
				{
#pragma warning disable CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
					Library.Library.DeleteImage(staff.StaffImage);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
					staff.StaffImage = Library.Library.UploadImage("Staff", staff.imageStaff);
				}
				if (staff.Pass != null) staff.Pass = Library.Library.MD5(staff.Pass);
				else staff.Pass = passOld;
				_context.Update(staff);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["IdBranch"] = new SelectList(_context.Branches, "IdBranch", "IdBranch", staff.IdBranch);
			return View(staff);
		}

	}
}
