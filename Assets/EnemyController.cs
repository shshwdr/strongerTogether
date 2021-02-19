﻿using System.Collections;
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
    protected EnemyController mergingOther;
    public Animator deathAnimator;

    float offMergeDistance = 0;

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
        offMergeDistance = GetComponent<CircleCollider2D>().radius * 2f;
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
        if (isDead || EnemyManager.instance.player.isDead)
        {
            agent.isStopped = true;
            return;
        }
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
            if (!mergingOther || mergingOther.isDead||(mergingOther.transform.position - transform.position).magnitude>= offMergeDistance)
            {
                StopMerging();
            }
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
                if (!enemy || enemy.isDead)
                {
                    continue;
                }
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
        if (isDead)
        {
            return;
        }
        if(collision.GetComponent<EnemyController>()&& canBePairedWith(collision.GetComponent<EnemyController>()))
        {
            StartCoroutine( Merge(collision.GetComponent<EnemyController>()));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isDead)
        {
            return;
        }
        //if(isMerging &&  collision.GetComponent<EnemyController>() == mergingOther)
        //{
        //    StopMerging();
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (isBoss() && collision.GetComponent<PlayerController>())
        //{
        //    collision.GetComponent<PlayerController>().getDamage();
        //}
    }
    void StopMerging() {
        if (mergingOther)
        {
            mergingOther.isMerging = false;
            mergingOther.emotesController.showEmote(EmoteType.heartBreak);
        }
        isMerging = false;
        emotesController.showEmote(EmoteType.heartBreak);
        mergingOther.mergingOther = null;
        mergingOther = null;
        StopAllCoroutines();
    }

    IEnumerator Merge(EnemyController other)
    {
        other.isMerging = true;
        isMerging = true;
        mergingOther = other;
        other.mergingOther = this;
        emotesController.showEmote(EmoteType.heart,true);
        other.emotesController.showEmote(EmoteType.heart,true);
        yield return new WaitForSeconds(2);
        if (isDead)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (isMerging)
        {

            //this is the main merger.
            //show merging effect
            //destory these two and create new monster
            Destroy(gameObject);
            Destroy(other.gameObject);
            GameObject mergedMonster = Instantiate(mergedToMonster, (transform.position + other.transform.position) / 2.0f, Quaternion.identity);

            mergedMonster.GetComponent<EnemyController>().emotesController.showEmote(EmoteType.happy);
            if (EnemyManager.instance.bossController)
            {
                EnemyManager.instance.bossController.spawnersMerge();
            }
            AudioManager.Instance.playMerge();
        }


    }

    protected override void Die()
    {
        if (EnemyManager.instance.bossController)
        {
            EnemyManager.instance.bossController.spawnersDie();
        }
        base.Die();
        EnemyManager.instance.updateEnemies();
        animator.SetTrigger("die");
        //deathAnimator.enabled = true;
        Destroy(gameObject, 0.3f);
    }

}
