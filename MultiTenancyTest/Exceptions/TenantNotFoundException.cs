namespace MultiTenancyTest.Exceptions
{
    public class TenantNotFoundException : Exception
    {
        public TenantNotFoundException(string tenantId)
            : base($"Tenant with ID '{tenantId}' not found.")
        {
        }
    }


}
