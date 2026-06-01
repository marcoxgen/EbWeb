using Microsoft.AspNetCore.Authorization;

namespace EbWeb.Models.Common.Security;

public class AdGroupRequirement : IAuthorizationRequirement
{
    public string NomeModulo { get; }
    public AdGroupRequirement(string nomeModulo) => NomeModulo = nomeModulo;
}