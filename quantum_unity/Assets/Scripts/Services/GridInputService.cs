using ServiceLocator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInputService : IService
{
    public EventHandler<GridEntity> onGridSelected;

    public void GridSelected(GridEntity grid)
    {
        Debug.Log("Selected grid");
        onGridSelected?.Invoke(this, grid);
    }
}
