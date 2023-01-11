using System;
using UnityEngine;

namespace HemdemGames.SplineTool.Behaviours
{
    public class SplineFollower : MonoBehaviour
    {
        [SerializeField] private bool isFollow = true;
        [SerializeField,Min(0)] private float followSpeed = 1;
        [SerializeField] private SplineBehaviour splineBehaviour;

        public float Percent => Distance / splineBehaviour.Length;
        public float Distance { get; private set; }
        public SplineBehaviour SplineBehaviour => splineBehaviour;

        public float FollowSpeed
        {
            get => followSpeed;
            set => followSpeed = Mathf.Max(followSpeed, 0);
        }

        public event Action OnPathCompleted;
        
        private void Update()
        {
            if (!isFollow) return;
            
            Distance += followSpeed * Time.deltaTime;

            if (Distance >= splineBehaviour.Length)
            {
                OnPathComplete();
                OnPathCompleted?.Invoke();
            }

            UpdateTransform();
        }

        private void UpdateTransform()
        {
            OnTransformPreupdate();
            var sample = splineBehaviour.EvaluateByDistance(Distance);
            transform.position = sample.Position;
            transform.rotation = sample.Rotation;
            OnTransformUpdate();
        }

        public void SetSpline(SplineBehaviour splineBehaviour)
        {
            this.Distance = Mathf.Clamp(Distance, 0, splineBehaviour.Length);
            this.splineBehaviour = splineBehaviour;
        }
        
        public void SetDistance(float distance)
        {
            Distance = Mathf.Clamp(distance, 0, splineBehaviour.Length);
        }

        protected virtual void OnTransformPreupdate()
        {
            
        }

        protected virtual void OnTransformUpdate()
        {
            
        }
        
        protected virtual void OnPathComplete()
        {
            StopFollowing();
        }

        public void StartFollowing() => isFollow = true;
        public void StopFollowing() => isFollow = false;
    }
}