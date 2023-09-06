using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models;

public partial class ProductType
{
	[Display(Name = "Mã loại")]
	public string? Idtype { get; set; } = null!;

	[Display(Name = "Tên loại")]
	[Required(ErrorMessage = "Vui lòng nhập tên loại ")]
	public string TypeName { get; set; } = null!;

	[Display(Name = "Bí danh")]
	public string? TypeNameSlug { get; set; }

	[Display(Name = "Hình ảnh")]
	public string? ImageType { get; set; }

	[Display(Name = "Hiển thị")]
	public short? ViewInFrontEnd { get; set; }

	// không được ánh xạ
	[NotMapped]
	[Display(Name = "File hình ảnh")]
	public IFormFile? File { get; set; }

	[Display(Name = "Danh mục")]
	public virtual ICollection<Category> Categories { get; } = new List<Category>();
}
