using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Stored_Procedures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ILoveBaku.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<ProductsSpecificationsValuesStringsLangs> ProductsSpecificationsValuesStringsLangs { get; set; }
        DbSet<ProductsLangs> ProductsLangs { get; set; }
        DbSet<Suppliers> Suppliers { get; set; }
        DbSet<UsersTokensTypes> UsersTokensTypes { get; set; }
        DbSet<ProductsStockReviewStatuses> ProductsStockReviewStatuses { get; set; }
        DbSet<ProductsStockReviews> ProductsStockReviews { get; set; }
        DbSet<CategoriesFiles> CategoriesFiles { get; set; }
        DbSet<ProductsCashOutDetails> ProductsCashOutDetails { get; set; }
        DbSet<ProductsCashOutAddresses> ProductsCashOutAddresses { get; set; }
        DbSet<Branches> Branches { get; set; }
        DbSet<BranchesFloors> BranchesFloors { get; set; }
        DbSet<BranchesFloorsRelations> BranchesFloorsRelations { get; set; }
        DbSet<BranchesPlaces> BranchesPlaces { get; set; }
        DbSet<BranchesSectorsRelations> BranchesSectorsRelations { get; set; }
        DbSet<BranchesSectorsShelfsRelations> BranchesSectorsShelfsRelations { get; set; }
        DbSet<Cart> Carts { get; set; }
        DbSet<CartDetail> CartDetails { get; set; }
        DbSet<CartStatus> CartStatuses { get; set; }
        DbSet<CartOrder> CartOrders { get; set; }
        DbSet<CartOrderStatus> CartOrderStatuses { get; set; }
        DbSet<CashDesk> CashDesk { get; set; }
        DbSet<CashDeskConfigurations> CashDeskConfigurations { get; set; }
        DbSet<CashDeskSeance> CashDeskSeance { get; set; }
        DbSet<CashDeskSeanceManualProducts> CashDeskSeanceManualProducts { get; set; }
        DbSet<CashDeskSeanceManualTransactionsTypes> CashDeskSeanceManualTransactionsTypes { get; set; }
        DbSet<CashDeskSeanceStatuses> CashDeskSeanceStatuses { get; set; }
        DbSet<Categories> Categories { get; set; }
        DbSet<CategoriesLangs> CategoriesLangs { get; set; }
        DbSet<CategoriesSpecifications> CategoriesSpecifications { get; set; }
        DbSet<CategoriesSpecificationsGroups> CategoriesSpecificationsGroups { get; set; }
        DbSet<CategoriesSpecificationsGroupsLangs> CategoriesSpecificationsGroupsLangs { get; set; }
        DbSet<CategoriesSpecificationsLangs> CategoriesSpecificationsLangs { get; set; }
        DbSet<CategoriesSpecificationsProperties> CategoriesSpecificationsProperties { get; set; }
        DbSet<CategoriesSpecificationsPropertiesLangs> CategoriesSpecificationsPropertiesLangs { get; set; }
        DbSet<CategoriesSpecificationsRelations> CategoriesSpecificationsRelations { get; set; }
        DbSet<CategoriesSpecificationsStatuses> CategoriesSpecificationsStatuses { get; set; }
        DbSet<CategoriesSpecificationsTypes> CategoriesSpecificationsTypes { get; set; }
        DbSet<CategorySpecificationsTypesControllers> CategorySpecificationsTypesControllers { get; set; }
        DbSet<CategorySpecificationsTypesControllersSpecifications> CategorySpecificationsTypesControllersSpecifications { get; set; }
        DbSet<CategorySpecificationsTypesControllersSpecificationsProperties> CategorySpecificationsTypesControllersSpecificationsProperties { get; set; }
        DbSet<CategorySpecificationsTypesControllersSpecificationsValuesList> CategorySpecificationsTypesControllersSpecificationsValuesList { get; set; }
        DbSet<CategorySpecificationsTypesControllersSpecificationsValuesStrings> CategorySpecificationsTypesControllersSpecificationsValuesStrings { get; set; }
        DbSet<CategorySpecificationsTypesSpecificationsRelations> CategorySpecificationsTypesSpecificationsRelations { get; set; }
        DbSet<Companies> Companies { get; set; }
        DbSet<CompanyDebts> CompanyDebts { get; set; }
        DbSet<CompanyDetails> CompanyDetails { get; set; }
        DbSet<CompanyDetailsTypes> CompanyDetailsTypes { get; set; }
        DbSet<CompanyTransactions> CompanyTransactions { get; set; }
        DbSet<CompanyTransactionsStatuses> CompanyTransactionsStatuses { get; set; }
        DbSet<CompanyTransactionsTypes> CompanyTransactionsTypes { get; set; }
        DbSet<ConfigurationsTypes> ConfigurationsTypes { get; set; }
        DbSet<Contacts> Contacts { get; set; }
        DbSet<ContactsStatuses> ContactsStatuses { get; set; }
        DbSet<Contents> Contents { get; set; }
        DbSet<ContentsCategories> ContentsCategories { get; set; }
        DbSet<ContentsCategoriesLangs> ContentsCategoriesLangs { get; set; }
        DbSet<ContentsLangs> ContentsLangs { get; set; }
        DbSet<Countries> Countries { get; set; }
        DbSet<ErrorLogs> ErrorLogs { get; set; }
        DbSet<Files> Files { get; set; }
        DbSet<FilesFolders> FilesFolders { get; set; }
        DbSet<FilesTypes> FilesTypes { get; set; }
        DbSet<FilesTypesGroups> FilesTypesGroups { get; set; }
        DbSet<Keywords> Keywords { get; set; }
        DbSet<LangStatuses> LangStatuses { get; set; }
        DbSet<Langs> Langs { get; set; }
        DbSet<Menu> Menu { get; set; }
        DbSet<MenuBannerItems> MenuBannerItems { get; set; }
        DbSet<MenuCategoriesItems> MenuCategoriesItems { get; set; }
        DbSet<MenuLangs> MenuLangs { get; set; }
        DbSet<MenuTypes> MenuTypes { get; set; }
        DbSet<News> News { get; set; }
        DbSet<NewsFiles> NewsFiles { get; set; }
        DbSet<NewsFilesTypes> NewsFilesTypes { get; set; }
        DbSet<NewsKeywords> NewsKeywords { get; set; }
        DbSet<NewsLangs> NewsLangs { get; set; }
        DbSet<NewsLangsStatuses> NewsLangsStatuses { get; set; }
        DbSet<ProductGroups> ProductGroups { get; set; }
        DbSet<ProductSpecificationValuesBarcodes> ProductSpecificationValuesBarcodes { get; set; }
        DbSet<Products> Products { get; set; }
        DbSet<ProductsFiles> ProductsFiles { get; set; }
        DbSet<ProductsCashOut> ProductsCashOut { get; set; }
        DbSet<ProductsCashOutBonusInfo> ProductsCashOutBonusInfo { get; set; }
        DbSet<ProductsCashOutCards> ProductsCashOutCards { get; set; }
        DbSet<ProductsCashOutPayments> ProductsCashOutPayments { get; set; }
        DbSet<ProductsCashOutPaymentsCards> ProductsCashOutPaymentsCards { get; set; }
        DbSet<ProductsCashOutPaymentsTypes> ProductsCashOutPaymentsTypes { get; set; }
        DbSet<ProductsCashOutShippingsPackets> ProductsCashOutShippingsPackets { get; set; }
        DbSet<ProductsCashOutShippingsPacketsDetails> ProductsCashOutShippingsPacketsDetails { get; set; }
        DbSet<ProductsCashOutShippingsPacketsDetailsStatuses> ProductsCashOutShippingsPacketsDetailsStatuses { get; set; }
        DbSet<ProductsCashOutShippingsPacketsStatuses> ProductsCashOutShippingsPacketsStatuses { get; set; }
        DbSet<ProductsCashOutStatuses> ProductsCashOutStatuses { get; set; }
        DbSet<ProductsStock> ProductsStock { get; set; }
        DbSet<ProductsStockDiscounts> ProductsStockDiscounts { get; set; }
        DbSet<ProductsStockDiscountsStatuses> ProductsStockDiscountsStatuses { get; set; }
        DbSet<ProductsStockLocations> ProductsStockLocations { get; set; }
        DbSet<ProductsStockLocationsCount> ProductsStockLocationsCount { get; set; }
        DbSet<ProductsStockSaleAmounts> ProductsStockSaleAmounts { get; set; }
        DbSet<ProductsStockSaleAmountsTypes> ProductsStockSaleAmountsTypes { get; set; }
        DbSet<ProductsStockSpecificationValuesInt> ProductsStockSpecificationValuesInt { get; set; }
        DbSet<ProductsStockSpecificationsValuesList> ProductsStockSpecificationsValuesList { get; set; }
        DbSet<ProductsStockSpecificationsValuesStrings> ProductsStockSpecificationsValuesStrings { get; set; }
        DbSet<ProductsStockStatuses> ProductsStockStatuses { get; set; }
        DbSet<ProductsTransactionDetails> ProductsTransactionDetails { get; set; }
        DbSet<ProductsTransactions> ProductsTransactions { get; set; }
        DbSet<ProductsTransactionsCount> ProductsTransactionsCount { get; set; }
        DbSet<ProductsTransactionsDetailsPlaces> ProductsTransactionsDetailsPlaces { get; set; }
        DbSet<ProductsTransactionsStatuses> ProductsTransactionsStatuses { get; set; }
        DbSet<ProductsTransactionsTypes> ProductsTransactionsTypes { get; set; }
        DbSet<Regions> Regions { get; set; }
        DbSet<Sliders> Sliders { get; set; }
        DbSet<SlidersLangs> SlidersLangs { get; set; }
        DbSet<Translations> Translations { get; set; }
        DbSet<TranslationsGroups> TranslationsGroups { get; set; }
        DbSet<TranslationsLangs> TranslationsLangs { get; set; }
        DbSet<Users> Users { get; set; }
        DbSet<UsersAddressInfo> UsersAddressInfo { get; set; }
        DbSet<UsersCards> UsersCards { get; set; }
        DbSet<UsersCardsStatuses> UsersCardsStatuses { get; set; }
        DbSet<UsersCardsTypes> UsersCardsTypes { get; set; }
        DbSet<UsersCardsValues> UsersCardsValues { get; set; }
        DbSet<UsersCardsValuesTypes> UsersCardsValuesTypes { get; set; }
        DbSet<UsersClaims> UsersClaims { get; set; }
        DbSet<UsersClaimsModules> UsersClaimsModules { get; set; }
        DbSet<UsersClaimsModulesGroups> UsersClaimsModulesGroups { get; set; }
        DbSet<UsersClaimsRelations> UsersClaimsRelations { get; set; }
        DbSet<UsersClaimsTypes> UsersClaimsTypes { get; set; }
        DbSet<UsersExternalLogins> UsersExternalLogins { get; set; }
        DbSet<UsersExternalLoginsProviders> UsersExternalLoginsProviders { get; set; }
        DbSet<UsersLogins> UsersLogins { get; set; }
        DbSet<UsersRoles> UsersRoles { get; set; }
        DbSet<UsersRolesClaims> UsersRolesClaims { get; set; }
        DbSet<UsersRolesGroups> UsersRolesGroups { get; set; }
        DbSet<UsersRolesRelations> UsersRolesRelations { get; set; }
        DbSet<UsersStatuses> UsersStatuses { get; set; }
        DbSet<UsersTokens> UsersTokens { get; set; }
        DbSet<UsersTokensStatuses> UsersTokensStatuses { get; set; }
        DbSet<WishLists> WishLists { get; set; }


        void Entry(object entity, EntityState entityState);
        Task<List<ProductValueTable>> GetValuesTablesByCategoryId(int categoryId);
        Task<List<ProductList_sp>> ProductListStoredProcedure(string culture);
        IQueryable<object> GetTable(string entityName);
        IQueryable<dynamic> GetTableDynamically(string entity);
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken token);
        Task<int> SaveChangesAsync();
        Task<List<TEntity>> Exec<TEntity>(string storedProcedureName, params string[] parameters) where TEntity : class;
    }
}
