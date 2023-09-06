using FashionStore.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FashionStore.Controllers
{
	public class CartController : Controller
	{
		private readonly FashionStoreDbContext _context;

		public CartController(FashionStoreDbContext context)
		{
			_context = context;
		}

		// Add to card
		[HttpGet]
		public IActionResult AddToCart(int productColor, string productId, int productSize, int quantityBuy)
		{
			string result = "";
			bool checkLogin = true;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			string customerID = HttpContext.Session.GetString("_idCustomer");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
			if (customerID != null)
			{
				var size = _context.ProductSizes.Where(m => m.Idsize == productSize).FirstOrDefault();
				var product = _context.Products.Where(m => m.IdProduct == productId).FirstOrDefault();

				// get cart customer
				var getCartCustomer = _context.Carts.Where(m => m.ID_Customer == customerID).ToList();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				if (quantityBuy > size.QuanityProduct)
				{
					result = "Số lượng sản phẩm hiện không còn đủ";
				}
				else
				{
					// check product exit cart
					var productExit = getCartCustomer.Where(m => m.ID_Product == productId && m.Idcolor == productColor && m.Idsize == productSize).FirstOrDefault();
					// exit
					if(productExit != null)
					{
						if(size.QuanityProduct > quantityBuy)
						{
							productExit.QuantityBuy += quantityBuy;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
							productExit.Total += (product.ProductPrice + size.Delta) * quantityBuy;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
							_context.Update(productExit);
							size.QuanityProduct -= quantityBuy;
							_context.Update(size);
							_context.SaveChanges();
							result = "Thêm vào giỏ hàng thành công";
						}
						else result = "Số lượng hàng không còn đủ";
					}
					else
					{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
						int total = (product.ProductPrice + size.Delta) * quantityBuy;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
						var cart = new Cart()
						{
							Idcolor = productColor,
							Idsize = productSize,
							ID_Product = productId,
							QuantityBuy = quantityBuy,
							ID_Customer = customerID,
							Total = total
						};
						_context.Add(cart);
						int reusltSave = _context.SaveChanges();
						if (reusltSave == 1)
						{
							result = "Thêm vào giỏ hàng thành công";
							size.QuanityProduct -= quantityBuy;
							_context.Update(size);
							_context.SaveChanges();
						}
						else result = "Thêm vào giỏ hàng thất bại";
					}
				}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			}
			else
			{
				checkLogin = false;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
				result = Url.Action("LoginCustomer","Customer");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
			}
			return Json(new { checkLogin, result });
		}

		// load list Cart
		public IActionResult Index()
		{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			string getCustomer = HttpContext.Session.GetString("_idCustomer");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
			if(getCustomer == null) {
				return RedirectToAction("LoginCustomer", "Customer");
			}
			var listCartItem = _context.Carts.Include(m=>m.IdProductNavigation)
				.Include(m=>m.IdcolorNavigation).Include(m=>m.IdsizeNavigation).
				Where(m => m.ID_Customer == getCustomer);
			ViewBag.CodeProvince = new SelectList(_context.Provinces.ToList(), "CodeProvince", "NameProvince");
			return View(listCartItem);
		}

		// delete Cart
		public IActionResult DeleteCart(int getCartID)
		{
			string result = "";
			bool ok = true;
			var getCartItem = _context.Carts.Where(m => m.IdCart == getCartID).FirstOrDefault();
			if(getCartItem != null)
			{
				var getProductQuanity = _context.ProductSizes.Where(m => m.Idsize == getCartItem.Idsize).FirstOrDefault();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				getProductQuanity.QuanityProduct += getCartItem.QuantityBuy;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				_context.Remove(getCartItem);
				_context.Update(getProductQuanity);
				int complete = _context.SaveChanges();
				if (complete > 1)
				{
					result = "Xóa thành công";
				}
				else {
					ok = false; result = "Xóa thất bại";
				};
			}
			return Json(new { ok, result });
		}

		// update cart up tăng
		[HttpPost]
		public IActionResult UpdateUp(int cartID)
		{
			string alert = "";
			var cartItem = _context.Carts.Where(m => m.IdCart == cartID).FirstOrDefault();
			if(cartItem != null)
			{
				var productPrice = _context.Products.Where(m => m.IdProduct == cartItem.ID_Product).FirstOrDefault();
				var productQuanity = _context.ProductSizes.Where(m => m.Idsize == cartItem.Idsize).FirstOrDefault();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				if(productQuanity.QuanityProduct < 1)
				{
					alert = "Sản phẩm đã hết. Xin lỗi vì sự bất tiện này!!!";
				}
				else
				{
					// update số lượng
					cartItem.QuantityBuy++;
					// update giá tiền
#pragma warning disable CS8602 // Dereference of a possibly null reference.
					cartItem.Total += (productPrice.ProductPrice + productQuanity.Delta);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
					// update số lượng sản phẩm
					productQuanity.QuanityProduct--;
					_context.Update(cartItem);
					_context.Update(productQuanity);
					int resultUpdate = _context.SaveChanges();
					if (resultUpdate > 1)
					{
						alert = "Cập nhật thành công";
					}
					else alert = "Cập nhật thất bại";
				}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			}
			return Json(alert);
		}

		// update giảm
		[HttpPost]
		public IActionResult UpdateDown(int cartID)
		{
			string alert = "";
			var cartItem = _context.Carts.Where(m => m.IdCart == cartID).FirstOrDefault();
			if (cartItem != null)
			{
				var productPrice = _context.Products.Where(m => m.IdProduct == cartItem.ID_Product).FirstOrDefault();
				var productQuanity = _context.ProductSizes.Where(m => m.Idsize == cartItem.Idsize).FirstOrDefault();
				// update số lượng
				cartItem.QuantityBuy--;
				// update giá tiền
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				cartItem.Total -= (productPrice.ProductPrice + productQuanity.Delta);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				productQuanity.QuanityProduct ++;
				_context.Update(cartItem);
				_context.Update(productQuanity);
				int resultUpdate = _context.SaveChanges();
				if (resultUpdate > 1)
				{
					alert = "Cập nhật thành công";
				}
				else alert = "Cập nhật thất bại";
				// check số lượng 0 xóa để không bị âm
				if (cartItem.QuantityBuy == 0)
				{
					_context.Remove(cartItem);
					int resultDelete = _context.SaveChanges();
					if (resultDelete > 0)
					{
						alert = "Cập nhật thành công";
					}
					else alert = "Cập nhật thất bại";
				}
			}
			return Json(alert);
		}

		// get huyện
		[HttpPost]
		public IActionResult getDistrict(string provinceID)
		{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			string customerID = HttpContext.Session.GetString("_idCustomer");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
			var getDistrict = _context.Districts.Where(m => m.ProvinceId == provinceID).
				Select(s => new
				{
					idDistrict = s.CodeDistrict,
					nameDistrict = s.NameDistrict,
					priceShipping = _context.OrderShippings.Where(m => m.CodeProvince == provinceID).Select(m => m.Price).FirstOrDefault(),
					sumOrder = _context.Carts.Where(m=>m.ID_Customer == customerID).Sum(m=>m.Total)
				}).ToList();
			if (getDistrict != null)
			{
				var resultJson = JsonConvert.SerializeObject(getDistrict);
				return Json(resultJson);
			}
			else return Json(null);
		}

		// get xã
		[HttpPost]
		public IActionResult getCoummune(string distrcitID)
		{
			var getDistrict = _context.Communes.Where(m => m.CodeDistrict == distrcitID).
				Select(s => new
				{
					idCommune = s.CommuneId,
					nameCoummune = s.NameCommune,
				}).ToList();
			if (getDistrict != null)
			{
				var resultJson = JsonConvert.SerializeObject(getDistrict);
				return Json(resultJson);
			}
			else return Json(null);
		}

		// check out
		[HttpPost]
		public IActionResult CheckOut(int total, int shipping, string phoneOrder, string nameOrder, string addressOrder)
		{
			string idCustomer = "";
			bool isSuccess = true;
			string isAlert = "";
			// send maik
			string Subject = "XÁC NHẬN ĐƠN HÀNG TỪ CỬA HÀNG FASHION STORE";
			string bodyGmail = "";
			int stt = 0;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			if (HttpContext.Session.GetString("_idCustomer") != null) idCustomer = HttpContext.Session.GetString("_idCustomer");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
			var checkOut = new Order()
			{
				Idorder = Guid.NewGuid().ToString(),
				IdCustomer = idCustomer,
				ShippingPrice = shipping,
				FullName = nameOrder,
				Phone = phoneOrder,
				AddressSend = addressOrder,
				TotalAll = total,
				// đơn hàng mới
				TypeOrder = 0,
				CreateAt = DateTime.Now,
			};
			_context.Add(checkOut);
			_context.SaveChanges();

			bodyGmail += "<h3> Xác nhận đơn hàng từ FASHION STORE </h3>";
			bodyGmail += "<p>Người nhận: "+ nameOrder + "</p>";
			bodyGmail += "<p>Địa chỉ: "+addressOrder+"</p>";
			bodyGmail += "<p>SĐT: "+phoneOrder+"</p>";
			bodyGmail += "<p>Phí vận chuyển: " + shipping.ToString("N0") + " VNĐ</p>";
			bodyGmail += "<p>Tổng tiền: " + total.ToString("N0") + " VNĐ</p>";
			bodyGmail += "<p>Ngày đặt hàng: "+DateTime.Now.ToShortDateString()+"</p>";
			string pathOrderConfirmation = "https://localhost:44302/Cart/orderConfirm/?id=" + checkOut.Idorder;
			bodyGmail += "<p><b>Lưu ý:</b>Khách hàng vui lòng <a href="+pathOrderConfirmation+ "> <b>xác nhận</b></a> để cửa hàng tiến hành lên đơn</p>";

			bodyGmail += "<table>";
			bodyGmail += "<tr>";
			bodyGmail += "<th>STT</th>";
			bodyGmail += "<th>Tên sản phẩm</th>";
			bodyGmail += "<th>Màu sắc</th>";
			bodyGmail += "<th>Size</th>";
			bodyGmail += "<th>Số lượng</th>";
			bodyGmail += "<th>Tổng tiền</th>";
			bodyGmail += "</tr>";
			foreach (var item in _context.Carts.Include(m=>m.IdProductNavigation)
				.Include(m=>m.IdsizeNavigation)
				.Include(m=>m.IdcolorNavigation).
				Where(m=>m.ID_Customer == idCustomer).ToList())
			{
				var orderDetail = new OrderDetail()
				{
					Idorder = checkOut.Idorder,
					IdProduct = item.ID_Product,
					Idcolor = item.Idcolor,
					Idsize = item.Idsize,
					QuantityBuy = item.QuantityBuy,
					Total = item.Total
				};

				stt++;
				bodyGmail += "<tr>";
				bodyGmail += "<td>"+ stt +"</td>";
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				bodyGmail += "<td>"+item.IdProductNavigation.ProductName+"</td>";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				bodyGmail += "<td>"+item.IdcolorNavigation.ColorName+"</td>";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				bodyGmail += "<td>"+item.IdsizeNavigation.SizeName+"</td>";
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				bodyGmail += "<td>"+item.QuantityBuy+"</td>";
#pragma warning disable CS8629 // Nullable value type may be null.
				bodyGmail += "<td>"+item.Total.Value.ToString("N0")+" VNĐ</td>";
#pragma warning restore CS8629 // Nullable value type may be null.
				bodyGmail += "</tr>";
				_context.Add(orderDetail);
				//xoa tat ca trong gio hang
				_context.Remove(item);
				int success = _context.SaveChanges();
				if (success > 0)
				{
					isSuccess = true;
					isAlert = "Cảm ơn bạn đã mua hàng tại cửa hàng. Chúc quý khách một ngày an lành";
				}
				else
				{
					isAlert = "Mua hàng thất bại.Chân thành xin lỗi vì sự bất tiện này";
					isSuccess = false;
				}
			}
			bodyGmail += "<table>";
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			string gmailCustomer = HttpContext.Session.GetString("_gmailCustomer").ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			Library.Library.sendMail_UseGmail(gmailCustomer, Subject, bodyGmail);;
			return Json(new { isSuccess,isAlert });
		}


		public IActionResult orderConfirm(string id)
		{
			var getOrder = _context.Orders.Where(m => m.Idorder == id).FirstOrDefault();
			if(getOrder != null)
			{
				// đơn hàng được duyệt
				getOrder.TypeOrder = 1;
				getOrder.UpdateAt = DateTime.Now;
				_context.Update(getOrder);
				_context.SaveChanges();

				return RedirectToAction("Index", "Home");
			}
			else
			{
				return NotFound();
			}
#pragma warning disable CS0162 // Unreachable code detected
			return View();
#pragma warning restore CS0162 // Unreachable code detected
		}
	}
}
