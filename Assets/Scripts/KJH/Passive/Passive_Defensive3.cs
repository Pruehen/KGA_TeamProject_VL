using EnumTypes;

public class Passive_Defensive3 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive3);
    }

    public bool IsActive = false;
    public override void Active()//"치명적 피해를 입었을 때 {0}초동안 체력이 {1}이하로 떨어지지 않는다. (게임 당 {2}회)",
    {
        
    }

    public override void DeActive()
    {

    }
}
