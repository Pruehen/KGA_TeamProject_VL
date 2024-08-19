using EnumTypes;

public class Passive_Defensive2 : Passive
{
    public override void SetPassiveData()
    {
        base._passiveData = JsonDataManager.GetPassive(PassiveID.Defensive2);
    }
    public override void Active()//"적을 {0}마리 처치할 때마다 현재 체력을 최대 체력의 {1}%만큼 회복.",
    {
        base._state.ChangeHp(_state.GetMaxHp() * 0.01f * base._passiveData.VelueList[1]);
    }

    public override void DeActive()
    {

    }
}
