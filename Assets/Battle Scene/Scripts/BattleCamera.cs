using UnityEditor.Experimental.GraphView;
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

    void Update()
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


        if (right != left)
        {            
            if (right)
            {
                veloz.x = 1;
            }
            else if (left)
            {
                veloz.x = -1;
            }
            rb.velocity = veloz;
        }        
        if (right == left)
        {            
            rb.velocity = Vector2.zero;
        }
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
            cmr.orthographicSize = cmrsz;
        }
    }
}
