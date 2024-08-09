using TMPro;
using UI.Extension;
using UnityEngine;

public class DmgText : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] TextMeshProUGUI text;
    float activeTime = 0;
    public void Init(int dmg, Vector3 originPos)
    {
        rect.SetUIPos_WorldToScreenPos(originPos);

        activeTime = 0;
        text.text = dmg.ToString();
    }

    private void Update()
    {
        rect.anchoredPosition += new Vector2(0, 100) * Time.deltaTime;
        activeTime += Time.deltaTime;

        if(activeTime > 1)
        {
            ObjectPoolManager.Instance.EnqueueObject(this.gameObject);
        }
    }
}
