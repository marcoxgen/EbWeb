using EbWeb.Models.InputModels;
using EbWeb.Models.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace EbWeb.Customizations.ModelBinders;

public class AbilitazioneMifidListInputModelBinder : IModelBinder
{
    private readonly IOptionsMonitor<AbilitazioniMifidOptions> abilitazioniMifidOptions;
    
    public AbilitazioneMifidListInputModelBinder(IOptionsMonitor<AbilitazioniMifidOptions> abilitazioniMifidOptions)
    {
        this.abilitazioniMifidOptions = abilitazioniMifidOptions;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var httpContext = bindingContext.HttpContext;
        var query = httpContext.Request.Query;
        var cookieOptions = new CookieOptions { HttpOnly = true, Secure = true };
        
        var matricola = bindingContext.ValueProvider.GetValue("matricola").FirstValue;
        string intestazione = bindingContext.ValueProvider.GetValue("intestazione").FirstValue;
        string descrUO = bindingContext.ValueProvider.GetValue("descrUO").FirstValue;
        var flagAbilitatoMifid = bindingContext.ValueProvider.GetValue("flagAbilitatoMifid");
        int page = Convert.ToInt32(bindingContext.ValueProvider.GetValue("page").FirstValue);
        string orderBy = bindingContext.ValueProvider.GetValue("orderBy").FirstValue!;
        bool ascending = Convert.ToBoolean(bindingContext.ValueProvider.GetValue("ascending").FirstValue);
        var escluso = bindingContext.ValueProvider.GetValue("escluso");

        AbilitazioniMifidOptions options = abilitazioniMifidOptions.CurrentValue;

        int? matricolaResult = int.TryParse(matricola, out var matricolaParsed) ? matricolaParsed : null;

        bool? flagAbilitatoMifidResult = null;
        if (flagAbilitatoMifid != ValueProviderResult.None && !string.IsNullOrWhiteSpace(flagAbilitatoMifid.FirstValue))
        {
            if (bool.TryParse(flagAbilitatoMifid.FirstValue, out bool parsedValue))
            {
                flagAbilitatoMifidResult = parsedValue;
            }
        }
        
        bool esclusoResult;
        if (escluso != ValueProviderResult.None)
        {
            esclusoResult = Convert.ToBoolean(escluso.FirstValue);
            httpContext.Response.Cookies.Append("Mifid_Escluso_Cookie", escluso.ToString().ToLower(), cookieOptions);
        }
        else if (query.Count > 0)
        {
            esclusoResult = false;
            httpContext.Response.Cookies.Append("Mifid_Escluso_Cookie", "false", cookieOptions);
        }
        else
        {
            if (httpContext.Request.Cookies.TryGetValue("Mifid_Escluso_Cookie", out string savedValue))
            {
                esclusoResult = Convert.ToBoolean(savedValue);
            }
            else
            {
                esclusoResult = options.Escluso;
            }
        }

        AbilitazioneMifidListInputModel inputModel = new(matricolaResult, intestazione, descrUO, flagAbilitatoMifidResult, page, orderBy, ascending, options.PerPage, esclusoResult, options.Order);
        bindingContext.Result = ModelBindingResult.Success(inputModel);

        return Task.CompletedTask;
    }
}