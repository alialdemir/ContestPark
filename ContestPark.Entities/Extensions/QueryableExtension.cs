using ContestPark.Entities.Models;
using System;
using System.Linq;

namespace ContestPark.Extensions
{
    public static class QueryableExtension
    {
        /// <summary>
        /// IQueryable tipindeki query'leri apiden dönüş yapabilecek türde(ServiceModel) dönüştürür
        /// </summary>
        /// <typeparam name="TSource">Generic model type</typeparam>
        /// <param name="source">query</param>
        /// <param name="paging">Paging</param>
        /// <returns>ServiceModel<TSource> service result</returns>
        public static ServiceModel<TSource> ToServiceModel<TSource>(this IQueryable<TSource> source, PagingModel paging)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (paging == null)
                throw new ArgumentNullException(nameof(paging));

            ServiceModel<TSource> serviceModel = new ServiceModel<TSource>
            {
                Count = source.Count(),
                PageSize = paging.PageSize,
                PageNumber = paging.PageNumber,
            };
            if (IsGetData(serviceModel.Count, paging)) serviceModel.Items = source
                                                                .Skip(paging.PageSize * (paging.PageNumber - 1))
                                                                .Take(paging.PageSize)
                                                                .AsEnumerable();
            return serviceModel;
        }

        private static bool IsGetData(long count, PagingModel paging)
        {
            if (paging.PageNumber <= 0 || paging.PageSize <= 0) return false;
            return !((paging.PageNumber - 1) > count / paging.PageSize);
        }
    }
}