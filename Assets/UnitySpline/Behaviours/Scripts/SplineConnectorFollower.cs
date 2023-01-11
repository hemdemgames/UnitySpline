using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineConnectorFollower : MonoBehaviour
    {
        [SerializeField] private SplineFollower splineFollower;
        
        private bool isMoveOnConnectorSpline => currentConnector != null;
        private SplineConnector currentConnector;
        
        private void OnEnable()
        {
            splineFollower.OnPathCompleted += OnPathComplete;
        }

        private void OnPathComplete()
        {
            float distance = splineFollower.Distance;
            distance %= splineFollower.SplineBehaviour.Length;
            
            if (!isMoveOnConnectorSpline)
            {
                var hasConnectors = SplineConnectionManagement.TryGetConnectorCollection(splineFollower.SplineBehaviour, out var connectors);

                if (hasConnectors && connectors.Count > 0)
                {
                    currentConnector = connectors.GetConnector(Random.Range(0, connectors.Count));
                    splineFollower.SetDistance(distance);
                    splineFollower.SetSpline(currentConnector);
                }
            }
            else
            {
                distance = currentConnector.TargetSpline.Length * currentConnector.TargetSplinePercent;
                splineFollower.SetSpline(currentConnector.TargetSpline);
                splineFollower.SetDistance(distance);
                currentConnector = null;
            }
            
            splineFollower.StartFollowing();
        }

        private void OnDisable()
        {
            splineFollower.OnPathCompleted -= OnPathComplete;   
        }

        private void OnValidate()
        {
            splineFollower ??= GetComponent<SplineFollower>();
        }
    }
}