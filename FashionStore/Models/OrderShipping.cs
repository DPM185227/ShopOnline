using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class OrderShipping
{
	[Display(Name = "Mã")]
	public int Id { get; set; }

	[Display(Name="Mã tỉnh")]
    public string CodeProvince { get; set; } = null!;

	[Display(Name = "Giá")]
	public int? Price { get; set; }

	[Display(Name = "Mã tỉnh")]
	public virtual Province? CodeProvinceNavigation { get; set; } = null!;
}
