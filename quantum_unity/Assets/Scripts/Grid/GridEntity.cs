using Quantum.Prototypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class GridEntity : MonoBehaviour, IPointerDownHandler
{
    GridInputService _gridInputService;
    [HideInInspector]public EntityComponentGrid _gridData;
    public void Initialize(GridInputService gridInputService, int gridLine, int gridIndex)
    {
        _gridData = GetComponent<EntityComponentGrid>();
        _gridData.Prototype.line = gridLine;
        _gridData.Prototype.index = gridIndex;
        _gridInputService = gridInputService;
        UpdateView();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _gridInputService.GridSelected(_gridData);
    }

    public void UpdateView()
    {
        if (_gridData.Prototype.isObstacle)
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
        }else if (_gridData.Prototype.isCollectable)
        {
            GetComponent<MeshRenderer>().material.color = Color.yellow;
        }
    }
}
