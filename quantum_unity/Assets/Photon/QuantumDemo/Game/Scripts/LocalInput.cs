﻿using System;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using ServiceLocator;
using Assets.Scripts.Data;

public class LocalInput : MonoBehaviour
{
    InputService _inputService;
    PlayerCommandData _commandData;
    bool sendInput = false;

    private void Awake()
    {
        _inputService = Locator.Instance.Get<InputService>();
        _inputService.inputCommandEvent += OnInputCommandInvoked;
    }

    private void OnInputCommandInvoked(object sender, PlayerCommandData data)
    {
        _commandData = data;
        sendInput = true;
    }

    private void Start()
    {
        QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    public void PollInput(CallbackPollInput callback)
    {
        if (!sendInput) return;

        Quantum.Input input = new Quantum.Input
        {
            character = _commandData.entity,
            selectedLine = _commandData.grid.Prototype.line,
            selectedGrid = _commandData.grid.Prototype.index,
        };
        callback.SetInput(input, DeterministicInputFlags.Repeatable);
        sendInput = false;
    }
}
