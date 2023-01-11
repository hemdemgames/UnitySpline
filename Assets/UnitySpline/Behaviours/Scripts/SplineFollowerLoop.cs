namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineFollowerLoop : SplineFollower
    {
        protected override void OnPathComplete()
        {
            SetDistance(Distance % SplineBehaviour.Length);
            
            base.OnPathComplete();
            
            StartFollowing();
        }
    }
}