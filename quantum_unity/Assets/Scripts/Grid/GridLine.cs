using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridLine 
{
    public GridEntity[] line;
    private int _lineIndex;
    public void InitializeGrids(GridInputService gridInputService, int lineIndex)
    {
        _lineIndex = lineIndex;
        int counter = 0;
        foreach (var grid in line)
        {
            grid.Initialize(gridInputService,_lineIndex,counter);
            counter++;
        }
    }

    public void InformDataChanged(int index, bool isOccupied, bool isCollectable)
    {
        var grid = line[index];
        grid._gridData.Prototype.isOccupied = isOccupied;
        grid._gridData.Prototype.isCollectable = isCollectable;
        grid.UpdateState();
    }
}
