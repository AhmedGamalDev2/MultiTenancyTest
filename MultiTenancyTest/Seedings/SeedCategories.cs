using MultiTenancyTest.Data;

namespace MultiTenancyTest.Seedings
{
    public static class SeedCategories
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Check if any categories already exist
            //if (!context.Categories.Any())
            //{
                context.Categories.AddRange(
                    new Category { Name = "Category1" }
                    //new Category { Name = "Category2", TenantId = "Tenant2" },
                    //new Category { Name = "Category3", TenantId = "Tenant1" },
                    //new Category { Name = "Category4", TenantId = "Tenant2" }
                );

                context.SaveChanges();
            //}
        }
    }
}
