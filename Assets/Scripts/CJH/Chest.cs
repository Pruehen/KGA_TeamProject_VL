using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public void UseItemGet()
    {
        Debug.Log("상자깡");
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Chest");
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
    }
}