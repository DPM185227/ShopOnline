using FashionStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FashionStore.Controllers
{
	public class CustomerController : Controller
	{
		private readonly FashionStoreDbContext _context;

		public CustomerController(FashionStoreDbContext context)
		{
			_context = context;
		}
		// register
		public IActionResult RegisterAccount()
		{
			return View();
		}

		// register post
		[HttpPost]
		public IActionResult RegisterAccount(Customer customer)
		{
			if(ModelState.IsValid)
			{
				if(_context.Customers.Where(m=>m.Gmail == customer.Gmail).FirstOrDefault() != null
					|| _context.Customers.Where(m => m.Gmail == customer.CustomerPhone).FirstOrDefault() != null
					|| _context.Customers.Where(m => m.Gmail == customer.CurstomerIdentification).FirstOrDefault() != null)
				{
					ViewData["Error"] = "Thông tin bạn nhập đã trùng với thông tin của 1 khách hàng khác";
					return View(customer);
				}
				else
				{
					customer.IdCustomer = Guid.NewGuid().ToString();
					if (customer.imageCustomer != null)
					{
						customer.CustomerImage = Library.Library.UploadImage("Customer", customer.imageCustomer);
					}
#pragma warning disable CS8604 // Possible null reference argument for parameter 's' in 'string Library.MD5(string s)'.
					customer.Pass = Library.Library.MD5(customer.Pass);
#pragma warning restore CS8604 // Possible null reference argument for parameter 's' in 'string Library.MD5(string s)'.
					// chưa active
					customer.CustomerAccountType = 0;
					// code
					customer.Authentication_code = Guid.NewGuid().ToString();
					_context.Add(customer);
					_context.SaveChanges();
					// send mail
					string body = "Mã kích hoạt là " + customer.Authentication_code;
#pragma warning disable CS8604 // Possible null reference argument for parameter 'strTo' in 'bool Library.sendMail_UseGmail(string strTo, string strSubject, string strBody)'.
					if (Library.Library.sendMail_UseGmail(customer.Gmail, "XÁC NHẬN TÀI KHOẢN", body))
					{
						return RedirectToAction("LoginCustomer");
					}
					else
					{
						return View(customer);
					}
#pragma warning restore CS8604 // Possible null reference argument for parameter 'strTo' in 'bool Library.sendMail_UseGmail(string strTo, string strSubject, string strBody)'.
#pragma warning disable CS0162 // Unreachable code detected
					return RedirectToAction("LoginCustomer", "Customer");
#pragma warning restore CS0162 // Unreachable code detected
				}
			}
			// change
			return View(customer);
		}

		// get 
		[HttpGet]
		public IActionResult LoginCustomer()
		{
			return View();
		}

		// post
		[HttpPost]
		public IActionResult LoginCustomer(string userName, string passWord)
		{
			var customer = _context.Customers.Where(m => m.UserName == userName && m.Pass == Library.Library.MD5(passWord)).FirstOrDefault();
			if(customer != null)
			{
					// đăng ký session
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				HttpContext.Session.SetString("_idCustomer", customer.IdCustomer.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				HttpContext.Session.SetString("_imgCustomer", customer.CustomerImage.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				HttpContext.Session.SetString("_nameCustomer", customer.CustomerName.ToString());
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				HttpContext.Session.SetString("_gmailCustomer", customer.Gmail.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				// 
				if (customer.CustomerAccountType == 1)
				{
					return RedirectToAction("ActiveAccount");
				}
				else if(customer.CustomerAccountType == 0)
				{
					return RedirectToAction("Index", "Home");
				}
				else
				{
					return View();
				}
			}
			else
			{
				ViewBag.Error = "Tên đăng nhập hoặc tài khoản sai";
				return View();
			}
		}

		[HttpGet]
		public IActionResult ActiveAccount()
		{
			return View();
		}

		[HttpPost]
		public IActionResult ActiveAccount(string code)
		{
			var getCustomerId = HttpContext.Session.GetString("_idCustomer");
			if (getCustomerId != null)
			{
				var getCustomer = _context.Customers.Where(m => m.IdCustomer == getCustomerId).FirstOrDefault();
				if (getCustomer != null)
				{
					if(getCustomer.Authentication_code == code)
					{
						// active
						getCustomer.CustomerAccountType = 1;
						_context.Update(getCustomer);
						_context.SaveChanges();
						return RedirectToAction("Index", "Home");
					}
					else
					{
						ViewData["Error"] = "Mã kích hoạt không đúng";
						return View();
					}
				}
				else
				{
					ViewData["Error"] = "Người dùng không tồn tại";
					return View();
				}
			}
			else
			{
				return NotFound();
			}
		}

		// đăng xuất
		public IActionResult LogoutAccount()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}

		// đơn hàng của tôi
		public IActionResult MyOrder()
		{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			string idCustomer = HttpContext.Session.GetString("_idCustomer");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
			if(idCustomer != null)
			{
				var myOrders = _context.Orders.Include(m=>m.IdCustomerNavigation).Where(m => m.IdCustomer == idCustomer).ToList();
				ViewBag.myOrderDetail = _context.OrderDetails.Include(m => m.IdProductNavigation).Include(m => m.IdsizeNavigation)
					.Include(m => m.IdcolorNavigation).ToList();
				return View(myOrders);
			}
			else
			{
				return RedirectToAction("LoginCustomer", "Customer");
			}
#pragma warning disable CS0162 // Unreachable code detected
			return View();
#pragma warning restore CS0162 // Unreachable code detected
		}

		// comment
		[HttpPost]
		public IActionResult AddComment(string content,List<IFormFile> fileUpload, string idProduct)
		{
			if (HttpContext.Session.GetString("_commentSuccess") != null)
				HttpContext.Session.Remove("_commentSuccess");
			//...................//////////////////////////////
			if(content == null)
			{
				ViewBag.Error = "Vui lòng nhập nội dung bình luận";
				return View("ProductDetail");
			}
			else
			{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
				string idCustomer = HttpContext.Session.GetString("_idCustomer");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
				if(idCustomer!= null)
				{
					var comment = new Comment();
					comment.IdCustomer = idCustomer;
					comment.IdProduct = idProduct;
					comment.Content = content;
					if (fileUpload != null)
						comment.Folder = Library.Library.uploadMultipleFile(fileUpload);
					comment.StatusComment = 0;
					comment.CreateAt = DateTime.Now;

					_context.Add(comment);
					_context.SaveChanges();

					HttpContext.Session.SetString("_commentSuccess", "Bình luận thành công");

					return RedirectToAction("ProductDetail", "Home", new {idProduct = idProduct});
				}
				else
				{
					return RedirectToAction("LoginCustomer", "Customer");
				}
			}
#pragma warning disable CS0162 // Unreachable code detected
			return View();
#pragma warning restore CS0162 // Unreachable code detected
		}

        // load list comment
        [HttpPost]
        public IActionResult LoadComment(string idProduct)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var listComment = _context.Comments.Include(m => m.IdCustomerNavigation).Where(m => m.IdProduct == idProduct)
                .Select(comment => new
                {
                    idComment = comment.IdComment,
                    contentComment = comment.Content,
                    folderComment = comment.Folder,
                    statusComment = comment.StatusComment,
                    imageCustomer = comment.IdCustomerNavigation.CustomerImage,
                    nameCustomer = comment.IdCustomerNavigation.CustomerName,
                    createAt = comment.CreateAt
                })
                .ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            // check null
            if (listComment.Count() > 0)
            {
                var commentObj = JsonConvert.SerializeObject(listComment);
                return Json(commentObj);
            }
            else
            {
                return Json(null);
            }
#pragma warning disable CS0162 // Unreachable code detected
            return Json(null);
#pragma warning restore CS0162 // Unreachable code detected
        }

		// xóa comment
		public IActionResult DeleteComment(int idComment, string idProduct)
		{
			var getComment = _context.Comments.Where(m => m.IdComment == idComment).FirstOrDefault();
			if(getComment != null)
			{
#pragma warning disable CS8604 // Possible null reference argument for parameter 'path' in 'string[] Directory.GetFiles(string path)'.
				foreach(var file in Directory.GetFiles(getComment.Folder))
				{
					Library.Library.DeleteImageFolder(file);
				}
#pragma warning restore CS8604 // Possible null reference argument for parameter 'path' in 'string[] Directory.GetFiles(string path)'.
				Directory.Delete(getComment.Folder);
				_context.Remove(getComment);
				_context.SaveChanges();
				return RedirectToAction("ProductDetail", "Home", new { idProduct = idProduct });
			}
			else
			{
				return NotFound();
			}
#pragma warning disable CS0162 // Unreachable code detected
			return RedirectToAction("ProductDetail", "Home", new { idProduct = idProduct });
#pragma warning restore CS0162 // Unreachable code detected
		}

		[HttpPost]
		public IActionResult CancelOrder(string idOrder)
		{
			var getOrder = _context.Orders.Where(m => m.Idorder == idOrder).FirstOrDefault();
			if (getOrder != null)
			{
				// update type of 
				getOrder.TypeOrder = 9;
				getOrder.UpdateAt = DateTime.Now;
				_context.Orders.Update(getOrder);
				foreach (var itemOrder in _context.OrderDetails.Where(m => m.Idorder == idOrder).ToList())
				{
					var getProductQuanity = _context.ProductSizes.Where(m => m.Idsize == itemOrder.Idsize).FirstOrDefault();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
					getProductQuanity.QuanityProduct += itemOrder.QuantityBuy;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
					itemOrder.QuantityBuy = 0;
					itemOrder.Total = 0;
					_context.ProductSizes.Update(getProductQuanity);
				}
				getOrder.TotalAll = 0;
				int resultSave = _context.SaveChanges();
				if (resultSave > 0)
				{
					return Json("Cập nhật thành công");
				}
				else return Json("Cập nhật thất bại");
			}
			else return Json(null);
		}

    }
}
