using ServiceLocator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDataService : IService
{
    private int _localPlayerResourceAmount;
    private int _opponentResourceAmount;
    public EventHandler<PlayerResourceEventData> playerResourceEvent;

    public void UpdatePlayerResourceAmount(int teamId, int resourceAmount)
    {
        var id = QuantumRunner.Default.Game.GetLocalPlayers()[0];
        if (id == teamId)
        {
            _localPlayerResourceAmount += resourceAmount;
            playerResourceEvent?.Invoke(this, new PlayerResourceEventData() { teamId = id, resourceAmount = _localPlayerResourceAmount });
        }
        else
        {
            _opponentResourceAmount += resourceAmount;
            playerResourceEvent?.Invoke(this, new PlayerResourceEventData() { teamId = id, resourceAmount = _opponentResourceAmount });
        }
    }
}
public record PlayerResourceEventData
{
    public int teamId;
    public int resourceAmount;
}