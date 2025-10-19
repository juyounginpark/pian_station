using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentMove : MonoBehaviour
{


    public Animator anim;
    public SpriteRenderer sp;
    public Rigidbody2D rg;

    public Animator eanim;
    public SpriteRenderer esp;


    public GameObject bullet;

    public bool ready;
    public bool iscrawl;
    public bool shift;
    public bool isground;
    public bool die;
    public bool iswall;
    public bool isladder;
   

    public bool ladderon;


    public float dx;
    public float dy;
    public float speed;
    public float jumppower;

    // Start is called before the first frame update
    void Start()
    {
        anim = transform.GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        rg = GetComponent<Rigidbody2D>();
        eanim = transform.GetChild(0).transform.GetComponent<Animator>();
        esp = transform.GetChild(0).transform.GetComponent<SpriteRenderer>();

        jumppower = 2f;
        isladder = false;
        ladderon = false;
  
    }

    // Update is called once per frame
    void Update()
    {
        dx = Input.GetAxisRaw("Horizontal");
        dy = Input.GetAxisRaw("Vertical");
        anim.SetFloat("dx", Mathf.Abs(dx));
        anim.SetFloat("dy", Mathf.Abs(dy));

        Fliping();

        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetBool("death", true);
            anim.SetTrigger("die");

        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            anim.SetBool("death", false);

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ready =! ready;
            anim.SetBool("ready", true);

        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shift = !shift;
            anim.SetBool("shift", shift);


        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetTrigger("attack1");


        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            anim.SetTrigger("attack2");


        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            anim.SetTrigger("attack3");


        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

            anim.SetTrigger("swordattack1");
            eanim.SetTrigger("swordattack1");


        }


        if (Input.GetKeyDown(KeyCode.T))
        {

            anim.SetTrigger("damage");


        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            iscrawl =! iscrawl;

            anim.SetBool("iscrawl", iscrawl);

            if (iscrawl)
            {
                GetComponent<CapsuleCollider2D>().size = new Vector2(0.25f, 0.45f);
                GetComponent<CapsuleCollider2D>().offset = new Vector2(0, -0.27f);

            }
            else
            {
                GetComponent<CapsuleCollider2D>().size = new Vector2(0.25f, 0.9f);
                GetComponent<CapsuleCollider2D>().offset = new Vector2(0, -0.04f);

            }



        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();

        }


        Speeding();

    }

    private void FixedUpdate()
    {
        Moving();


    }

    private void Moving()
    {
        this.gameObject.transform.position += Vector3.right * dx * 0.01f * speed;

        if (anim.GetBool("iswall"))
        {
            rg.AddForce(Vector3.up * 8f, ForceMode2D.Force);
        
        }

        if (anim.GetBool("isladder"))
        {
            if (ladderon)
            {
                this.gameObject.transform.position += Vector3.up * dy * 0.01f * speed;
            }
            else 
            {
                if (dy > 0)
                {
                    ladderon = true;
                    anim.SetTrigger("ladderon");

                    rg.gravityScale = 0;
                
                }
            
            }

           

        }


    }



    private void Jump()
    {



        if (isground)
        {
            isground = false;
            anim.SetTrigger("jump");

         

        }


    }

    private void Jumping()
    {
        rg.AddForce(Vector3.up * jumppower, ForceMode2D.Impulse);

        if (dx != 0)
        {
            rg.AddForce(Vector3.right * dx , ForceMode2D.Impulse);
        }

    }

    private void Speeding()
    {
        if (ready)
        {
            speed = 1;
            anim.SetBool("ready", true);

            //레디 문제생길 수도 있음

        }
        else
        {
            anim.SetBool("ready", false);

            if (shift)
            {
                speed = 4;

            }
            else { speed = 2; }
        }



    }

    private void Fliping()
    {
        if (dx < 0 && sp.flipX == false)
        {
            sp.flipX = true;
            esp.flipX = true;

        }
        else if (dx > 0 && sp.flipX == true)
        {
            sp.flipX = false;
            esp.flipX = false;

        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            isground = true;
            anim.SetBool("iswall", false);
        }

        if (collision.CompareTag("wall") && !isground)
        {
            anim.SetBool("iswall", true);
        }

        if (collision.CompareTag("cliff"))
        {
            anim.SetBool("isedge", true);
            anim.SetTrigger("flap");
        }

        if (collision.CompareTag("ladder"))
        {
            isladder = true;
            anim.SetBool("isladder", true);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.CompareTag("ground"))
        {
            if (!isground)
            {
                isground = true;
                anim.SetBool("iswall", false);

            }

        }


        if (collision.CompareTag("wall") && !isground)
        {
            anim.SetBool("iswall", true);
        }

        if (collision.CompareTag("cliff"))
        {
            anim.SetBool("isedge", true);
       
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ground"))
        {
            isground = false;
        }

        if (collision.CompareTag("wall"))
        {
            anim.SetBool("iswall", false);
        }

        if (collision.CompareTag("cliff"))
        {
            anim.SetBool("isedge", false);
        }


        if (collision.CompareTag("ladder"))
        {
          
            anim.SetBool("ladderon", false);
            anim.SetBool("isladder", false);
            isladder = false;
            ladderon = false;
            rg.gravityScale = 1;

        }
    }


}
