using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    [SerializeField] private Timer _spikeTimer;
    [SerializeField] private Spike _spike;
    [SerializeField] private GameObject _anticipateModel;

    private void OnEnable()
    {
        _anticipateModel.SetActive(true);
        _spike.gameObject.SetActive(false);
        _spikeTimer.StartTimer();
        _spikeTimer.OnEnd += EnableSpike;
        _spike.OnDead += DoReset;
    }

    private void Update()
    {
        _spikeTimer.DoUpdate(Time.deltaTime);
    }

    private void EnableSpike()
    {
        _spike.gameObject.SetActive(true);
        _anticipateModel.SetActive(false);
    }
    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
    private void DoReset()
    {
        _spikeTimer.ResetTimer();
        gameObject.SetActive(false);
    }
}
