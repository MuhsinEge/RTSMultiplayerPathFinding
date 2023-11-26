using System;
using Photon.Deterministic;
using Quantum;
using UnityEngine;

public class LocalInput : MonoBehaviour {

    public EntityView characterLink;
    public bool sendInput = false;
  private void Start() {
    QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
  }

  public void PollInput(CallbackPollInput callback) {
        if (!sendInput) return;
        
        Quantum.Input input = new Quantum.Input
        {
            character = characterLink.EntityRef,
            selectedGrid = 0,
            selectedLine = 0,
        };
        callback.SetInput(input, DeterministicInputFlags.Repeatable);
        sendInput = false;
    }
}
