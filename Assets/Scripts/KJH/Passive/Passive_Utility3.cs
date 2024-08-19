using EnumTypes;

public class Passive_Utility3 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility3);
        BlueChipCount = (int)_passiveData.VelueList[0];
        AddLevel = (int)_passiveData.VelueList[1];
    }

    public int BlueChipCount { get; private set; }
    public int AddLevel { get; private set; }

    public override void Active()//"블루칩 선택지가 {0}개로 감소하지만 {1} 더 높은 레벨의 블루칩이 등장한다.",
    {

    }

    public override void DeActive()
    {

    }
}
