using EnumTypes;

public class Passive_Defensive5 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive5);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
