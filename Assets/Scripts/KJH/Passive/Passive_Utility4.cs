using EnumTypes;

public class Passive_Utility4 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility4);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
