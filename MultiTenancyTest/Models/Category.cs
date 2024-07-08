namespace MultiTenancyTest.Models
{
    public class Category: IMustHaveTenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TenantId { get; set; } = null!;
    }
}
