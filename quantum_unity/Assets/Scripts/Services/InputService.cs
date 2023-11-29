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

    public void OnGridSelected(object sender, EntityComponentGrid grid)
    {
        if(_selectedCharacter != null) {
            var command = new PlayerCommandData
            {
                entity = _selectedCharacter.GetComponent<EntityView>().EntityRef,
                grid = grid,
            };

            inputCommandEvent?.Invoke(this, command);
        }
    }

    public void OnCharacterSelected(object sender, Character character)
    {
        var prototype = character.characterLink.Prototype;
        if (QuantumRunner.Default.Game.GetLocalPlayers()[0] != prototype.teamId)
        {
            return;
        }
        _selectedCharacter?.ToggleHalo(false);
        _selectedCharacter = character;
        _selectedCharacter.ToggleHalo(true);
    }

}
