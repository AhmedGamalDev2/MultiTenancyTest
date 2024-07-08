using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MultiTenancyTest.Services;

namespace MultiTenancyTest.Data
{
    public class ApplicationDbContext:DbContext
    {
        private readonly ITenantService _tenantService;
        public string tenantId { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
             tenantId = _tenantService.GetCurrentTenant()?.TId;//? => is important
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnectionString();
            if(!string.IsNullOrEmpty(tenantConnectionString))
            {
                var dbProvider = _tenantService.GetDatabaseProvider();
                if(dbProvider?.ToLower() == "mssql")
                {
                    optionsBuilder.UseSqlServer(tenantConnectionString);
                }
            }

         }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // هنا حلينا مشكلة الفلترة مع كل request ولكن واحنا فقط بنعمل retreive the data
            modelBuilder.Entity<Product>().HasQueryFilter(f => f.TenantId == tenantId);// currentTentantId //HasQueryFilter(f => f.TenantId ==  _tenantService.GetCurrentTenant().TId;)
            modelBuilder.Entity <Category>().HasQueryFilter(f => f.TenantId == tenantId);// currentTentantId //HasQueryFilter(f => f.TenantId ==  _tenantService.GetCurrentTenant().TId;)
            base.OnModelCreating(modelBuilder);
        }



        // وبرضو في حالة ال adding مش هنقعد كل شوية نضيف ال tenantId اللي هو جاي في ال request الى كل table بيتم انشاءه 
        // ولكن هنعمل override على الدالة savechanges 
        //يعني لما تيجي تعمل savechanges لأي entity هنضيف مع ال entity دي column تاني اسمه tenantId وهيكون قيمته هي قيمة ا ل tenantId اللي جاي في ال request headers
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ////ChangeTracker.Entries<IMustHaveTenant>() => هنا باختار كل ال entities اللي بتعمل implement for IMustHaveTenant ودا هو المطلوب
            ////.Where(f=>f.State== EntityState.Added) => وهنا بعمل select لكل ال entities اللي هي بيحصل ليها عمليه adding

            //foreach (var entity in ChangeTracker.Entries<IMustHaveTenant>().Where(f=>f.State== EntityState.Added))
            //{ //هنا لل adding فقط
            //    //entity => دي ال  entity اللي بنلف عليها في الوقت الحالي 
            //    //entity.Entity.TenantId = tenantId;//tenantId => comming from request header
            //}

            foreach (var entity in ChangeTracker.Entries<IMustHaveTenant>())
            { // هنا لل adding and updating
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.TenantId = tenantId;
                        break;
                    case EntityState.Modified:
                        if (entity.Entity.TenantId != tenantId)
                        {
                            throw new UnauthorizedAccessException("You do not have permission to modify this resource.");
                        }
                        break;
                }
            } 
                return base.SaveChangesAsync(cancellationToken);
        }
    }
}

