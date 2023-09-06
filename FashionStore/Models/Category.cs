using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models;

public partial class Category
{
	[Display(Name = "Mã loại")]
	public string? IdCategory { get; set; } = null!;

	[Display(Name ="Tên danh mục")]
    public string CategoryName { get; set; } = null!;

	[Display(Name = "Bí danh")]
	public string? CategoryNameSlug { get; set; }

	[Display(Name = "Hình ảnh danh mục")]
	public string? ImageCategory { get; set; }

	[Display(Name = "Hiển thị")]
	public short? ViewInFrontEnd { get; set; }

	[Display(Name = "Mã loại")]
	public string? Idtype { get; set; }
	[NotMapped]
	[Display(Name = "File hình ảnh")]
	public IFormFile? File { get; set; }

	[Display(Name = "Mã loại")]
	public virtual ProductType? IdtypeNavigation { get; set; }

	[Display(Name = "Sản phẩm")]
	public virtual ICollection<Product> Products { get; } = new List<Product>();
}
