using System.Collections.Generic;
using UnityEngine;
using ServiceLocator;
using Quantum;

public class GridLayoutEntity : MonoBehaviour 
{
    public GridLine[] grids;
    private ResourceDataService _resourceDataService;
    private void Awake()
    {
        _resourceDataService = Locator.Instance.Get<ResourceDataService>();
        var gridInputService = Locator.Instance.Get<GridInputService>();
        int counter = 0;
        foreach (var line in grids)
        {
            line.InitializeGrids(gridInputService, counter);
            counter++;
        }
    }

    private void Start()
    {
        QuantumEvent.Subscribe<EventGridDataEvent>(this, OnGridDataChangedEventHandler);
    }

    private void OnGridDataChangedEventHandler(EventGridDataEvent e)
    {
        CheckResourceGain(e);
        grids[e.line].InformDataChanged(e.index, e.occupied, e.collectable);
    }

    public void CheckResourceGain(EventGridDataEvent e)
    {
        if (e.occupierTeam == -1) return;
        if (grids[e.line].line[e.index]._gridData.Prototype.isCollectable)
        {
            _resourceDataService.UpdatePlayerResourceAmount(e.occupierTeam, grids[e.line].line[e.index]._gridData.Prototype.resourceAmount);
        }
    }
}
