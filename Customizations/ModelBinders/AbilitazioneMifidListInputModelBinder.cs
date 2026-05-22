using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using EbWeb.Models.AbilitazioniMifid.InputModels;
using EbWeb.Models.AbilitazioniMifid.Options;

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
        string ruolo = bindingContext.ValueProvider.GetValue("ruolo").FirstValue;
        var flagAbilitatoMifid = bindingContext.ValueProvider.GetValue("flagAbilitatoMifid");
        var abilitatoFinanceWMP = bindingContext.ValueProvider.GetValue("abilitatoFinanceWMP");
        int page = Convert.ToInt32(bindingContext.ValueProvider.GetValue("page").FirstValue);
        string orderBy = bindingContext.ValueProvider.GetValue("orderBy").FirstValue!;
        bool ascending = Convert.ToBoolean(bindingContext.ValueProvider.GetValue("ascending").FirstValue);

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

        bool? abilitatoFinanceWMPResult = null;
        if (abilitatoFinanceWMP != ValueProviderResult.None && !string.IsNullOrWhiteSpace(abilitatoFinanceWMP.FirstValue))
        {
            if (bool.TryParse(abilitatoFinanceWMP.FirstValue, out bool parsedValue))
            {
                abilitatoFinanceWMPResult = parsedValue;
            } 
        }

        AbilitazioneMifidListInputModel inputModel = new(matricolaResult, intestazione, descrUO, ruolo, flagAbilitatoMifidResult, abilitatoFinanceWMPResult, page, orderBy, ascending, options.PerPage, options.Order);
        bindingContext.Result = ModelBindingResult.Success(inputModel);

        return Task.CompletedTask;
    }
}
