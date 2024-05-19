using Meta.WitAi.Json;
using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestart : MonoBehaviour
{
    [SerializeField] string otherSceneName;

    public void ChangeScene(WitResponseNode commandResult)
    {
        var intent = WitResultUtilities.GetIntentName(commandResult);

        switch (intent)
        {
            case "travel":
                Debug.Log("sceneName to load: " + otherSceneName);
                SceneManager.LoadScene(otherSceneName);

                break;
            case "restart":
                Debug.Log("restarting scene");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

                break;
            default:
                break;
        }
    }
}
