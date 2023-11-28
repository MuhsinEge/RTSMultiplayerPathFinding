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
    private Color _initialColor;
    private MeshRenderer _renderer;
    public void Initialize(GridInputService gridInputService, int gridLine, int gridIndex)
    {
        _gridData = GetComponent<EntityComponentGrid>();
        _renderer = GetComponent<MeshRenderer>();
        _initialColor = _renderer.material.color;
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
            _renderer.material.color = Color.blue;
        }else if (_gridData.Prototype.isCollectable)
        {
            _renderer.material.color = Color.green;
        }else if(_gridData.Prototype.isOccupied)
        {
            _renderer.material.color = Color.grey;
        }
        else
        {
            _renderer.material.color = _initialColor;
        }
    }
}
