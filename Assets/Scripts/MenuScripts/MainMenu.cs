using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private AsyncOperation async;

    public void ResumeTime()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void BtnLoadScene()
    {
        if (async == null)
        {
            Scene currScene = SceneManager.GetActiveScene();
            async = SceneManager.LoadSceneAsync(currScene.buildIndex + 1);
            async.allowSceneActivation = true;
        }
    }
    private bool ok = false;
    public void BtnLoadScene(int i)
    {
        
        if (async == null)
        {
            Scene currScene = SceneManager.GetActiveScene();
            async = SceneManager.LoadSceneAsync(i);
            async.allowSceneActivation = true;
        }

    }

    public void BtnLoadScene(string i)
    {
        if (async == null)
        {
            Scene currScene = SceneManager.GetActiveScene();
            async = SceneManager.LoadSceneAsync(i);
            async.allowSceneActivation = true;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
