using Quantum.Prototypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class GridEntity : MonoBehaviour, IPointerDownHandler
{
    GridInputService _gridInputService;
    private EntityComponentGrid _gridData;
    [HideInInspector] public int gridLine;
    [HideInInspector] public int gridIndex;
    public void Initialize(GridInputService gridInputService, int gridLine, int gridIndex)
    {
        this.gridLine = gridLine;
        this.gridIndex = gridIndex;
        _gridData = GetComponent<EntityComponentGrid>();
        if (_gridData.Prototype.isObstacle)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        
        _gridInputService = gridInputService;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _gridInputService.GridSelected(this);
    }
}
