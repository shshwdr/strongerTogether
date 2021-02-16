using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : HPCharacterController
{
    NavMeshAgent agent;

    public EnemyType enemyType;

    public int mergeLevel;

    public bool isMerging;

    public GameObject mergedToMonster;

    public float invincibleSpeedScale = 0.3f;
    float originSpeed;
    SpriteRenderer m_Renderer;

    //Rigidbody2D rb;
    // Start is called before the first frame update
    protected override void Start()
    {

        //rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        EnemyManager.instance.enemiesDictionary[enemyType].Add(this);
        base.Start();
        EnemyManager.instance.updateEnemies();
        originSpeed = agent.speed;
        m_Renderer = spriteObject. GetComponent<SpriteRenderer>();
    }

    bool isBoss()
    {
        return mergeLevel >= EnemyManager.instance.enemyMaxLevel;
    }
    public bool canBePaired()
    {
        return !isDead && !isMerging && !isBoss();
    }

    float getDistanceToTarget(Transform target)
    {
        //todo use navmesh distance instead of position distance
        return ((Vector2)transform.position - (Vector2)target.position).magnitude;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isStuned)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            //return;
        }

        else if (isMerging)
        {
            //if far away, stop merging
            //return;
        }

        //move
        else
        {
            //find the cloest target, either player or enemy with same type and merge level
            float shortestDistance = getDistanceToTarget(EnemyManager.instance.player.transform);
            Transform shortestTarget = EnemyManager.instance.player.transform;
            if (m_Renderer.isVisible)
            {
                agent.speed = originSpeed;
            }
            else
            {
                agent.speed = originSpeed * invincibleSpeedScale;

                shortestTarget = transform;
                shortestDistance = float.MaxValue;
            }
            foreach (EnemyController enemy in EnemyManager.instance.enemiesDictionary[enemyType])
            {
                if (enemy == this)
                {
                    continue;
                }
                if (!enemy.canBePaired())
                {
                    continue;
                }
                if (enemy.mergeLevel != mergeLevel)
                {
                    continue;
                }
                float distance = getDistanceToTarget(enemy.transform);
                if (distance < shortestDistance)
                {
                    shortestTarget = enemy.transform;
                    shortestDistance = distance;
                }
            }

            agent.isStopped = false;
            agent.SetDestination(shortestTarget.position);
            testFlip(agent.velocity);
        }
        animator.SetFloat("speed", agent.velocity.magnitude);
    }

    bool canBePairedWith(EnemyController other)
    {
        if (!other.canBePaired() || !canBePaired())
        {
            return false;
        }
        if (other.mergeLevel != mergeLevel)
        {
            return false;
        }
        if (other.enemyType != enemyType)
        {
            return false;
        }
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<EnemyController>()&& canBePairedWith(collision.GetComponent<EnemyController>()))
        {
            Merge(collision.GetComponent<EnemyController>());

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (isBoss() && collision.GetComponent<PlayerController>())
        //{
        //    collision.GetComponent<PlayerController>().getDamage();
        //}
    }


    public void Merge(EnemyController other)
    {
        //this is the main merger.
        other.isMerging = true;
        isMerging = true;
        //show merging effect
        //destory these two and create new monster
        Destroy(gameObject);
        Destroy(other.gameObject);
        GameObject mergedMonster = Instantiate(mergedToMonster, (transform.position + other.transform.position) / 2.0f, Quaternion.identity);

        mergedMonster.GetComponent<EnemyController>().emotesController.showEmote(EmoteType.heart);


    }

    protected override void Die()
    {
        base.Die();
        EnemyManager.instance.updateEnemies();
    }

}
