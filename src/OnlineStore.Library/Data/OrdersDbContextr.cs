using Microsoft.EntityFrameworkCore;
using OnlineStore.Library.ArticleService.Models;
using OnlineStore.Library.OrdersService.Models;

namespace OnlineStore.Library.Data
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<PriceList> PriceLists { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderedArticle> OrderedArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OrderedArticle>()
                .HasOne<Order>(e => e.Order)
                .WithMany(d => d.Articles)
                .HasForeignKey(e => e.OrderId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}