using EnumTypes;

public class Passive_Utility2 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility2);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
