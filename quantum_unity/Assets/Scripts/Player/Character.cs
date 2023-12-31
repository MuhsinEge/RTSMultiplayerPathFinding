using Quantum;
using ServiceLocator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector] public EntityComponentCharacterLink characterLink;
    [SerializeField] private GameObject halo;
    private CharacterInputService _characterInputService;

    private void Awake()
    {
        _characterInputService = Locator.Instance.Get<CharacterInputService>();
        characterLink = GetComponent<EntityComponentCharacterLink>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _characterInputService.CharacterSelected(this);
    }

    public void ToggleHalo(bool isSelected)
    {
        halo.SetActive(isSelected);
    }

    private void Start()
    {
        QuantumEvent.Subscribe<EventCharacterTargetEvent>(this, OnCharacterTargetDataChangedEvent);
    }

    private void OnCharacterTargetDataChangedEvent(EventCharacterTargetEvent e)
    {
        if (e.team == characterLink.Prototype.teamId && e.index == characterLink.Prototype.playerIndex)
        {
            characterLink.Prototype.targetLine = e.targetLine;
            characterLink.Prototype.targetGrid = e.targetGrid;
        }

    }
}
