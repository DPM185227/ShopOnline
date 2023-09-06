using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class Comment
{
	[Display(Name = "Mã bình luận")]
	public int IdComment { get; set; }

	[Display(Name = "Mã khách hàng")]
	public string? IdCustomer { get; set; }

	[Display(Name = "Mã sản phẩm")]
	public string? IdProduct { get; set; }

	[Display(Name = "Mã bài viết")]
	public int? IdPost { get; set; }

	[Display(Name = "Nội dung")]
	public string? Content { get; set; }

	[Display(Name = "Hình ảnh")]
	public string? Folder { get; set; }

	[Display(Name = "Trạng thái bình luận")]
	public short? StatusComment { get; set; }

	[Display(Name = "Ngày tạo")]
	public DateTime? CreateAt { get; set; }

	[Display(Name = "Trả lời bình luận")]
	public virtual ICollection<CommentRep> CommentReps { get; } = new List<CommentRep>();

	[Display(Name = "Mã khách hàng")]
	public virtual Customer? IdCustomerNavigation { get; set; }

	[Display(Name = "Mã sản phẩm")]
	public virtual Product? IdProductNavigation { get; set; }
}
