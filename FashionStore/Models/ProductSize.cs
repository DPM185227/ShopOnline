using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class ProductSize
{
	[Display(Name = "Mã size")]
	public int Idsize { get; set; }

	[Display(Name = "Tên size")]
	public string SizeName { get; set; } = null!;

	[Display(Name = "Giá tiền cộng vào")]
	public int Delta { get; set; }

	[Display(Name = "Số lượng sản phẩm")]
	public int QuanityProduct { get; set; }

	[Display(Name = "Mã màu")]
	public int? Idcolor { get; set; }

	[Display(Name = "Giỏ hàng")]
	public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

	[Display(Name = "Mã màu")]
	public virtual ProductColor? IdcolorNavigation { get; set; }

	[Display(Name = "Đặt hàng chi tiết")]
	public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
