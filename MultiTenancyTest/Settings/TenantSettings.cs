namespace MultiTenancyTest.Settings
{
    public class TenantSettings
    {
        // دا هيبقى جواه الاتنين اللي هم ال Configration and Tenant
        //وهو الشكل دا اللي هضيفه داخل ال app Settings 
        public Configration Defaults { get; set; } = null!;
        public List<Tenant> Tenants { get; set; } = new();
    }
}
