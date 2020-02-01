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


    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(waitTime);
        SteamVR_Fade.Start(Color.black, fadeTime);

        yield return new WaitForSeconds(fadeTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Ben");
        Destroy(player);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        

    }

}
