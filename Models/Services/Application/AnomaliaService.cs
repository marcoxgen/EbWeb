using EbWeb.Models.ViewModels;

namespace EbWeb.Models.Services.Application
{
    public class AnomaliaService : IAnomaliaService
    {
        public AnagraficaViewModel GetAnagrafica(int id)
        {
            var anagrafica = new AnagraficaViewModel
            {
                Nag = id,
                Intestazione = $"Intestazione {id}"
            };

            return anagrafica;
        }
    }
}