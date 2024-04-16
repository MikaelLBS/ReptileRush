using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovment : MonoBehaviour, IDataPersitiens
{

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 10f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb.AddForce(Vector2.down*1000);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            Application.Quit();
        if (Input.GetButtonDown("Submit"))
            DataPersistenceManager.Instance.SaveGame();

        horizontal = Input.GetAxisRaw("Horizontal");
        Flip();

        
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
        {
            animator.SetTrigger("Jump");
            StartCoroutine(InAirCheck());
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }


        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            animator.SetTrigger("Jump");
            StartCoroutine(InAirCheck());
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetKeyUp(KeyCode.W) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
    IEnumerator InAirCheck()
    {
        yield return new WaitForSeconds(0.2f);
        while (!IsGrounded())
        {
            yield return new WaitForFixedUpdate();
        }
        animator.ResetTrigger("Jump");
    }
    private void FixedUpdate()
    {
        rb.velocity=new Vector2(horizontal*speed, rb.velocity.y);

        if (rb.velocity.y < -1.5f)
            animator.SetBool("Falling", true);
        else
            animator.SetBool("Falling", false);

        if (Mathf.Abs(rb.velocity.x) > 0.1f)
            animator.SetBool("IsMoving", true);
        else
            animator.SetBool("IsMoving",false);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if(isFacingRight && horizontal <0f || !isFacingRight && horizontal >0)
        {
            isFacingRight=!isFacingRight;
            transform.Rotate(0,180,0);

        }
    }

    public void LoadData(GameData data)
    {
        if (data.playerPosWasSaved)
            transform.position = new Vector2(data.worldPos.x,data.worldPos.y);
    }
    public void SaveData(ref GameData data)
    {
        data.playerPosWasSaved = true;
        data.worldPos = (transform.position.x,transform.position.y);
    }
}
