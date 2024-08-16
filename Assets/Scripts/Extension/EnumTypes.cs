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
        Generic3
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
        Utility5
    }

    public enum EnemyAttackType
    {
        Melee,
        Jump
    }

    public enum PlayerAttackType
    {
        RangeNormalAttack1,
        RangeNormalAttack2,
        RangeNormalAttack3,
        RangeNormalAttack4,

        RangeSkillAttack1,
        RangeSkillAttack2,
        RangeSkillAttack3,
        RangeSkillAttack4,

        MeleeNormalAttack1,
        MeleeChargeAttack2,

        MeleeSkillAttack1,
        MeleeSkillAttack2,
        MeleeSkillAttack3,
        MeleeSkillAttack4
    }
    public enum PlayerAttackKind
    {
        RangeNormalAttack,
        RangeSkillAttack,
        MeleeSkillAttack,
        MeleeNormalAttack,
        MeleeChargedAttack,
    }

    public static class EnumAttackHelper
    {
        public static PlayerAttackKind GetKindOfAttack(PlayerAttackType attackType) => attackType switch
        {
            PlayerAttackType.RangeNormalAttack1 => PlayerAttackKind.RangeNormalAttack,
            PlayerAttackType.RangeNormalAttack2 => PlayerAttackKind.RangeNormalAttack,
            PlayerAttackType.RangeNormalAttack3 => PlayerAttackKind.RangeNormalAttack,
            PlayerAttackType.RangeNormalAttack4 => PlayerAttackKind.RangeNormalAttack,

            PlayerAttackType.RangeSkillAttack1 => PlayerAttackKind.RangeSkillAttack,
            PlayerAttackType.RangeSkillAttack2 => PlayerAttackKind.RangeSkillAttack,
            PlayerAttackType.RangeSkillAttack3 => PlayerAttackKind.RangeSkillAttack,
            PlayerAttackType.RangeSkillAttack4 => PlayerAttackKind.RangeSkillAttack,

            PlayerAttackType.MeleeNormalAttack1 => PlayerAttackKind.MeleeNormalAttack,
            PlayerAttackType.MeleeChargeAttack2 => PlayerAttackKind.MeleeNormalAttack,

            PlayerAttackType.MeleeSkillAttack1 => PlayerAttackKind.MeleeSkillAttack,
            PlayerAttackType.MeleeSkillAttack2 => PlayerAttackKind.MeleeSkillAttack,
            PlayerAttackType.MeleeSkillAttack3 => PlayerAttackKind.MeleeSkillAttack,
            PlayerAttackType.MeleeSkillAttack4 => PlayerAttackKind.MeleeSkillAttack,
            _ => (PlayerAttackKind)999,
        };
    }
}