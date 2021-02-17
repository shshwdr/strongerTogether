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
        ambience = FMODUnity.RuntimeManager.CreateInstance("event:/Level 2");
        ambience.setVolume(0.1f);
        ambience.start();
    }

    public void SetAmbienceParamter(float param)
    {
        if (currentAmbienceIntensity != param)
        {
            print("set ambience to " + param);
            currentAmbienceIntensity = param;
            ambience.setParameterByName("Intensity", param);
            //ambience.setParameterByName()
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        ambience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //Destroy(ambience);
    }
}
