using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace EbWeb.Models.ViewModels
{
    public class RevisioneViewModel
    {
        public int Id { get; set; }
        public int? Tipo_Colonna { get; set; }
        public long Nag_Affidato { get; set; }
        public required string? Intestazione { get; set; }
        public string? NomeColonna { get; set; }
        public string? Info { get; set; }
        [Column("Note Istruttore")]
        public string? NoteIstruttore { get; set; }
        public required string? Filtro { get; set; }

        public static RevisioneViewModel FromDataRowSearch(DataRow revisioneRow)
        {
            var revisioneViewModel = new RevisioneViewModel
            {
                Nag_Affidato = revisioneRow.Field<long>("Nag_Affidato"),
                Intestazione = revisioneRow.Field<string?>("Intestazione"),
                Filtro = revisioneRow.Field<string?>("Filtro")
            };
            return revisioneViewModel;
        }

        public static RevisioneViewModel FromDataRow(DataRow revisioneRow)
        {
            var revisioneViewModel = new RevisioneViewModel
            {
                Id = revisioneRow.Field<int>("Id"),
                Tipo_Colonna = revisioneRow.Field<int>("Tipo_Colonna"),
                Nag_Affidato = revisioneRow.Field<long>("Nag_Affidato"),
                Intestazione = revisioneRow.Field<string?>("Intestazione"),
                NomeColonna = revisioneRow.Field<string?>("NomeColonna"),
                Info = revisioneRow.Field<string?>("Info"),
                NoteIstruttore = revisioneRow.Field<string?>("Note_Istruttore"),
                Filtro = revisioneRow.Field<string?>("Filtro")
            };
            return revisioneViewModel;
        }
    }
}
