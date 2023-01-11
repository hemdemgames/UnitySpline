using System.Collections.Generic;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineConnectorCollection
    {
        private List<SplineConnector> connectors = new List<SplineConnector>();

        public int Count => connectors.Count;

        public SplineConnector GetConnector(int index) => connectors[index];
        
        public void RemoveConnector(SplineConnector connector)
        {
            connectors.Remove(connector);
        }
        
        public void AddConnector(SplineConnector connector)
        {
            connectors.Add(connector);
        }
    }
}