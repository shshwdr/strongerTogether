using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//using PixelCrushers.DialogueSystem;
public class PlayerController: HPCharacterController
{
   // public static Player instance = null;
    public Rigidbody2D rb;
    Vector2 movement;
    public float moveSpeed = 5f;

    public GameObject meleeAttackCollider;
    Vector3 originMeleeAttackPosition;


   // private void Awake()
    //{
        //if (instance == null)

        //    //if not, set instance to this
        //    instance = this;

        ////If instance already exists and it's not this:
        //else if (instance != this)
        //{
        //    instance.gameObject.transform.position = transform.position;
        //    //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
        //    Destroy(gameObject);
        //}


        ////Sets this to not be destroyed when reloading scene
        //DontDestroyOnLoad(gameObject);
    //}
    // Start is called before the first frame update
    protected override void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        EnemyManager.instance.player = this;
        originMeleeAttackPosition = meleeAttackCollider.transform.localPosition;
        base.Start();
    }

    //public void Damage(int dam = 1)
    //{

    //    GameOver();
    //}

    //void GameOver()
    //{
    //    if (GameManager.instance.dontDie)
    //    {
    //        return;
    //    }
    //    SoundManager.instance.playBGM(gameoverClip);
    //    GameManager.instance.GameOver();


    //}

    //public void pause()
    //{
    //    GameManager.instance.isPaused = true;
    //}
    //public void resume()
    //{
    //    GameManager.instance.isPaused = false;
    //}
    // Update is called once per frame
    override protected void Update()
    {

        //if (GameManager.instance.isCheatOn && Input.GetKeyDown(KeyCode.M))
        //{
        //    GameOver();
        //    return;
        //}
        //if (GameManager.instance.isPaused)
        //{
        //    moveSpeed = 0;
        //    movement = Vector2.zero;
        //    return;
        //}

        movement.x = Input.GetAxisRaw("Horizontal");

        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        movement.y = Input.GetAxisRaw("Vertical");
        float speed = movement.sqrMagnitude;
        movement = Vector2.ClampMagnitude(movement, 1);
        animator.SetFloat("speed", movement.sqrMagnitude);


        base.Update();


        //melee prepare
        if (speed > 0.01f)
        {
            meleeAttackCollider.SetActive(true);
            var dir = new Vector3(movement.x, movement.y, 0)*0.08f;
            // The shortcuts way
            //meleeAttackCollider. transform.DOMove(transform.position + dir, 1);
            // The generic way
            DOTween.To(() => meleeAttackCollider.transform.localPosition, x => meleeAttackCollider.transform.localPosition = x, dir, 0.5f);

            //DOTween.To(() => transform.position, x => transform.position = x, new Vector3(2, 2, 2), 1);

        }
        else
        {

            meleeAttackCollider.SetActive(false);
            meleeAttackCollider.transform.localPosition = Vector3.zero;
            stopAttackAnim();
        }
        // transform
    }

    public void attackAnim()
    {
        animator.SetBool("attack", true);
        animator.SetFloat("attackHorizontal", movement.x);

        animator.SetFloat("attackVertical", movement.y);
    }

    public void stopAttackAnim()
    {

        animator.SetBool("attack", false);
    }

    private void LateUpdate()
    {

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        testFlip(movement);
        // rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "coat" && !isCamouflage)
    //    {
    //        isCamouflage = true;
    //        Destroy(collision.gameObject);
    //        GetComponent<SpriteRenderer>().sprite = camouflagedSprite;
    //        animator.runtimeAnimatorController = camouflagedAnimator;
    //        isCamouflage = true;
    //        exit.SetActive(true);
    //        DialogueManager.ShowAlert("You are camouflaged");
    //    }
    //}
}
