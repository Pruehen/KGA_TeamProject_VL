using UnityEngine;

public class DmgTextManager : SceneSingleton<DmgTextManager>
{
    [SerializeField] GameObject Prefab_DmgText;

    public void OnDmged(int dmg, Vector3 originPos)
    {
        DmgText dmgText = ObjectPoolManager.Instance.DequeueObject(Prefab_DmgText, this.transform).GetComponent<DmgText>();

        dmgText.Init(dmg, originPos);
    }

    public void OnDmged(float dmg, Vector3 originPos)
    {
        OnDmged((int)dmg, originPos);
    }
}
