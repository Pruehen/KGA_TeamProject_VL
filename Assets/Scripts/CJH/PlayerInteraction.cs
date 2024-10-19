using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public LayerMask interactiveLayerMask;

    public int distance;

    private void Awake()
    {
        InputManager.Instance.PropertyChanged += OnInputPropertyChanged;
    }
    private void OnDestroy()
    {
        InputManager.Instance.PropertyChanged -= OnInputPropertyChanged;
    }
    void OnInputPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(InputManager.Instance.IsInteractiveBtnClick):                
                if (InputManager.Instance.IsInteractiveBtnClick == true)
                {
                    Collider[] colliders = Physics.OverlapSphere(this.transform.position, distance, interactiveLayerMask);
                    if (colliders.Length > 0)
                    {
                        Chest chest = colliders[0].gameObject.GetComponent<Chest>();
                        chest.UseItemGet();
                    }
                }
                break;
        }
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, distance, interactiveLayerMask);
        if (colliders.Length > 0)
        {
                UIManager.Instance.Interactable(true);
        }
        else
        {
            UIManager.Instance.Interactable(false);
        }
    }




}
