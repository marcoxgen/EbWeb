using EbWeb.Models.InputModels;
using EbWeb.Models.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace EbWeb.Customizations.ModelBinders;

public class SchedaBudgetListInputModelBinder : IModelBinder
{
    private readonly IOptionsMonitor<SchedeBudgetOptions> schedeBudgetOptions;
    public SchedaBudgetListInputModelBinder(IOptionsMonitor<SchedeBudgetOptions> schedeBudgetOptions)
    {
        this.schedeBudgetOptions = schedeBudgetOptions;
    }
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        //Recupero i valori grazie ai value provider
        string etichettaSelezionata = bindingContext.ValueProvider.GetValue("EtichettaSelezionata").FirstValue;
        int page = Convert.ToInt32(bindingContext.ValueProvider.GetValue("Page").FirstValue);
        string OrderBy = bindingContext.ValueProvider.GetValue("OrderBy").FirstValue;
        bool Ascending = Convert.ToBoolean(bindingContext.ValueProvider.GetValue("Ascending").FirstValue);

        //Creo l'istanza del SchedaBudgetListInputModel
        SchedeBudgetOptions options = schedeBudgetOptions.CurrentValue;
        var inputModel = new SchedaBudgetListInputModel(etichettaSelezionata, page, OrderBy, Ascending, options.PerPage, options.Order);

        //Imposto il risultato per notificare che la creazione è avvenuta con successo
        bindingContext.Result = ModelBindingResult.Success(inputModel);

        //Restituisco un task completato
        return Task.CompletedTask;
    }
}