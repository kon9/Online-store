namespace OnlineStore.Library.Options
{
    public class ServiceAddressOptions
    {
        public const string ServiceAddress = nameof(ServiceAddressOptions);
        public string IdentityServer { get; set; }
        public string UserManagementService { get; set; }
    }
}