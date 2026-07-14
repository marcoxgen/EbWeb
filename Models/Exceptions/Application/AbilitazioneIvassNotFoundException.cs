namespace EbWeb.Models.Exceptions.Application;

public class AbilitazioneIvassNotFoundException : Exception
{
    public AbilitazioneIvassNotFoundException(int matricola) : base($"Abilitazione Ivass con matricola {matricola} non trovata")
    {
    }    
}