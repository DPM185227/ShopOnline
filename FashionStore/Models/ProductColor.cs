using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models;

public partial class ProductColor
{
	[Display(Name = "Mã màu")]
	public int Idcolor { get; set; }

	[Display(Name = "Tên màu")]
	public string ColorName { get; set; } = null!;

	[Display(Name = "Màu hình ảnh sản phẩm")]
	public string? ImageProductColor { get; set; }

	[Display(Name = "Mặc định")]
	public short? Active { get; set; }

	[Display(Name = "Mã sản phẩm")]
	public string? IdProduct { get; set; }

	[Display(Name = "Hình ảnh màu")]
	public string? ImageColor { get; set; }

	[NotMapped]
	[Display(Name = "Hình ảnh màu")]
	public IFormFile? imageColor { get; set; }

	[NotMapped]
	[Display(Name = "Màu hình ảnh sản phẩm")]
	public IFormFile? imageProductColor { get; set; }

	[Display(Name = "Giỏ hàng")]
	public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

	[Display(Name = "Mã sản phẩm")]
	public virtual Product? IdProductNavigation { get; set; }

	[Display(Name = "Đặt hàng chi tiết")]
	public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

	[Display(Name = "Size sản phẩm")]
	public virtual ICollection<ProductSize> ProductSizes { get; } = new List<ProductSize>();
}
