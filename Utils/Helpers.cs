using System.Linq.Expressions;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ZooKeepers.Models;

namespace ZooKeepers.Helpers
{
    public class AnimalControllerHelpers
    {

    public static IQueryable<Animal> OrderDirection
    (
        IQueryable<Animal> searchQuery,
        Expression<Func<Animal, object>> filter,
        string orderDirection
    )
    {
        if (orderDirection == "False") 
            return searchQuery.OrderBy(filter);
        else
            return searchQuery.OrderByDescending(filter);
    }

    public static Expression<Func<Animal, object>> GetFilterLambdaExpression(string filter)
    {
        
        switch(filter.ToLower())
        {
            case "name":
            return animal => animal.Name;

            case "classification":
            return animal => animal.Classification;
            
            case "dateacquired":
            return animal  => animal.DateAcquired;

            case "age":
            return animal => animal.DateOfBirth;

            case "species":
            return animal => animal.Species;
            
            default:
            return animal => animal.Species;
        } 
    }
    public static object CreateProblemDetailsObject(HttpStatusCode statuscode, string message)
    {
        return new ProblemDetails 
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Status = (int?)statuscode,
            Detail = message,
        };
    }

    }
}