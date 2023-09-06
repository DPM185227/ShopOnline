using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models;

public partial class Post
{
	[Display(Name = "Mã bài viết")]
	public int IdBaiViet { get; set; }

	[Display(Name = "Mã nhân viên")]
	public string? IdStaff { get; set; }

	[Display(Name = "Tiêu đề")]
	public string Title { get; set; } = null!;

	[Display(Name = "Mô tả bài viết")]
	public string? DescriptionPost { get; set; }

	[Display(Name = "Xem bài viết")]
	public int? ViewPost { get; set; }

	[Display(Name = "Nội dung")]
	public string Content { get; set; } = null!;

	[Display(Name = "Hình ảnh")]
	public string? Banner { get; set; }

	[Display(Name = "Trạng thái bài viết")]
	public short? StatusPost { get; set; }

	[NotMapped]
	[Display(Name = "Hình ảnh")]
	public IFormFile? banner { get; set; }

	[Display(Name = "Nhân viên")]
	public virtual Staff? IdStaffNavigation { get; set; }
}
