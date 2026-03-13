using System.DirectoryServices.AccountManagement;
using System.Runtime.Versioning;
using System.Security.Principal;

namespace EbWeb.Models.Services.Application;

[SupportedOSPlatform("windows")]
public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserName()
    {
        var nome = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        if (string.IsNullOrEmpty(nome))
        {
            nome = WindowsIdentity.GetCurrent().Name;
        }

        if (string.IsNullOrEmpty(nome)) return "Anonimo";

        return Path.GetFileName(nome) ?? nome;
    }

    public string GetDisplayName()
    {
        var accountName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        
        if (string.IsNullOrEmpty(accountName)) return "Anonimo";

        try
        {
            using var context = new PrincipalContext(ContextType.Domain);
            using var user = UserPrincipal.FindByIdentity(context, accountName);

            if (user != null)
            {
                string displayName = $"{user.Surname ?? ""} {user.GivenName ?? ""}".Trim();
                return string.IsNullOrEmpty(displayName) ? GetUserName() : displayName;
            }
        }
        catch
        {
            // Fallback in caso di problemi con Active Directory
        }

        return GetUserName();
    }
}