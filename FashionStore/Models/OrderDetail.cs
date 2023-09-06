using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class OrderDetail
{
	[Display(Name = "Mã đặt hàng chi tiết")]
	public int IdOrderDetail { get; set; }

	[Display(Name = "Mã đặt hàng")]
	public string? Idorder { get; set; }

	[Display(Name = "Mã sản phẩm")]
	public string? IdProduct { get; set; }

	[Display(Name = "Mã màu")]
	public int? Idcolor { get; set; }

	[Display(Name = "Mã size")]
	public int? Idsize { get; set; }

	[Display(Name = "Số lượng")]
	public int QuantityBuy { get; set; }

	[Display(Name = "Tổng cộng")]
	public int? Total { get; set; }

	[Display(Name = "Sản phẩm")]
	public virtual Product? IdProductNavigation { get; set; }

	[Display(Name = "Màu")]
	public virtual ProductColor? IdcolorNavigation { get; set; }

	[Display(Name = "Đặt hàng")]
	public virtual Order? IdorderNavigation { get; set; }

	[Display(Name = "Size")]
	public virtual ProductSize? IdsizeNavigation { get; set; }
}
