using ServiceLocator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Initiailze()
    {
        Locator.Initialize();
        Application.targetFrameRate = 60;
        Locator.Instance.Register(new GridInputService());
        Locator.Instance.Register(new CharacterInputService());
        Locator.Instance.Register(new InputService());
        Locator.Instance.Register(new ResourceDataService());
        SceneManager.LoadSceneAsync(0);
    }
}