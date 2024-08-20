using EnumTypes;

public class Passive_Offensive4 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive4);
    }
    public override void Active()//"특수 공격력 {0}% 증가, 스킬의 공격 범위 {1}% 증가, 스킬 게이지 획득량 {2}% 감소",
    {
        base._state.SkillPowerMulti += base._passiveData.VelueList[0] * 0.01f;
        _state.attackRangeMulti += base._passiveData.VelueList[1] * 0.01f;
        _state.SkillGaugeRecoveryMulti -= base._passiveData.VelueList[2] * 0.01f;
    }

    public override void DeActive()
    {

    }
}
