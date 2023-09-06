using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FashionStore.Models;

public partial class CommentRep
{
	[Display(Name = "Mã trả lời bình luận")]
	public int? IdcommentRep { get; set; }

	[Display(Name = "Nội dung")]
#pragma warning disable CS8618 // Non-nullable property 'Content' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
	public string Content { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Content' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

	[Display(Name = "Mã nhân viên")]
	public string? ID_Staff { get; set; }

	[Display(Name = "Trạng thái bình luận")]
	public short StatusComment { get;set; }

	[Display(Name = "Mã bình luận")]
	public int? IdComment { get; set; }

	[Display(Name = "Mã bình luận")]
	public virtual Comment? IdCommentNavigation { get; set; }

	[Display(Name = "Mã nhân viên")]
	public virtual Staff? IDStaffNavigation { get; set; }
}
