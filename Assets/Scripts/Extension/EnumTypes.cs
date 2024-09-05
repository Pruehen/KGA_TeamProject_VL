using System;
using UnityEngine.Assertions;

namespace EnumTypes
{
    public enum BlueChipID
    {       
        Melee1,
        Melee2,
        Range1,
        Range2,
        Hybrid1,
        Hybrid2,
        Generic1,
        Generic2,
        Generic3,
        None
    }

    public enum PassiveID
    {        
        Offensive1,
        Offensive2,
        Offensive3,
        Offensive4,
        Offensive5,
        Defensive1,
        Defensive2,
        Defensive3,
        Defensive4,
        Defensive5,
        Utility1,
        Utility2,
        Utility3,
        Utility4,
        Utility5,
        None
    }

    public enum AttackType
    {
        Melee,
        Jump
    }

    //AttackKind로 대체 타수는 따로 카운팅
    //public enum PlayerAttackType
    //{
    //    RangeNormalAttack1,
    //    RangeNormalAttack2,
    //    RangeNormalAttack3,
    //    RangeNormalAttack4,

    //    RangeDashAttack,

    //    RangeSkillAttack1,
    //    RangeSkillAttack2,
    //    RangeSkillAttack3,
    //    RangeSkillAttack4,

    //    MeleeNormalAttack1,
    //    MeleeChargeAttack2,

    //    MeleeDashAttack,

    //    MeleeSkillAttack1,
    //    MeleeSkillAttack2,
    //    MeleeSkillAttack3,
    //    MeleeSkillAttack4,


    //}
    public enum PlayerAttackKind
    {
        RangeNormalAttack,
        MeleeNormalAttack,
        RangeSkillAttack,
        MeleeSkillAttack,
        MeleeChargedAttack,
        RangeDashAttack,
        MeleeDashAttack,
    }

    ////플레이어 공격 타입 대체에 따른 필요성 사라짐
    //public static class EnumAttackHelper
    //{
    //    public static PlayerAttackKind GetKindOfAttack(PlayerAttackKind kind, int combo)
    //    {
    //        if(kind == PlayerAttackKind.RangeNormalAttack)
    //        {
    //            switch (combo % PlayerMaster.Instance.Attack)
    //        }
    //    }
    //}
    public enum PlayerSkill
    {
        RangeSkillAttack1,
        RangeSkillAttack2,
        RangeSkillAttack3,
        RangeSkillAttack4,
        MeleeSkillAttack1,
        MeleeSkillAttack2,
        MeleeSkillAttack3_1,
        MeleeSkillAttack3_2,
        MeleeSkillAttack3_3,
        MeleeSkillAttack4
    }


    public enum AIState
    {
        Idle,
        Patrol,
        Wait,
        Chase,
        JustLostTarget,
        Attack,
        Dead
    }
    public enum EnemyType
    {
        Normal,
        Jump,
        Range
    }

    public enum DamageType
    {
        Normal,
        NonKnockback,
    }
    [Flags]
    public enum AttackRangeType
    {
        Close = 1,
        Far = 2
    }
    [Flags]
    public enum Phase
    {
        First = 1,
        Second = 2,
        Third = 4,
        Fourth = 8,
    }
}