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
        spawnId += 1;//Random.Range(0, 3);
        if(spawnId == 3)
        {
            spawnId = 0;
        }
        //while (spawnId == previousId)
        //{
        //    spawnId = Random.Range(0, 3);
        //}
        Debug.Log("previous " + previousId + " now " + spawnId);
        agent.SetDestination(bossTargets.transform.GetChild(spawnId).transform.position);
        Debug.Log("remaining after select " + agent.remainingDistance);
    }

    IEnumerator spawn()
    {
        Debug.Log("start spawn");
        isSpawning = true;
        animator.SetBool("spawn", true);
        EnemyManager.instance.spawnMinions(spawnPositions.transform.GetChild(spawnId).transform.position);
        yield return new WaitForSeconds(1);
        animator.SetBool("spawn", false);
        Debug.Log("select next");
        selectNextSpawnId();
        yield return new WaitForSeconds(1f);

        Debug.Log("spawn finished");

        Debug.Log("remaining after finishe " + agent.remainingDistance);
        isSpawning = false;
    }
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        bossTargets = GameObject.Find("bossTarget");
        spawnPositions = GameObject.Find("spawnPosition");
        EnemyManager.instance.bossController = this;
    }

    public void spawnersDie()
    {
        getDamage();
    }
    public void spawnersMerge()
    {
        emotesController.showEmote(EmoteType.happy);
    }

    // Update is called once per framee
    protected override void Update()
    {
        base.Update();

        testFlip(agent.velocity);
    }
}
