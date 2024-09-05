using UnityEngine;

[CreateAssetMenu(fileName = "SO_Boss_Default_MoveModule", menuName = "Enemy/AttackModule/SO_Boss_Default_Move")]
public class SO_Boss_Default_MoveModule : SO_RangeModule
{
    public override void StartAction(EnemyBase owner)
    {
        owner.Move.IsForceMove = true;
        owner.Move.SetRandomForceMoveTarget();
    }

    public override void UpdateAction(EnemyBase owner)
    {
        bool isArrived = owner.NavAgent.remainingDistance <= owner.NavAgent.stoppingDistance;
        if (isArrived)
        {
            owner.Move.IsForceMove = false;
        }
    }
}
