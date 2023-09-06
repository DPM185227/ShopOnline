using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FashionStore.Models;

public partial class Customer
{
	[Display(Name = "Mã khách hàng")]
	public string? IdCustomer { get; set; } = null!;

	[Display(Name ="Tên khách hàng")]
    public string CustomerName { get; set; } = null!;

	[Display(Name = "Địa chỉ khách hàng")]
	public string CustomerAddress { get; set; } = null!;

	[Display(Name = "SĐT khách hàng")]
	public string CustomerPhone { get; set; } = null!;

	[Display(Name = "Ngày sinh")]
	[DataType(DataType.DateTime)]
	public DateTime CurstomerBirtday { get; set; }

	[Display(Name = "CMND")]
	public string CurstomerIdentification { get; set; } = null!;

	[Display(Name = "Hình ảnh")]
	public string? CustomerImage { get; set; }

	[Display(Name = "Tên đăng nhập")]
	public string UserName { get; set; } = null!;

	[Display(Name = "Mật khẩu")]
	public string? Pass { get; set; } = null!;
	// active or not active or block
	[Display(Name = "Tình trạng tài khoản")]
	public int? CustomerAccountType { get; set; }

	[Display(Name = "Mã xác thực")]
	public string? Authentication_code { get; set;}

	[Unique]
	[DataType(DataType.EmailAddress)]
	[Display(Name = "Mail")]
	public string? Gmail { get; set; }

	[NotMapped]
	[Display(Name = "Hình ảnh")]
	public IFormFile? imageCustomer { get; set; }

	[Display(Name = "Bình luận")]
	public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

	[Display(Name = "Đặt hàng")]
	public virtual ICollection<Order> Orders { get; } = new List<Order>();

	[Display(Name = "Giỏ hàng")]
	public virtual ICollection<Cart> Carts { get; } = new List<Cart>();
}
