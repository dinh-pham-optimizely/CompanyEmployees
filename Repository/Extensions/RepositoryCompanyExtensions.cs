﻿using Repository.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions;

public static class RepositoryCompanyExtensions
{
    public static IQueryable<Company> Search(this IQueryable<Company> companies, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return companies;
        }

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return companies.Where(c => c.Name.ToLower().Contains(lowerCaseTerm));
    }

    public static IQueryable<Company> Sort(this IQueryable<Company> companies, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            return companies.OrderBy(c => c.Name);
        }

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Company>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            return companies.OrderBy(c => c.Name);
        }

        return companies.OrderBy(orderQuery);
    }
}
