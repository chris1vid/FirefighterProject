using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Utility;

public class TogglePanel : MonoBehaviour
{

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject[] panels;
    [SerializeField] private Selectable[] defaultOptions;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject successPanel;

    public void PanelToggle(int pos)
    {
        Input.ResetInputAxes();
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(pos == i);
            if (pos == i)
            {
                //defaultOptions[i].Select();
            }
        }
    }


    public void doPause()
    {
        if (panels[0].gameObject.activeInHierarchy == true) // pausing
        {
            Time.timeScale = 0;

            playerObject.GetComponent<SimpleMouseRotator>().enabled = false;
            
            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;


            PanelToggle(2);

        }
        else if (panels[0].gameObject.activeInHierarchy == false) // resuming

        {
            Time.timeScale = 1;

            playerObject.GetComponent<SimpleMouseRotator>().enabled = true;
            
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

            PanelToggle(0);
        }
    }

    // Use this for initialization
    void Start()
    {
        PanelToggle(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverPanel.gameObject.activeInHierarchy == false && successPanel.gameObject.activeInHierarchy == false) 
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                doPause();
            }
        }
    }
}
