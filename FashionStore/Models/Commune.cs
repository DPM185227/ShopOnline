using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class Commune
{
	[Display(Name = "Mã xã")]
	public string CommuneId { get; set; } = null!;

	[Display(Name = "Tên xã")]
	public string NameCommune { get; set; } = null!;

	[Display(Name = "Loại xã")]
	public string TypeCommune { get; set; } = null!;

	[Display(Name = "Tên huyện")]
	public string? CodeDistrict { get; set; }

	[Display(Name = "Mã huyện")]
	public virtual District? CodeDistrictNavigation { get; set; }
}
