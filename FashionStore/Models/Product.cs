using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class Product
{
	[Display(Name = "Mã sản phẩm")]
	public string? IdProduct { get; set; } = null!;

	[Display(Name = "Tên sản phẩm")]
	public string? ProductName { get; set; } = null!;

	[Display(Name = "Giá sản phẩm")]
	public int ProductPrice { get; set; }

	[Display(Name = "Mô tả sản phẩm")]
	public string? ProductDescription { get; set; }

	[Display(Name = "Bí danh")]
	public string? ProductNameSlug { get; set; }

	[Display(Name = "Mã danh mục")]
	public string? IdCategory { get; set; }

	[Display(Name = "Mã nhân viên")]
	public string? IdStaff { get; set; }

	[Display(Name = "Mã chi nhánh")]
	public string IdBranch { get; set; } = null!;

	[Display(Name ="Hiển thị")]
	public short ViewInFrontEnd { get; set; }

	[Display(Name = "Bình luận")]
	public virtual ICollection<Comment>? Comments { get; } = new List<Comment>();

	[Display(Name = "Mã chi nhánh")]
	public virtual Branch? IdBranchNavigation { get; set; } = null!;

	[Display(Name = "Mã danh mục")]
	public virtual Category? IdCategoryNavigation { get; set; }

	[Display(Name = "Mã nhân viên")]
	public virtual Staff? IdStaffNavigation { get; set; }

	[Display(Name = "Đặt hàng chi tiết")]
	public virtual ICollection<OrderDetail>? OrderDetails { get; } = new List<OrderDetail>();

	[Display(Name = "Màu sản phẩm")]
	public virtual ICollection<ProductColor> ProductColors { get; } = new List<ProductColor>();

	[Display(Name = "Phần trăm giảm giá")]
	public virtual ICollection<SaleProductPercent> SaleProductPercents { get; } = new List<SaleProductPercent>();

	[Display(Name = "Giỏ hàng")]
	public virtual ICollection<Cart> Carts { get; } = new List<Cart>();

	[Display(Name = "Tag")]
	public virtual ICollection<Tag> Tags { get; } = new List<Tag>();
}
