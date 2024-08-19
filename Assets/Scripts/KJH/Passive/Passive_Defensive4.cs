using EnumTypes;

public class Passive_Defensive4 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive4);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
