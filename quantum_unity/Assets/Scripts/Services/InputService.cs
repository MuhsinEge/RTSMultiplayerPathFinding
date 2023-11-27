using Assets.Scripts.Data;
using ServiceLocator;
using System;
using UnityEngine;

public class InputService : IService
{
    public EventHandler<PlayerCommandData> inputCommandEvent;
    private GridInputService _gridInputService;
    private CharacterInputService _characterInputService;
    private Character _selectedCharacter = null;
    public InputService() {
        _gridInputService = Locator.Instance.Get<GridInputService>();
        _characterInputService = Locator.Instance.Get<CharacterInputService>();
        _gridInputService.onGridSelected += OnGridSelected;
        _characterInputService.onCharacterSelected += OnCharacterSelected;
    }

    public void OnGridSelected(object sender, GridEntity grid)
    {
        if(_selectedCharacter != null) {
            var selectedCharacterPrototype = _selectedCharacter.characterLink.Prototype;
            if (selectedCharacterPrototype.targetLine != -1 || selectedCharacterPrototype.targetGrid != -1)
            {
                _selectedCharacter = null;
                Debug.Log("Character Already Moving.");
                return;
            }

            var command = new PlayerCommandData
            {
                entity = _selectedCharacter.GetComponent<EntityView>().EntityRef,
                grid = grid,
            };

            inputCommandEvent?.Invoke(this, command);
            _selectedCharacter = null;
        }
    }

    public void OnCharacterSelected(object sender, Character character)
    {
        var prototype = character.characterLink.Prototype;
        if (prototype.targetLine != -1 && prototype.targetGrid != -1)
        {
            Debug.Log("Character Can Not Be Selected Since Its Moving.");
            _selectedCharacter = null;
            return;
        }

        _selectedCharacter = character;
    }

}
