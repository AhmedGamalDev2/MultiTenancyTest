namespace MultiTenancyTest.Exceptions
{

    public class TenantIdMissingException : Exception
    {
        public TenantIdMissingException()
            : base("No tenant ID provided in the request header.")
        {
        }
    }
}
