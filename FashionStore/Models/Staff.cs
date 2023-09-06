using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models;

public partial class Staff
{
	[Display(Name = "Mã nhân viên")]
	public string? IdStaff { get; set; } = null!;

	[Display(Name = "Tên nhân viên")]
	public string StaffName { get; set; } = null!;

	[Display(Name = "Địa chỉ")]
	public string StaffAddress { get; set; } = null!;

	[Display(Name = "SĐT")]
	public string StaffPhone { get; set; } = null!;

	[Display(Name = "Ngày sinh")]
	public DateTime StaffBirthday { get; set; }

	[Display(Name = "CMND")]
	public string StaffIdentification { get; set; } = null!;

	[Display(Name = "Hình ảnh")]
	public string? StaffImage { get; set; }

	[Display(Name = "Tên đăng nhập")]
	public string UserName { get; set; } = null!;

	[Display(Name = "Mật khẩu")]
	public string? Pass { get; set; } = null!;

	[Display(Name = "Loại tài khoản")]
	public short? AccountType { get; set; }

	[Display(Name = "Chức vụ")]
	public short? StaffAccountType { get; set; }

	[Display(Name = "Chi nhánh")]
	public string IdBranch { get; set; } = null!;

	[NotMapped]
	[Display(Name = "Hình ảnh")]
	public IFormFile? imageStaff { get; set; }

	[Display(Name = "Mã chi nhánh")]
	public virtual Branch? IdBranchNavigation { get; set; } = null!;

	[Display(Name = "Đặt hàng")]
	public virtual ICollection<Order>? Orders { get; } = new List<Order>();

	[Display(Name = "Bài viết")]
	public virtual ICollection<Post>? Posts { get; } = new List<Post>();

	[Display(Name = "Sản phẩm")]
	public virtual ICollection<Product>? Products { get; } = new List<Product>();

	[Display(Name = "Trả lời bình luận")]
	public virtual ICollection<CommentRep>? CommentReps { get; set; } = new List<CommentRep>();
}
