using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class BossController : HPCharacterController
{
    NavMeshAgent agent;

    GameObject bossTargets;
    GameObject spawnPositions;
    int spawnId = -1;
    int previousId = -1;
    public bool isSpawning = false;

    int maxStage = 2;
    int stage = 0;
    bool isDashing;
    Vector3 dashTarget;
    // Start is called before the first frame update

    public void spawnPublic()
    {
        StartCoroutine(spawn());
    }

    public void spawnPublic2()
    {
        StartCoroutine(spawn2());
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
    }

    public void selectNextSpawnIdAndMove()
    {
        selectNextSpawnId();
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
        selectNextSpawnIdAndMove();
        yield return new WaitForSeconds(1f);

        Debug.Log("spawn finished");

        Debug.Log("remaining after finishe " + agent.remainingDistance);
        isSpawning = false;
    }
    IEnumerator spawn2()
    {
        Debug.Log("start spawn");
        isSpawning = true;
        isDashing = false;
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
        dashToPlayer();
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
    protected override void Die()
    {
        isDead = true;
        agent.isStopped = true;
        StopAllCoroutines();
        if (stage != maxStage)
        {
            //go to next stage
            stage++;
            animator.SetTrigger("die");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collider = collision.collider;
        if (isDashing && collider.GetComponent<FakeObstacles>())
        {
            collider.GetComponent<FakeObstacles>().getCollide(Vector3.zero);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDashing&& collision.GetComponent<FakeObstacles>())
        {
            collision.GetComponent<FakeObstacles>().getCollide(Vector3.zero);
        }
    }

    public void dashToPlayer()
    {
        var playerPosition = EnemyManager.instance.player.transform.position;
        dashTarget = playerPosition;
        rb.DOMove(playerPosition, 1f);
        isDashing = true;
    }

    public bool isDashFinished()
    {
        return ((Vector2) transform.position - (Vector2)dashTarget).magnitude <= 0.05f;
    }

    public void Revive()
    {
        isDead = false;
        hp = maxHp;
        updateHP();
    }
}
