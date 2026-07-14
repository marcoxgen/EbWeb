using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using EbWeb.Models.AbilitazioniIvass.InputModels;
using EbWeb.Models.AbilitazioniIvass.Options;

namespace EbWeb.Customizations.ModelBinders;

public class AbilitazioneIvassListInputModelBinder : IModelBinder
{
    private readonly IOptionsMonitor<AbilitazioniIvassOptions> _abilitazioniIvassOptions;
    
    public AbilitazioneIvassListInputModelBinder(IOptionsMonitor<AbilitazioniIvassOptions> abilitazioniIvassOptions)
    {
        _abilitazioniIvassOptions = abilitazioniIvassOptions;
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
        var flagFormatoMifid = bindingContext.ValueProvider.GetValue("flagFormatoMifid");
        var flagAbilitatoFinanza = bindingContext.ValueProvider.GetValue("flagAbilitatoFinanza");
        var abilitatoOperativitaIvass = bindingContext.ValueProvider.GetValue("abilitatoOperativitaIvass");
        int page = Convert.ToInt32(bindingContext.ValueProvider.GetValue("page").FirstValue);
        string orderBy = bindingContext.ValueProvider.GetValue("orderBy").FirstValue!;
        bool ascending = Convert.ToBoolean(bindingContext.ValueProvider.GetValue("ascending").FirstValue);

        AbilitazioniIvassOptions options = _abilitazioniIvassOptions.Get("AbilitazioniIvass");

        int? matricolaResult = int.TryParse(matricola, out var matricolaParsed) ? matricolaParsed : null;

        bool? flagFormatoMifidResult = null;
        if (flagFormatoMifid != ValueProviderResult.None && !string.IsNullOrWhiteSpace(flagFormatoMifid.FirstValue))
        {
            if (bool.TryParse(flagFormatoMifid.FirstValue, out bool parsedValue))
            {
                flagFormatoMifidResult = parsedValue;
            }
        }

        bool? flagAbilitatoFinanzaResult = null;
        if (flagAbilitatoFinanza != ValueProviderResult.None && !string.IsNullOrWhiteSpace(flagAbilitatoFinanza.FirstValue))
        {
            if (bool.TryParse(flagAbilitatoFinanza.FirstValue, out bool parsedValue))
            {
                flagAbilitatoFinanzaResult = parsedValue;
            }
        }

        bool? abilitatoOperativitaIvassResult = null;
        if (abilitatoOperativitaIvass != ValueProviderResult.None && !string.IsNullOrWhiteSpace(abilitatoOperativitaIvass.FirstValue))
        {
            if (bool.TryParse(abilitatoOperativitaIvass.FirstValue, out bool parsedValue))
            {
                abilitatoOperativitaIvassResult = parsedValue;
            }
        }

        AbilitazioneIvassListInputModel inputModel = new(matricolaResult, intestazione, descrUO, ruolo, flagFormatoMifidResult, flagAbilitatoFinanzaResult, abilitatoOperativitaIvassResult, page, orderBy, ascending, options.PerPage, options.Order);
        bindingContext.Result = ModelBindingResult.Success(inputModel);

        return Task.CompletedTask;
    }
}
