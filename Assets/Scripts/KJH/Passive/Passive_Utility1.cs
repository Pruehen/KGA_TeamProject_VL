using EnumTypes;

public class Passive_Utility1 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility1);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
