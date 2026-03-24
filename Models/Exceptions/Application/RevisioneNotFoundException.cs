namespace EbWeb.Models.Exceptions.Application;

public class RevisioneNotFoundException : Exception
{
    public RevisioneNotFoundException(int id) : base($"Revisione {id} non trovata")
    {
    }
}