using System.Collections.Generic;
using UnityEngine;
using ServiceLocator;
using Quantum;

public class GridLayoutEntity : MonoBehaviour 
{
    public GridLine[] grids;
    private void Awake()
    {
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
        grids[e.line].InformDataChanged(e.index);
    }
}
