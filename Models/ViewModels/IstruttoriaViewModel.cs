using EbWeb.Models.Entities;

namespace EbWeb.Models.ViewModels
{
    public class IstruttoriaViewModel
    {
        public long? NumeroPratica { get; set; }
        public int? Nag { get; set; }
        public string? Intestazione { get; set; }
        public string? TipoControparteCod { get; set; }
        public string? Gestore { get; set; }
        public string? OrganoDeliberanteCod { get; set; }
        public DateOnly? DataInserimentoPratica { get; set; }
        public DateOnly? DataUltimoTrasferimento { get; set; }
        public string? UltimoCodGoBack { get; set; }
        public DateOnly? DataUltimoGoBack { get; set; }
        public DateOnly? EliminaCode { get; set; }
        public string? TipoIstruttoriaCod { get; set; }
        public string? StatoPraticaCod { get; set; }
        public string? GradoRischioCod { get; set; }
        public string? ProfiloAntiriciclaggio { get; set; }
        public int? Rating { get; set; }
        public string? ClusterPratica { get; set; }
        public string? Istruttore { get; set; }
        public string? Note { get; set; }
        public string? NoteEscalationIndicatoriBilancio { get; set; }
        
        public static IstruttoriaViewModel FromEntity(Istruttoria istruttoria)
        {
            return new IstruttoriaViewModel
            {
                NumeroPratica = istruttoria.Numero_Pratica,
                Nag = istruttoria.Nag,
                Intestazione = istruttoria.Intestazione,
                TipoControparteCod = istruttoria.Tipo_Controparte_Cod,
                Gestore = istruttoria.Gestore,
                OrganoDeliberanteCod = istruttoria.Organo_Deliberante_Cod,
                DataInserimentoPratica = istruttoria.Data_Inserimento_Pratica,
                DataUltimoTrasferimento = istruttoria.Data_Ultimo_Trasferimento,
                UltimoCodGoBack = istruttoria.Ultimo_Cod_GoBack,
                DataUltimoGoBack = istruttoria.Data_Ultimo_GoBack,
                EliminaCode = istruttoria.Elimina_Code,
                TipoIstruttoriaCod = istruttoria.Tipo_Istruttoria_Cod,
                StatoPraticaCod = istruttoria.Stato_Pratica_Cod,
                GradoRischioCod = istruttoria.Grado_Rischio_Cod,
                ProfiloAntiriciclaggio = istruttoria.Profilo_Antiriciclaggio,
                Rating = istruttoria.Rating,
                ClusterPratica = istruttoria.Cluster_Pratica,
                Istruttore = istruttoria.Istruttore,
                Note = istruttoria.Note,
                NoteEscalationIndicatoriBilancio = istruttoria.Note_escalation_indicatori_bilancio
            };
        }
    }
}
