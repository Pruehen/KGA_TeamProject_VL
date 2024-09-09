using EnumTypes;

public class Passive_Offensive3 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive3);
    }
    public override void Active()//"일반&특수 공격력이 {0, 1}% 증가, 대쉬 시간 {2}% 감소 및 스테미나 소모량 {3}% 증가",
    {
        base._state.AttackPowerMulti += base._passiveData.VelueList[0] * 0.01f;
        base._state.SkillPowerMulti += base._passiveData.VelueList[1] * 0.01f;
        base._state.DashTimeMulti -= base._passiveData.VelueList[2] * 0.01f;
        base._state.DashCostMulti += base._passiveData.VelueList[3] * 0.01f;
    }

    public override void DeActive()
    {

    }
}
