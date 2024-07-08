using MultiTenancyTest.Data;
using MultiTenancyTest.Services;

namespace MultiTenancyTest
{
    public static class ConfigrationServices
    {
        public static IServiceCollection AddTenancy(this IServiceCollection services,
       ConfigurationManager configuration)
        {

           services.AddScoped<ITenantService, TenantService>();

            //  ممكن منحتجش السطر دا 
            //  بس الطريقة دي اهم من الطريقة اللي تحت علشان بتشتخدم ال Dependancy injection يعني هاعرف استخدمها في ال controllers
            //.Configure<TenantSettings> == .Bind(option)
           services.Configure<TenantSettings>(configuration.GetSection(nameof(TenantSettings)));//هاجيب الداتا من المكان دا builder.Configuration.GetSection(nameof(TenantSettings)) علشان امابها (map it) في الكلاس TenantSetings class

            TenantSettings option = new(); // دي ممكن نستخدمها لو عايز امثلا الداتا نفسها هنا في ال program ولو عايز استخدمها في اي حاجة تاني هنا 
           configuration.GetSection(nameof(TenantSettings)).Bind(option);//.Configure<TenantSettings> == .Bind(option)


            var defaultDbProvider = option.Defaults.DBProvider;
            if (defaultDbProvider.ToLower() == "mssql")
            {
               services.AddDbContext<ApplicationDbContext>(f => f.UseSqlServer());
            }
            foreach (var tenant in option.Tenants)
            {
                var connectionString = tenant.ConnectionString ?? option.Defaults.ConnectionString;

                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.SetConnectionString(connectionString);

                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }

            return services;
        }
    }
}
