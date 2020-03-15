using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Effects;
using UnityStandardAssets.Utility;

public class Extinguishing : MonoBehaviour
{
    public float multiplier = 1f;
    [SerializeField] private float reduceFactor = 0.8f;
    public GameObject checkbox;
    private AudioSource audioS;
    private float lastExtinguish;
    [SerializeField] private float flameIdlePeriod = 1.0f;
    [SerializeField] private gamemaster code;
    public float fireNum;


    // Use this for initialization
    void Start()
    {
        checkbox.SetActive(false);
        ParticleSystemMultiplier SysMul = GetComponent<ParticleSystemMultiplier>();
        multiplier = SysMul.multiplier;
        audioS = GetComponent<AudioSource>();
    }

    bool catcher = true;
    // Update is called once per frame
    void Extinguish()
    {
        multiplier *= reduceFactor;
        audioS.volume *= reduceFactor;
        var systems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in systems)
        {
            ParticleSystem.MainModule mainModule = system.main;
            mainModule.startSizeMultiplier *= reduceFactor;
            mainModule.startSpeedMultiplier *= reduceFactor;
            //mainModule.startLifetimeMultiplier *= Mathf.Lerp(multiplier, 1, 0.5f);
            //system.Clear();
            system.Play();
            lastExtinguish = Time.time;
        }
        if (multiplier <= 0.01f && checkbox.activeInHierarchy == false && catcher)
        {
            catcher = false;
            var systemsa = GetComponentsInChildren<ParticleSystem>();

            foreach (var system in systemsa)
            {
                var emission = system.emission;
                emission.enabled = false;
            }
            checkbox.SetActive(true);
            //checkbox.transform.parent = null;
            code.currScore += 100;
            code.extinguishedFires++;
            catcher = true;
        }
    }

    public void DisableCheck()
    {
        checkbox.SetActive(false);
    }

    IEnumerator WaitFor(float x)
    {
        yield return new WaitForSeconds(x);
    }

    private void Update()
    {
        if (multiplier > 0.01f && checkbox.activeInHierarchy == false)
        {
            code.structureHP -= 0.01f;
        }
        
        if (Time.time > lastExtinguish + flameIdlePeriod && multiplier < 1.0f && checkbox.active == false)
        {
            multiplier /= reduceFactor;
            audioS.volume /= reduceFactor;
            var systems = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem system in systems)
            {
                ParticleSystem.MainModule mainModule = system.main;
                mainModule.startSizeMultiplier /= reduceFactor;
                mainModule.startSpeedMultiplier /= reduceFactor;
                //mainModule.startLifetimeMultiplier *= Mathf.Lerp(multiplier, 1, 0.5f);
                //system.Clear();
                system.Play();
            }
            lastExtinguish += 0.4f;
            Debug.Log(multiplier);
        }
    }
    /* have update called
    if a time set by extinguish is met when update runs...
    the bool regrow is then true, which disables when extinguish is run again
    while regrow is true, the fire grows back at a certain rate 
    (use waitforseconds?)*/
}
