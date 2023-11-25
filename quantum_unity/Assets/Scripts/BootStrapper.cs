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
        // Initialize default service locator.
        Locator.Initialize();
        Application.targetFrameRate = 60;

        // Register all your services next.
        Locator.Instance.Register(new GridInputService());
        Locator.Instance.Register(new CharacterInputService());
        // Application is ready to start, load your main scene.
        SceneManager.LoadSceneAsync(2);
    }
}