using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    [SerializeField] private Timer _spikeTimer;
    [SerializeField] private Spike _spike;
    [SerializeField] private GameObject _anticipateModel;
    [SerializeField] private GameObject _model;
    [SerializeField] private GameObject _spawnEffect;

    [SerializeField] private Timer _spikeModelTimer;
    [SerializeField] private Timer _enqueueTimer;

    ParticleSystem _anticipateParticle;
    ParticleSystem _modelParticle;
    ParticleSystem _spawnParticle;
    private void Awake()
    {
        _anticipateParticle = _anticipateModel.GetComponent<ParticleSystem>();
        _modelParticle = _model.GetComponent<ParticleSystem>();
        _spawnParticle = _spawnEffect.GetComponent<ParticleSystem>();

        _spikeModelTimer.Init(.3f, PauseModel);
        _enqueueTimer.Init(3f, Enqueue);
    }
    private void OnEnable()
    {
        _anticipateModel.SetActive(true);
        _anticipateParticle.Play();

        _spike.gameObject.SetActive(false);
        _spikeTimer.StartTimer();
        _spikeTimer.OnEnd += EnableSpike;
        _spike.OnDead += DoReset;
    }

    private void Update()
    {
        _spikeTimer.DoUpdate(Time.deltaTime);
        _spikeModelTimer.DoUpdate(Time.deltaTime);
        _enqueueTimer.DoUpdate(Time.deltaTime);
    }

    private void EnableSpike()
    {
        _spike.EnableSpike();
        _modelParticle.Play();
        _spikeModelTimer.StartTimer();
        _spike.EnablePhysics();
        _spawnParticle.Play();

        _anticipateModel.SetActive(false);
    }
    private void DoReset()
    {
        _modelParticle.Play();
        _spike.DisablePhysics();
        _spikeTimer.ResetTimer();
    }
    private void PauseModel()
    {
        _spike.EnableDestroy();
        _modelParticle.Pause();
    }
    private void Enqueue()
    {
        _spawnParticle.Clear();
        ObjectPoolManager.Instance.EnqueueObject(gameObject);
    }
}
