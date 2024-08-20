using EnumTypes;

public class Passive_Offensive5 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Offensive5);
        ValueChangeRatio = _passiveData.VelueList[0] * 0.01f;
    }
    public float ValueChangeRatio { get; private set; }
    public override void Active()//"특수 공격력의 {0}%만큼을 일반 공격력으로 치환",
    {

    }

    public override void DeActive()
    {

    }
}
