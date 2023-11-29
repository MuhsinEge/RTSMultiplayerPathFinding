using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServiceLocator;
using System;

public class CharacterInputService : IService
{
    public EventHandler<Character> onCharacterSelected;

    public void CharacterSelected(Character character)
    {
        onCharacterSelected?.Invoke(this, character);
    }
}
