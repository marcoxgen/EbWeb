namespace EbWeb.Models.ViewModels;

public class AbilitazioneMifidViewModel
{
    public int Matricola { get; set; }
    public string? Intestazione { get; set; }
    public string? DescrUO { get; set; }
    public string? CodiceFiscale { get; set; }
    public string? Ruolo { get; set; }
    public string? TitoloStudio { get; set; }
    public string? TitoloStudioMifid { get; set; }
    public int MesiPeriodoSupervisione { get; set; }
    public DateOnly DataConseguimentoTitoloStudio { get; set; }
    public DateOnly DataAbilitazioneMifid { get; set; }
    public DateOnly DataSospensione { get; set; }
    public DateOnly DataTermineDospensione { get; set; }
    public string? NecessarioAssessment { get; set; }
    public DateOnly DataSuperamentoAssessment { get; set; }
    public DateOnly DataAbilitazioneTitoli { get; set; }
    public int AnniEsperienzadeguata { get; set; }
    public DateOnly DataInizioSupervisione { get; set; }
    public DateOnly DataFineSupervisione { get; set; }
    public int MatricolaSupervisore { get; set; }
    public string? IntestazioneSupervisore { get; set; }
    public int MatricolaSostitutoSupervisore { get; set; }
    public string? IntestazioneSostitutoSupervisore { get; set; }
    public string? Formazione2024 { get; set; }
    public string? Formazione2025 { get; set; }
    public string? Note { get; set; }
    public DateOnly DataUltimoAggiornamento { get; set; }
    public string? GeneraLetteraX { get; set; }
    public string? GeneraLetteraY { get; set; }
    public string? GeneraLetteraZ { get; set; }
}