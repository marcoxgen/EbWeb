using EbWeb.Models.InputModels;
using EbWeb.Models.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace EbWeb.Customizations.ModelBinders;

public class IstruttoriaListInputModelBinder : IModelBinder
{
    private readonly IOptionsMonitor<IstruttorieOptions> istruttorieOptions;
    public IstruttoriaListInputModelBinder(IOptionsMonitor<IstruttorieOptions> istruttorieOptions)
    {
        this.istruttorieOptions = istruttorieOptions;
    }
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        //Recupero i valori grazie ai value provider
        int nag = Convert.ToInt32(bindingContext.ValueProvider.GetValue("Nag").FirstValue);
        string cluster_pratica = bindingContext.ValueProvider.GetValue("Cluster_Pratica").FirstValue;
        bool istruttore = Convert.ToBoolean(bindingContext.ValueProvider.GetValue("Istruttore").FirstValue);
        int page = Convert.ToInt32(bindingContext.ValueProvider.GetValue("Page").FirstValue);
        string OrderBy = bindingContext.ValueProvider.GetValue("OrderBy").FirstValue;
        bool Ascending = Convert.ToBoolean(bindingContext.ValueProvider.GetValue("Ascending").FirstValue);

        //Creo l'istanza del IstruttoriaListInputModel
            IstruttorieOptions options = istruttorieOptions.CurrentValue;
            var inputModel = new IstruttoriaListInputModel(nag, cluster_pratica, istruttore, page, OrderBy, Ascending, options.PerPage, options.Order);

        //Imposto il risultato per notificare che la creazione è avvenuta con successo
        bindingContext.Result = ModelBindingResult.Success(inputModel);

        //Restituisco un task completato
        return Task.CompletedTask;
    }
}