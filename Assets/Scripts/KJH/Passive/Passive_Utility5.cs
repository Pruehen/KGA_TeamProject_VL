using EnumTypes;

public class Passive_Utility5 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility5);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
