using EnumTypes;

public class Passive_Defensive2 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive2);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
