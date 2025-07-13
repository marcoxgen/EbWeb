using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EbWeb.Models.Exceptions.Application
{
    public class AnomaliaNotFoundException : Exception
    {
        public AnomaliaNotFoundException(int anomalieId) : base($"Anomalie {anomalieId} not found")
        {
        }
    }
}