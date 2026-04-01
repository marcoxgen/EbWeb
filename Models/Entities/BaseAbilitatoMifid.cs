using DocumentFormat.OpenXml.Spreadsheet;

namespace EbWeb.Models.Entities;

public partial class BaseAbilitatoMifid
{
    public BaseAbilitatoMifid(int matricola)
    {
        Matricola = matricola;
        Titolo_di_studio_Mifid_Cod = 0;
        Necessario_assessment = false;
        Abilitato_Finance_WMP = false;
        Escluso = false;
    }

    public int Matricola { get; set; }
    public string? Titolo_di_studio { get; set; }
    public byte? Titolo_di_studio_Mifid_Cod { get; set; }
    public DateOnly? Data_conseguimento_titolo_di_studio { get; set; }
    public DateOnly? Data_abilitazione_Mifid { get; set; }
    public DateOnly? Data_sospensione { get; set; }
    public DateOnly? Data_termine_sospensione { get; set; }
    public bool? Necessario_assessment { get; set; }
    public DateOnly? Data_superamento_assessment { get; set; }
    public DateOnly? Data_abilitazione_titoli { get; set; }
    public DateOnly? Data_inizio_supervisione { get; set; }
    public DateOnly? Data_fine_supervisione { get; set; }
    public int? Matricola_supervisore { get; set; }
    public int? Matricola_sostituto_supervisore { get; set; }
    public string? Formazione_2024 { get; set; }
    public string? Formazione_2025 { get; set; }
    public string? Note { get; set; }
    public bool? Abilitato_Finance_WMP { get; set; }
    public bool? Escluso { get; set; }
    public DateOnly? Data_Ultimo_Aggiornamento { get; set; }

    public void ChangeTitoloDiStudio(string titoloStudio)
    {
        Titolo_di_studio = titoloStudio;
    }

    public void ChangeTitoloDiStudioMifidCod(byte? titoloStudioMifidCod)
    {
        Titolo_di_studio_Mifid_Cod = titoloStudioMifidCod;
    }

    public void ChangeDataConseguimentoTitoloStudio(DateOnly? dataConseguimentoTitoloStudio)
    {
        Data_conseguimento_titolo_di_studio = dataConseguimentoTitoloStudio;
    }

    public void ChangeDatAbilitazioneMifid(DateOnly? dataAbilitazioneMifid)
    {
        Data_abilitazione_Mifid = dataAbilitazioneMifid;
    }

    public void ChangeDataSospensione(DateOnly? dataSospensione)
    {
        Data_sospensione = dataSospensione;
    }

    public void ChangeDataTermineSospensione(DateOnly? dataTermineSospensione)
    {
        Data_termine_sospensione = dataTermineSospensione;
    }

    public void ChangeNecessarioAssessment(bool? necessarioAssessment)
    {
        Necessario_assessment = necessarioAssessment;
    }

    public void ChangeDataSuperamentoAssessment(DateOnly? dataSuperamentoAssessment)
    {
        Data_superamento_assessment = (Necessario_assessment == true) ? dataSuperamentoAssessment : null;
    }

    public void ChangeDataAbilitazioneTitoli(DateOnly? dataAbilitazioneTitoli)
    {
        Data_abilitazione_titoli = dataAbilitazioneTitoli;
    }
    public void ChangeDataInizioSupervisione(DateOnly? dataInizioSupervisione)
    {
        Data_inizio_supervisione = dataInizioSupervisione;
    }

    public void ChangeDataFineSupervisione(DateOnly? dataFineSupervisione)
    {
        Data_fine_supervisione = dataFineSupervisione;
    }

    public void ChangeMatricolaSupervisore(int? matricolaSupervisore)
    {
        Matricola_supervisore = matricolaSupervisore;
    }

    public void ChangeMatricolaSostitutoSupervisore(int? matricolaSostitutoSupervisore)
    {
        Matricola_sostituto_supervisore = matricolaSostitutoSupervisore;
    }

    public void ChangeFormazione2024(string? formazione2024)
    {
        Formazione_2024 = formazione2024;
    }


    public void ChangeFormazione2025(string? formazione2025)
    {
        Formazione_2025 = formazione2025;
    }

    public void ChangeNote(string? note)
    {
        Note = note;
    }

    public void ChangeAbilitatoFinanceWMP(bool? abilitatoFinanceWMP)
    {
        Abilitato_Finance_WMP = abilitatoFinanceWMP;
    }

    public void ChangeEscluso(bool? escluso)
    {
        Escluso = escluso;
    }

    public void ChangeDataUltimoAggiornamento()
    {
        Data_Ultimo_Aggiornamento = DateOnly.FromDateTime(DateTime.Now);
    }
}