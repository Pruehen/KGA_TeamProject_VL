using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    PlayerAttackSystem Attack;
    PlayerRangeAttack Range;
    PlayerMeleeAttack Melee;
    PlayerMaster _PlayerMaster;

    private void Start()
    {
        TryGetComponent(out _PlayerMaster);
        TryGetComponent(out Attack);
        Range = Attack._rangeAttack;
        Melee = Attack._meleeAttack;
    }

    private void ShootProjectile() => Range.ShootProjectile();
    private void ChargeStartL() => Melee.ChargeStart(true);
    private void ChargeStartR() => Melee.ChargeStart(false);
    private void ChargeEnd() => Melee.ChargeEnd();
    private void EnableDamageBox_Player() => Attack.EnableDamageBox();
    private void StartAbsorb() => Attack.PlayerMod.ActiveAbsorb();
    private void OnUseSkillGauge() => _PlayerMaster._PlayerInstanteState.TryUseSkillGauge2();
}
