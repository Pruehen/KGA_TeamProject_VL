using EnumTypes;

public class Passive_Utility2 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility2);
        ratioValue = 1 + (_passiveData.VelueList[0] * 0.01f);
    }

    public float ratioValue { get; private set; }
    public override void Active()//"��ȭ ȹ�淮�� {0}% ����",
    {

    }

    public override void DeActive()
    {

    }
}
