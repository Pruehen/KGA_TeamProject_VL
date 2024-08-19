using EnumTypes;

public class Passive_Defensive1 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive1);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
