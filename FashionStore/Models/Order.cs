using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class Order
{
	[Display(Name = "Mã đặt hàng")]
	public string Idorder { get; set; } = null!;

	[Display(Name = "Mã khách hàng")]
	public string? IdCustomer { get; set; }

	[Display(Name = "Mã nhân viên")]
	public string? IdStaff { get; set; }

	[Display(Name = "Phí giao hàng")]
	public int? ShippingPrice { get; set; }

	[Display(Name = "Số tiền được giảm")]
	public int? DiscountPrice { get; set; }

	[Display(Name = "Tên đầy đủ")]
	public string FullName { get; set; } = null!;

	[Display(Name = "SĐT")]
	public string Phone { get; set; } = null!;

	[Display(Name = "Địa chỉ giao hàng")]
	public string AddressSend { get; set; } = null!;

	[Display(Name = "Địa chỉ giao hàng")]
	public int? TotalAll { get; set; }

	[Display(Name = "Tổng tiền")]
	public int? TypeOrder { get; set; }

	[Display(Name = "Ngày tạo")]
	public DateTime? CreateAt { get; set; }

	[Display(Name = "Ngày cập nhật")]
	public DateTime? UpdateAt { get; set; }

	[Display(Name = "Trạng thái đơn hàng")]
	public virtual Customer? IdCustomerNavigation { get; set; }

	[Display(Name = "Mã nhân viên")]
	public virtual Staff? IdStaffNavigation { get; set; }

	[Display(Name = "Đặt hàng chi tiết")]
	public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
