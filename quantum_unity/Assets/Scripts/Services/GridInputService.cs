using ServiceLocator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInputService : IService
{
    public EventHandler<EntityComponentGrid> onGridSelected;

    public void GridSelected(EntityComponentGrid grid)
    {
        Debug.Log("Selected grid");
        onGridSelected?.Invoke(this, grid);
    }
}
