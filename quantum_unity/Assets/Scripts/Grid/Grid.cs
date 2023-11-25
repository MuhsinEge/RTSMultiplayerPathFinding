using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class Grid : MonoBehaviour, IPointerDownHandler
{
    private void Start()
    {
        this.enabled = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked on grid");
    }
}
