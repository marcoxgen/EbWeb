namespace EbWeb.Models.Exceptions.Application;

public class AbilitazioneMifidNotFoundException : Exception
{
    public AbilitazioneMifidNotFoundException(int matricola) : base($"Abilitazione Mifid con matricola {matricola} non trovata")
    {
    }    
}