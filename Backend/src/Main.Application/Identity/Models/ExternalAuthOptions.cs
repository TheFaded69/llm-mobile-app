namespace Main.Application.Identity.Models;

public class ExternalAuthOptions
{
    public GoogleAuthOptions Google { get; set; } = new();
    public AppleAuthOptions Apple { get; set; } = new();
}

public class GoogleAuthOptions
{
    public List<string> AllowedAudiences { get; set; } = [];
}

public class AppleAuthOptions
{
    public List<string> AllowedAudiences { get; set; } = [];
}
