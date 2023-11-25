using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridLine 
{
    public Grid[] line;

    public void InitializeGrids(GridInputService gridInputService)
    {
        foreach (var grid in line)
        {
            grid.Initialize(gridInputService);
        }
    }
}
