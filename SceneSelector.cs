using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.Json;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.VolumeComponent;

public class SceneSelector : MonoBehaviour
{
    [SerializeField] string wakeSceneName;
    [SerializeField] string sleepSceneName;

    public void SelectScene(WitResponseNode commandResult)
    {
        var intent = WitResultUtilities.GetIntentName(commandResult);

        switch (intent)
        {
            case "wake":
                Debug.Log("sceneName to load: " + wakeSceneName);
                SceneManager.LoadScene(wakeSceneName);

                break;
            case "sleep":
                Debug.Log("sceneName to load: " + sleepSceneName);
                SceneManager.LoadScene(sleepSceneName);

                break;
            default:
                break;
        }
    }
}
