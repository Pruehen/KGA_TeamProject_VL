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

    public override void Active()//"���Ĩ �������� {0}���� ���������� {1} �� ���� ������ ���Ĩ�� �����Ѵ�.",
    {

    }

    public override void DeActive()
    {

    }
}
