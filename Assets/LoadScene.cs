using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] int waitTime;
    [SerializeField] int fadeTime;
    [SerializeField] GameObject player;
    [SerializeField] string sceneToLoad = "Ben";
    [SerializeField] Color color;


    private void Start()
    {
        if (color == null) color = Color.black;
        StartCoroutine(LoadGame());
    }

    public LoadScene(int waitTime, int fadeTime, string sceneToLoad, Color color = default)
    {
        this.waitTime = waitTime;
        this.fadeTime = fadeTime;
        this.sceneToLoad = sceneToLoad;
        if(color == default)
        {
            this.color = Color.black;
        }
        else
        {
            this.color = color;
        }

    }

    public IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(waitTime);
        SteamVR_Fade.Start(color, fadeTime);

        yield return new WaitForSeconds(fadeTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        Destroy(player);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }  

    }

}
