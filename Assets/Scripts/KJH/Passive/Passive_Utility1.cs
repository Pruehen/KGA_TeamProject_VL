using EnumTypes;

public class Passive_Utility1 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility1);
        count = (int)_passiveData.VelueList[0];
    }

    public int count { get; private set; }
    public override void Active()//"���Ĩ �������� ���ΰ�ħ �� �� �ִ�. (���� �� {0}ȸ)",
    {
        count--;
    }

    public override void DeActive()
    {

    }
}
