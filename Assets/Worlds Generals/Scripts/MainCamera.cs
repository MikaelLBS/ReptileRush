using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x,player.position.y,-10);
    }
}
