using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class Province
{
	[Display(Name = "Mã tỉnh")]
	public string CodeProvince { get; set; } = null!;

	[Display(Name = "Tên tỉnh")]
	public string NameProvince { get; set; } = null!;

	[Display(Name = "Loại tỉnh")]
	public string TypeProvince { get; set; } = null!;

	[Display(Name = "Chi nhánh")]
	public virtual ICollection<Branch> Branches { get; } = new List<Branch>();

	[Display(Name = "Huyện")]
	public virtual ICollection<District> Districts { get; } = new List<District>();

	[Display(Name = "Giao hàng")]
	public virtual ICollection<OrderShipping> OrderShippings { get; } = new List<OrderShipping>();
}
