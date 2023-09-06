using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class District
{
	[Display(Name = "Mã huyện")]
	public string CodeDistrict { get; set; } = null!;

	[Display(Name = "Tên huyện")]
	public string NameDistrict { get; set; } = null!;

	[Display(Name = "Loại huyện")]
	public string TypeDistrict { get; set; } = null!;

	[Display(Name = "Mã tỉnh")]
	public string? ProvinceId { get; set; }

	[Display(Name = "Chi nhánh")]
	public virtual ICollection<Branch> Branches { get; } = new List<Branch>();

	[Display(Name = "Xã")]
	public virtual ICollection<Commune> Communes { get; } = new List<Commune>();

	[Display(Name = "Tỉnh")]
	public virtual Province? Province { get; set; }
}
