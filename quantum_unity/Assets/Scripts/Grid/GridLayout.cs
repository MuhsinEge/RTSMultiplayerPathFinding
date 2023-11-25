using System.Collections.Generic;
using UnityEngine;
using ServiceLocator;
public class GridLayout : MonoBehaviour
{
    public GridLine[] grids;
    private void Start()
    {
    }
    private void Awake()
    {
        var gridInputService = Locator.Instance.Get<GridInputService>();
        foreach (var line in grids)
        {
            line.InitializeGrids(gridInputService);    
        }
    }
}
