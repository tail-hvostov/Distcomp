using ArticleHouse.DAO.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticleHouse.DAO;

public class ApplicationContext : DbContext
{
    public DbSet<ArticleModel> Articles {get; set;} = null!;
    public DbSet<CreatorModel> Creators {get; set;} = null!;
    public DbSet<MarkModel> Marks {get; set;} = null!;
    public DbSet<CommentModel> Comments {get; set;} = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}
}