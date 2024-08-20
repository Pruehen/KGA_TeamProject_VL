using EnumTypes;

public class Passive_Defensive5 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive5);
    }
    public override void Active()//"근거리 모드에서 원거리 모드가 될 때마다 스태미나 {0}% 회복",
    {
        _state.StaminaRatioChange(_passiveData.VelueList[0] * 0.01f);        
    }

    public override void DeActive()
    {

    }
}
