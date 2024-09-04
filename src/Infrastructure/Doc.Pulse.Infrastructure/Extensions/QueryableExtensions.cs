using Doc.Pulse.Contracts.Interfaces;
using Doc.Pulse.Core.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Doc.Pulse.Core.Attributes;

namespace Doc.Pulse.Infrastructure.Extensions;

public static class QueryableExtensions
{
    //public static async Task<(IQueryable<T>?, int?)> SimplePaginatedQuery<T>(this DbSet<T> dbSet, IPaginatedQuery request, CancellationToken cancellationToken = default)
    public static async Task<PaginatedQuery<T>> SimplePaginatedQuery<T>(this DbSet<T> dbSet, IPaginatedRequest request, CancellationToken cancellationToken = default)
        where T : class
    {
        if (request == default)
            return PaginatedQuery.New(dbSet.AsQueryable(), await dbSet.CountAsync(cancellationToken));

        var queryable = dbSet
            .OrderByColumn(request.SortBy, request.SortDesc)
            .PaginateFilter(request.Filter)
            .AsQueryable();

        int? total = null;
        if (request.IsPaginated())
        {
            total = await queryable.CountAsync(cancellationToken);

            queryable = queryable
                .Skip(request.SkipAmount ?? 0)
                .Take(request.TakeAmount ?? Globals.MaxPaginatedPage)
                .AsQueryable();
        }

        return PaginatedQuery.New(queryable, total);
    }

    public static IQueryable<T> SimpleFilteredQuery<T>(this DbSet<T> dbSet, IFilteredRequest request)
        where T : class
    {
        var queryable = dbSet
            .OrderByColumn(request.SortBy, request.SortDesc)
            .PaginateFilter(request.Filter)
            .AsQueryable();

        return queryable;
    }

    public static IOrderedQueryable<T> OrderByColumn<T>(this IQueryable<T> source, string? sortOrder, bool? isDescending)
    {
        var sortColumn = sortOrder ??= "Id";
        var sortDescending = isDescending ??= false;

        if (sortDescending)
        {
            return source.OrderByDescending(e => EF.Property<object>(e!, sortColumn));
        }
        else
        {
            return source.OrderBy(e => EF.Property<object>(e!, sortColumn));
        }
    }

