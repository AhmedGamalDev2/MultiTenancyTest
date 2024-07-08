
using Microsoft.Extensions.Options;
using MultiTenancyTest.Exceptions;

namespace MultiTenancyTest.Services
{
    public class TenantService : ITenantService
    {
        private Tenant? _currentTenant;
        private HttpContext? _httpContext; //علشان اقدر استخدم ال  HttpContext دا لازما اعمل injection for IHttpContextAccessor 
        //private Tenant? _tenant;
        private readonly TenantSettings _tenantSettings;
        private readonly ILogger<TenantService> _logger;

        public TenantService(IHttpContextAccessor _httpContextAccessor, IOptions<TenantSettings> tenantSettings, ILogger<TenantService> logger)
        {
            //map htttpcontex
            _httpContext = _httpContextAccessor.HttpContext;
            _logger = logger;

            #region دا كله علشان نعمل initialize for _currentTenant
            //read tenan from header request 
            _tenantSettings = tenantSettings.Value;

            if (_httpContext is not null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    ////tenantId:: is tenantId that commming from request header 
                    ////get tenant from configration settings and set it in _currentTenant
                    ///// here we initialize _currentTenant
                    //_currentTenant = _tenantSettings.Tenants.FirstOrDefault(f => f.TId == tenantId);
                    //if(_currentTenant is null)
                    //{
                    //    throw new Exception("Invalid tenant Id");
                    //}
                    //if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
                    //{ // لو ال ConnectionString اللي جاية في ال request فاضية خلي قيمتها ال defualt connectionstring او اللي هي shared 
                    //    _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
                    //}

                    SetCurrentTenant(tenantId);
                }
                else
                {
                    //throw new Exception("No tenant provided");
                    _logger.LogError("No tenant ID provided in the request header.");
                    throw new TenantIdMissingException();
                }
            }
            #endregion

        }
        public string? GetConnectionString()
        {
            var currentConnectionString = _currentTenant is null
            ? _tenantSettings.Defaults.ConnectionString
            : _currentTenant.ConnectionString;

            return currentConnectionString;
        }

        public Tenant? GetCurrentTenant()
        {
            return _currentTenant;
        }

        public string? GetDatabaseProvider()
        {
            return _tenantSettings.Defaults.DBProvider;
        }

        private void SetCurrentTenant(string tenantId)
        {
            //tenantId:: is tenantId that commming from request header 
            //get tenant from configration settings and set it in _currentTenant
            _currentTenant = _tenantSettings.Tenants.FirstOrDefault(f => f.TId == tenantId);
            if (_currentTenant is null)
            {
                //throw new Exception("Invalid tenant Id");
                _logger.LogError($"Tenant with ID '{tenantId}' not found.");
                throw new TenantNotFoundException(tenantId);
            }
            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            { // لو ال ConnectionString اللي جاية في ال request فاضية خلي قيمتها ال defualt connectionstring او اللي هي shared 
                _logger.LogInformation("Connection string not provided for tenant '{tenantId}', using default connection string.");

                _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
            }
        }
    }
}
