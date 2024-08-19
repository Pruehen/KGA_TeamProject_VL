using EnumTypes;
using UnityEngine;

public class Passive_Offensive2 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive2);
    }
    public override void Active()
    {

    }

    public override void DeActive()
    {

    }
}
