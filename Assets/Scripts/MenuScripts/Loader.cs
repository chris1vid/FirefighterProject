using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField] public Image loadingBar;
    private bool ready = false;
    [SerializeField] public float delay = 1;
    [SerializeField] public int sceneToLoad = 2;

    private AsyncOperation async;

    // Use this for initialization
    void Start()
    {
        BeginLoad();
        ready = false;
    }

    void Activate()
    {
        ready = true;
    }


    void BeginLoad()
    {

        Input.ResetInputAxes(); //reset the input
        System.GC.Collect(); //clean the assets from ram we wont need anymore
        Scene currScene = SceneManager.GetActiveScene(); // checking current scene
        if (sceneToLoad == -1)
        {
            async = SceneManager.LoadSceneAsync(currScene.buildIndex + 1); // load following scene
        }
        else
        {
            async = SceneManager.LoadSceneAsync(sceneToLoad);
        }
        async.allowSceneActivation = false; // wait before moving to the loaded screen

        Invoke("Activate", delay);

    }

    // Update is called once per frame
    void Update()
    {


        if (loadingBar)
        {
            loadingBar.fillAmount = async.progress + 0.1f;
        }

        if (async.progress >= 0.89 && SplashScreen.isFinished && ready) // check if loaded above 89%
        {
            async.allowSceneActivation = true; // allow moving to the next screen
           
        }
    }
}
