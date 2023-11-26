using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class GridEntity : MonoBehaviour, IPointerDownHandler
{
    GridInputService _gridInputService;
    [HideInInspector] public int gridLine;
    [HideInInspector] public int gridIndex;
    public void Initialize(GridInputService gridInputService, int gridLine, int gridIndex)
    {
        this.gridLine = gridLine;
        this.gridIndex = gridIndex;
        _gridInputService = gridInputService;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _gridInputService.GridSelected(this);
    }
}
