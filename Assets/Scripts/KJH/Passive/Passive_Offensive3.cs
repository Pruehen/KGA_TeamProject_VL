using EnumTypes;

public class Passive_Offensive3 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive3);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
