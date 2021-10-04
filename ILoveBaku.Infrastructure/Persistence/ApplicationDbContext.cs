using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using ILoveBaku.Infrastructure.Extensions;
using System.Text;
using System.Threading.Tasks;
using ILoveBaku.Application.CQRS.Product.Queries.GetProducts;
using ILoveBaku.Domain.Stored_Procedures;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualBasic.CompilerServices;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Application.CQRS.ProductCashOut.Queries.GetProductCashOutDetails;

namespace ILoveBaku.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        #region Entities
        public virtual DbSet<ProductsSpecificationsValuesStringsLangs> ProductsSpecificationsValuesStringsLangs { get; set; }
        public virtual DbSet<ProductsLangs> ProductsLangs { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<UsersTokensTypes> UsersTokensTypes { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<ProductsStockReviewStatuses> ProductsStockReviewStatuses { get; set; }
        public virtual DbSet<ProductsStockReviews> ProductsStockReviews { get; set; }
        public virtual DbSet<CategoriesFiles> CategoriesFiles { get; set; }
        public virtual DbSet<ProductsCashOutAddresses> ProductsCashOutAddresses { get; set; }
        public virtual DbSet<ProductsCashOutDetails> ProductsCashOutDetails { get; set; }
        public virtual DbSet<Branches> Branches { get; set; }
        public virtual DbSet<ProductsFiles> ProductsFiles { get; set; }
        public virtual DbSet<BranchesFloors> BranchesFloors { get; set; }
        public virtual DbSet<BranchesFloorsRelations> BranchesFloorsRelations { get; set; }
        public virtual DbSet<BranchesPlaces> BranchesPlaces { get; set; }
        public virtual DbSet<BranchesSectorsRelations> BranchesSectorsRelations { get; set; }
        public virtual DbSet<BranchesSectorsShelfsRelations> BranchesSectorsShelfsRelations { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartDetail> CartDetails { get; set; }
        public virtual DbSet<CartStatus> CartStatuses { get; set; }
        public virtual DbSet<CartOrder> CartOrders { get; set; }
        public virtual DbSet<CartOrderStatus> CartOrderStatuses { get; set; }
        public virtual DbSet<CashDesk> CashDesk { get; set; }
        public virtual DbSet<CashDeskConfigurations> CashDeskConfigurations { get; set; }
        public virtual DbSet<CashDeskSeance> CashDeskSeance { get; set; }
        public virtual DbSet<CashDeskSeanceManualProducts> CashDeskSeanceManualProducts { get; set; }
        public virtual DbSet<CashDeskSeanceManualTransactionsTypes> CashDeskSeanceManualTransactionsTypes { get; set; }
        public virtual DbSet<CashDeskSeanceStatuses> CashDeskSeanceStatuses { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<CategoriesLangs> CategoriesLangs { get; set; }
        public virtual DbSet<CategoriesSpecifications> CategoriesSpecifications { get; set; }
        public virtual DbSet<CategoriesSpecificationsGroups> CategoriesSpecificationsGroups { get; set; }
        public virtual DbSet<CategoriesSpecificationsGroupsLangs> CategoriesSpecificationsGroupsLangs { get; set; }
        public virtual DbSet<CategoriesSpecificationsLangs> CategoriesSpecificationsLangs { get; set; }
        public virtual DbSet<CategoriesSpecificationsProperties> CategoriesSpecificationsProperties { get; set; }
        public virtual DbSet<CategoriesSpecificationsPropertiesLangs> CategoriesSpecificationsPropertiesLangs { get; set; }
        public virtual DbSet<CategoriesSpecificationsRelations> CategoriesSpecificationsRelations { get; set; }
        public virtual DbSet<CategoriesSpecificationsStatuses> CategoriesSpecificationsStatuses { get; set; }
        public virtual DbSet<CategoriesSpecificationsTypes> CategoriesSpecificationsTypes { get; set; }
        public virtual DbSet<CategorySpecificationsTypesControllers> CategorySpecificationsTypesControllers { get; set; }
        public virtual DbSet<CategorySpecificationsTypesControllersSpecifications> CategorySpecificationsTypesControllersSpecifications { get; set; }
        public virtual DbSet<CategorySpecificationsTypesControllersSpecificationsProperties> CategorySpecificationsTypesControllersSpecificationsProperties { get; set; }
        public virtual DbSet<CategorySpecificationsTypesControllersSpecificationsValuesList> CategorySpecificationsTypesControllersSpecificationsValuesList { get; set; }
        public virtual DbSet<CategorySpecificationsTypesControllersSpecificationsValuesStrings> CategorySpecificationsTypesControllersSpecificationsValuesStrings { get; set; }
        public virtual DbSet<CategorySpecificationsTypesSpecificationsRelations> CategorySpecificationsTypesSpecificationsRelations { get; set; }
        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<CompanyDebts> CompanyDebts { get; set; }
        public virtual DbSet<CompanyDetails> CompanyDetails { get; set; }
        public virtual DbSet<CompanyDetailsTypes> CompanyDetailsTypes { get; set; }
        public virtual DbSet<CompanyTransactions> CompanyTransactions { get; set; }
        public virtual DbSet<CompanyTransactionsStatuses> CompanyTransactionsStatuses { get; set; }
        public virtual DbSet<CompanyTransactionsTypes> CompanyTransactionsTypes { get; set; }
        public virtual DbSet<ConfigurationsTypes> ConfigurationsTypes { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<ContactsStatuses> ContactsStatuses { get; set; }
        public virtual DbSet<Contents> Contents { get; set; }
        public virtual DbSet<ContentsCategories> ContentsCategories { get; set; }
        public virtual DbSet<ContentsCategoriesLangs> ContentsCategoriesLangs { get; set; }
        public virtual DbSet<ContentsLangs> ContentsLangs { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<ErrorLogs> ErrorLogs { get; set; }
        public virtual DbSet<FilesFolders> FilesFolders { get; set; }
        public virtual DbSet<FilesTypes> FilesTypes { get; set; }
        public virtual DbSet<FilesTypesGroups> FilesTypesGroups { get; set; }
        public virtual DbSet<Keywords> Keywords { get; set; }
        public virtual DbSet<LangStatuses> LangStatuses { get; set; }
        public virtual DbSet<Langs> Langs { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuBannerItems> MenuBannerItems { get; set; }
        public virtual DbSet<MenuCategoriesItems> MenuCategoriesItems { get; set; }
        public virtual DbSet<MenuLangs> MenuLangs { get; set; }
        public virtual DbSet<MenuTypes> MenuTypes { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsFiles> NewsFiles { get; set; }
        public virtual DbSet<NewsFilesTypes> NewsFilesTypes { get; set; }
        public virtual DbSet<NewsKeywords> NewsKeywords { get; set; }
        public virtual DbSet<NewsLangs> NewsLangs { get; set; }
        public virtual DbSet<NewsLangsStatuses> NewsLangsStatuses { get; set; }
        public virtual DbSet<ProductGroups> ProductGroups { get; set; }
        public virtual DbSet<ProductSpecificationValuesBarcodes> ProductSpecificationValuesBarcodes { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductsCashOut> ProductsCashOut { get; set; }
        public virtual DbSet<ProductsCashOutBonusInfo> ProductsCashOutBonusInfo { get; set; }
        public virtual DbSet<ProductsCashOutCards> ProductsCashOutCards { get; set; }
        public virtual DbSet<ProductsCashOutPayments> ProductsCashOutPayments { get; set; }
        public virtual DbSet<ProductsCashOutPaymentsCards> ProductsCashOutPaymentsCards { get; set; }
        public virtual DbSet<ProductsCashOutPaymentsTypes> ProductsCashOutPaymentsTypes { get; set; }
        public virtual DbSet<ProductsCashOutShippingsPackets> ProductsCashOutShippingsPackets { get; set; }
        public virtual DbSet<ProductsCashOutShippingsPacketsDetails> ProductsCashOutShippingsPacketsDetails { get; set; }
        public virtual DbSet<ProductsCashOutShippingsPacketsDetailsStatuses> ProductsCashOutShippingsPacketsDetailsStatuses { get; set; }
        public virtual DbSet<ProductsCashOutShippingsPacketsStatuses> ProductsCashOutShippingsPacketsStatuses { get; set; }
        public virtual DbSet<ProductsCashOutStatuses> ProductsCashOutStatuses { get; set; }
        public virtual DbSet<ProductsStock> ProductsStock { get; set; }
        public virtual DbSet<ProductsStockDiscounts> ProductsStockDiscounts { get; set; }
        public virtual DbSet<ProductsStockDiscountsStatuses> ProductsStockDiscountsStatuses { get; set; }
        public virtual DbSet<ProductsStockLocations> ProductsStockLocations { get; set; }
        public virtual DbSet<ProductsStockLocationsCount> ProductsStockLocationsCount { get; set; }
        public virtual DbSet<ProductsStockSaleAmounts> ProductsStockSaleAmounts { get; set; }
        public virtual DbSet<ProductsStockSaleAmountsTypes> ProductsStockSaleAmountsTypes { get; set; }
        public virtual DbSet<ProductsStockSpecificationValuesInt> ProductsStockSpecificationValuesInt { get; set; }
        public virtual DbSet<ProductsStockSpecificationsValuesList> ProductsStockSpecificationsValuesList { get; set; }
        public virtual DbSet<ProductsStockSpecificationsValuesStrings> ProductsStockSpecificationsValuesStrings { get; set; }
        public virtual DbSet<ProductsStockStatuses> ProductsStockStatuses { get; set; }
        public virtual DbSet<ProductsTransactionDetails> ProductsTransactionDetails { get; set; }
        public virtual DbSet<ProductsTransactions> ProductsTransactions { get; set; }
        public virtual DbSet<ProductsTransactionsCount> ProductsTransactionsCount { get; set; }
        public virtual DbSet<ProductsTransactionsDetailsPlaces> ProductsTransactionsDetailsPlaces { get; set; }
        public virtual DbSet<ProductsTransactionsStatuses> ProductsTransactionsStatuses { get; set; }
        public virtual DbSet<ProductsTransactionsTypes> ProductsTransactionsTypes { get; set; }
        public virtual DbSet<Regions> Regions { get; set; }
        public virtual DbSet<Sliders> Sliders { get; set; }
        public virtual DbSet<SlidersLangs> SlidersLangs { get; set; }
        public virtual DbSet<Translations> Translations { get; set; }
        public virtual DbSet<TranslationsGroups> TranslationsGroups { get; set; }
        public virtual DbSet<TranslationsLangs> TranslationsLangs { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersAddressInfo> UsersAddressInfo { get; set; }
        public virtual DbSet<UsersCards> UsersCards { get; set; }
        public virtual DbSet<UsersCardsStatuses> UsersCardsStatuses { get; set; }
        public virtual DbSet<UsersCardsTypes> UsersCardsTypes { get; set; }
        public virtual DbSet<UsersCardsValues> UsersCardsValues { get; set; }
        public virtual DbSet<UsersCardsValuesTypes> UsersCardsValuesTypes { get; set; }
        public virtual DbSet<UsersClaims> UsersClaims { get; set; }
        public virtual DbSet<UsersClaimsModules> UsersClaimsModules { get; set; }
        public virtual DbSet<UsersClaimsModulesGroups> UsersClaimsModulesGroups { get; set; }
        public virtual DbSet<UsersClaimsRelations> UsersClaimsRelations { get; set; }
        public virtual DbSet<UsersClaimsTypes> UsersClaimsTypes { get; set; }
        public virtual DbSet<UsersExternalLogins> UsersExternalLogins { get; set; }
        public virtual DbSet<UsersExternalLoginsProviders> UsersExternalLoginsProviders { get; set; }
        public virtual DbSet<UsersLogins> UsersLogins { get; set; }
        public virtual DbSet<UsersRoles> UsersRoles { get; set; }
        public virtual DbSet<UsersRolesClaims> UsersRolesClaims { get; set; }
        public virtual DbSet<UsersRolesGroups> UsersRolesGroups { get; set; }
        public virtual DbSet<UsersRolesRelations> UsersRolesRelations { get; set; }
        public virtual DbSet<UsersStatuses> UsersStatuses { get; set; }
        public virtual DbSet<UsersTokens> UsersTokens { get; set; }
        public virtual DbSet<UsersTokensStatuses> UsersTokensStatuses { get; set; }
        public virtual DbSet<WishLists> WishLists { get; set; }
        #endregion

        public virtual DbSet<ProductList_sp> ProductsList_sp { get; set; }
        public virtual DbSet<ProductValueTable> ProductValueTable { get; set; }
        public virtual DbSet<ProductStockDto> Products_sp { get; set; }
        public virtual DbSet<ProductCashOutDetailDto> ProductsCashOut_sp { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        public async Task<List<ProductList_sp>> ProductListStoredProcedure(string culture)
        {
            try
            {
                // Processing.
                string sqlQuery = $"EXEC [dbo].[ProductList_sp] '{culture}'";
                var data = await Set<ProductList_sp>().FromSqlRaw(sqlQuery).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ProductValueTable>> GetValuesTablesByCategoryId(int categoryId)
        {
            try
            {
                // Processing.
                string sqlQuery = $"EXEC [dbo].[GetValuesTablesByCategoryId] '{categoryId}'";
                var data = await Set<ProductValueTable>().FromSqlRaw(sqlQuery).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductList_sp>().HasNoKey();
            modelBuilder.Entity<ProductValueTable>().HasNoKey();

            modelBuilder.Entity<Branches>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedIp).HasColumnName("CreatedIP");
            });

            modelBuilder.Entity<BranchesFloors>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<BranchesPlaces>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<BranchesSectorsShelfsRelations>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CartDetail>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CashDesk>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CashDeskConfigurations>(entity =>
            {
                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CashDeskSeance>(entity =>
            {
                entity.Property(e => e.CurrentAmount).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.EndAmount).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartAmount).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CashDeskSeanceManualTransactionsTypes>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CashDeskSeanceStatuses>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<CategoriesLangs>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CategoriesSpecifications>(entity =>
            {
                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<CategoriesSpecificationsGroups>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<CategoriesSpecificationsGroupsLangs>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<CategoriesSpecificationsLangs>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<CategoriesSpecificationsProperties>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CategoriesSpecificationsPropertiesLangs>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CategoriesSpecificationsRelations>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<CategoriesSpecificationsStatuses>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CategoriesSpecificationsTypes>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CategorySpecificationsTypesControllers>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CategorySpecificationsTypesControllersSpecifications>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CategorySpecificationsTypesControllersSpecificationsProperties>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CategorySpecificationsTypesControllersSpecificationsValuesStrings>(entity =>
            {
                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Companies>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedIp).HasColumnName("CreatedIP");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Voen)
                    .IsRequired()
                    .HasColumnName("VOEN")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CompanyDebts>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CompanyDetails>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Contacts)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactsName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedIp).HasColumnName("CreatedIP");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Sun)
                    .IsRequired()
                    .HasColumnName("SUN")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CompanyDetailsTypes>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<CompanyTransactions>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedIp).HasColumnName("CreatedIP");
            });

            modelBuilder.Entity<CompanyTransactionsStatuses>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<CompanyTransactionsTypes>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ConfigurationsTypes>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contacts>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ContactsStatuses>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contents>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ContentsCategories>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ContentsCategoriesLangs>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ContentsLangs>(entity =>
            {
                entity.Property(e => e.ContentHtml)
                    .IsRequired()
                    .HasColumnName("ContentHTML");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.SubTitle).HasMaxLength(1000);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ErrorLogs>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedIp)
                    .HasColumnName("CreatedIP")
                    .HasColumnType("datetime");

                entity.Property(e => e.LogText)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("URL")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FilesFolders>(entity =>
            {
                entity.ToTable("FilesFolders ");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<FilesTypes>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<FilesTypesGroups>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Keywords>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<LangStatuses>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Langs>(entity =>
            {
                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Link)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MenuBannerItems>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Link)
                    .IsRequired()
                    .IsUnicode(false);
            });


            modelBuilder.Entity<MenuLangs>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MenuTypes>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<News>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ShowDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<NewsFiles>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<NewsFilesTypes>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<NewsLangs>(entity =>
            {
                entity.Property(e => e.ContentHtml)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.TitleUrl)
                    .IsRequired()
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<NewsLangsStatuses>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProductGroups>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedIp).HasColumnName("CreatedIP");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProductSpecificationValuesBarcodes>(entity =>
            {
                entity.HasIndex(e => e.Value)
                    .HasName("Idx_ProductSpecificationValuesBarcodes")
                    .IsUnique();
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedIp).HasColumnName("CreatedIP");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProductsCashOut>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(200);
            });

            modelBuilder.Entity<ProductsCashOutBonusInfo>(entity =>
            {
                entity.Property(e => e.BonusAmount).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.BonusCount).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<ProductsCashOutPayments>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProductsCashOutPaymentsTypes>(entity =>
            {
                entity.HasNoKey();

                entity.HasIndex(e => e.Id)
                    .HasName("Unq_ProductsCashOutPaymentsTypes_Id")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<ProductsCashOutShippingsPackets>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ResponsablePerson)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProductsCashOutShippingsPacketsDetails>(entity =>
            {
                entity.Property(e => e.TrackingNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductsCashOutShippingsPacketsDetailsStatuses>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProductsCashOutShippingsPacketsStatuses>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProductsCashOutStatuses>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ProductsStock>(entity =>
            {
                entity.Property(e => e.Count).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.PriorityDate).HasColumnType("datetime");

                entity.Property(e => e.TaxPercent).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProductsStockDiscounts>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.DiscountValue).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.ExpireDate).HasColumnType("datetime");

                entity.Property(e => e.MinimumOrder).HasColumnType("decimal(10, 4)");
            });

            modelBuilder.Entity<ProductsStockDiscountsStatuses>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProductsStockLocations>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProductsStockSaleAmounts>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 4)");
            });

            modelBuilder.Entity<ProductsStockSaleAmountsTypes>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProductsStockStatuses>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProductsTransactionDetails>(entity =>
            {
                entity.Property(e => e.BuyAmount).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.CostAmount).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.Count).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DiscountPercent).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PayAmount).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<ProductsTransactions>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(200);

                entity.Property(e => e.ReceipDate).HasColumnType("datetime");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.TotalDiscountAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TotalPayAmount).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProductsTransactionsCount>(entity =>
            {
                entity.Property(e => e.Count).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProductsTransactionsStatuses>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ProductsTransactionsTypes>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Regions>(entity =>
            {
                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Sliders>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Link)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<SlidersLangs>(entity =>
            {
                entity.Property(e => e.SubTitle)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Translations>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.TransKey)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TranslationsGroups>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TranslationsLangs>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(1000);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedIp).HasColumnName("CreatedIP");

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(20);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pin)
                    .HasColumnName("PIN")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<UsersAddressInfo>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsersCards>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<UsersCardsStatuses>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<UsersCardsTypes>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<UsersCardsValues>(entity =>
            {
                entity.Property(e => e.Value).HasColumnType("decimal(10, 4)");
            });

            modelBuilder.Entity<UsersCardsValuesTypes>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<UsersClaims>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.DisplayName).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsersClaimsModules>(entity =>
            {
                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Controller)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UsersClaimsModulesGroups>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UsersClaimsRelations>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<UsersClaimsTypes>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UsersExternalLogins>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ProviderKey)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsersExternalLoginsProviders>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<UsersLogins>(entity =>
            {
                entity.HasIndex(e => e.UsersId)
                    .HasName("Idx_UsersLogins")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedIp).HasColumnName("CreatedIP");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LockoutEnabled)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(8000);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(8000);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<UsersRoles>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UsersRolesClaims>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<UsersRolesGroups>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UsersRolesRelations>(entity =>
            {
                entity.HasKey(e => e.UsersId)
                    .HasName("Idx_UsersRolesRelations");

                entity.Property(e => e.UsersId).ValueGeneratedNever();
            });

            modelBuilder.Entity<UsersStatuses>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UsersTokens>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ExpireDate).HasColumnType("datetime");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(130);
            });

            modelBuilder.Entity<UsersTokensStatuses>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WishLists>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProductCashOutDetailDto>(entity =>
            {
                entity.HasNoKey();
            });

            base.OnModelCreating(modelBuilder);
        }

        public IQueryable<object> GetTable(string entityName)
        {
            Type type = null;
            var properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == entityName)
                {
                    type = property.PropertyType;
                }
            }

            var method = this.GetType().GetMethod("GetTableType", BindingFlags.NonPublic | BindingFlags.Instance);
            var inovokeMethod = method.MakeGenericMethod(new Type[] { type });
            return (IQueryable<object>)inovokeMethod.Invoke(this, new object[] { type.GetGenericArguments()[0] });
        }

        public IQueryable<dynamic> GetTableDynamically(string entity)
        {
            Type type = null;
            var properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == entity)
                {
                    type = property.PropertyType;
                }
            }

            var method = this.GetType().GetMethod("GetTableType", BindingFlags.NonPublic | BindingFlags.Instance);
            var inovokeMethod = method.MakeGenericMethod(new Type[] { type });
            return (IQueryable<dynamic>)inovokeMethod.Invoke(this, new object[] { type.GetGenericArguments()[0] });
        }

        private T GetTableType<T>(Type type)
        {
            return (T)((IDbSetCache)this).GetOrAddSet(this.GetDependencies().SetSource, type);
        }

        public void Entry(object entity, EntityState entityState)
        {
            this.Entry(entity).State = entityState;
        }

        public async Task<List<TEntity>> Exec<TEntity>(string storedProcedureName, params string[] parameters) where TEntity : class
        {
            var str = @$"EXEC {storedProcedureName} {string.Join(", ", parameters.Where(p => !string.IsNullOrEmpty(p))
                                                                                                                 .Select(p => $"'{p}'"))
                                                                                           .Trim().TrimEnd(',')}";
            var data= Set<TEntity>().FromSqlRaw(@$"EXEC {storedProcedureName} {string.Join(", ", parameters.Where(p => !string.IsNullOrEmpty(p))
                                                                                                                 .Select(p => $"'{p}'"))
                                                                                           .Trim().TrimEnd(',')}").ToList();
            return await Set<TEntity>().FromSqlRaw(@$"EXEC {storedProcedureName} {string.Join(", ", parameters.Where(p => !string.IsNullOrEmpty(p))
                                                                                                                 .Select(p => $"'{p}'"))
                                                                                           .Trim().TrimEnd(',')}").ToListAsync();
        }
    }
}
