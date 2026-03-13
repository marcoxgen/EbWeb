using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

namespace EbWeb.Models.Services.Application;

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
        string userName = Path.GetFileName(nome);

        return userName;
    }

    public string GetDisplayName()
    {
        var accountName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        if (string.IsNullOrEmpty(accountName)) return "Anonimo";

        using var context = new PrincipalContext(ContextType.Domain);
        using var user = UserPrincipal.FindByIdentity(context, accountName);

        if (user != null)
        {
            var givenName = user.GivenName ?? "";
            var surname = user.Surname ?? "";
            return $"{surname} {givenName}".Trim();
        }

        return GetUserName(); // fallback
    }
}