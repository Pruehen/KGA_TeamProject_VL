using EnumTypes;

public class Passive_Utility4 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility4);
    }
    public override void Active()//"������ �����ϸ� �ֺ��� ���Ĩ ���ڰ� ���´�.",
    {
        base._state.SpawnBluechipChest();
    }

    public override void DeActive()
    {

    }
}
