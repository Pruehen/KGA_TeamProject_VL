using EnumTypes;

public class Passive_Utility5 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility5);
    }
    public override void Active()//"���Ÿ� �ִ� �ڿ� �������� {0}��ŭ, �ٰŸ� �ִ� �ڿ� �������� {1}��ŭ ����",
    {

    }

    public override void DeActive()
    {

    }
}
