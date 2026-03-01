using ArticleHouse.DAO;
using ArticleHouse.DAO.Implementations.Memory;
using ArticleHouse.DAO.Implementations;
using ArticleHouse.DAO.Interfaces;
using ArticleHouse.DAO.Models;
using ArticleHouse.Service.Implementations;
using ArticleHouse.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ArticleHouse;

static internal class ServiceProviderExtensions
{
    public static IServiceCollection AddArticleHouseServices(this IServiceCollection collection, string? connection)
    {
        collection.AddScoped<ICreatorService, CreatorService>();
        collection.AddScoped<IArticleService, ArticleService>();
        collection.AddScoped<ICommentService, CommentService>();

        collection.AddScoped<IMarkService, MarkService>();
        collection.AddSingleton<IMarkDAO, MemoryMarkDAO>();

        collection.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection).UseSnakeCaseNamingConvention());
        return collection;
    }
}