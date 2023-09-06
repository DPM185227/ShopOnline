using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class SaleCode
{
    public int IdSaleCode { get; set; }

	[Display(Name ="Ngày bắt đầu")]
    public DateTime StartDay { get; set; }

	[Display(Name = "Ngày kết thúc")]
	public DateTime EndDay { get; set; }

	[Display(Name = "Mã giảm giá")]
	public string? Code { get; set; }

	[Display(Name = "Giảm giá tiền")]
	public int? MoneyDiscount { get; set; }
}
