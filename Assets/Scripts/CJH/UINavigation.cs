using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINavigation : MonoBehaviour
{
    [SerializeField] Button holdStartButton;
    [SerializeField] Button pickStartButton;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(holdStartButton.gameObject);
    }

    void Update()
    {
        
    }

    public void OnPress()
    {
        EventSystem.current.SetSelectedGameObject(pickStartButton.gameObject);

    }


}
