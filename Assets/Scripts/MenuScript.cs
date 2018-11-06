using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private bool loading = false;
    public Text loadingText;
    public Button playButton;
    public Button exitButton;
   
	// Use this for initialization
	void Start () {
        loadingText.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (loading)
        {
            if (!loadingText.enabled)
                loadingText.enabled = true;

            if (playButton.enabled)
                playButton.enabled = false;

            if (exitButton.enabled)
                exitButton.enabled = false;

            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
        }
	}

    public void LoadGame()
    {
        //SceneManager.LoadScene("Level");
        if (!loading)
        {
            loading = true;
            StartCoroutine(LoadGameASync());
        }
    }
    
    IEnumerator LoadGameASync()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("Level");

        while(!async.isDone)
        {
            yield return null;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
