using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : HPCharacterController
{
    NavMeshAgent agent;

    GameObject bossTargets;
    GameObject spawnPositions;
    int spawnId = -1;
    int previousId = -1;
    public bool isSpawning = false;
    // Start is called before the first frame update

    public void spawnPublic()
    {
        StartCoroutine(spawn());
    }

    public void selectNextSpawnId()
    {
        previousId = spawnId;
        spawnId = Random.Range(0, 3);
        while (spawnId == previousId)
        {
            spawnId = Random.Range(0, 3);
        }
        agent.SetDestination(bossTargets.transform.GetChild(spawnId).transform.position);
        isSpawning = false;
    }

    IEnumerator spawn()
    {
        isSpawning = true;
        animator.SetBool("spawn", true);
        EnemyManager.instance.spawnMinions(spawnPositions.transform.GetChild(spawnId).transform.position);
        yield return new WaitForSeconds(1);
        animator.SetBool("spawn", false);
        selectNextSpawnId();
    }
    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        bossTargets = GameObject.Find("bossTarget");
        spawnPositions = GameObject.Find("spawnPosition");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
