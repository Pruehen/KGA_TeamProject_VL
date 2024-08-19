using EnumTypes;

public class Passive_Utility1 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility1);
        count = (int)_passiveData.VelueList[0];
    }

    public int count { get; private set; }
    public override void Active()//"블루칩 선택지를 새로고침 할 수 있다. (게임 당 {0}회)",
    {
        count--;
    }

    public override void DeActive()
    {

    }
}
