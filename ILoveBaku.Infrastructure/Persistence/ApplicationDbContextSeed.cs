using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using ILoveBaku.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ILoveBaku.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(this ApplicationDbContext _context)
        {
            await _context.RequiredDataSeedAsync();
            await _context.RequiredLanguagesSeedAsync();
            Guid userId = await _context.UserSeedAsync();
            Guid roleId = await _context.UserRoleSeedAsync();

            if (userId != Guid.Empty && roleId != Guid.Empty)
                await _context.UserRoleRelationAsync(userId, roleId);

            await _context.UserLoginSeedAsync();
            await _context.CashDeskSeedAsync();
        }

        private static async Task RequiredLanguagesSeedAsync(this IApplicationDbContext _context)
        {
            if (_context.Langs.Count() == 0)
            {
                Langs langsAz = new Langs
                {
                    Id = 10,
                    LangsStatusesId = 10,
                    Culture = "az-AZ",
                    DisplayName = "Azerbaijan",
                    Name = "az",
                    Priority = 10
                };

                Langs langsEn = new Langs
                {
                    Id = 20,
                    LangsStatusesId = 10,
                    Culture = "en-US",
                    DisplayName = "English",
                    Name = "en",
                    Priority = 20
                };

                Langs langsRu = new Langs
                {
                    Id = 30,
                    LangsStatusesId = 10,
                    Culture = "ru-RU",
                    DisplayName = "Russian",
                    Name = "ru",
                    Priority = 30
                };

                await _context.Langs.AddAsync(langsAz);
                await _context.Langs.AddAsync(langsEn);
                await _context.Langs.AddAsync(langsRu);
                await _context.SaveChangesAsync();

            }

        }

        private static async Task<Guid> UserSeedAsync(this IApplicationDbContext _context)
        {
            if (!await _context.Users.AnyAsync())
            {
                Users appUser = new Users
                {
                    Id = Guid.NewGuid(),
                    Name = "Ferid",
                    Surname = "XXXXX",
                    UsersStatusesId = (int)UserStatus.Active,
                    Patronymic = "XXXX",
                    ContactEmail = "info@nurayza.az",
                    ContactNumber = "0XXXXXXXXX",
                    DocumentNumber = "AZEXXXXXXXX",
                    Pin = "XXXXXXX",
                    CreatedDate = DateTime.Now,
                    CreatedIp = 1,
                    UpdatedDate = DateTime.Now,
                    BranchesId = await _context.BranchSeedAsync()
                };

                await _context.Users.AddAsync(appUser);
                await _context.SaveChangesAsync();

                return appUser.Id;
            }

            return Guid.Empty;
        }
        private static async Task RequiredDataSeedAsync(this IApplicationDbContext _context)
        {
            if (!await _context.UsersStatuses.AnyAsync())
            {
                UsersStatuses userActiveStatus = new UsersStatuses
                {
                    Name = "Aktiv",
                    Id = 10
                };

                UsersStatuses userDeactiveStatus = new UsersStatuses
                {
                    Name = "Deaktiv",
                    Id = 20
                };

                await _context.UsersStatuses.AddAsync(userActiveStatus);
                await _context.UsersStatuses.AddAsync(userDeactiveStatus);
            }

            if (!await _context.CompanyDetailsTypes.AnyAsync())
            {
                CompanyDetailsTypes type = new CompanyDetailsTypes
                {
                    Id = 10,
                    Name = "Satiş nöqtəsi",
                    Description = "test",
                };

                CompanyDetailsTypes type1 = new CompanyDetailsTypes
                {
                    Id = 20,
                    Name = "Firma",
                    Description = "test",
                };

                await _context.CompanyDetailsTypes.AddAsync(type);
                await _context.CompanyDetailsTypes.AddAsync(type1);
            }

            if (!await _context.UsersTokensStatuses.AnyAsync())
            {
                UsersTokensStatuses active = new UsersTokensStatuses
                {
                    Id = 10,
                    Name = "Aktiv"
                };

                UsersTokensStatuses deactive = new UsersTokensStatuses
                {
                    Id = 20,
                    Name = "Deaktiv"
                };

                await _context.UsersTokensStatuses.AddAsync(active);
                await _context.UsersTokensStatuses.AddAsync(deactive);
            }

            if (!await _context.UsersCardsStatuses.AnyAsync())
            {
                UsersCardsStatuses status1 = new UsersCardsStatuses
                {
                    Id = 10,
                    Name = "Aktiv"
                };

                UsersCardsStatuses status2 = new UsersCardsStatuses
                {
                    Id = 20,
                    Name = "Deaktiv"
                };

                await _context.UsersCardsStatuses.AddAsync(status1);
                await _context.UsersCardsStatuses.AddAsync(status2);
            }

            if (!await _context.UsersExternalLoginsProviders.AnyAsync())
            {
                UsersExternalLoginsProviders provider1 = new UsersExternalLoginsProviders
                {
                    Id = 10,
                    Name = "Facebook"
                };

                UsersExternalLoginsProviders provider2 = new UsersExternalLoginsProviders
                {
                    Id = 20,
                    Name = "Google"
                };

                await _context.UsersExternalLoginsProviders.AddAsync(provider1);
                await _context.UsersExternalLoginsProviders.AddAsync(provider2);
            }

            if(!await _context.CompanyTransactionsTypes.AnyAsync())
            {
                CompanyTransactionsTypes type1 = new CompanyTransactionsTypes
                {
                    Id = 10,
                    Name = "Mədaxil",
                };

                CompanyTransactionsTypes type2 = new CompanyTransactionsTypes
                {
                    Id = 20,
                    Name = "Məxaric",
                };

                await _context.CompanyTransactionsTypes.AddAsync(type1);
                await _context.CompanyTransactionsTypes.AddAsync(type2);
            }


            if (!await _context.CompanyTransactionsStatuses.AnyAsync())
            {
                CompanyTransactionsStatuses status1 = new CompanyTransactionsStatuses
                {
                    Id = 10,
                    Name = "Aktiv",
                    ParentId = 0
                };

                CompanyTransactionsStatuses status2 = new CompanyTransactionsStatuses
                {
                    Id = 20,
                    Name = "Imtina Edilmiş",
                    ParentId = 0
                };

                await _context.CompanyTransactionsStatuses.AddAsync(status1);
                await _context.CompanyTransactionsStatuses.AddAsync(status2);
            }

            if(!await _context.ProductsTransactionsTypes.AnyAsync())
            {
                ProductsTransactionsTypes type1 = new ProductsTransactionsTypes
                {
                    Id = 10,
                    Description = "test",
                    IsActive = true,
                    Name = "Firmadan daxil olma",
                    ParentId = 0,
                    Priority = 10
                };
            }

            if (!await _context.MenuTypes.AnyAsync())
            {
                MenuTypes menuType1 = new MenuTypes
                {
                    Id = 10,
                    IsActive = true,
                    Name = "Header"
                };

                MenuTypes menuType2 = new MenuTypes
                {
                    Id = 20,
                    IsActive = false,
                    Name = "Footer"
                };

                await _context.MenuTypes.AddAsync(menuType1);
                await _context.MenuTypes.AddAsync(menuType2);
            }

            if (!await _context.FilesTypesGroups.AnyAsync())
            {
                FilesTypesGroups group = new FilesTypesGroups
                {
                    Id = 10,
                    Name = "Picture"
                };

                FilesTypesGroups group2 = new FilesTypesGroups
                {
                    Id = 20,
                    Name = "Icon"
                };

                await _context.FilesTypesGroups.AddAsync(group);
                await _context.FilesTypesGroups.AddAsync(group2);
                await _context.SaveChangesAsync();

            }

            if(!await _context.CategoriesSpecificationsStatuses.AnyAsync())
            {
                var categorySpecStatus = new CategoriesSpecificationsStatuses
                {
                    Id = 10,
                    Name = "Hər yerdə görünsün"
                };

                await _context.CategoriesSpecificationsStatuses.AddAsync(categorySpecStatus);
            }
            if(!await _context.FilesFolders.AnyAsync())
            {
                FilesFolders folder1 = new FilesFolders
                {
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsAllowDelete = false,
                    Name = "Categories",
                    ParentId = 0,
                    Priority = 10
                };

                FilesFolders folder2 = new FilesFolders
                {
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsAllowDelete = false,
                    Name = "Products",
                    ParentId = 0,
                    Priority = 10
                };

                await _context.FilesFolders.AddAsync(folder1);
                await _context.FilesFolders.AddAsync(folder2);
            }

            if(!await _context.UsersCardsTypes.AnyAsync())
            {
                UsersCardsTypes card1 = new UsersCardsTypes
                {
                    Name = "Bonus kartı",
                    Id = 10
                };

                UsersCardsTypes card2 = new UsersCardsTypes
                {
                    Name = "Hədiyyə kartı",
                    Id = 20
                };

                await _context.UsersCardsTypes.AddAsync(card1);
                await _context.UsersCardsTypes.AddAsync(card2);
            }

            if (!await _context.FilesTypes.AnyAsync())
            {
                FilesTypes type1 = new FilesTypes
                {
                    FilesTypesGroupsId = 10,
                    Name = "jpg"
                };

                FilesTypes type2 = new FilesTypes
                {
                    FilesTypesGroupsId = 10,
                    Name = "png"
                };
                FilesTypes type3 = new FilesTypes
                {
                    FilesTypesGroupsId = 10,
                    Name = "jpeg"
                };
                FilesTypes type4 = new FilesTypes
                {
                    FilesTypesGroupsId = 10,
                    Name = "jfif"
                };
                FilesTypes type5 = new FilesTypes
                {
                    FilesTypesGroupsId = 20,
                    Name = "svg"
                };

                await _context.FilesTypes.AddAsync(type1);
                await _context.FilesTypes.AddAsync(type2);
                await _context.FilesTypes.AddAsync(type3);
                await _context.FilesTypes.AddAsync(type4);
                await _context.FilesTypes.AddAsync(type5);
            }

            if (!await _context.ProductsStockSaleAmountsTypes.AnyAsync())
            {
                ProductsStockSaleAmountsTypes type = new ProductsStockSaleAmountsTypes
                {
                    Id = 10,
                    Name = "Pərakəndə satış"
                };
                ProductsStockSaleAmountsTypes type2 = new ProductsStockSaleAmountsTypes
                {
                    Id = 20,
                    Name = "Topdan satış"
                };

                await _context.ProductsStockSaleAmountsTypes.AddAsync(type);
                await _context.ProductsStockSaleAmountsTypes.AddAsync(type2);
            }

            if (!await _context.ProductsStockStatuses.AnyAsync())
            {
                ProductsStockStatuses status = new ProductsStockStatuses
                {
                    Id = 10,
                    Name = "Aktiv"
                };

                ProductsStockStatuses status2 = new ProductsStockStatuses
                {
                    Id = 20,
                    Name = "Deaktiv"
                };

                await _context.ProductsStockStatuses.AddAsync(status);
                await _context.ProductsStockStatuses.AddAsync(status2);
            }


            if (!await _context.ProductsTransactionsStatuses.AnyAsync())
            {
                ProductsTransactionsStatuses status = new ProductsTransactionsStatuses
                {
                    Id = 10,
                    IsActive = true,
                    Name = "Bitmiş",
                    Priority = 10
                };

                ProductsTransactionsStatuses status2 = new ProductsTransactionsStatuses
                {
                    Id = 20,
                    IsActive = true,
                    Name = "Gözləmədə",
                    Priority = 20
                };

                await _context.ProductsTransactionsStatuses.AddAsync(status);
                await _context.ProductsTransactionsStatuses.AddAsync(status2);

            }

            if(!await _context.LangStatuses.AnyAsync())
            {
                LangStatuses status1 = new LangStatuses
                {
                    Id=10,
                    Name = "Aktiv"
                };

                LangStatuses status2 = new LangStatuses
                {
                    Id=20,
                    Name = "Deaktiv"
                };

                await _context.LangStatuses.AddAsync(status1);
                await _context.LangStatuses.AddAsync(status2);
            }

            if(!await _context.CartStatuses.AnyAsync())
            {
                await _context.CartStatuses.AddAsync(new Domain.Entities.CartStatus
                {
                    Id = 10,
                    Name = "Gözləmədə"
                });

                await _context.CartStatuses.AddAsync(new Domain.Entities.CartStatus
                {
                    Id = 20,
                    Name = "Ödənilib"
                });
            }


            //if(!await _context.ProductsCashOutPaymentsTypes.AnyAsync())
            //{
            //    var type1 = new ProductsCashOutPaymentsTypes
            //    {
            //        Id = 10,
            //        IsActive = true,
            //        Name = "Nağd",
            //        Priority = 10
            //    };

            //    var type2 = new ProductsCashOutPaymentsTypes
            //    {
            //        Id = 20,
            //        IsActive = true,
            //        Name = "Nağdsız",
            //        Priority = 20
            //    };

            //    await _context.ProductsCashOutPaymentsTypes.AddAsync(type1);
            //    await _context.ProductsCashOutPaymentsTypes.AddAsync(type2);
            //}

            if(!await _context.Countries.AnyAsync())
            {
                Countries country = new Countries()
                {
                    Name = "Azərbaycan",
                    IsActive = true,
                    Priority = 10
                };
                await _context.Countries.AddAsync(country);
                await _context.SaveChangesAsync();

                Regions region = new Regions
                {
                    CountryId = _context.Countries.FirstOrDefault().Id,
                    IsActive = 1,
                    Name = "Bakı",
                    Priority = 10
                };

                await _context.Regions.AddAsync(region);
                await _context.SaveChangesAsync();
            }


            if(!await _context.ProductsCashOutStatuses.AnyAsync())
            {
                var status1 = new ProductsCashOutStatuses
                {
                    Id = 10,
                    IsActive = true,
                    Name = "Ödənilib"
                };

                var status2 = new ProductsCashOutStatuses
                {
                    Id = 20,
                    IsActive = true,
                    Name = "Imtina edilib"
                };

                var status3 = new ProductsCashOutStatuses
                {
                    Id = 30,
                    IsActive = true,
                    Name = "Gözləmədə"
                };

                await _context.ProductsCashOutStatuses.AddAsync(status1);
                await _context.ProductsCashOutStatuses.AddAsync(status2);
                await _context.ProductsCashOutStatuses.AddAsync(status3);

            }

            //if(!await _context.ProductsCashOutShippingsPacketsStatuses.AnyAsync())
            //{
            //    var status1 = new ProductsCashOutShippingsPacketsStatuses
            //    {
            //        Id = 10,
            //        Name = "Hazırlanır"
            //    };

            //    var status2 = new ProductsCashOutShippingsPacketsDetailsStatuses
            //    {
            //        Name = "Aktiv",
            //        Id = 10
            //    };

            //    var status3 = new ProductsCashOutShippingsPacketsDetailsStatuses
            //    {
            //        Name = "Deaktiv",
            //        Id = 20
            //    };

            //    await _context.ProductsCashOutShippingsPacketsStatuses.AddAsync(status1);
            //    await _context.ProductsCashOutShippingsPacketsDetailsStatuses.AddAsync(status2);
            //    await _context.ProductsCashOutShippingsPacketsDetailsStatuses.AddAsync(status3);
            //}

            if(!await _context.ProductsStockReviewStatuses.AnyAsync())
            {
                var reviewstatus = new ProductsStockReviewStatuses
                {
                    Id = 10,
                    Name = "Təsdiqlənmiş"
                };

                var reviewstatus2 = new ProductsStockReviewStatuses
                {
                    Id = 20,
                    Name = "Ləğv edilmiş"
                };

                await _context.ProductsStockReviewStatuses.AddAsync(reviewstatus);
                await _context.ProductsStockReviewStatuses.AddAsync(reviewstatus2);
            }

            //if(!await _context.ProductsCashOutShippingsPackets.AnyAsync())
            //{
            //    var packet = new ProductsCashOutShippingsPackets
            //    {
            //        ProductsCashOutShippingsPacketsStatusesId = 10,
            //        CreatedDate = DateTime.Now,
            //        DeliveryCompaniesId = 0,
            //        Name = "Online",
            //        ResponsablePerson = "Online"
            //    };

            //    await _context.ProductsCashOutShippingsPackets.AddAsync(packet);
            //}

            await _context.SaveChangesAsync();
        }

        private static async Task<int> BranchSeedAsync(this IApplicationDbContext _context)
        {
            if (!await _context.Branches.AnyAsync())
            {
                Branches branch = new Branches
                {
                    CreatedDate = DateTime.Now,
                    CreatedIp = 1,
                    CompanyDetailsId = await _context.CompanyDetailSeedAsync(),
                };

                BranchesFloors floor = new BranchesFloors
                {
                    Id = 1,
                    Name = "Online"
                };
                await _context.BranchesFloors.AddAsync(floor);

                
                await _context.Branches.AddAsync(branch);
                await _context.SaveChangesAsync();

                BranchesPlaces place = new BranchesPlaces
                {
                    BranchesId = branch.Id,
                    IsSalesRows = true,
                    IsActive = true,
                    Name = "Online",
                    Priority = 10
                };
                await _context.BranchesPlaces.AddAsync(place);
                await _context.SaveChangesAsync();

                BranchesFloorsRelations relation = new BranchesFloorsRelations
                {
                    BranchesFloorsId = _context.BranchesFloors.FirstOrDefault().Id,
                    BranchesPlacesId = _context.BranchesPlaces.FirstOrDefault().Id
                };
                await _context.BranchesFloorsRelations.AddAsync(relation);

                return branch.Id;
            }

            return 1;
        }

        private static async Task<int> CompanyDetailSeedAsync(this IApplicationDbContext _context)
        {
            if (!await _context.CompanyDetails.AnyAsync())
            {
                CompanyDetails companyDetail = new CompanyDetails
                {
                    Name = "ILoveBaku Online",
                    Sun = "TEST",
                    ContactsName = "XXXX XXXXX",
                    CreatedDate = DateTime.Now,
                    Contacts = "050XXXXXXX",
                    Address = "Genclik pr 47A",
                    CreatedIp = 1,
                    Description = "test",
                    IsActive = true,
                    Email = "info@nurayza.az",
                    CompaniesId = await _context.CompanySeedAsync(),
                    CompanyDetailsTypesId = (int)CompanyDetailType.SatishNoqtesi
                };

                await _context.CompanyDetails.AddAsync(companyDetail);
                await _context.SaveChangesAsync();
                return companyDetail.Id;
            }

            return 0;
        }

        private static async Task<int> CompanySeedAsync(this IApplicationDbContext _context)
        {
            if (!await _context.Companies.AnyAsync())
            {
                Companies company = new Companies
                {
                    CreatedDate = DateTime.Now,
                    CreatedIp = 1,
                    IsActive = true,
                    Name = "Nurayza",
                    Voen = "test"
                };

                await _context.Companies.AddAsync(company);
                await _context.SaveChangesAsync();
                return company.Id;
            }

            return 0;
        }

        private static async Task<Guid> UserRoleSeedAsync(this IApplicationDbContext _context)
        {
            if (!await _context.UsersRoles.AnyAsync())
            {
                UsersRoles role = new UsersRoles
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    UsersRolesGroupsId = await _context.UserRolesGroupSeedAsync()
                };

                await _context.UsersRoles.AddAsync(role);
                await _context.SaveChangesAsync();
                return role.Id;
            }

            return Guid.Empty;
        }

        private static async Task<Guid> UserRolesGroupSeedAsync(this IApplicationDbContext _context)
        {
            if (!await _context.UsersRolesGroups.AnyAsync())
            {
                UsersRolesGroups roleGroup = new UsersRolesGroups
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin group"
                };

                await _context.UsersRolesGroups.AddAsync(roleGroup);
                await _context.SaveChangesAsync();
                return roleGroup.Id;
            }

            return Guid.Empty;
        }

        private static async Task UserRoleRelationAsync(this IApplicationDbContext _context, Guid userId, Guid roleId)
        {
            if (!await _context.UsersRolesRelations.AnyAsync())
            {
                UsersRolesRelations relations = new UsersRolesRelations
                {
                    UsersId = userId,
                    UsersRolesId = roleId
                };
                await _context.UsersRolesRelations.AddAsync(relations);
                await _context.SaveChangesAsync();
            }
        }

        private static async Task UserLoginSeedAsync(this IApplicationDbContext _context)
        {
            if (!await _context.UsersLogins.AnyAsync() && await _context.Users.CountAsync() == 1)
            {
                byte[] salt = Hashing.GenerateSalt();
                UsersLogins userLogin = new UsersLogins
                {
                    Id = Guid.NewGuid(),
                    UsersId = (await _context.Users.FirstOrDefaultAsync()).Id,
                    Salt = salt,
                    CreatedDate = DateTime.Now,
                    Email = "info@nurayza.az",
                    CreatedIp = 1,
                    UpdatedDate = DateTime.Now,
                    Password = Hashing.Hash("nurayza123", salt),
                };

                await _context.UsersLogins.AddAsync(userLogin);
                await _context.SaveChangesAsync();
            }
        }


        private static async Task CashDeskSeedAsync(this IApplicationDbContext _context)
        {
            if(!await _context.CashDesk.AnyAsync())
            {
                CashDesk cashDesk = new CashDesk
                {
                    BranchesFloorsRelationsId = _context.BranchesFloorsRelations.FirstOrDefault().Id,
                    Description = ".....",
                    IsActive = true,
                    Name = "Online",
                };
                await _context.CashDesk.AddAsync(cashDesk);


                CashDeskSeanceStatuses status = new CashDeskSeanceStatuses
                {
                    Id = 10,
                    IsActive = true,
                    Name = "Davam edir",
                    Priority = 10
                };

                CashDeskSeanceStatuses status2 = new CashDeskSeanceStatuses
                {
                    Id = 20,
                    IsActive = true,
                    Name = "Bitib",
                    Priority = 20
                };
                await _context.CashDeskSeanceStatuses.AddAsync(status);
                await _context.CashDeskSeanceStatuses.AddAsync(status2);

                await _context.SaveChangesAsync();


                var cashDeskSeance = new CashDeskSeance
                {
                    CashDeskId = _context.CashDesk.FirstOrDefault().Id,
                    StartAmount = 0,
                    StartDate = DateTime.Now,
                    CashDeskSeanceStatusesId = Convert.ToByte(CashDeskSeanceStatus.Continue),
                    CurrentAmount = 0,
                    EndDate = DateTime.Now,
                    Description = "....",
                    EndAmount = 0,
                    UsersId = _context.Users.FirstOrDefault().Id
                };
                await _context.CashDeskSeance.AddAsync(cashDeskSeance);
                await _context.SaveChangesAsync();
            }
        }
    }
}
