using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class camera_jump : MonoBehaviour
{
    bool InvertGravity = false;
    float gravity = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKey(KeyCode.Space))       
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            //rb.velocity = new Vector3(0f, 1f);
            rb.gravityScale = gravity;
            if (!InvertGravity)
            {
                InvertGravity = true;
                gravity = -1.0f;
            }

            else
            {
                InvertGravity= false;
                gravity = 1.0f;
            }           
        }
    }
}
