using EnumTypes;

public class Passive_Utility5 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility5);
    }
    public override void Active()//"���Ÿ� �ִ� �ڿ� �������� {0}��ŭ, �ٰŸ� �ִ� �ڿ� �������� {1}��ŭ ����",
    {
        _state.Passive_Utility5_Active((int)_passiveData.VelueList[0], (int)_passiveData.VelueList[1]);
    }

    public override void DeActive()
    {

    }
}