    public static IQueryable<T> PaginateFilter<T>(this IQueryable<T> source, string? jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString)) return source;
        IQueryable<T> newSource = source;

        var values = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
        if (values == null) return source;

        foreach (var entry in values)
        {
            Type entityType = typeof(T);
            var propertyInfo = entityType.GetProperty(entry.Key);
            var filterAttribute = propertyInfo?.GetCustomAttribute<PaginateFilterAttribute>();

            if (propertyInfo == null || filterAttribute == null)
            {
                // filtering on this property is not allowed..... warning??
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                newSource = newSource.FilterByColumnStartsWith(entry.Key, entry.Value.ToString()!);
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    var v = converter.ConvertFrom(entry.Value);

                    newSource = newSource.FilterByColumnEqual(entry.Key, v);
                }
            }
        }

        return newSource;
    }

    private static IQueryable<T> FilterByColumnStartsWith<T>(this IQueryable<T> source, string columnName, string columnValue)
    {
        if (columnName == null) throw new ArgumentNullException(nameof(columnName));
        if (columnValue == null) throw new ArgumentNullException(nameof(columnValue));

        return source.Where(e => EF.Functions.Like(EF.Property<string>(e!, columnName), $"{columnValue}%"));
    }

    private static IQueryable<T> FilterByColumnEqual<T, Y>(this IQueryable<T> source, string columnName, Y columnValue)
    {
        if (columnName == null) throw new ArgumentNullException(nameof(columnName));
        if (columnValue == null) throw new ArgumentNullException(nameof(columnValue));

        return source.Where(e => EF.Property<Y>(e!, columnName)!.Equals(columnValue));
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    {
        if (condition) return source.Where(predicate);

        return source;
    }

    public static IQueryable<T> FilterByColumnLikeIfNotNullOrEmpty<T>(this IQueryable<T> source, Expression<Func<T, string?>> propertyExpression, string columnValue)
    {
        ArgumentNullException.ThrowIfNull(propertyExpression);
        
        if (string.IsNullOrEmpty(columnValue)) 
            return source;


        var propertyName = propertyExpression.PropertyNameFromExpression();

        return source.Where(e => EF.Functions.Like(EF.Property<string>(e!, propertyName).ToLowerInvariant(), $"{columnValue.ToLowerInvariant()}%"));
    }

    private static string PropertyNameFromExpression<T>(this Expression<Func<T, string?>> prop)
    {
        if (prop.Body is MemberExpression mExpr)
            return mExpr.Member.Name;
        else if (prop.Body is UnaryExpression uExpr)
            return ((MemberExpression)(uExpr).Operand).Member.Name;
        else 
            throw new NotImplementedException();
    }

    //        public static IOrderedQueryable<T> OrderByColumn<T>(this IQueryable<T> source, string columnPath)
    //            => source.OrderByColumnUsing(columnPath, "OrderBy");

    //        public static IOrderedQueryable<T> OrderByColumnDescending<T>(this IQueryable<T> source, string columnPath)
    //            => source.OrderByColumnUsing(columnPath, "OrderByDescending");

    //        public static IOrderedQueryable<T> ThenByColumn<T>(this IOrderedQueryable<T> source, string columnPath)
    //            => source.OrderByColumnUsing(columnPath, "ThenBy");

    //        public static IOrderedQueryable<T> ThenByColumnDescending<T>(this IOrderedQueryable<T> source, string columnPath)
    //            => source.OrderByColumnUsing(columnPath, "ThenByDescending");

    //        private static IOrderedQueryable<T> OrderByColumnUsing<T>(this IQueryable<T> source, string columnPath, string method)
    //        {
    //            var parameter = Expression.Parameter(typeof(T), "item");
    //            var member = columnPath.Split('.')
    //                .Aggregate((Expression)parameter, Expression.PropertyOrField);
    //            var keySelector = Expression.Lambda(member, parameter);
    //            var methodCall = Expression.Call(typeof(Queryable), method, new[]
    //                    { parameter.Type, member.Type },
    //                source.Expression, Expression.Quote(keySelector));

    //            return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
    //        }

    //public static IQueryable<T> Filter<T>(this IQueryable<T> source, string propertyName, string filterValue)
    //{
    //    var param = Expression.Parameter(typeof(T), "e");
    //    var body = (Expression)param;
    //    foreach (var propName in propertyName.Split('.'))
    //    {
    //        body = Expression.PropertyOrField(body, propName);
    //    }

    //    body = Expression.Call(body, "ToLower", Type.EmptyTypes);
    //    body = Expression.Call(typeof(DbFunctionsExtensions), "Like", Type.EmptyTypes,
    //        Expression.Constant(EF.Functions), body, Expression.Constant($"{filterValue}%".ToLower()));

    //    var lambda = Expression.Lambda(body, param);

    //    var queryExpr = Expression.Call(typeof(Queryable), "Where", new[] { typeof(T) }, source.Expression, lambda);

    //    return source.Provider.CreateQuery<T>(queryExpr);
    //}
}


public class PaginatedQuery
{
    public int? PrePaginatedCount { get; init; } = default!;

    protected PaginatedQuery(int? total)
    {
        this.PrePaginatedCount = total;
    }

    public static PaginatedQuery<T> New<T>(IQueryable<T> queryable, int? total)
    {
        return PaginatedQuery<T>.New(queryable, total);
    }
}
public class PaginatedQuery<T> : PaginatedQuery
{
    public IQueryable<T> Queryable { get; init; } = default!;

    protected PaginatedQuery(IQueryable<T> queryable, int? total) : base(total)
    {
        Queryable = queryable;
    }

    public static PaginatedQuery<T> New(IQueryable<T> queryable, int? total) 
    {
        return new PaginatedQuery<T>(queryable, total);
    }
}