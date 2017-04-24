namespace MobileApi.Server.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MobileApi.Server.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MobileApi.Server.Infrastructure.ApplicationDbContext context)
        {

        }
    }
}
