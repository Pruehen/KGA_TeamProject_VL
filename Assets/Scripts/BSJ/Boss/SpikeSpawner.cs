using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    [SerializeField] private SpikeTimer _spikeTimer;
    [SerializeField] private Spike _spike;
    [SerializeField] private GameObject _anticipateModel;

    private void OnEnable()
    {
        _anticipateModel.SetActive(true);
        _spike.gameObject.SetActive(false);
        _spikeTimer.OnTimerEnd += EnableSpike;
        _spikeTimer.OnTimerEnd += DisableSelf;
        _spike.OnDead += DisableSelf;
        _spikeTimer.StartTimer();
    }

    private void EnableSpike()
    {
        _spike.gameObject.SetActive(true);
    }
    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
    private void DisableAncitipate()
    {
        _anticipateModel.SetActive(false);
    }
}
