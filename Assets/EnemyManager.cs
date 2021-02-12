using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Demons, Undeads, Orcs };
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;
    public PlayerController player;
    public int enemyMaxLevel = 3;
    private void Awake()
    {
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
        {
            instance.gameObject.transform.position = transform.position;
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }


        //Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);

        enemiesDictionary = new Dictionary<EnemyType, List<EnemyController>>();
        foreach (EnemyType t in System.Enum.GetValues(typeof(EnemyType)))
        {
            enemiesDictionary[t] = new List<EnemyController>();
        }
    }

    public Dictionary<EnemyType, List<EnemyController>> enemiesDictionary;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
