using EnumTypes;

public class Passive_Offensive1 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive1);
    }
    public override void Active()//현재 체력이 80% 이상일 때, 일반&특수 공격력이 15%만큼 증가
    {
        base._state.AttackPowerMulti += base._passiveData.VelueList[1];
        base._state.SkillPowerMulti += base._passiveData.VelueList[2];
    }

    public override void DeActive()
    {
        base._state.AttackPowerMulti -= base._passiveData.VelueList[1];
        base._state.SkillPowerMulti -= base._passiveData.VelueList[2];
    }
}
