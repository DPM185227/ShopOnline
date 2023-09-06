using FashionStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FashionStore.Controllers
{
	public class HomeController : Controller
	{
		private readonly FashionStoreDbContext _context;

		public HomeController(FashionStoreDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			ViewData["listSize"] = _context.ProductSizes.ToList();
			ViewData["listColor"] = _context.ProductColors.Include(m => m.IdProductNavigation).ToList();
			ViewData["listLoaiSanPham"] = _context.ProductTypes.ToList();

			// productSale
			ViewBag.productSale = _context.SaleProductPercents.ToList();
			// get productOrderDetail
			ViewBag.orderTop5 = _context.OrderDetails.Include(m=>m.IdcolorNavigation).GroupBy(m => m.IdProduct).Select(s => s.First()).ToList();
			ViewBag.saleTop5 = _context.SaleProductPercents.OrderBy(m=>m.SalePercent).Where(m => DateTime.Now.Date >= m.StartDay.Date && DateTime.Now.Date <= m.EndDay.Date).Take(4).ToList();
			var listProduct = _context.Products.ToList();
			return View(listProduct);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		//
		public IActionResult LoadCategory()
		{
			var listProductType = _context.ProductTypes.Where(m => m.ViewInFrontEnd == 0).Select(s => new
			{
				IdType = s.Idtype,
				TypeName = s.TypeName,
				category = _context.Categories.Where(m => m.ViewInFrontEnd == 0 && m.Idtype == s.Idtype).ToList()
			}).ToList();
			return Json(listProductType);
		}

		// get thong tin mau
		[HttpGet]
		public IActionResult getInfoColor(int idColor)
		{
			var colorInfo = _context.ProductColors.Where(m => m.Idcolor == idColor).FirstOrDefault();
			if (colorInfo != null)
			{
				var jsonConvert = JsonConvert.SerializeObject(colorInfo);
				return Json(jsonConvert);
			}
			else
			{
				return Json("Faild");
			}

		}

		// product detail
		[HttpGet]
		public IActionResult ProductDetail(string idProduct)
		{
			ViewBag.listColor = _context.ProductColors.Where(m => m.IdProduct == idProduct).ToList();
            var product = _context.Products.Where(m => m.IdProduct == idProduct).FirstOrDefault();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            ViewBag.listProduct = _context.Products.Where(m => m.IdCategory == product.IdCategory).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			ViewBag.Tag = _context.Tags.Where(m => m.IdProduct == idProduct).ToList();
			ViewBag.Precent = _context.SaleProductPercents.Where(m => m.IdProduct == idProduct && (DateTime.Now.Date >= m.StartDay.Date && DateTime.Now.Date <= m.EndDay.Date)).FirstOrDefault();
			// get comment of product
			ViewBag.commentList = _context.Comments.Include(m=>m.IdCustomerNavigation).Where(m => m.IdProduct == idProduct).ToList();
			ViewBag.commentRep = _context.CommentReps.Include(m => m.IDStaffNavigation).Include(m=>m.IdCommentNavigation).ToList();
			ViewBag.checkCommentOrder = _context.OrderDetails.Include(m=>m.IdProductNavigation).Include(m=>m.IdorderNavigation).ToList();
			// 
			ViewBag.listCodeProvince = new SelectList(_context.Provinces.ToList(), "CodeProvince", "NameProvince");
			return View(product);
		}

		// get size
		[HttpGet]
		public IActionResult getSize(int idColor)
		{
			if(idColor != 0)
			{
				// trả về danh sách nếu muốn lấy một số cột không lấy toàn bảng thì dùng select ra các cột muốn lấy
				/* nếu dùng select phải dùng lamda => new: new dùng để khởi tạo một đối tượng mới bao gồm các thông tin muốn 
				 * lấy và đặt tên cho từng thuộc tính và gán giá trị cho chúng */
				var getColor = _context.ProductColors.Where(m=>m.Idcolor == idColor).FirstOrDefault();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				var listSize = _context.ProductSizes.Where(m => m.Idcolor == idColor)
				.Select(s => new
				{
					idSize = s.Idsize,
					nameSize = s.SizeName,
					// truy xuất xuống bảng Color, bảng Color truy xuất xuống product lấy thông tin của giá sản phẩm
					productPrice = _context.SaleProductPercents.Where(m => m.IdProduct == s.IdcolorNavigation.IdProduct).FirstOrDefault() != null
					? s.IdcolorNavigation.IdProductNavigation.ProductPrice - (s.IdcolorNavigation.IdProductNavigation.ProductPrice / _context.SaleProductPercents.Where(m => m.IdProduct == s.IdcolorNavigation.IdProduct).FirstOrDefault().SalePercent)
					: s.IdcolorNavigation.IdProductNavigation.ProductPrice,
					stringPathColor = s.IdcolorNavigation.ImageProductColor
				}).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				if (listSize.Count() > 0)
				{
					var convertJson = JsonConvert.SerializeObject(listSize);
					return Json(convertJson);
				}
			}
			return Json(null);
		}


		// get info size
		[HttpGet]
		public IActionResult getInfoSize(int idSize)
		{
			var getInfoSize = _context.ProductSizes.Where(m => m.Idsize == idSize).FirstOrDefault();
			if(getInfoSize != null)
			{
				return Json(getInfoSize.Delta);
			}
			return Json("Faild");
		}

		// get product type
		public IActionResult ViewProductType(string idType)
		{
			if(idType != null)
			{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				var productOfProductType = _context.Products.Include(m=>m.IdCategoryNavigation).Where(m => m.IdCategoryNavigation.Idtype == idType).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				ViewBag.listCategory = _context.Categories.Where(m => m.Idtype == idType).ToList();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				ViewBag.getNameProductType = _context.ProductTypes.Where(m => m.Idtype == idType).FirstOrDefault().TypeName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				ViewBag.listProductColor = _context.ProductColors.ToList();
				return View(productOfProductType);
			}
			else return NotFound();
		}

		public IActionResult ViewCategory(string idCategory)
		{
			if (idCategory != null)
			{
				var getProductCategory = _context.Products.Where(m => m.IdCategory == idCategory).ToList();
                ViewBag.listProductColor = _context.ProductColors.ToList();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				ViewBag.nameCateogry = _context.Categories.Where(m => m.IdCategory == idCategory).FirstOrDefault().CategoryName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                return View(getProductCategory);
			}
			else return NotFound();
		}


		public IActionResult ViewTag(string nameTag)
		{
			if (nameTag != null)
			{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				var getListProductTag = _context.Tags.Where(m => m.NameTag == nameTag).Include(m => m.IdProductNavigation)
					.Select(s => new
					{
						IdProduct = s.IdProductNavigation.IdProduct,
						ProductName = s.IdProductNavigation.ProductName,
						ProductPrice = s.IdProductNavigation.ProductPrice,
					}).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				ViewBag.listProductColor = _context.ProductColors.ToList();
				return View(getListProductTag);
			}
			else return NotFound();
		}


		[HttpPost]
		public IActionResult Search(string keySearch)
		{
			if (keySearch != null)
			{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				var productList = _context.Products.Where(m => m.ProductName.Contains(keySearch)).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				ViewBag.listProductColor = _context.ProductColors.ToList();
				return View(productList);
			}
			else return RedirectToAction("Index");
		}

		///
		public IActionResult Blogs()
		{
			var Blogs = _context.Posts.Include(m=>m.IdStaffNavigation).Where(m=>m.StatusPost == 0).ToList();
			if (Blogs.Count() > 0) return View(Blogs);
			else return View();
		}

		public IActionResult ViewBlog(int id)
		{
			var blog = _context.Posts.Include(m => m.IdStaffNavigation).Where(m => m.IdBaiViet == id).FirstOrDefault();
			ViewBag.listComment = _context.Comments.Include(m => m.IdCustomerNavigation).Where(m => m.IdPost == id).ToList();
			if (blog != null)
			{
				blog.ViewPost++;
				_context.Posts.Update(blog);
				_context.SaveChanges();
				return View(blog);
			}
			else return View();
		}

		[HttpPost]
		public IActionResult PostComment(int id, string cmt)
		{
			if(cmt != null)
			{
				var commentPost = new Comment
				{
					IdCustomer = HttpContext.Session.GetString("_idCustomer"),
					IdPost = id,
					Content = cmt,
					CreateAt = DateTime.Now,
					StatusComment = 0
				};
				_context.Comments.Add(commentPost);
				_context.SaveChanges();
				return RedirectToAction("ViewBlog", new { id = id });
			}
			else
			{
				ViewBag.Error = "Vui lòng nhập nội dung bình luận";
				return View();
			}
		}


		public IActionResult DeleteComment(int id, int idPost)
		{
			var getComment = _context.Comments.Where(m => m.IdComment == id).FirstOrDefault();
			if(getComment != null)
			{
				_context.Remove(getComment);
				_context.SaveChanges();
				return RedirectToAction("ViewBlog", new { id = idPost });
			}
			return View();
		}

		// fitter product 
		// step 1: load view url
		public IActionResult Shop()
		{
			ViewBag.listColor = _context.ProductColors.Include(m => m.IdProductNavigation).ToList();
			ViewBag.listCategory = _context.Categories.ToList();
			var products = _context.Products.ToList();
			return View(products);
		}
		// step 2: load list product use ajax and jquery
		public IActionResult loadAllProduct()
		{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			var _listProduct = _context.Products.Select(s=> new
			{
				idProduct = s.IdProduct,
				nameProduct = s.ProductName,
                productPrecent = _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().SalePercent != null ? _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().SalePercent : 0,
                productPrice = _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault() != null
                    ? s.ProductPrice - (s.ProductPrice / _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().SalePercent)
                    : s.ProductPrice,
                imageProduct = _context.ProductColors.Where(m=>m.IdProduct == s.IdProduct).FirstOrDefault().ImageProductColor,
				listColor = _context.ProductColors.Where(m=>m.IdProduct == s.IdProduct).ToList()
			}).Take(3).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			if(_listProduct.Count() > 0)
			{
				return Json(new { code = 200, data=JsonConvert.SerializeObject(_listProduct), msg="Load thành công"}) ;
			}
			else
                return Json(new { code = 200, msg = "Load thất bại" });
        }

		// step 3: get categoy
		public  IActionResult getCategory()
		{
			var listCategory = _context.Categories.Select(s => new
			{
				idCategory = s.IdCategory,
				nameCategory = s.CategoryName
			}).ToList();
			if(listCategory.Count() > 0)
			{
				return Json(new
				{
					code = 200,
					data = JsonConvert.SerializeObject(listCategory),
					msg = "Load thành công"
				}) ;;
			}
			else return Json(new
			{
				code = 500,
				msg = "thất bại"
			}); ;
		}


		public IActionResult priceFilter(int priceFiilter)
		{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			var _listProduct = _context.Products.Where(m=>m.ProductPrice >= 0 && m.ProductPrice <= priceFiilter).Select(s => new
			{
				idProduct = s.IdProduct,
				nameProduct = s.ProductName,
				productPrecent = _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().SalePercent != null ? _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().SalePercent : 0,
				productPrice = _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault() != null
					? s.ProductPrice - (s.ProductPrice / _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().SalePercent)
					: s.ProductPrice,
				imageProduct = _context.ProductColors.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().ImageProductColor,
				listColor = _context.ProductColors.Where(m => m.IdProduct == s.IdProduct).ToList()
			}).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			if (_listProduct.Count() > 0)
			{
				return Json(new { code = 200, data = JsonConvert.SerializeObject(_listProduct), msg = "Load thành công" });
			}
			else
				return Json(new { code = 200, msg = "Load thất bại" });
		}


		public IActionResult categoryFilter(string idCategory)
		{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
			var _listProduct = _context.Products.Where(m => m.IdCategory == idCategory).Select(s => new
			{
				idProduct = s.IdProduct,
				nameProduct = s.ProductName,
				productPrecent = _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().SalePercent != null ? _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().SalePercent : 0,
				productPrice = _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault() != null
					? s.ProductPrice - (s.ProductPrice / _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().SalePercent)
					: s.ProductPrice,
				imageProduct = _context.ProductColors.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().ImageProductColor,
				listColor = _context.ProductColors.Where(m => m.IdProduct == s.IdProduct).ToList()
			}).ToList();
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			if (_listProduct.Count() > 0)
			{
				return Json(new { code = 200, data = JsonConvert.SerializeObject(_listProduct), msg = "Load thành công" });
			}
			else
				return Json(new { code = 500, msg = "Load thất bại" });
		}

        public IActionResult loadMore(int dataShow)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var _listProduct = _context.Products.Select(s => new
            {
                idProduct = s.IdProduct,
                nameProduct = s.ProductName,
				productPrecent = _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().SalePercent != null ? _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().SalePercent : 0,
				productPrice = _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault() != null
					? s.ProductPrice - (s.ProductPrice / _context.SaleProductPercents.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().SalePercent)
					: s.ProductPrice,
				imageProduct = _context.ProductColors.Where(m => m.IdProduct == s.IdProduct).FirstOrDefault().ImageProductColor,
                listColor = _context.ProductColors.Where(m => m.IdProduct == s.IdProduct).ToList()
            }).Take(dataShow).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS0472 // The result of the expression is always 'true' since a value of type 'int' is never equal to 'null' of type 'int?'
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (_listProduct.Count() > 0)
            {
                return Json(new { code = 200, data = JsonConvert.SerializeObject(_listProduct), msg = "Load thành công" });
            }
            else
                return Json(new { code = 200, msg = "Load thất bại" });
        }


		[HttpPost]
		public IActionResult getBranch(string idProvince, string idDistrict, string idProduct)
		{
			// get Branch product
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			var getProductBranch = _context.Products.Where(m => m.IdProduct == idProduct).FirstOrDefault().IdBranch;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			// check quantiy > 0
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			var checkQuatity = _context.ProductSizes.Include(m=>m.IdcolorNavigation).Where(m => m.QuanityProduct > 0 && m.IdcolorNavigation.IdProduct == idProduct).FirstOrDefault();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
			if (checkQuatity != null)
			{
				// get list brand
				var getBranch = _context.Branches.Where(m => m.IdBranch == getProductBranch && m.CodeDistrict == idDistrict && m.CodeProvince == idProvince)
					.Select(s => new
					{
						nameBranch = s.LocationName,
						locationMap = s.LoactionMap
					}).ToList();
				// = : gán giá trị == so sánh
				if (getBranch.Count() > 0)
				{
					return Json(new { code = 200, data = JsonConvert.SerializeObject(getBranch), type = 0, msg = "Lấy ds thành công" });
				}
				else
				{
					var goiY = _context.Branches.Where(m => m.IdBranch == getProductBranch && m.CodeProvince == idProvince).Select(s => new
					{
						nameBranch = s.LocationName,
						locationMap = s.LoactionMap
					}).ToList();
					if (goiY.Count() > 0)
						return Json(new { code = 200, data = JsonConvert.SerializeObject(goiY), type = 1, msg = "Lấy ds thành công" });
					else return Json(null);
				}
			}
			else return Json(new { code = 500, msg = "Lấy ds thất bại" });

        }
		// GET: Admin/Customer/Edit/5
		public IActionResult UpdateCustomer()
		{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
			string id = HttpContext.Session.GetString("_idCustomer");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
			if (id == null || _context.Customers == null)
			{
				return NotFound();
			}

			var customer = _context.Customers.Where(m=>m.IdCustomer == id).FirstOrDefault();
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
		public IActionResult UpdateCustomer(string oldPass,string oldImage, Customer customer)
		{
			if (ModelState.IsValid)
			{
				HttpContext.Session.SetString("_nameCustomer", customer.CustomerName);
				if (customer.Pass != null)
				{
					customer.Pass = Library.Library.MD5(customer.Pass);
				}
				else customer.Pass = oldPass;

				if (customer.imageCustomer != null)
				{
#pragma warning disable CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
					Library.Library.DeleteImage(customer.CustomerImage);
#pragma warning restore CS8604 // Possible null reference argument for parameter 'pathDelete' in 'bool Library.DeleteImage(string pathDelete)'.
					customer.CustomerImage = Library.Library.UploadImage("Customer", customer.imageCustomer);
					HttpContext.Session.SetString("_imgCustomer", customer.CustomerImage);
				}
				else customer.CustomerImage = oldImage;
				_context.Customers.Update(customer);
				_context.SaveChanges();
				return RedirectToAction("Index","Home");
			}
			return View(customer);
		}



	}
}
