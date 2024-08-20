using EnumTypes;

public class Passive_Defensive3 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive3);
        ActiveCount = (int)base._passiveData.VelueList[2];
        ActiveTime = base._passiveData.VelueList[0];
        HpHoldValue = base._passiveData.VelueList[1];
    }

    public int ActiveCount { get; private set; }
    public float ActiveTime { get; private set; }
    public float HpHoldValue { get; private set; }
    public override void Active()//"치명적 피해를 입었을 때 {0}초동안 체력이 {1}이하로 떨어지지 않는다. (게임 당 {2}회)",
    {
    }
    public void Active(out float holdTime)
    {
        ActiveCount--;
        holdTime = ActiveTime;
    }

    public override void DeActive()
    {

    }
}
