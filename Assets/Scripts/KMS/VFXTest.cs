using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Zenject.CheatSheet;
using Zenject.SpaceFighter;

public class VFXTest : MonoBehaviour
{
    public GameObject VFX1;
    public GameObject TargetTransfom1;
    public GameObject VFX2;
    public GameObject TargetTransfom2;
    public GameObject VFX3;
    public GameObject TargetTransfom3;
    public GameObject VFX4;
    public GameObject TargetTransfom4;
    public GameObject VFX5;
    public GameObject TargetTransfom5;

    public void InstantiateVFX1()
    {
        GameObject newVFX = Instantiate(VFX1, TargetTransfom1.transform.position, Quaternion.identity);
        newVFX.GetComponent<ParticleSystem>().Play();
    }
    public void InstantiateVFX2()
    {
        GameObject newVFX = Instantiate(VFX2, TargetTransfom2.transform.position, Quaternion.identity);
        newVFX.GetComponent<ParticleSystem>().Play();
    }
    public void InstantiateVFX3()
    {
        GameObject newVFX = Instantiate(VFX3, TargetTransfom3.transform.position, Quaternion.identity);
        newVFX.GetComponent<ParticleSystem>().Play();
    }
    public void InstantiateVFX4()
    {
        GameObject newVFX = Instantiate(VFX4, TargetTransfom4.transform.position, Quaternion.identity);
        newVFX.GetComponent<ParticleSystem>().Play();
    }
    public void InstantiateVFX5()
    {
        GameObject newVFX = Instantiate(VFX5, TargetTransfom5.transform.position, Quaternion.identity);
        newVFX.GetComponent<ParticleSystem>().Play();
    }

}
