using EnumTypes;

public class Passive_Defensive1 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive1);
    }
    public override void Active()//"최대 체력이 {0}% 증가하는 대신 일반&특수 공격력 {1}% 감소",
    {
        base._state.MaxHpMulti += base._passiveData.VelueList[0] * 0.01f;
        base._state.AttackPowerMulti -= base._passiveData.VelueList[1] * 0.01f;
        base._state.SkillPowerMulti -= base._passiveData.VelueList[2] * 0.01f;
    }

    public override void DeActive()
    {

    }
}
