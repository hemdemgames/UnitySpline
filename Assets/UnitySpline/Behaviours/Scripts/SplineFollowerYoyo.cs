using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineFollowerYoyo : SplineFollower
    {
        private bool isReverseFollow;

        private float distanceCache;
        
        protected override void OnTransformPreupdate()
        {
            distanceCache = Distance;

            if (!isReverseFollow) return;
            
            SetDistance(SplineBehaviour.Length - distanceCache);
        }

        protected override void OnTransformUpdate()
        {
            if (isReverseFollow) transform.rotation *= Quaternion.Euler(0, 180, 0);
            SetDistance(distanceCache);
        }

        protected override void OnPathComplete()
        {
            isReverseFollow = !isReverseFollow;
            
            SetDistance(Distance % SplineBehaviour.Length);
            
            base.OnPathComplete();
            
            StartFollowing();
        }
    }
}