using EnumTypes;

public class Passive_Offensive1 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive1);
    }
    public override void Active()
    {
        
    }

    public override void DeActive()
    {
        
    }
}
