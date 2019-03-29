using ContestPark.Entities.Helpers;
using ContestPark.Entities.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace ContestPark.Extensions
{
    public static class QueryableExtension
    {
        /// <summary>
        /// IFindFluent tipindeki query'leri apiden dönüş yapabilecek türde(ServiceModel) dönüştürür
        /// </summary>
        /// <typeparam name="TSource">Generic model type</typeparam>
        /// <param name="source">query</param>
        /// <param name="paging">Paging</param>
        /// <returns>ServiceModel<TSource> service result</returns>
        public static ServiceModel<TSource> QueryPaging<TSource>(this IDbConnection source, string query, object param, PagingModel paging)
        {
            Check.IsNull(source, nameof(source));
            Check.IsNull(paging, nameof(paging));

            dynamic paramss = CreateExpandoFromObject(param);
            paramss.PageSize = paging.PageSize;
            paramss.PageNumber = PageNumberCalculate(paging);

            string percent = "SELECT TOP (100) PERCENT " + query.Substring(6);
            string query1 = $@"SELECT COUNT(*) FROM ({percent}) AS c;",
                   query2 = query + " OFFSET @PageNumber ROWS FETCH NEXT @PageSize ROWS ONLY";
            var queryMultiple = source.QueryMultiple(query1 + query2, (object)paramss);

            ServiceModel<TSource> serviceModel = new ServiceModel<TSource>
            {
                Count = queryMultiple.Read<int>().FirstOrDefault(),
                PageSize = paging.PageSize,
                PageNumber = paging.PageNumber,
            };

            if (IsGetData(serviceModel.Count, paging))
                serviceModel.Items = queryMultiple.Read<TSource>();

            return serviceModel;
        }

        public static long PageNumberCalculate(PagingModel paging)
        {
            return paging.PageSize * (paging.PageNumber - 1);
        }

        private static ExpandoObject CreateExpandoFromObject(object source)
        {
            var result = new ExpandoObject();
            if (source == null) return result;

            IDictionary<string, object> dictionary = result;
            foreach (var property in source
                .GetType()
                .GetProperties()
                .Where(p => p.CanRead && p.GetMethod.IsPublic))
            {
                dictionary[property.Name] = property.GetValue(source, null);
            }
            return result;
        }

        public static bool IsGetData(long count, PagingModel paging)
        {
            if (paging.PageNumber <= 0 || paging.PageSize <= 0) return false;
            return !((paging.PageNumber - 1) > count / paging.PageSize);
        }
    }
}