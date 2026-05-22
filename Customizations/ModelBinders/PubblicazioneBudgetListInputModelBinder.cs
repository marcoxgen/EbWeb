using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using EbWeb.Models.AlimentazioneBudget.InputModels;
using EbWeb.Models.AlimentazioneBudget.Options;

namespace EbWeb.Customizations.ModelBinders;

public class PubblicazioneBudgetListInputModelBinder : IModelBinder
{
    private readonly IOptionsMonitor<AlimentazioneBudgetOptions> alimentazioneBudgetOptions;
    public PubblicazioneBudgetListInputModelBinder(IOptionsMonitor<AlimentazioneBudgetOptions> alimentazioneBudgetOptions)
   {
        this.alimentazioneBudgetOptions = alimentazioneBudgetOptions;
    }
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        //Recupero i valori grazie ai value provider
        int id_pubblicazione = Convert.ToInt32(bindingContext.ValueProvider.GetValue("Id_Pubblicazione").FirstValue ?? "0");
        int page = Convert.ToInt32(bindingContext.ValueProvider.GetValue("Page").FirstValue);
        string orderBy = bindingContext.ValueProvider.GetValue("OrderBy").FirstValue!;
        bool ascending = Convert.ToBoolean(bindingContext.ValueProvider.GetValue("Ascending").FirstValue);

        AlimentazioneBudgetOptions options = alimentazioneBudgetOptions.CurrentValue;
        var inputModel = new PubblicazioneBudgetListInputModel(id_pubblicazione, page, orderBy, ascending, options.PerPage, options.Order);

        //Imposto il risultato per notificare che la creazione è avvenuta con successo
        bindingContext.Result = ModelBindingResult.Success(inputModel);

        //Restituisco un task completato
        return Task.CompletedTask;
    }
}