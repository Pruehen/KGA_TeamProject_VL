using EnumTypes;

public class Passive_Defensive4 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive4);
    }

    public float value { get; private set; }
    public override void Active()//"���� óġ �� ������ ���� �������� �Դ� ���ذ� {0}%�� �����Ѵ� (�ִ� {1}%)",
    {
        value += base._passiveData.VelueList[0];
        if(value > base._passiveData.VelueList[1])
        {
            value = base._passiveData.VelueList[1];
        }
    }

    public override void DeActive()
    {
        value = 0;
    }
}
