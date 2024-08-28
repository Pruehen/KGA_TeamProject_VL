using UnityEngine;

public class DmgTextManager : SceneSingleton<DmgTextManager>
{
    [SerializeField] GameObject Prefab_DmgText;
    [SerializeField] SO_SKillEvent Hit;
    public void OnDmged(float dmg, Vector3 originPos)
    {
        DmgText dmgText = ObjectPoolManager.Instance.DequeueObject(Prefab_DmgText, this.transform).GetComponent<DmgText>();
        GameObject hitVFX = ObjectPoolManager.Instance.DequeueObject(Hit.preFab);
        hitVFX.transform.position= originPos;
        Debug.Log(originPos);
        dmgText.Init(dmg, originPos);
    }
}
