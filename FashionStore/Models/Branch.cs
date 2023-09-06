using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class Branch
{
	[Display(Name = "Mã chi nhánh")]
	public string IdBranch { get; set; } = null!;

	[Display(Name = "Địa chỉ")]
	[Required(ErrorMessage = "Vui lòng nhập địa chỉ chi nhánh")]
	public string LocationName { get; set; } = null!;

	[Display(Name = "Vị trí bản đồ")]
	[Required(ErrorMessage = "Vui lòng nhập vị trí bản đồ")]
	public string LoactionMap { get; set; } = null!;

	[Display(Name = "Số ĐT chi nhánh")]
	[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Vui lòng nhập số điện thoại đúng định dạng")]
	[Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
	public string PhoneBrand { get; set; } = null!;

	[Display(Name = "Mã tỉnh")]
	[Required(ErrorMessage = "Vui lòng chọn mã tỉnh")]
	public string CodeProvince { get; set; } = null!;

	[Display(Name = "Mã huyện/thành phố")]
	[Required(ErrorMessage = "Vui lòng chọn mã huyện/thành phố")]
	public string CodeDistrict { get; set; } = null!;

	[Display(Name = "Mã huyện/thành phố")]
	[Required(ErrorMessage = "Vui lòng chọn mã huyện/thành phố")]
	public virtual District CodeDistrictNavigation { get; set; } = null!;

	[Display(Name = "Mã tỉnh")]
	[Required(ErrorMessage = "Vui lòng chọn mã tỉnh")]
	public virtual Province CodeProvinceNavigation { get; set; } = null!;

	[Display(Name = "Sản phẩm")]
	public virtual ICollection<Product> Products { get; } = new List<Product>();

	[Display(Name = "Nhân viên")]
	public virtual ICollection<Staff> Staff { get; } = new List<Staff>();
}
