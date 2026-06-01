using EbWeb.Models.AlimentazioneBudget.Entities;
using System.Text.Json;

namespace EbWeb.Models.AlimentazioneBudget.ViewModels;

public class AzionePubblicazioneViewModel
{
    public int IdAzione { get; set; }
    public int IdPubblicazione { get; set; }
    public char TipoPub { get; set; }
    public DateOnly DataRif { get; set; }
    public int IdTask { get; set; }
    public string Descrizione { get; set; }
    public short Ordine { get; set; }
    public byte Livello { get; set; }
    public bool? Abilitato { get; set; }
    public string? NomeDatabase { get; set; }
    public string? Comando { get; set; }
    public string? Istruzioni { get; set; }
    public bool Esito { get; set; }
    public DateTime? DataEsecuzione { get; set; }
    public string? Messaggio { get; set; }
    public string? Note { get; set; }
    public bool FlagEsecuzione { get; set; }
    public string? Risultati { get; set; }
    public JsonTabellaResult? TabellaRisultati { get; set; }
    public string ClasseColore => Esito ? "text-success" : "text-danger";
    public string IconaStato => Esito ? "fa-check-circle" : "fa-times-circle";
    public int PaddingLivello => Livello * 30;
    public string ClasseGrassetto => Livello == 0 ? "fw-bold" : string.Empty;
    public string DataEsecuzioneFriendly
    {
        get
        {
            if (!DataEsecuzione.HasValue) return string.Empty;
            
            var diff = DateTime.Now - DataEsecuzione.Value;

            if (diff.TotalMinutes < 1)
                return "0 m";

            if (diff.TotalMinutes < 60)
                return $"{(int)diff.TotalMinutes} m";
            
            if (diff.TotalHours < 24)
                return $"{(int)diff.TotalHours} h";
            
            if (diff.TotalDays < 7)
                return $"{(int)diff.TotalDays} g";
            
            return DataEsecuzione.Value.ToString("dd/MM/yyyy");
        }
    }

    public static AzionePubblicazioneViewModel FromEntity(AzionePubblicazione azione)
    {
        var azionePubblicazione = new AzionePubblicazioneViewModel
        {
            IdAzione = azione.Id_Azione,
            IdPubblicazione = azione.Id_Pubblicazione,
            TipoPub = azione.TipoPub,
            DataRif = azione.DataRif,
            IdTask = azione.Id_Task,
            Descrizione = azione.Descrizione,
            Ordine = azione.Ordine,
            Livello = azione.Livello,
            Abilitato = azione.Abilitato,
            NomeDatabase = azione.Nome_Database,
            Comando = azione.Comando,
            Istruzioni = azione.Istruzioni,
            Esito = azione.Esito,
            DataEsecuzione = azione.Data_Esecuzione,
            Messaggio = azione.Messaggio,
            Note = azione.Note,
            Risultati = azione.Risultati,
            FlagEsecuzione = azione.Flag_Esecuzione
        };

        if (!string.IsNullOrWhiteSpace(azionePubblicazione.Risultati))
        {
            try
            {
                // Deserializziamo il JSON in una lista di dizionari dinamici
                var righe = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(azionePubblicazione.Risultati);

                if (righe != null && righe.Any())
                {
                    azionePubblicazione.TabellaRisultati = new JsonTabellaResult
                    {
                        Colonne = righe.First().Keys.ToList(),
                        Righe = righe
                    };
                }
            }
            catch (JsonException)
            {
                // Il JSON non è valido o non è un array (es. è un messaggio d'errore o stringa vuota)
                // Puoi gestire l'errore o lasciare TabellaRisultati a null
            }
        }

        return azionePubblicazione;
    }
}

public class JsonTabellaResult
{
    public List<string> Colonne { get; set; } = new();
    public List<Dictionary<string, object>> Righe { get; set; } = new();
}