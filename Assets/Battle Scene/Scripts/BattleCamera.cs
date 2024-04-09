using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Camera cmr;
    [SerializeField] float cmrsz = 5;

    Vector2 veloz = new Vector2(0, 0);
    bool left = false;
    bool right = false;
    bool up = false;
    bool down = false;
    float deltaTime;
    float accumulatedTime = 0f;
    float zoomInterval = 1f; // 50 milliseconds

    private void Start()
    {
        deltaTime = Time.deltaTime;
    }

    void Update()
    {
        accumulatedTime += deltaTime;

        GetInput();
        Move();

        while (accumulatedTime >= zoomInterval) { 
            Zoom();
            accumulatedTime -= zoomInterval;
        }
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
    }
}
