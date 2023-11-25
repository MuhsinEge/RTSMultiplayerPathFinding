using ServiceLocator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInputService : IService
{
    public EventHandler<Grid> onGridSelected;

    public void GridSelected(Grid grid)
    {
        Debug.Log("Selected grid");
        onGridSelected?.Invoke(this, grid);
    }
}
