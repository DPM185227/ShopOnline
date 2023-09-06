using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class Cart
{
	[Display(Name = "Mã giỏ hàng")]
	public int IdCart { get; set; }

	[Display(Name = "Mã màu")]
	public int? Idcolor { get; set; }

	[Display(Name = "Mã size")]
	public int? Idsize { get; set; }

	[Display(Name = "Mã sản phẩm")]
#pragma warning disable CS8618 // Non-nullable property 'ID_Product' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
	public string ID_Product { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'ID_Product' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

	[Display(Name = "Mã khách hàng")]
#pragma warning disable CS8618 // Non-nullable property 'ID_Customer' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
	public string ID_Customer { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'ID_Customer' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

	[Display(Name = "Số lượng")]
	public int QuantityBuy { get; set; }

	[Display(Name = "Tổng")]
	public int? Total { get; set; }

	[Display(Name = "Mã màu")]
	public virtual ProductColor? IdcolorNavigation { get; set; }

	[Display(Name = "Mã size")]
	public virtual ProductSize? IdsizeNavigation { get; set; }

	[Display(Name = "Mã sản phẩm")]
	public virtual Product? IdProductNavigation { get; set; }

	[Display(Name = "Mã nhân viên")]
	public virtual Customer? IdCustomerNavigation { get; set; }
}
