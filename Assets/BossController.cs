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
    public int[] maxHps;
    int maxStage = 2;
    int stage = 0;
    bool isDashing;
    float dashTime = 2f;
    float currentDashTimer;
    BoxCollider2D solidCollider;
    // Start is called before the first frame update

    public void spawnPublic()
    {
        StartCoroutine(spawn());
    }

    public void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(EnemyManager.instance.player.transform.position);
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

        solidCollider.enabled = true;
        animator.SetBool("spawn", true);
        EnemyManager.instance.spawnMinions(spawnPositions.transform.GetChild(spawnId).transform.position);
        yield return new WaitForSeconds(2);
        animator.SetBool("spawn", false);
        //Debug.Log("select next");
        selectNextSpawnId();
        yield return new WaitForSeconds(1f);

        //Debug.Log("spawn finished");

        //Debug.Log("remaining after finishe " + agent.remainingDistance);
        isSpawning = false;
        dashToPlayer();
    }
    protected override void Start()
    {
        maxHp = maxHps[stage];
        base.Start();
        solidCollider = GetComponent<BoxCollider2D>();
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
        currentDashTimer += Time.deltaTime;

        testFlip(agent.velocity);
    }
    protected override void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        agent.isStopped = true;
        isSpawning = false;
        StopAllCoroutines();
        if (stage != maxStage)
        {
            //go to next stage
            stage++;
            animator.SetTrigger("die");

            AudioManager.Instance.playBossDamage(stage);
        }
        else
        {
            Destroy(gameObject);
            AudioManager.Instance.playBossDefeat();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collider = collision.collider;
        if (isDashing && collider.GetComponent<FakeObstacles>())
        {
            collider.GetComponent<FakeObstacles>().getCollide(Vector3.zero);
        }
        if (isDashing && collider.GetComponent<PlayerController>())
        {
            collider.GetComponent<PlayerController>().getDamage();
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
        var dir = (playerPosition - transform.position).normalized;

        //int layerMask = 1 << 12;
        //layerMask = ~layerMask;
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 12.0f, layerMask);
        var targetPosition = playerPosition+dir*0.2f;
        rb.AddForce(dir * 100);
        //rb.DOMove(targetPosition, dashTime);
        isDashing = true;
        //solidCollider.enabled = false;
        currentDashTimer = 0;
    }

    public bool isDashFinished()
    {
        return currentDashTimer >= dashTime;
    }

    public void Revive()
    {

        maxHp = maxHps[stage];
        isDead = false;
        hp = maxHp;
        updateHP();
    }
}
