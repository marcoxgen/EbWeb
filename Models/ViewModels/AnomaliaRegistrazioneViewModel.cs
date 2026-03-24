using System.Data;

namespace EbWeb.Models.ViewModels;

public class AnomaliaRegistrazioneViewModel
{
    public int Nag { get; set; }
    public required string? CodiceFiscale { get; set; }
    public string? Intestazione { get; set; }
    public int? IdSocio { get; set; }
    public string? AnomaliaDes { get; set; }

    public static AnomaliaRegistrazioneViewModel FromDataRow(DataRow anomaliaRegistrazioneRow)
    {
        var anomaliaRegistrazioneViewModel = new AnomaliaRegistrazioneViewModel {
            Nag = anomaliaRegistrazioneRow.Field<int>("NAG"),
            CodiceFiscale = anomaliaRegistrazioneRow.Field<string?>("codice_fiscale"),
            Intestazione = anomaliaRegistrazioneRow.Field<string?>("INTESTAZIONE"),
            IdSocio = anomaliaRegistrazioneRow.Field<int?>("Id_Socio"),
            AnomaliaDes = anomaliaRegistrazioneRow.Field<string>("Anomalia_Des")
        };
        return anomaliaRegistrazioneViewModel;
    }
}