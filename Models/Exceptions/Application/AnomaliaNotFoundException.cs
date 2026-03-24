namespace EbWeb.Models.Exceptions.Application;

public class AnomaliaNotFoundException : Exception
{
    public AnomaliaNotFoundException(int anomalieId) : base($"Anomalie {anomalieId} not found")
    {
    }
}