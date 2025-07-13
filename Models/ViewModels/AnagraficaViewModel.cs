using System.Data;

namespace EbWeb.Models.ViewModels
{
    public class AnagraficaViewModel
    {
        public int Nag { get; set; }
        public required string? Intestazione { get; set; }

        public static AnagraficaViewModel FromDataRow(DataRow anagrafeRow)
        {
            var anagrafeViewModel = new AnagraficaViewModel
            {
                Nag = anagrafeRow.Field<int>("NAG"),
                Intestazione = anagrafeRow.Field<string?>("Intestazione")
            };
            return anagrafeViewModel;
        }
    }
}