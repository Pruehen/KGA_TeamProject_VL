using EnumTypes;

public class Passive_Offensive4 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive4);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
