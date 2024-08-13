using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Check Is Object null")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class BTC_IsTransformNull : Conditional
    {
        public SharedTransform isNull;

        public override TaskStatus OnUpdate()
        {
            if(isNull.Value == null)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}
