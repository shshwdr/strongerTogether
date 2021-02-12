using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FModSoundManager : Singleton<FModSoundManager>
{
    FMOD.Studio.EventInstance ambience;
    float currentAmbienceIntensity;
    // Start is called before the first frame update
    void Start()
    {
        ambience = FMODUnity.RuntimeManager.CreateInstance("event:/FMOD Test");
        ambience.start();
    }

    public void SetAmbienceParamter(float param)
    {
        if (currentAmbienceIntensity != param)
        {
            print("set ambience to " + param);
            currentAmbienceIntensity = param;
            ambience.setParameterByName("Intensity", param);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
