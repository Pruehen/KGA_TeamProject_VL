using UnityEngine;

public class DmgTextManager : SceneSingleton<DmgTextManager>
{
    [SerializeField] GameObject Prefab_DmgText;

    public void OnDmged(float dmg, Vector3 originPos)
    {
        DmgText dmgText = ObjectPoolManager.Instance.DequeueObject(Prefab_DmgText, this.transform).GetComponentInChildren<DmgText>();

        dmgText.Init(dmg, originPos);
    }
}
