using EnumTypes;

public class Passive_Utility5 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility5);
    }
    public override void Active()//"원거리 최대 자원 보유량이 {0}만큼, 근거리 최대 자원 보유량이 {1}만큼 증가",
    {

    }

    public override void DeActive()
    {

    }
}
