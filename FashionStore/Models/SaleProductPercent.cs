using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class SaleProductPercent
{
	[Display(Name = "Mã giảm giá")]
	public int IdSale { get; set; }

	[Display(Name ="Ngày bắt đầu")]
    public DateTime StartDay { get; set; }

	[Display(Name = "Ngày kết thúc")]
	public DateTime EndDay { get; set; }

	[Display(Name = "Mã sản phẩm")]
	public string? IdProduct { get; set; }

	[Display(Name = "Phần trăm")]
	public int SalePercent { get; set; }

	[Display(Name = "Mã sản phẩm")]
	public virtual Product? IdProductNavigation { get; set; }
}
