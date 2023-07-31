using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainTheme;
    public AudioClip menuTheme;

    string sceneName;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if (newSceneName != sceneName)
        {
            sceneName = newSceneName;
            // not call it directly to handle the issue that has to do with AudioManager (didn't quite understand)
            Invoke("PlayMusic", .2f);
        }
    }

    void PlayMusic()
    {
        AudioClip clipToPlay = null;
        if (sceneName == "Menu")
        {
            clipToPlay = menuTheme;
        } else if (sceneName == "Game")
        {
            clipToPlay = mainTheme;
        }

        if (clipToPlay != null)
        {
            AudioManager.instance.PlayMusic(clipToPlay, 2);
            Invoke("PlayMusic", clipToPlay.length);
        }
    }
}
