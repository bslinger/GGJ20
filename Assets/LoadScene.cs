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


    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    public LoadScene(int waitTime, int fadeTime, string sceneToLoad)
    {
        this.waitTime = waitTime;
        this.fadeTime = fadeTime;
        this.sceneToLoad = sceneToLoad;
    }

    public IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(waitTime);
        SteamVR_Fade.Start(Color.black, fadeTime);

        yield return new WaitForSeconds(fadeTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        Destroy(player);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }  

    }

}
