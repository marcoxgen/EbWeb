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
        
        string search = bindingContext.ValueProvider.GetValue("Search").FirstValue;
        int page = Convert.ToInt32(bindingContext.ValueProvider.GetValue("Page").FirstValue);
        string orderBy = bindingContext.ValueProvider.GetValue("OrderBy").FirstValue!;
        bool ascending = Convert.ToBoolean(bindingContext.ValueProvider.GetValue("Ascending").FirstValue);
        var escluso = bindingContext.ValueProvider.GetValue("Escluso");

        AbilitazioniMifidOptions options = abilitazioniMifidOptions.CurrentValue;

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

        AbilitazioneMifidListInputModel inputModel = new(search, page, orderBy, ascending, options.PerPage, esclusoResult, options.Order);
        bindingContext.Result = ModelBindingResult.Success(inputModel);

        return Task.CompletedTask;
    }
}