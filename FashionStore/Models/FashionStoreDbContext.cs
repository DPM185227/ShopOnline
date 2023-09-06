using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FashionStore.Models;

public partial class FashionStoreDbContext : DbContext
{
    public FashionStoreDbContext()
    {
    }

    public FashionStoreDbContext(DbContextOptions<FashionStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CommentRep> CommentReps { get; set; }

    public virtual DbSet<Commune> Communes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderShipping> OrderShippings { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductColor> ProductColors { get; set; }

    public virtual DbSet<ProductSize> ProductSizes { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<SaleCode> SaleCodes { get; set; }

    public virtual DbSet<SaleProductPercent> SaleProductPercents { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#pragma warning disable CS1030 // #warning: 'To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.'
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
	   => optionsBuilder.UseSqlServer("Data Source=SQL8005.site4now.net;Initial Catalog=db_a97cd8_shopthoitrang;User Id=db_a97cd8_shopthoitrang_admin;Password=352553880nht;");
	   //=> optionsBuilder.UseSqlServer("Data Source=DESKTOP-RFEBHD1\\SQLEXPRESS01;Initial Catalog=db_a95414_shoponline;Integrated Security=True;TrustServerCertificate=True;");
#pragma warning restore CS1030 // #warning: 'To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.'

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.IdBranch).HasName("PK__Branch__261C45A4044346CF");

            entity.ToTable("Branch");

            entity.Property(e => e.IdBranch)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_BRANCH");
            entity.Property(e => e.CodeDistrict)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CodeProvince)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.LoactionMap).IsUnicode(false);
            entity.Property(e => e.PhoneBrand)
                .HasMaxLength(11)
                .IsUnicode(false);

            entity.HasOne(d => d.CodeDistrictNavigation).WithMany(p => p.Branches)
                .HasForeignKey(d => d.CodeDistrict)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Branch__CodeDist__3F466844");

            entity.HasOne(d => d.CodeProvinceNavigation).WithMany(p => p.Branches)
                .HasForeignKey(d => d.CodeProvince)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Branch__CodeProv__3E52440B");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.IdCart).HasName("PK__Cart__7A1680A501B2F0CB");

            entity.ToTable("Cart");

            entity.Property(e => e.IdCart).HasColumnName("ID_CART");
            entity.Property(e => e.Idcolor).HasColumnName("IDColor");
            entity.Property(e => e.Idsize).HasColumnName("IDSize");
            entity.Property(e => e.QuantityBuy)
                .HasDefaultValueSql("((0))")
                .HasColumnName("Quantity_Buy");
            entity.Property(e => e.Total).HasDefaultValueSql("((0))");
			entity.Property(e => e.ID_Product).HasColumnName("ID_Product");
			entity.Property(e => e.ID_Customer).HasColumnName("ID_Customer");

            entity.HasOne(d => d.IdcolorNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Idcolor)
                .HasConstraintName("FK_Cart_Color");

			entity.HasOne(d => d.IdsizeNavigation).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Idsize)
                .HasConstraintName("FK_Cart_Size");

			entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Carts)
				.HasForeignKey(d => d.ID_Product)
				.HasConstraintName("FK_Cart_Product");

			entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.Carts)
				.HasForeignKey(d => d.ID_Customer)
				.HasConstraintName("FK_Cart_Customer");
		});

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PK__Category__6DB3A68A1CF622AE");

            entity.ToTable("Category");

            entity.Property(e => e.IdCategory)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Category");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
            entity.Property(e => e.CategoryNameSlug)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CategoryName_Slug");
            entity.Property(e => e.Idtype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IDType");
            entity.Property(e => e.ImageCategory).IsUnicode(false);
            entity.Property(e => e.ViewInFrontEnd).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.IdtypeNavigation).WithMany(p => p.Categories)
                .HasForeignKey(d => d.Idtype)
                .HasConstraintName("FK_PRODUCTTYPE_Category");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.IdComment).HasName("PK__Comment__E211A5C28703970D");

            entity.ToTable("Comment");

            entity.Property(e => e.IdComment).HasColumnName("ID_COMMENT");
            entity.Property(e => e.Content).HasMaxLength(255);
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.Folder)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IdCustomer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Customer");
            entity.Property(e => e.IdPost)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_POST");
            entity.Property(e => e.IdProduct)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_PRODUCT");
            entity.Property(e => e.StatusComment).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK_COMMENT_CUSTOMER");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK_POST_COMMENT");
        });

        modelBuilder.Entity<CommentRep>(entity =>
        {
            entity.HasKey(e => e.IdcommentRep).HasName("PK__CommentR__F80A9408705B5793");

            entity.ToTable("CommentRep");

            entity.Property(e => e.IdcommentRep).HasColumnName("IDCommentRep");
            entity.Property(e => e.Content).HasMaxLength(255);
            entity.Property(e => e.ID_Staff)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IdComment).HasColumnName("ID_Comment");
            entity.Property(e => e.StatusComment).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.IdCommentNavigation).WithMany(p => p.CommentReps)
                .HasForeignKey(d => d.IdComment)
                .HasConstraintName("FK_Comment_CommentRep");

			entity.HasOne(d => d.IDStaffNavigation).WithMany(p => p.CommentReps)
			   .HasForeignKey(d => d.ID_Staff)
			   .HasConstraintName("FK_Staff_CommentRep");
		});

        modelBuilder.Entity<Commune>(entity =>
        {
            entity.HasKey(e => e.CommuneId).HasName("PK__Commune__3E2EBD1240DA38A5");

            entity.ToTable("Commune");

            entity.Property(e => e.CommuneId)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CodeDistrict)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.NameCommune).HasMaxLength(100);
            entity.Property(e => e.TypeCommune).HasMaxLength(30);

            entity.HasOne(d => d.CodeDistrictNavigation).WithMany(p => p.Communes)
                .HasForeignKey(d => d.CodeDistrict)
                .HasConstraintName("FK_Commune_District");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.IdCustomer).HasName("PK__Customer__2D8FDE5FDF98FA35");

            entity.ToTable("Customer");

            entity.Property(e => e.IdCustomer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Customer");
            entity.Property(e => e.CurstomerBirtday).HasColumnType("datetime");
            entity.Property(e => e.CurstomerIdentification)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CustomerAddress).HasMaxLength(255);
            entity.Property(e => e.CustomerImage).IsUnicode(false);
            entity.Property(e => e.CustomerName).HasMaxLength(255).IsUnicode(true);
            entity.Property(e => e.CustomerPhone).HasMaxLength(255);
            entity.Property(e => e.Pass)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.CodeDistrict).HasName("PK__District__7B7C4FA6090B5B2A");

            entity.ToTable("District");

            entity.Property(e => e.CodeDistrict)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.NameDistrict).HasMaxLength(100);
            entity.Property(e => e.ProvinceId)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.TypeDistrict).HasMaxLength(30);

            entity.HasOne(d => d.Province).WithMany(p => p.Districts)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK_District_Province");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Idorder).HasName("PK__Orders__5CBBCADBC4953D86");

            entity.Property(e => e.Idorder)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IDOrder");
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.IdCustomer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Customer");
            entity.Property(e => e.IdStaff)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Staff");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalAll).HasDefaultValueSql("((0))");
            entity.Property(e => e.TypeOrder).HasDefaultValueSql("((0))");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK_ORDERS_CUSTOMER");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdStaff)
                .HasConstraintName("FK_Orders_STAFF");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.IdOrderDetail).HasName("PK__OrderDet__D8E06C5196DA8AF6");

            entity.Property(e => e.IdProduct)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_PRODUCT");
            entity.Property(e => e.Idcolor).HasColumnName("IDColor");
            entity.Property(e => e.Idorder)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IDOrder");
            entity.Property(e => e.Idsize).HasColumnName("IDSize");
            entity.Property(e => e.QuantityBuy)
                .HasDefaultValueSql("((0))")
                .HasColumnName("Quantity_Buy");
            entity.Property(e => e.Total).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK_Product_OrderDetail");

            entity.HasOne(d => d.IdcolorNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.Idcolor)
                .HasConstraintName("FK_Color_OrderDetail");

            entity.HasOne(d => d.IdorderNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.Idorder)
                .HasConstraintName("FK_ORDER_ORDERDETAIL");

            entity.HasOne(d => d.IdsizeNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.Idsize)
                .HasConstraintName("FK_Size_OrderDetail");
        });

        modelBuilder.Entity<OrderShipping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderShi__3214EC2701838513");

            entity.ToTable("OrderShipping");

            entity.Property(e => e.Id).HasColumnName("ID");            
            entity.Property(e => e.CodeProvince)
                .HasMaxLength(5)
                .IsUnicode(false);          
            entity.Property(e => e.Price).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.CodeProvinceNavigation).WithMany(p => p.OrderShippings)
                .HasForeignKey(d => d.CodeProvince)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDERSHIPPING_TP");
           
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.IdBaiViet).HasName("PK__Post__233A87AC3A8B3207");

            entity.ToTable("Post");

            entity.Property(e => e.IdBaiViet).HasColumnName("ID_BaiViet");
            entity.Property(e => e.Banner)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IdStaff)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Staff");
            entity.Property(e => e.ViewPost).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.IdStaff)
                .HasConstraintName("FK_POST_STAFF");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Product__69B20C20FD30FFD6");

            entity.ToTable("Product");

            entity.Property(e => e.IdProduct)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_PRODUCT");
            entity.Property(e => e.IdBranch)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_BRANCH");
            entity.Property(e => e.IdCategory)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Category");
            entity.Property(e => e.IdStaff)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Staff");
            entity.Property(e => e.ProductNameSlug).IsUnicode(false);

            entity.HasOne(d => d.IdBranchNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdBranch)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRODUCT_BRAND");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK_PRODUCT_CATEGORY");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdStaff)
                .HasConstraintName("FK_PRODUCT_STAFF");
        });

        modelBuilder.Entity<ProductColor>(entity =>
        {
            entity.HasKey(e => e.Idcolor).HasName("PK__ProductC__E424D936D828D904");

            entity.ToTable("ProductColor");

            entity.Property(e => e.Idcolor).HasColumnName("IDColor");
            entity.Property(e => e.ColorName).HasMaxLength(100);
            entity.Property(e => e.ImageProductColor).IsUnicode(false);
            entity.Property(e => e.IdProduct)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_PRODUCT");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductColors)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK_Product_Color");
        });

        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.HasKey(e => e.Idsize).HasName("PK__ProductS__C4E3CC40D01FD9C4");

            entity.ToTable("ProductSize");

            entity.Property(e => e.Idsize).HasColumnName("IDSize");
            entity.Property(e => e.Idcolor).HasColumnName("IDColor");
            entity.Property(e => e.SizeName).HasMaxLength(100);

            entity.HasOne(d => d.IdcolorNavigation).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.Idcolor)
                .HasConstraintName("FK_Size_Color");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.Idtype).HasName("PK__Product___B2733399C49C40FE");

            entity.ToTable("Product_type");

            entity.Property(e => e.Idtype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IDType");
            entity.Property(e => e.ImageType).IsUnicode(false);
            entity.Property(e => e.TypeName).HasMaxLength(255);
            entity.Property(e => e.TypeNameSlug)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ViewInFrontEnd).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.CodeProvince).HasName("PK__Province__5D7010F2C43E0E45");

            entity.ToTable("Province");

            entity.Property(e => e.CodeProvince)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.NameProvince).HasMaxLength(100);
            entity.Property(e => e.TypeProvince).HasMaxLength(30);
        });

        modelBuilder.Entity<SaleCode>(entity =>
        {
            entity.HasKey(e => e.IdSaleCode).HasName("PK__SaleCode__373741C41C6F543F");

            entity.ToTable("SaleCode");

            entity.Property(e => e.IdSaleCode).HasColumnName("ID_SaleCode");
            entity.Property(e => e.EndDay).HasColumnType("datetime");
            entity.Property(e => e.MoneyDiscount).HasDefaultValueSql("((0))");
            entity.Property(e => e.StartDay).HasColumnType("datetime");
        });

        modelBuilder.Entity<SaleProductPercent>(entity =>
        {
            entity.HasKey(e => e.IdSale).HasName("PK__Sale_Pro__E8F6CE85F8C48D01");

            entity.ToTable("Sale_Product_Percent");

            entity.Property(e => e.IdSale).HasColumnName("ID_SALE");
            entity.Property(e => e.EndDay).HasColumnType("datetime");
            entity.Property(e => e.IdProduct)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_PRODUCT");
            entity.Property(e => e.SalePercent).HasDefaultValueSql("((0))");
            entity.Property(e => e.StartDay).HasColumnType("datetime");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.SaleProductPercents)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK_Product_SALE");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.IdStaff).HasName("PK__Staff__922B8FE65A76F2D5");

            entity.Property(e => e.IdStaff)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_Staff");
            entity.Property(e => e.AccountType).HasDefaultValueSql("((1))");
            entity.Property(e => e.IdBranch)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_BRANCH");
            entity.Property(e => e.Pass)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StaffAccountType).HasDefaultValueSql("((0))");
            entity.Property(e => e.StaffAddress).HasMaxLength(255);
            entity.Property(e => e.StaffBirthday).HasColumnType("datetime");
            entity.Property(e => e.StaffIdentification)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.StaffImage).IsUnicode(false);
            entity.Property(e => e.StaffName).HasMaxLength(255);
            entity.Property(e => e.StaffPhone).HasMaxLength(255);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdBranchNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.IdBranch)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_STAFF_BRAND");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tag__3214EC27770795E5");

            entity.ToTable("Tag");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdProduct)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ID_PRODUCT");
            entity.Property(e => e.NameTagSlug).IsUnicode(false);

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Tags)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK_Product_TAG");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
