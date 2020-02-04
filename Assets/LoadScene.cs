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
    [SerializeField] bool stopMusic = false;


    private void Start()
    {
        if (color == null) color = Color.black;
        StartCoroutine(LoadGame());
    }

    public LoadScene(int waitTime, int fadeTime, string sceneToLoad, Color color = default, bool stopMusic = false)
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
        this.stopMusic = stopMusic;

    }

    public IEnumerator LoadGame()
    {

        yield return new WaitForSeconds(waitTime);

        SteamVR_Fade.Start(color, fadeTime);
        if (stopMusic)
        {
            AudioSource music;
            GameObject go = GameObject.FindGameObjectWithTag("music");
            music = go.GetComponent<AudioSource>();
            StartCoroutine(StopMusic(go, music));
        }
        yield return new WaitForSeconds(fadeTime);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        Destroy(player);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }  

    }

    IEnumerator StopMusic(GameObject musicPlayer, AudioSource music)
    {
        float vol = music.volume;
        float elapsedTime = 0;
        while (elapsedTime <= 1)
        {
            elapsedTime += Time.deltaTime;
            float newVol = Time.deltaTime;
            Mathf.Clamp(newVol, 0, 1);
            music.volume -= newVol;
            Debug.Log(newVol);
            yield return null;
        }
        Destroy(musicPlayer);
    }

}
