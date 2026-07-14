using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using EbWeb.Models.Common.Options;

namespace EbWeb.Models.Common.Security;

public class AdGroupHandler<TOptions> : AuthorizationHandler<AdGroupRequirement>
    where TOptions : SecurityOptions, new()
{
    private readonly IOptionsMonitor<TOptions> _optionsMonitor;

    public AdGroupHandler(IOptionsMonitor<TOptions> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdGroupRequirement requirement)
    {
        var optionName = typeof(TOptions).Name.Replace("Options", "");

        if (!string.Equals(
                optionName,
                requirement.NomeModulo,
                StringComparison.OrdinalIgnoreCase))
        {
            return Task.CompletedTask;
        }

        if (context.User?.Identity == null ||
            !context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var config = _optionsMonitor.Get(requirement.NomeModulo);
        var allowedGroups = config?.AllowedGroups ?? Array.Empty<string>();

        if (allowedGroups.Length == 0)
        {
            return Task.CompletedTask;
        }

        if (context.User.Identity is WindowsIdentity winIdentity)
        {
            var winPrincipal = new WindowsPrincipal(winIdentity);

            foreach (var rawGroup in allowedGroups)
            {
                if (winPrincipal.IsInRole(rawGroup.Trim()))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
        }

        return Task.CompletedTask;
    }
}