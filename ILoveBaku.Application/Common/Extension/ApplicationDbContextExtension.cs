using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.CQRS.Product.Models;
using ILoveBaku.Domain.Entities;
using ILoveBaku.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ILoveBaku.Application.Common.Extension
{
    static class ApplicationDbContextExtension
    {
        public static IQueryable<T> Search<T>(this IQueryable<T> items, params KeyValuePair<string, object>[] searchingProperties)
        {
            foreach (KeyValuePair<string, object> searchingProperty in searchingProperties)
            {
                object value = searchingProperty.Value;

                //Lambdaya oturulecek parametr/argument T type inde example: (x) => 
                ParameterExpression parametr = Expression.Parameter(typeof(T));

                //Hemin T type nin hansi property-si oldugunu gotururuk example: (x) => x.propertyName
                MemberExpression property = Expression.Property(parametr, searchingProperty.Key);

                Type propertyType = property.Type;

                value = Convert.ChangeType(value, propertyType);

                value = Expression.Constant(value, propertyType);

                // property-nin deyerinin axtarilan deyere beraberliyini yoxlamaq example: (x) => x.propertyName == value
                Expression condition = Expression.Equal(property, (ConstantExpression)value);

                //condition-na gore lambda expression hazirlanir
                Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(condition, parametr);

                //sonda expression tree func delegate-a compile olunur
                items = items.Where(expression);
            }

            return items;
        }

        public static List<T> Search<T>(this List<T> items, params KeyValuePair<string, object>[] searchingProperties)
        {
            foreach (KeyValuePair<string, object> searchingProperty in searchingProperties)
            {
                object value = searchingProperty.Value;

                //Lambdaya oturulecek parametr/argument T type inde example: (x) => 
                ParameterExpression parametr = Expression.Parameter(typeof(T));

                //Hemin T type nin hansi property-si oldugunu gotururuk example: (x) => x.propertyName
                MemberExpression property = Expression.Property(parametr, searchingProperty.Key);

                Type propertyType = property.Type;

                value = Convert.ChangeType(value, propertyType);

                value = Expression.Constant(value, propertyType);

                // property-nin deyerinin axtarilan deyere beraberliyini yoxlamaq example: (x) => x.propertyName == value
                Expression condition = Expression.Equal(property, (ConstantExpression)value);

                //condition-na gore lambda expression hazirlanir
                Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(condition, parametr);

                //sonda expression tree func delegate-a compile olunur
                items = items.Where(expression.Compile()).ToList();
            }

            return items;
        }

        public static decimal GetPrice(this ProductsStock productStock)
        {
            return productStock.Product.DefaultSaleAmount.Value;
            //return productStock.pro;

            // return productStock.Sales.FirstOrDefault(s => s.ProductStockSaleAmountsTypesId == (byte)ProductStockSaleAmountType.Retail).Amount;
        }

        //public static decimal GetDiscountedPrice(this ProductsStock productStock)
        //{
        //    return productStock.GetPrice()
        //                          .PercentReductionOf(productStock.ProductsStockDiscountsDetails
        //                                                             .FirstOrDefault(psdd => psdd.IsActive &&
        //                                                                                     psdd.ProductsStockDiscounts.ProductsStockDiscountsStatusesId == 10 &&
        //                                                                                     psdd.ProductsStockDiscounts.ExpireDate >= DateTime.Now)
        //                                                                .ProductsStockDiscounts.DiscountValue);
        //}

        public static decimal GetPrice(this ProductsStock productStock, ProductStockSaleAmountType saleAmountType = ProductStockSaleAmountType.NonSelected)
        {
            return productStock.Sales.FirstOrDefault(s => ((int)saleAmountType).IsZore() ? s.ProductStockSaleAmountsTypesId == (byte)ProductStockSaleAmountType.Retail :
                                                                                           s.ProductStockSaleAmountsTypesId == (byte)saleAmountType).Amount.Round(2);
        }

        public static decimal GetDiscountedPrice(this ProductsStock productStock, ProductStockSaleAmountType saleAmountType = ProductStockSaleAmountType.NonSelected)
        {
            return productStock.GetPrice(saleAmountType)
                             .PercentReductionOf(productStock.ProductsStockDiscountsDetails.Where(psdd => psdd.IsActive && psdd.ProductsStockDiscounts.ExpireDate >= DateTime.Now)
                                .Sum(ps => ps.ProductsStockDiscounts.DiscountValue)).Round(2);
        }

        public static async Task<Langs> GetLanguage(this IApplicationDbContext context, string culture)
        {
            return await context.Langs.FirstOrDefaultAsync(l => l.Culture == culture);
        }

        public static async Task<Langs> GetLanguage(this IApplicationDbContext context, Expression<Func<Langs, bool>> expression)
        {
            return await context.Langs.FirstOrDefaultAsync(expression);
        }
    }
}
