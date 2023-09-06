using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionStore.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    ID_Customer = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CustomerAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CurstomerBirtday = table.Column<DateTime>(type: "datetime", nullable: false),
                    CurstomerIdentification = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    CustomerImage = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    UserName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Pass = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CustomerAccountType = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__2D8FDE5FDF98FA35", x => x.ID_Customer);
                });

            migrationBuilder.CreateTable(
                name: "Product_type",
                columns: table => new
                {
                    IDType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TypeNameSlug = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ImageType = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ViewInFrontEnd = table.Column<short>(type: "smallint", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product___B2733399C49C40FE", x => x.IDType);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    CodeProvince = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    NameProvince = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeProvince = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Province__5D7010F2C43E0E45", x => x.CodeProvince);
                });

            migrationBuilder.CreateTable(
                name: "SaleCode",
                columns: table => new
                {
                    ID_SaleCode = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDay = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDay = table.Column<DateTime>(type: "datetime", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoneyDiscount = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SaleCode__373741C41C6F543F", x => x.ID_SaleCode);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    ID_Category = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CategoryName_Slug = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ImageCategory = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ViewInFrontEnd = table.Column<short>(type: "smallint", nullable: true, defaultValueSql: "((0))"),
                    IDType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__6DB3A68A1CF622AE", x => x.ID_Category);
                    table.ForeignKey(
                        name: "FK_PRODUCTTYPE_Category",
                        column: x => x.IDType,
                        principalTable: "Product_type",
                        principalColumn: "IDType");
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    CodeDistrict = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    NameDistrict = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeDistrict = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ProvinceId = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__District__7B7C4FA6090B5B2A", x => x.CodeDistrict);
                    table.ForeignKey(
                        name: "FK_District_Province",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "CodeProvince");
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    ID_BRANCH = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoactionMap = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    PhoneBrand = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    CodeProvince = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    CodeDistrict = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Branch__261C45A4044346CF", x => x.ID_BRANCH);
                    table.ForeignKey(
                        name: "FK__Branch__CodeDist__3F466844",
                        column: x => x.CodeDistrict,
                        principalTable: "District",
                        principalColumn: "CodeDistrict");
                    table.ForeignKey(
                        name: "FK__Branch__CodeProv__3E52440B",
                        column: x => x.CodeProvince,
                        principalTable: "Province",
                        principalColumn: "CodeProvince");
                });

            migrationBuilder.CreateTable(
                name: "Commune",
                columns: table => new
                {
                    CommuneId = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    NameCommune = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeCommune = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CodeDistrict = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Commune__3E2EBD1240DA38A5", x => x.CommuneId);
                    table.ForeignKey(
                        name: "FK_Commune_District",
                        column: x => x.CodeDistrict,
                        principalTable: "District",
                        principalColumn: "CodeDistrict");
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    ID_Staff = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StaffName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StaffAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StaffPhone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StaffBirthday = table.Column<DateTime>(type: "datetime", nullable: false),
                    StaffIdentification = table.Column<string>(type: "varchar(12)", unicode: false, maxLength: 12, nullable: false),
                    StaffImage = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    UserName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Pass = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    AccountType = table.Column<short>(type: "smallint", nullable: true, defaultValueSql: "((1))"),
                    StaffAccountType = table.Column<short>(type: "smallint", nullable: true, defaultValueSql: "((0))"),
                    ID_BRANCH = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Staff__922B8FE65A76F2D5", x => x.ID_Staff);
                    table.ForeignKey(
                        name: "FK_STAFF_BRAND",
                        column: x => x.ID_BRANCH,
                        principalTable: "Branch",
                        principalColumn: "ID_BRANCH");
                });

            migrationBuilder.CreateTable(
                name: "OrderShipping",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeProvince = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    CodeDistrict = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    CommuneId = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderShi__3214EC2701838513", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ORDERSHIPPING_QH",
                        column: x => x.CodeDistrict,
                        principalTable: "District",
                        principalColumn: "CodeDistrict");
                    table.ForeignKey(
                        name: "FK_ORDERSHIPPING_TP",
                        column: x => x.CodeProvince,
                        principalTable: "Province",
                        principalColumn: "CodeProvince");
                    table.ForeignKey(
                        name: "FK_ORDERSHIPPING_XA",
                        column: x => x.CommuneId,
                        principalTable: "Commune",
                        principalColumn: "CommuneId");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    IDOrder = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ID_Customer = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ID_Staff = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ShippingPrice = table.Column<int>(type: "int", nullable: true),
                    DiscountPrice = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AddressSend = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAll = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    TypeOrder = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders__5CBBCADBC4953D86", x => x.IDOrder);
                    table.ForeignKey(
                        name: "FK_ORDERS_CUSTOMER",
                        column: x => x.ID_Customer,
                        principalTable: "Customer",
                        principalColumn: "ID_Customer");
                    table.ForeignKey(
                        name: "FK_Orders_STAFF",
                        column: x => x.ID_Staff,
                        principalTable: "Staff",
                        principalColumn: "ID_Staff");
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    ID_BaiViet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Staff = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionPost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewPost = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Banner = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    StatusPost = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Post__233A87AC3A8B3207", x => x.ID_BaiViet);
                    table.ForeignKey(
                        name: "FK_POST_STAFF",
                        column: x => x.ID_Staff,
                        principalTable: "Staff",
                        principalColumn: "ID_Staff");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ID_PRODUCT = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductPrice = table.Column<int>(type: "int", nullable: true),
                    ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductNameSlug = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ID_Category = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ID_Staff = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ID_BRANCH = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ViewInFrontEnd = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__69B20C20FD30FFD6", x => x.ID_PRODUCT);
                    table.ForeignKey(
                        name: "FK_PRODUCT_BRAND",
                        column: x => x.ID_BRANCH,
                        principalTable: "Branch",
                        principalColumn: "ID_BRANCH");
                    table.ForeignKey(
                        name: "FK_PRODUCT_CATEGORY",
                        column: x => x.ID_Category,
                        principalTable: "Category",
                        principalColumn: "ID_Category");
                    table.ForeignKey(
                        name: "FK_PRODUCT_STAFF",
                        column: x => x.ID_Staff,
                        principalTable: "Staff",
                        principalColumn: "ID_Staff");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    ID_COMMENT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Customer = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ID_PRODUCT = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ID_POST = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Folder = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    StatusComment = table.Column<short>(type: "smallint", nullable: true, defaultValueSql: "((0))"),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comment__E211A5C28703970D", x => x.ID_COMMENT);
                    table.ForeignKey(
                        name: "FK_COMMENT_CUSTOMER",
                        column: x => x.ID_Customer,
                        principalTable: "Customer",
                        principalColumn: "ID_Customer");
                    table.ForeignKey(
                        name: "FK_POST_COMMENT",
                        column: x => x.ID_PRODUCT,
                        principalTable: "Product",
                        principalColumn: "ID_PRODUCT");
                });

            migrationBuilder.CreateTable(
                name: "ProductColor",
                columns: table => new
                {
                    IDColor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FolderProductColor = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Active = table.Column<short>(type: "smallint", nullable: true),
                    ID_PRODUCT = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ImageColor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductC__E424D936D828D904", x => x.IDColor);
                    table.ForeignKey(
                        name: "FK_Product_Color",
                        column: x => x.ID_PRODUCT,
                        principalTable: "Product",
                        principalColumn: "ID_PRODUCT");
                });

            migrationBuilder.CreateTable(
                name: "Sale_Product_Percent",
                columns: table => new
                {
                    ID_SALE = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDay = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDay = table.Column<DateTime>(type: "datetime", nullable: false),
                    ID_PRODUCT = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    SalePercent = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sale_Pro__E8F6CE85F8C48D01", x => x.ID_SALE);
                    table.ForeignKey(
                        name: "FK_Product_SALE",
                        column: x => x.ID_PRODUCT,
                        principalTable: "Product",
                        principalColumn: "ID_PRODUCT");
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameTagSlug = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ID_PRODUCT = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tag__3214EC27770795E5", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Product_TAG",
                        column: x => x.ID_PRODUCT,
                        principalTable: "Product",
                        principalColumn: "ID_PRODUCT");
                });

            migrationBuilder.CreateTable(
                name: "CommentRep",
                columns: table => new
                {
                    IDCommentRep = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Folder = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    StatusComment = table.Column<short>(type: "smallint", nullable: true, defaultValueSql: "((0))"),
                    ID_Comment = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CommentR__F80A9408705B5793", x => x.IDCommentRep);
                    table.ForeignKey(
                        name: "FK_Comment_CommentRep",
                        column: x => x.ID_Comment,
                        principalTable: "Comment",
                        principalColumn: "ID_COMMENT");
                });

            migrationBuilder.CreateTable(
                name: "ProductSize",
                columns: table => new
                {
                    IDSize = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SizeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Delta = table.Column<int>(type: "int", nullable: false),
                    QuanityProduct = table.Column<int>(type: "int", nullable: false),
                    IDColor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductS__C4E3CC40D01FD9C4", x => x.IDSize);
                    table.ForeignKey(
                        name: "FK_Size_Color",
                        column: x => x.IDColor,
                        principalTable: "ProductColor",
                        principalColumn: "IDColor");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    ID_CART = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDColor = table.Column<int>(type: "int", nullable: true),
                    IDSize = table.Column<int>(type: "int", nullable: true),
                    Quantity_Buy = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    Total = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cart__7A1680A501B2F0CB", x => x.ID_CART);
                    table.ForeignKey(
                        name: "FK_Cart_Color",
                        column: x => x.IDColor,
                        principalTable: "ProductColor",
                        principalColumn: "IDColor");
                    table.ForeignKey(
                        name: "FK_Cart_Size",
                        column: x => x.IDSize,
                        principalTable: "ProductSize",
                        principalColumn: "IDSize");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    IdOrderDetail = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDOrder = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ID_PRODUCT = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IDColor = table.Column<int>(type: "int", nullable: true),
                    IDSize = table.Column<int>(type: "int", nullable: true),
                    Quantity_Buy = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    Total = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__D8E06C5196DA8AF6", x => x.IdOrderDetail);
                    table.ForeignKey(
                        name: "FK_Color_OrderDetail",
                        column: x => x.IDColor,
                        principalTable: "ProductColor",
                        principalColumn: "IDColor");
                    table.ForeignKey(
                        name: "FK_ORDER_ORDERDETAIL",
                        column: x => x.IDOrder,
                        principalTable: "Orders",
                        principalColumn: "IDOrder");
                    table.ForeignKey(
                        name: "FK_Product_OrderDetail",
                        column: x => x.ID_PRODUCT,
                        principalTable: "Product",
                        principalColumn: "ID_PRODUCT");
                    table.ForeignKey(
                        name: "FK_Size_OrderDetail",
                        column: x => x.IDSize,
                        principalTable: "ProductSize",
                        principalColumn: "IDSize");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CodeDistrict",
                table: "Branch",
                column: "CodeDistrict");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CodeProvince",
                table: "Branch",
                column: "CodeProvince");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_IDColor",
                table: "Cart",
                column: "IDColor");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_IDSize",
                table: "Cart",
                column: "IDSize");

            migrationBuilder.CreateIndex(
                name: "IX_Category_IDType",
                table: "Category",
                column: "IDType");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ID_Customer",
                table: "Comment",
                column: "ID_Customer");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ID_PRODUCT",
                table: "Comment",
                column: "ID_PRODUCT");

            migrationBuilder.CreateIndex(
                name: "IX_CommentRep_ID_Comment",
                table: "CommentRep",
                column: "ID_Comment");

            migrationBuilder.CreateIndex(
                name: "IX_Commune_CodeDistrict",
                table: "Commune",
                column: "CodeDistrict");

            migrationBuilder.CreateIndex(
                name: "IX_District_ProvinceId",
                table: "District",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ID_PRODUCT",
                table: "OrderDetails",
                column: "ID_PRODUCT");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_IDColor",
                table: "OrderDetails",
                column: "IDColor");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_IDOrder",
                table: "OrderDetails",
                column: "IDOrder");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_IDSize",
                table: "OrderDetails",
                column: "IDSize");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ID_Customer",
                table: "Orders",
                column: "ID_Customer");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ID_Staff",
                table: "Orders",
                column: "ID_Staff");

            migrationBuilder.CreateIndex(
                name: "IX_OrderShipping_CodeDistrict",
                table: "OrderShipping",
                column: "CodeDistrict");

            migrationBuilder.CreateIndex(
                name: "IX_OrderShipping_CodeProvince",
                table: "OrderShipping",
                column: "CodeProvince");

            migrationBuilder.CreateIndex(
                name: "IX_OrderShipping_CommuneId",
                table: "OrderShipping",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_ID_Staff",
                table: "Post",
                column: "ID_Staff");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ID_BRANCH",
                table: "Product",
                column: "ID_BRANCH");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ID_Category",
                table: "Product",
                column: "ID_Category");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ID_Staff",
                table: "Product",
                column: "ID_Staff");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColor_ID_PRODUCT",
                table: "ProductColor",
                column: "ID_PRODUCT");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSize_IDColor",
                table: "ProductSize",
                column: "IDColor");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_Product_Percent_ID_PRODUCT",
                table: "Sale_Product_Percent",
                column: "ID_PRODUCT");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_ID_BRANCH",
                table: "Staff",
                column: "ID_BRANCH");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ID_PRODUCT",
                table: "Tag",
                column: "ID_PRODUCT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "CommentRep");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "OrderShipping");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Sale_Product_Percent");

            migrationBuilder.DropTable(
                name: "SaleCode");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductSize");

            migrationBuilder.DropTable(
                name: "Commune");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "ProductColor");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Product_type");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "District");

            migrationBuilder.DropTable(
                name: "Province");
        }
    }
}
