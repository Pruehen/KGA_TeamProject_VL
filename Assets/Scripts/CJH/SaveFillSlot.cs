using UnityEngine;
using UnityEngine.UI;

public class SaveFillSlot : MonoBehaviour
{
    [SerializeField] Text Text_CreateTime;
    [SerializeField] Text Text_TryCount;
    [SerializeField] Text Text_ClearCount;
    [SerializeField] Text Text_PlayTime;
    [SerializeField] Text Text_Emerald;
    
    public void SetData(int index)
    {        
        if (JsonDataManager.TryGetUserData(index, out UserData data))
        {
            Text_TryCount.gameObject.SetActive(true);
            Text_ClearCount.gameObject.SetActive(true);
            Text_PlayTime.gameObject.SetActive(true);
            Text_Emerald.gameObject.SetActive(true);

            Text_CreateTime.text = data.SaveTime.ToString();
            Text_TryCount.text = data.Count_Try.ToString();
            Text_ClearCount.text = data.Count_Clear.ToString();
            Text_PlayTime.text = data.PlayTime.ToString();
            Text_Emerald.text = data.Gold.ToString();
        }
        else
        {
            Text_CreateTime.text = "EMPTY";
            Text_TryCount.gameObject.SetActive(false);
            Text_ClearCount.gameObject.SetActive(false);
            Text_PlayTime.gameObject.SetActive(false);
            Text_Emerald.gameObject.SetActive(false);
        }
    }
}
