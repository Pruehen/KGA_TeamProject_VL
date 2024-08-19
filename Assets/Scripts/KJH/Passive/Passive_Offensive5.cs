using EnumTypes;

public class Passive_Offensive5 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive5);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
