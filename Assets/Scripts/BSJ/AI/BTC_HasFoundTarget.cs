using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Check Is Object null")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class BTC_HasFoundTarget : Conditional
    {
        private Detector _detector;
        public SharedTransform isNull;


        public override void OnStart()
        {
            if (isNull.Value != null)
                return;
            _detector = GetComponent<Detector>();
            if(_detector == null)
            {
                _detector = transform.GetComponentInChildren<Detector>();
            }
        }
        public override TaskStatus OnUpdate()
        {
            if(isNull.Value != null)
                return TaskStatus.Success;

            if(_detector.GetLatestTarget() != null && _detector.IsTargetVisible())
            {
                isNull.Value = _detector.GetLatestTarget();
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
    }
}
