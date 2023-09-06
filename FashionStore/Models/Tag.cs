using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class Tag
{
	[Display(Name = "Mã")]
	public int Id { get; set; }

	[Display(Name="Từ khóa Tag")]
    public string NameTag { get; set; } = null!;

	[Display(Name = "Bí danh")]
	public string? NameTagSlug { get; set; }

	[Display(Name = "Mã sản phẩm")]
	public string? IdProduct { get; set; }

	[Display(Name = "Mã sản phẩm")]
	public virtual Product? IdProductNavigation { get; set; }
}
