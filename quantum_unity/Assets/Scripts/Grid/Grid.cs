using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Grid : MonoBehaviour, IPointerDownHandler
{
    GridInputService _gridInputService;
    public void Initialize(GridInputService gridInputService)
    {
        _gridInputService = gridInputService;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _gridInputService.GridSelected(this);
    }
}
