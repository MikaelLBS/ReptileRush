using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    [SerializeField] Rigidbody2D Rigidbody2D;

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


        if (right && !left)
        {
            //move right
        }
        if (!right  && left)
        {
            //left
        }
        if (up && !down)
        {
            //zoom out
        }
        if (!up && down)
        {
            //zoom in
        }
    }
}
