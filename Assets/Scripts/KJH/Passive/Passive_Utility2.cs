using EnumTypes;

public class Passive_Utility2 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility2);
        ratioValue = _passiveData.VelueList[0] * 0.01f;
    }

    public float ratioValue { get; private set; }
    public override void Active()//"¿Á»≠ »πµÊ∑Æ¿Ã {0}% ¡ı∞°",
    {
        base._state.GoldMulti += ratioValue;
    }

    public override void DeActive()
    {
        base._state.GoldMulti -= ratioValue;
    }
}
