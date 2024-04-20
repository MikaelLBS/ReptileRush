using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Camera cmr;
    [SerializeField] float cmrsz = 5;
    [SerializeField] Transform groundTrans;

    Vector2 veloz = new Vector2(0, 0);
    bool left = false;
    bool right = false;
    bool up = false;
    bool down = false;
    //float deltaTime;
    float accumulatedTime = 0f;
    [SerializeField] float zoomInterval = 1f; // 50 milliseconds    
    [SerializeField] float zoomChangeAmount;
    float sizeLim;
    float widhLim;
    float widh;

    private void Start()
    {
        //deltaTime = Time.deltaTime; // BAD CODE: Time.deltaTime chances from one frame to the next

        sizeLim = groundTrans.lossyScale.x / 3.5556f;

        widhLim = Mathf.Abs(groundTrans.lossyScale.x / 2);
    }

    void Update()
    {
        accumulatedTime += Time.deltaTime;
        widh = cmrsz * 1.8f;        

        GetInput();

        while (accumulatedTime >= zoomInterval) { 
            Zoom();
            accumulatedTime -= zoomInterval;
        }

        Move();
    }

    void GetInput()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        if (horizontalMove < -0.1f)
            left = true;
        else
            left = false;
        if (horizontalMove > 0.1f)
            right = true;
        else
            right = false;
        if (verticalMove > 0.1f || Input.mouseScrollDelta.y > 0)
            up = true;
        else
            up = false;
        if (verticalMove < -0.1f || Input.mouseScrollDelta.y < 0)
            down = true;
        else
            down = false;
    }

    void Zoom()
    {
        if (up != down)
        {
            if (up)
            {
                cmrsz += zoomChangeAmount;
            }
            else if (down)
            {
                cmrsz -= zoomChangeAmount;
            }
            if(cmrsz < 1)
                cmrsz = 1;
            if (cmrsz > sizeLim)
                cmrsz = sizeLim;
            cmr.orthographicSize = cmrsz;
            
            cmr.transform.position = new Vector3(cmr.transform.position.x, cmrsz-5, cmr.transform.position.z);
        }
    }

    void Move()
    {
        if (right != left)
        {
            if (right)
            {
                veloz.x = 15;
            }
            else if (left)
            {
                veloz.x = -15;
            }
            rb.velocity = veloz;
        }
        if (right == left)
        {
            rb.velocity = Vector2.zero;
        }

        if (transform.position.x+widh > widhLim)            
            cmr.transform.position = new Vector3(widhLim-widh, cmr.transform.position.y, cmr.transform.position.z);
        //Debug.Log("to far left");
        if (transform.position.x-widh < -widhLim)
            cmr.transform.position = new Vector3(-widhLim+widh, cmr.transform.position.y, cmr.transform.position.z);
        //Debug.Log("to far right");
    }
}
