namespace OnlineStore.Library.Options;

public class AspIdentityApiOptions
{
    public const string SectionName = nameof(AspIdentityApiOptions);
    public string UserName { get; set; } = "admin@example.com";
    public string Password { get; set; } = "Admin123!";
}