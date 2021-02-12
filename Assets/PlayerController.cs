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
    Animator animator;

    bool facingRight = true;



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
        animator = GetComponent<Animator>();
        EnemyManager.instance.player = this;
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
        // transform
    }
    private void LateUpdate()
    {

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        if(facingRight == false && movement.x > 0)
        {
            flip();
        }
        if (facingRight == true && movement.x < 0)
        {
            flip();
        }
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
    void flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x = -scaler.x;
        transform.localScale = scaler;
    }
}
