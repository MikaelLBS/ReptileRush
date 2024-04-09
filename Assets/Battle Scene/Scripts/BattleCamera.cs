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
    float deltaTime;
    float accumulatedTime = 0f;
    float zoomInterval = 1f; // 50 milliseconds    
    float sizeLim;
    float widhLim;
    float widh;

    private void Start()
    {
        deltaTime = Time.deltaTime;

        sizeLim = groundTrans.lossyScale.x / 3.5556f;

        widhLim = Mathf.Abs(groundTrans.lossyScale.x / 2);
    }

    void Update()
    {
        accumulatedTime += deltaTime;
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
        if (Input.GetKey("left"))
            left = true;
        else
            left = false;
        if (Input.GetKey("right"))
            right = true;
        else
            right = false;
        if (Input.GetKey("up"))
            up = true;
        else
            up = false;
        if (Input.GetKey("down"))
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
                cmrsz++;
            }
            else if (down)
            {
                cmrsz--;
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
