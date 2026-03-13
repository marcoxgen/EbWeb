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
        //Recupero i valori grazie ai value provider
        int id_richiesta = Convert.ToInt32(bindingContext.ValueProvider.GetValue("Id_Richiesta").FirstValue);
        int page = Convert.ToInt32(bindingContext.ValueProvider.GetValue("Page").FirstValue);
        string OrderBy = bindingContext.ValueProvider.GetValue("OrderBy").FirstValue;
        bool Ascending = Convert.ToBoolean(bindingContext.ValueProvider.GetValue("Ascending").FirstValue);

        //Creo l'istanza del AgendaStipulaListInputModel
        AbilitazioniMifidOptions options = abilitazioniMifidOptions.CurrentValue;
        var inputModel = new AbilitazioneMifidListInputModel(id_richiesta, page, OrderBy, Ascending, options.PerPage, options.Order);

        //Imposto il risultato per notificare che la creazione è avvenuta con successo
        bindingContext.Result = ModelBindingResult.Success(inputModel);

        //Restituisco un task completato
        return Task.CompletedTask;
    }
}