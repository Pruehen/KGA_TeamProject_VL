using EnumTypes;

public class Passive_Utility4 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Utility4);
    }
    public override void Active()//"게임을 시작하면 주변에 블루칩 상자가 나온다.",
    {
        GameManager.Instance.SpawnBluechipChest();
    }

    public override void DeActive()
    {

    }
}
