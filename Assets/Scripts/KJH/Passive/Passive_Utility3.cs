using EnumTypes;

public class Passive_Utility3 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility3);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
