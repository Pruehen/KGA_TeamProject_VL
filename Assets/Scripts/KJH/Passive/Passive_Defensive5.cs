using EnumTypes;

public class Passive_Defensive5 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive5);
    }
    public override void Active()//"�ٰŸ� ��忡�� ���Ÿ� ��尡 �� ������ ���¹̳� {0}% ȸ��",
    {
        _state.StaminaRatioChange(_passiveData.VelueList[0] * 0.01f);        
    }

    public override void DeActive()
    {

    }
}
