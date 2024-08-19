using EnumTypes;

public class Passive_Defensive3 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive3);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
