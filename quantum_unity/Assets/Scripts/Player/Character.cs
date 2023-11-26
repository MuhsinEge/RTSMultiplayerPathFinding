using Quantum;
using ServiceLocator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour, IPointerDownHandler
{
    public EntityComponentCharacterLink link;
    private CharacterInputService _characterInputService;
    private void Awake()
    {
        _characterInputService = Locator.Instance.Get<CharacterInputService>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _characterInputService.CharacterSelected(this);
        Debug.Log(link.Prototype.teamId);
    }
}
