using System.Collections.Generic;

namespace HemdemGames.SplineTool.Behaviours
{
    public static class SplineConnectionManagement
    {
        private static Dictionary<SplineBehaviour, SplineConnectorCollection> connectorDict = new ();

        public static bool TryGetConnectorCollection(SplineBehaviour splineBehaviour, out SplineConnectorCollection connectorCollection)
        {
            try
            {
                connectorCollection = connectorDict[splineBehaviour];
                return true;
            }
            catch (KeyNotFoundException)
            {
                connectorCollection = null;
                return false;
            }
        }
        
        public static void AddSplineConnection(SplineBehaviour splineBehaviour, SplineConnector connector)
        {
            try
            {
                connectorDict[splineBehaviour].AddConnector(connector);
            }
            catch (KeyNotFoundException)
            {
                connectorDict.Add(splineBehaviour, new SplineConnectorCollection());
                AddSplineConnection(splineBehaviour, connector);
            }
        }

        public static void RemoveSplineConnection(SplineBehaviour splineBehaviour, SplineConnector connector)
        {
            try
            {
                connectorDict[splineBehaviour].RemoveConnector(connector);
            }
            catch (KeyNotFoundException) { }
        }
    }
}