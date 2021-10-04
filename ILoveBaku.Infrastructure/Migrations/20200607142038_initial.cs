using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ILoveBaku.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchesFloors",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchesFloors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchesPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchesId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 60, nullable: false),
                    IsSalesRows = table.Column<bool>(nullable: false),
                    Priority = table.Column<byte>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchesPlaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchesSectorsRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchesFloorsRelationsId = table.Column<int>(nullable: false),
                    BranchesSectorsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchesSectorsRelations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchesSectorsShelfsRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchesSectorsRelationsId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchesSectorsShelfsRelations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartOrderStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    KapitalOrderStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartOrderStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CartStatusId = table.Column<byte>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashDeskConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashDeskId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Value = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashDeskConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashDeskSeanceManualProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchesId = table.Column<int>(nullable: false),
                    ProductsId = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashDeskSeanceManualProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashDeskSeanceManualTransactionsTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Priority = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashDeskSeanceManualTransactionsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CashDeskSeanceStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Priority = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashDeskSeanceStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    Priority = table.Column<byte>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesSpecificationsGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesSpecificationsGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesSpecificationsStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesSpecificationsStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategorySpecificationsTypesControllers",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySpecificationsTypesControllers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategorySpecificationsTypesControllersSpecifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    TableName = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySpecificationsTypesControllersSpecifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategorySpecificationsTypesControllersSpecificationsProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategorySpecificationsTypesControllersSpecificationsId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySpecificationsTypesControllersSpecificationsProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategorySpecificationsTypesControllersSpecificationsValuesList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategorySpecificationsId = table.Column<int>(nullable: false),
                    CategorySpecificationsTypesControllersSpecificationsPropertiesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySpecificationsTypesControllersSpecificationsValuesList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategorySpecificationsTypesControllersSpecificationsValuesStrings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategorySpecificationsId = table.Column<int>(nullable: false),
                    CategorySpecificationsTypesControllersSpecificationsPropertiesId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySpecificationsTypesControllersSpecificationsValuesStrings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    VOEN = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedIP = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDebts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchesId = table.Column<int>(nullable: false),
                    SuppliersId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDebts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompaniesId = table.Column<int>(nullable: false),
                    CompanyDetailsTypesId = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    SUN = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    ContactsName = table.Column<string>(maxLength: 50, nullable: false),
                    Contacts = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 30, nullable: false),
                    Address = table.Column<string>(maxLength: 200, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedIP = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyDetailsTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    ParentId = table.Column<byte>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDetailsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyTransactionTypesId = table.Column<byte>(nullable: false),
                    ProductTransactionsId = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    CompanyTransactionsStatusesId = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedIP = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTransactionsStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    ParentId = table.Column<byte>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTransactionsStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTransactionsTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTransactionsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationsTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Phone = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    Message = table.Column<string>(maxLength: 2000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ContactsStatusesId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactsStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactsStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentsCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentsCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentsCategoriesLangs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentsCategoriesId = table.Column<int>(nullable: false),
                    LangsId = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentsCategoriesLangs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogText = table.Column<string>(maxLength: 1000, nullable: false),
                    URL = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedIP = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilesFolders ",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    IsAllowDelete = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesFolders ", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilesTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilesTypesGroupsId = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FilesTypesGroups",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesTypesGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Langs",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: false),
                    Culture = table.Column<string>(nullable: true),
                    LangsStatusesId = table.Column<byte>(nullable: false),
                    Priority = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Langs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LangStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LangStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Link = table.Column<string>(unicode: false, nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Priority = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    MenuTypesId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ShowDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsFilesTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsFilesTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsKeywords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsLangsId = table.Column<int>(nullable: false),
                    KeywordsId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsKeywords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsLangsStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsLangsStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOutBonusInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsCashOutCardsId = table.Column<int>(nullable: false),
                    BonusAmount = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    BonusCount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCashOutBonusInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOutPayments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsCashOutPaymentsTypesId = table.Column<int>(nullable: false),
                    ProductsCashOutId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCashOutPayments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOutPaymentsCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsCashOutPaymentsId = table.Column<int>(nullable: false),
                    UsersCardsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCashOutPaymentsCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOutPaymentsTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 40, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOutShippingsPackets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    DeliveryCompaniesId = table.Column<byte>(nullable: false),
                    ResponsablePerson = table.Column<string>(maxLength: 100, nullable: false),
                    ProductsCashOutShippingsPacketsStatusesId = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCashOutShippingsPackets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOutShippingsPacketsDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsCashOutShippingsPacketsId = table.Column<int>(nullable: false),
                    ProductsCashOutsId = table.Column<int>(nullable: false),
                    TrackingNumber = table.Column<Guid>(unicode: false, maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    ProductsCashOutShippingsPacketsDetailsStatuses = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCashOutShippingsPacketsDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOutShippingsPacketsDetailsStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCashOutShippingsPacketsDetailsStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOutShippingsPacketsStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCashOutShippingsPacketsStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOutStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCashOutStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsList_sp",
                columns: table => new
                {
                    ProductName = table.Column<string>(nullable: true),
                    ProductCreatedDate = table.Column<DateTime>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    ProductGroupId = table.Column<int>(nullable: false),
                    ProductGroupName = table.Column<string>(nullable: true),
                    ProductGroupCreatedDate = table.Column<DateTime>(nullable: false),
                    ProductPhoto = table.Column<string>(nullable: true),
                    CategoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ProductSpecificationValuesBarcodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsId = table.Column<int>(nullable: false),
                    CategoriesSpecificationsPropertiesId = table.Column<int>(nullable: false),
                    Value = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsManual = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecificationValuesBarcodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsStockDiscounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsStockDiscountsTypesId = table.Column<int>(nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(4, 2)", nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    MinimumOrder = table.Column<decimal>(type: "decimal(10, 4)", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductsStockDiscountsStatusesId = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsStockDiscounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsStockDiscountsStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsStockDiscountsStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsStockLocations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsStockId = table.Column<int>(nullable: false),
                    BranchesSectorsShelfsRelationsId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsStockLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsStockLocationsCount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductSstockLocationsId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsStockLocationsCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsStockSaleAmountsTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsStockSaleAmountsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsStockSpecificationsValuesList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsId = table.Column<int>(nullable: false),
                    CategoriesSpecificationsPropertiesId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsStockSpecificationsValuesList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsStockSpecificationsValuesStrings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsId = table.Column<int>(nullable: false),
                    CategoriesSpecificationsPropertiesId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsStockSpecificationsValuesStrings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsStockSpecificationValuesInt",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsId = table.Column<int>(nullable: false),
                    CategoriesSpecificationsPropertiesId = table.Column<int>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsStockSpecificationValuesInt", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsStockStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsStockStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsTransactionsTypesId = table.Column<byte>(nullable: false),
                    BranchesId = table.Column<int>(nullable: false),
                    SuppliersId = table.Column<int>(nullable: false),
                    ReceiptsNumber = table.Column<int>(nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    TotalPayAmount = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    ReceipDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductsTransactionsStatusesId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsTransactionsDetailsPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsStockLocationsId = table.Column<int>(nullable: false),
                    ProductsTransactionsDetailsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsTransactionsDetailsPlaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsTransactionsStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsTransactionsStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductsTransactionsTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsTransactionsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductValueTable",
                columns: table => new
                {
                    TableName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    IsActive = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(unicode: false, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    FilesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SlidersLangs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LangsId = table.Column<byte>(nullable: false),
                    SlidersId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    SubTitle = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlidersLangs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TranslationsGroupsId = table.Column<byte>(nullable: false),
                    TransKey = table.Column<string>(unicode: false, maxLength: 200, nullable: false),
                    Title = table.Column<string>(maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TranslationsGroups",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationsGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TranslationsLangs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TranslationsId = table.Column<int>(nullable: false),
                    LangsId = table.Column<byte>(nullable: false),
                    Text = table.Column<string>(maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslationsLangs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersAddressInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsersId = table.Column<Guid>(nullable: false),
                    RegionsId = table.Column<int>(nullable: false),
                    Address = table.Column<string>(maxLength: 500, nullable: false),
                    ZipCode = table.Column<string>(unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAddressInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersCardsStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCardsStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersCardsTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCardsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersCardsValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsersCardsId = table.Column<int>(nullable: false),
                    UsersCardsValuesTypesId = table.Column<byte>(nullable: false),
                    Value = table.Column<decimal>(type: "decimal(10, 4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCardsValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersCardsValuesTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCardsValuesTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsersClaimsTypesId = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersClaimsModules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsersClaimsModulesGroupsId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Controller = table.Column<string>(maxLength: 50, nullable: false),
                    Action = table.Column<string>(maxLength: 10, nullable: false),
                    Priority = table.Column<byte>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersClaimsModules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersClaimsModulesGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Priority = table.Column<byte>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersClaimsModulesGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersClaimsTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersClaimsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersExternalLogins",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsersId = table.Column<Guid>(nullable: false),
                    UsersExternalLoginsProvidersId = table.Column<byte>(nullable: false),
                    ProviderKey = table.Column<string>(unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersExternalLogins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersExternalLoginsProviders",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersExternalLoginsProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    UsersRolesGroupsId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersRolesClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsersRolesId = table.Column<Guid>(nullable: false),
                    ClaimsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRolesClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersRolesGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersRolesGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersRolesRelations",
                columns: table => new
                {
                    UsersId = table.Column<Guid>(nullable: false),
                    UsersRolesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Idx_UsersRolesRelations", x => x.UsersId);
                });

            migrationBuilder.CreateTable(
                name: "UsersStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersTokensStatuses",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTokensStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BranchesFloorsRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchesFloorsId = table.Column<byte>(nullable: false),
                    BranchesPlacesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchesFloorsRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchesFloorsRelations_BranchesPlaces_BranchesPlacesId",
                        column: x => x.BranchesPlacesId,
                        principalTable: "BranchesPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<string>(nullable: true),
                    SessionId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    CartId = table.Column<int>(nullable: false),
                    CartOrderStatusId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartOrders_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    CategoriesId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedIP = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductGroups_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesSpecificationsTypes",
                columns: table => new
                {
                    Id = table.Column<byte>(nullable: false),
                    CategorySpecificationsTypesControllersId = table.Column<byte>(nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    TableName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesSpecificationsTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesSpecificationsTypes_CategorySpecificationsTypesControllers_CategorySpecificationsTypesControllersId",
                        column: x => x.CategorySpecificationsTypesControllersId,
                        principalTable: "CategorySpecificationsTypesControllers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategorySpecificationsTypesSpecificationsRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategorySpecificationsTypesControllersId = table.Column<byte>(nullable: false),
                    CategorySpecificationsTypesControllersSpecificationsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategorySpecificationsTypesSpecificationsRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategorySpecificationsTypesSpecificationsRelations_CategorySpecificationsTypesControllersSpecifications_CategorySpecificatio~",
                        column: x => x.CategorySpecificationsTypesControllersSpecificationsId,
                        principalTable: "CategorySpecificationsTypesControllersSpecifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyDetailsId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedIP = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_CompanyDetails_CompanyDetailsId",
                        column: x => x.CompanyDetailsId,
                        principalTable: "CompanyDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    ContentLength = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    FilesFoldersId = table.Column<int>(nullable: false),
                    FilesTypesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_FilesFolders _FilesFoldersId",
                        column: x => x.FilesFoldersId,
                        principalTable: "FilesFolders ",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Files_FilesTypes_FilesTypesId",
                        column: x => x.FilesTypesId,
                        principalTable: "FilesTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesLangs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriesId = table.Column<int>(nullable: false),
                    LangsId = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesLangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesLangs_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriesLangs_Langs_LangsId",
                        column: x => x.LangsId,
                        principalTable: "Langs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesSpecificationsGroupsLangs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriesSpecificationsGroupsId = table.Column<int>(nullable: false),
                    LangsId = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesSpecificationsGroupsLangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesSpecificationsGroupsLangs_CategoriesSpecificationsGroups_CategoriesSpecificationsGroupsId",
                        column: x => x.CategoriesSpecificationsGroupsId,
                        principalTable: "CategoriesSpecificationsGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriesSpecificationsGroupsLangs_Langs_LangsId",
                        column: x => x.LangsId,
                        principalTable: "Langs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuCategoriesItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<int>(nullable: false),
                    CategoriesId = table.Column<int>(nullable: false),
                    CategoriesParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategoriesItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuCategoriesItems_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuCategoriesItems_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuLangs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    MenuId = table.Column<int>(nullable: false),
                    LangsId = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuLangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuLangs_Langs_LangsId",
                        column: x => x.LangsId,
                        principalTable: "Langs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuLangs_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsLangs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 500, nullable: false),
                    TitleUrl = table.Column<string>(unicode: false, maxLength: 800, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ContentHtml = table.Column<string>(maxLength: 4000, nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    NewsId = table.Column<int>(nullable: false),
                    NewsLangsStatusesId = table.Column<byte>(nullable: false),
                    LangsId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsLangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsLangs_Langs_LangsId",
                        column: x => x.LangsId,
                        principalTable: "Langs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsLangs_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOut",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashDeskSeanceId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductsCashOutStatusesId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCashOut", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsCashOut_ProductsCashOutStatuses_ProductsCashOutStatusesId",
                        column: x => x.ProductsCashOutStatusesId,
                        principalTable: "ProductsCashOutStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsTransactionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsId = table.Column<int>(nullable: false),
                    Count = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    BuyAmount = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    CostAmount = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    PayAmount = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductsTransactionsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsTransactionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsTransactionDetails_ProductsTransactions_ProductsTransactionsId",
                        column: x => x.ProductsTransactionsId,
                        principalTable: "ProductsTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOutAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsCashOutId = table.Column<int>(nullable: false),
                    UsersAddressInfoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCashOutAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsCashOutAddresses_UsersAddressInfo_UsersAddressInfoId",
                        column: x => x.UsersAddressInfoId,
                        principalTable: "UsersAddressInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashDesk",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchesFloorsRelationsId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashDesk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashDesk_BranchesFloorsRelations_BranchesFloorsRelationsId",
                        column: x => x.BranchesFloorsRelationsId,
                        principalTable: "BranchesFloorsRelations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    DefaultSaleAmount = table.Column<decimal>(nullable: true),
                    DefaultBuyAmount = table.Column<decimal>(nullable: true),
                    DefaultCostAmount = table.Column<decimal>(nullable: true),
                    TaxPercent = table.Column<decimal>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedIP = table.Column<int>(nullable: false),
                    ProductGroupsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductGroups_ProductGroupsId",
                        column: x => x.ProductGroupsId,
                        principalTable: "ProductGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesSpecifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriesSpecificationGroupId = table.Column<int>(nullable: false),
                    CategoriesSpecificationsTypeId = table.Column<byte>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    CategoriesSpecificationsStatusesId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesSpecifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesSpecifications_CategoriesSpecificationsTypes_CategoriesSpecificationsTypeId",
                        column: x => x.CategoriesSpecificationsTypeId,
                        principalTable: "CategoriesSpecificationsTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BranchesId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Surname = table.Column<string>(maxLength: 50, nullable: false),
                    Patronymic = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    DocumentNumber = table.Column<string>(maxLength: 20, nullable: true),
                    PIN = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    ContactNumber = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    ContactEmail = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    UsersStatusesId = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedIP = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Branches_BranchesId",
                        column: x => x.BranchesId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriesId = table.Column<int>(nullable: false),
                    FilesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesFiles_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriesFiles_Files_FilesId",
                        column: x => x.FilesId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentsCategoriesId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    IsVisible = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    FilesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contents_Files_FilesId",
                        column: x => x.FilesId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuBannerItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<int>(nullable: false),
                    FilesId = table.Column<int>(nullable: false),
                    Link = table.Column<string>(unicode: false, nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuBannerItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuBannerItems_Files_FilesId",
                        column: x => x.FilesId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsId = table.Column<int>(nullable: false),
                    NewsFilesTypesId = table.Column<byte>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    FilesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsFiles_Files_FilesId",
                        column: x => x.FilesId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsFiles_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsTransactionsCount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductsTransactionsDetailsId = table.Column<int>(nullable: false),
                    ProductsTransactionsDetailId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsTransactionsCount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsTransactionsCount_ProductsTransactionDetails_ProductsTransactionsDetailId",
                        column: x => x.ProductsTransactionsDetailId,
                        principalTable: "ProductsTransactionDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CashDeskSeance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashDeskId = table.Column<int>(nullable: false),
                    UsersId = table.Column<Guid>(nullable: false),
                    StartAmount = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    EndAmount = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    CashDeskSeanceStatusesId = table.Column<byte>(nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashDeskSeance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashDeskSeance_CashDesk_CashDeskId",
                        column: x => x.CashDeskId,
                        principalTable: "CashDesk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    CartId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartDetails_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsId = table.Column<int>(nullable: false),
                    FilesId = table.Column<int>(nullable: false),
                    IsMain = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsFiles_Files_FilesId",
                        column: x => x.FilesId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsFiles_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsStock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchesId = table.Column<int>(nullable: false),
                    Count = table.Column<decimal>(type: "decimal(10, 4)", nullable: false),
                    TaxPercent = table.Column<decimal>(type: "decimal(4, 2)", nullable: false),
                    BuyAmount = table.Column<decimal>(nullable: true),
                    CostAmount = table.Column<decimal>(nullable: true),
                    ProductStockStatusesId = table.Column<byte>(nullable: false),
                    MinCount = table.Column<int>(nullable: false),
                    PriorityDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: true),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsStock_Branches_BranchesId",
                        column: x => x.BranchesId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsStock_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesSpecificationsLangs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriesSpecificationsId = table.Column<int>(nullable: false),
                    LangsId = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesSpecificationsLangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesSpecificationsLangs_CategoriesSpecifications_CategoriesSpecificationsId",
                        column: x => x.CategoriesSpecificationsId,
                        principalTable: "CategoriesSpecifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriesSpecificationsLangs_Langs_LangsId",
                        column: x => x.LangsId,
                        principalTable: "Langs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesSpecificationsProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriesSpecificationId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesSpecificationsProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesSpecificationsProperties_CategoriesSpecifications_CategoriesSpecificationId",
                        column: x => x.CategoriesSpecificationId,
                        principalTable: "CategoriesSpecifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesSpecificationsRelations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriesId = table.Column<int>(nullable: false),
                    CategoriesSpecificationId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesSpecificationsRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesSpecificationsRelations_CategoriesSpecifications_CategoriesSpecificationId",
                        column: x => x.CategoriesSpecificationId,
                        principalTable: "CategoriesSpecifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserCardsTypesId = table.Column<byte>(nullable: false),
                    UsersId = table.Column<Guid>(nullable: false),
                    CardNumber = table.Column<long>(nullable: false),
                    UserCardsStatusesId = table.Column<byte>(nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersCards_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersClaimsRelations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsersId = table.Column<Guid>(nullable: false),
                    UsersClaimsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersClaimsRelations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersClaimsRelations_UsersClaims_UsersClaimsId",
                        column: x => x.UsersClaimsId,
                        principalTable: "UsersClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersClaimsRelations_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersLogins",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsersId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Password = table.Column<byte[]>(maxLength: 8000, nullable: false),
                    Salt = table.Column<byte[]>(maxLength: 8000, nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false, defaultValueSql: "((1))"),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedIP = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersLogins_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsersId = table.Column<Guid>(nullable: false),
                    UsersTokensStatusesId = table.Column<byte>(nullable: false),
                    Value = table.Column<string>(maxLength: 130, nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersTokens_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentsLangs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 500, nullable: false),
                    SubTitle = table.Column<string>(maxLength: 1000, nullable: true),
                    ContentHTML = table.Column<string>(nullable: false),
                    VisitorCount = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ContentsId = table.Column<int>(nullable: false),
                    LangsId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentsLangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentsLangs_Contents_ContentsId",
                        column: x => x.ContentsId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentsLangs_Langs_LangsId",
                        column: x => x.LangsId,
                        principalTable: "Langs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOutDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsCashOutId = table.Column<int>(nullable: false),
                    ProductsId = table.Column<int>(nullable: false),
                    ProductsTransactionsCountId = table.Column<int>(nullable: true),
                    Count = table.Column<decimal>(nullable: false),
                    SaleAmount = table.Column<decimal>(nullable: false),
                    TaxPercent = table.Column<decimal>(nullable: false),
                    DiscountPercent = table.Column<decimal>(nullable: false),
                    PayAmount = table.Column<decimal>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCashOutDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsCashOutDetails_ProductsCashOut_ProductsCashOutId",
                        column: x => x.ProductsCashOutId,
                        principalTable: "ProductsCashOut",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsCashOutDetails_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsCashOutDetails_ProductsTransactionsCount_ProductsTransactionsCountId",
                        column: x => x.ProductsTransactionsCountId,
                        principalTable: "ProductsTransactionsCount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductsStockDiscountsDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsStockDiscountsId = table.Column<int>(nullable: false),
                    ProductsStockId = table.Column<int>(nullable: false),
                    ProductsCount = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsStockDiscountsDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsStockDiscountsDetails_ProductsStockDiscounts_ProductsStockDiscountsId",
                        column: x => x.ProductsStockDiscountsId,
                        principalTable: "ProductsStockDiscounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsStockDiscountsDetails_ProductsStock_ProductsStockId",
                        column: x => x.ProductsStockId,
                        principalTable: "ProductsStock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsStockSaleAmounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsStockId = table.Column<int>(nullable: false),
                    ProductStockSaleAmountsTypesId = table.Column<byte>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10, 4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsStockSaleAmounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsStockSaleAmounts_ProductsStock_ProductsStockId",
                        column: x => x.ProductsStockId,
                        principalTable: "ProductsStock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WishLists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsersId = table.Column<Guid>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProductsStockId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishLists_ProductsStock_ProductsStockId",
                        column: x => x.ProductsStockId,
                        principalTable: "ProductsStock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesSpecificationsPropertiesLangs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriesSpecificationsPropertiesId = table.Column<int>(nullable: false),
                    LangsId = table.Column<byte>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesSpecificationsPropertiesLangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriesSpecificationsPropertiesLangs_CategoriesSpecificationsProperties_CategoriesSpecificationsPropertiesId",
                        column: x => x.CategoriesSpecificationsPropertiesId,
                        principalTable: "CategoriesSpecificationsProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriesSpecificationsPropertiesLangs_Langs_LangsId",
                        column: x => x.LangsId,
                        principalTable: "Langs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsCashOutCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductsCashOutId = table.Column<int>(nullable: false),
                    UsersCardsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsCashOutCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsCashOutCards_ProductsCashOut_ProductsCashOutId",
                        column: x => x.ProductsCashOutId,
                        principalTable: "ProductsCashOut",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductsCashOutCards_UsersCards_UsersCardsId",
                        column: x => x.UsersCardsId,
                        principalTable: "UsersCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CompanyDetailsId",
                table: "Branches",
                column: "CompanyDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchesFloorsRelations_BranchesPlacesId",
                table: "BranchesFloorsRelations",
                column: "BranchesPlacesId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_CartId",
                table: "CartDetails",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_ProductId",
                table: "CartDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartOrders_CartId",
                table: "CartOrders",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CashDesk_BranchesFloorsRelationsId",
                table: "CashDesk",
                column: "BranchesFloorsRelationsId");

            migrationBuilder.CreateIndex(
                name: "IX_CashDeskSeance_CashDeskId",
                table: "CashDeskSeance",
                column: "CashDeskId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesFiles_CategoriesId",
                table: "CategoriesFiles",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesFiles_FilesId",
                table: "CategoriesFiles",
                column: "FilesId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesLangs_CategoriesId",
                table: "CategoriesLangs",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesLangs_LangsId",
                table: "CategoriesLangs",
                column: "LangsId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesSpecifications_CategoriesSpecificationsTypeId",
                table: "CategoriesSpecifications",
                column: "CategoriesSpecificationsTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesSpecificationsGroupsLangs_CategoriesSpecificationsGroupsId",
                table: "CategoriesSpecificationsGroupsLangs",
                column: "CategoriesSpecificationsGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesSpecificationsGroupsLangs_LangsId",
                table: "CategoriesSpecificationsGroupsLangs",
                column: "LangsId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesSpecificationsLangs_CategoriesSpecificationsId",
                table: "CategoriesSpecificationsLangs",
                column: "CategoriesSpecificationsId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesSpecificationsLangs_LangsId",
                table: "CategoriesSpecificationsLangs",
                column: "LangsId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesSpecificationsProperties_CategoriesSpecificationId",
                table: "CategoriesSpecificationsProperties",
                column: "CategoriesSpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesSpecificationsPropertiesLangs_CategoriesSpecificationsPropertiesId",
                table: "CategoriesSpecificationsPropertiesLangs",
                column: "CategoriesSpecificationsPropertiesId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesSpecificationsPropertiesLangs_LangsId",
                table: "CategoriesSpecificationsPropertiesLangs",
                column: "LangsId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesSpecificationsRelations_CategoriesSpecificationId",
                table: "CategoriesSpecificationsRelations",
                column: "CategoriesSpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesSpecificationsTypes_CategorySpecificationsTypesControllersId",
                table: "CategoriesSpecificationsTypes",
                column: "CategorySpecificationsTypesControllersId");

            migrationBuilder.CreateIndex(
                name: "IX_CategorySpecificationsTypesSpecificationsRelations_CategorySpecificationsTypesControllersSpecificationsId",
                table: "CategorySpecificationsTypesSpecificationsRelations",
                column: "CategorySpecificationsTypesControllersSpecificationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_FilesId",
                table: "Contents",
                column: "FilesId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentsLangs_ContentsId",
                table: "ContentsLangs",
                column: "ContentsId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentsLangs_LangsId",
                table: "ContentsLangs",
                column: "LangsId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_FilesFoldersId",
                table: "Files",
                column: "FilesFoldersId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_FilesTypesId",
                table: "Files",
                column: "FilesTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuBannerItems_FilesId",
                table: "MenuBannerItems",
                column: "FilesId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategoriesItems_CategoriesId",
                table: "MenuCategoriesItems",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategoriesItems_MenuId",
                table: "MenuCategoriesItems",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuLangs_LangsId",
                table: "MenuLangs",
                column: "LangsId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuLangs_MenuId",
                table: "MenuLangs",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsFiles_FilesId",
                table: "NewsFiles",
                column: "FilesId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsFiles_NewsId",
                table: "NewsFiles",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsLangs_LangsId",
                table: "NewsLangs",
                column: "LangsId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsLangs_NewsId",
                table: "NewsLangs",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_CategoriesId",
                table: "ProductGroups",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductGroupsId",
                table: "Products",
                column: "ProductGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsCashOut_ProductsCashOutStatusesId",
                table: "ProductsCashOut",
                column: "ProductsCashOutStatusesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsCashOutAddresses_UsersAddressInfoId",
                table: "ProductsCashOutAddresses",
                column: "UsersAddressInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsCashOutCards_ProductsCashOutId",
                table: "ProductsCashOutCards",
                column: "ProductsCashOutId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsCashOutCards_UsersCardsId",
                table: "ProductsCashOutCards",
                column: "UsersCardsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsCashOutDetails_ProductsCashOutId",
                table: "ProductsCashOutDetails",
                column: "ProductsCashOutId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsCashOutDetails_ProductsId",
                table: "ProductsCashOutDetails",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsCashOutDetails_ProductsTransactionsCountId",
                table: "ProductsCashOutDetails",
                column: "ProductsTransactionsCountId");

            migrationBuilder.CreateIndex(
                name: "Unq_ProductsCashOutPaymentsTypes_Id",
                table: "ProductsCashOutPaymentsTypes",
                column: "Id",
                unique: true,
                filter: "[Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsFiles_FilesId",
                table: "ProductsFiles",
                column: "FilesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsFiles_ProductsId",
                table: "ProductsFiles",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "Idx_ProductSpecificationValuesBarcodes",
                table: "ProductSpecificationValuesBarcodes",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsStock_BranchesId",
                table: "ProductsStock",
                column: "BranchesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsStock_ProductId",
                table: "ProductsStock",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsStockDiscountsDetails_ProductsStockDiscountsId",
                table: "ProductsStockDiscountsDetails",
                column: "ProductsStockDiscountsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsStockDiscountsDetails_ProductsStockId",
                table: "ProductsStockDiscountsDetails",
                column: "ProductsStockId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsStockSaleAmounts_ProductsStockId",
                table: "ProductsStockSaleAmounts",
                column: "ProductsStockId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsTransactionDetails_ProductsTransactionsId",
                table: "ProductsTransactionDetails",
                column: "ProductsTransactionsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsTransactionsCount_ProductsTransactionsDetailId",
                table: "ProductsTransactionsCount",
                column: "ProductsTransactionsDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BranchesId",
                table: "Users",
                column: "BranchesId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersCards_UsersId",
                table: "UsersCards",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersClaimsRelations_UsersClaimsId",
                table: "UsersClaimsRelations",
                column: "UsersClaimsId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersClaimsRelations_UsersId",
                table: "UsersClaimsRelations",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "Idx_UsersLogins",
                table: "UsersLogins",
                column: "UsersId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersTokens_UsersId",
                table: "UsersTokens",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_ProductsStockId",
                table: "WishLists",
                column: "ProductsStockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchesFloors");

            migrationBuilder.DropTable(
                name: "BranchesSectorsRelations");

            migrationBuilder.DropTable(
                name: "BranchesSectorsShelfsRelations");

            migrationBuilder.DropTable(
                name: "CartDetails");

            migrationBuilder.DropTable(
                name: "CartOrders");

            migrationBuilder.DropTable(
                name: "CartOrderStatuses");

            migrationBuilder.DropTable(
                name: "CartStatuses");

            migrationBuilder.DropTable(
                name: "CashDeskConfigurations");

            migrationBuilder.DropTable(
                name: "CashDeskSeance");

            migrationBuilder.DropTable(
                name: "CashDeskSeanceManualProducts");

            migrationBuilder.DropTable(
                name: "CashDeskSeanceManualTransactionsTypes");

            migrationBuilder.DropTable(
                name: "CashDeskSeanceStatuses");

            migrationBuilder.DropTable(
                name: "CategoriesFiles");

            migrationBuilder.DropTable(
                name: "CategoriesLangs");

            migrationBuilder.DropTable(
                name: "CategoriesSpecificationsGroupsLangs");

            migrationBuilder.DropTable(
                name: "CategoriesSpecificationsLangs");

            migrationBuilder.DropTable(
                name: "CategoriesSpecificationsPropertiesLangs");

            migrationBuilder.DropTable(
                name: "CategoriesSpecificationsRelations");

            migrationBuilder.DropTable(
                name: "CategoriesSpecificationsStatuses");

            migrationBuilder.DropTable(
                name: "CategorySpecificationsTypesControllersSpecificationsProperties");

            migrationBuilder.DropTable(
                name: "CategorySpecificationsTypesControllersSpecificationsValuesList");

            migrationBuilder.DropTable(
                name: "CategorySpecificationsTypesControllersSpecificationsValuesStrings");

            migrationBuilder.DropTable(
                name: "CategorySpecificationsTypesSpecificationsRelations");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "CompanyDebts");

            migrationBuilder.DropTable(
                name: "CompanyDetailsTypes");

            migrationBuilder.DropTable(
                name: "CompanyTransactions");

            migrationBuilder.DropTable(
                name: "CompanyTransactionsStatuses");

            migrationBuilder.DropTable(
                name: "CompanyTransactionsTypes");

            migrationBuilder.DropTable(
                name: "ConfigurationsTypes");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "ContactsStatuses");

            migrationBuilder.DropTable(
                name: "ContentsCategories");

            migrationBuilder.DropTable(
                name: "ContentsCategoriesLangs");

            migrationBuilder.DropTable(
                name: "ContentsLangs");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "FilesTypesGroups");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "LangStatuses");

            migrationBuilder.DropTable(
                name: "MenuBannerItems");

            migrationBuilder.DropTable(
                name: "MenuCategoriesItems");

            migrationBuilder.DropTable(
                name: "MenuLangs");

            migrationBuilder.DropTable(
                name: "MenuTypes");

            migrationBuilder.DropTable(
                name: "NewsFiles");

            migrationBuilder.DropTable(
                name: "NewsFilesTypes");

            migrationBuilder.DropTable(
                name: "NewsKeywords");

            migrationBuilder.DropTable(
                name: "NewsLangs");

            migrationBuilder.DropTable(
                name: "NewsLangsStatuses");

            migrationBuilder.DropTable(
                name: "ProductsCashOutAddresses");

            migrationBuilder.DropTable(
                name: "ProductsCashOutBonusInfo");

            migrationBuilder.DropTable(
                name: "ProductsCashOutCards");

            migrationBuilder.DropTable(
                name: "ProductsCashOutDetails");

            migrationBuilder.DropTable(
                name: "ProductsCashOutPayments");

            migrationBuilder.DropTable(
                name: "ProductsCashOutPaymentsCards");

            migrationBuilder.DropTable(
                name: "ProductsCashOutPaymentsTypes");

            migrationBuilder.DropTable(
                name: "ProductsCashOutShippingsPackets");

            migrationBuilder.DropTable(
                name: "ProductsCashOutShippingsPacketsDetails");

            migrationBuilder.DropTable(
                name: "ProductsCashOutShippingsPacketsDetailsStatuses");

            migrationBuilder.DropTable(
                name: "ProductsCashOutShippingsPacketsStatuses");

            migrationBuilder.DropTable(
                name: "ProductsFiles");

            migrationBuilder.DropTable(
                name: "ProductsList_sp");

            migrationBuilder.DropTable(
                name: "ProductSpecificationValuesBarcodes");

            migrationBuilder.DropTable(
                name: "ProductsStockDiscountsDetails");

            migrationBuilder.DropTable(
                name: "ProductsStockDiscountsStatuses");

            migrationBuilder.DropTable(
                name: "ProductsStockLocations");

            migrationBuilder.DropTable(
                name: "ProductsStockLocationsCount");

            migrationBuilder.DropTable(
                name: "ProductsStockSaleAmounts");

            migrationBuilder.DropTable(
                name: "ProductsStockSaleAmountsTypes");

            migrationBuilder.DropTable(
                name: "ProductsStockSpecificationsValuesList");

            migrationBuilder.DropTable(
                name: "ProductsStockSpecificationsValuesStrings");

            migrationBuilder.DropTable(
                name: "ProductsStockSpecificationValuesInt");

            migrationBuilder.DropTable(
                name: "ProductsStockStatuses");

            migrationBuilder.DropTable(
                name: "ProductsTransactionsDetailsPlaces");

            migrationBuilder.DropTable(
                name: "ProductsTransactionsStatuses");

            migrationBuilder.DropTable(
                name: "ProductsTransactionsTypes");

            migrationBuilder.DropTable(
                name: "ProductValueTable");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "SlidersLangs");

            migrationBuilder.DropTable(
                name: "Translations");

            migrationBuilder.DropTable(
                name: "TranslationsGroups");

            migrationBuilder.DropTable(
                name: "TranslationsLangs");

            migrationBuilder.DropTable(
                name: "UsersCardsStatuses");

            migrationBuilder.DropTable(
                name: "UsersCardsTypes");

            migrationBuilder.DropTable(
                name: "UsersCardsValues");

            migrationBuilder.DropTable(
                name: "UsersCardsValuesTypes");

            migrationBuilder.DropTable(
                name: "UsersClaimsModules");

            migrationBuilder.DropTable(
                name: "UsersClaimsModulesGroups");

            migrationBuilder.DropTable(
                name: "UsersClaimsRelations");

            migrationBuilder.DropTable(
                name: "UsersClaimsTypes");

            migrationBuilder.DropTable(
                name: "UsersExternalLogins");

            migrationBuilder.DropTable(
                name: "UsersExternalLoginsProviders");

            migrationBuilder.DropTable(
                name: "UsersLogins");

            migrationBuilder.DropTable(
                name: "UsersRoles");

            migrationBuilder.DropTable(
                name: "UsersRolesClaims");

            migrationBuilder.DropTable(
                name: "UsersRolesGroups");

            migrationBuilder.DropTable(
                name: "UsersRolesRelations");

            migrationBuilder.DropTable(
                name: "UsersStatuses");

            migrationBuilder.DropTable(
                name: "UsersTokens");

            migrationBuilder.DropTable(
                name: "UsersTokensStatuses");

            migrationBuilder.DropTable(
                name: "WishLists");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "CashDesk");

            migrationBuilder.DropTable(
                name: "CategoriesSpecificationsGroups");

            migrationBuilder.DropTable(
                name: "CategoriesSpecificationsProperties");

            migrationBuilder.DropTable(
                name: "CategorySpecificationsTypesControllersSpecifications");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Langs");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "UsersAddressInfo");

            migrationBuilder.DropTable(
                name: "UsersCards");

            migrationBuilder.DropTable(
                name: "ProductsCashOut");

            migrationBuilder.DropTable(
                name: "ProductsTransactionsCount");

            migrationBuilder.DropTable(
                name: "ProductsStockDiscounts");

            migrationBuilder.DropTable(
                name: "UsersClaims");

            migrationBuilder.DropTable(
                name: "ProductsStock");

            migrationBuilder.DropTable(
                name: "BranchesFloorsRelations");

            migrationBuilder.DropTable(
                name: "CategoriesSpecifications");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProductsCashOutStatuses");

            migrationBuilder.DropTable(
                name: "ProductsTransactionDetails");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "BranchesPlaces");

            migrationBuilder.DropTable(
                name: "CategoriesSpecificationsTypes");

            migrationBuilder.DropTable(
                name: "FilesFolders ");

            migrationBuilder.DropTable(
                name: "FilesTypes");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "ProductsTransactions");

            migrationBuilder.DropTable(
                name: "ProductGroups");

            migrationBuilder.DropTable(
                name: "CategorySpecificationsTypesControllers");

            migrationBuilder.DropTable(
                name: "CompanyDetails");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
