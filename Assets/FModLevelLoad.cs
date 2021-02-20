using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FModLevelLoad : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string eventName;
    // Start is called before the first frame update
    void Start()
    {
        FModSoundManager.Instance.startEvent(eventName);

        EnemyManager.instance.updateEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
