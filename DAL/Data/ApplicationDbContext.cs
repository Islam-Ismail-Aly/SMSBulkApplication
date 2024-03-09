using BLL.Authentication;
using BLL.Entities;
using DAL.Entities;
using DAL.FluentApi;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Campaign> Campaigns => Set<Campaign>();
        public DbSet<SMSNumber> SMSNumbers => Set<SMSNumber>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // To apply all configurations in Fluent API by (Assembly)

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            #region Fluent API Configuration Classes

            // To apply configuration in Fluent API by (Configuration Class)

            //builder.ApplyConfiguration(new PhoneEntityConfiguration());
            //builder.ApplyConfiguration(new CampaignEntityConfiguration());
            //builder.ApplyConfiguration(new SubscriptionEntityConfiguration()); 
            #endregion
        }

    }
}
