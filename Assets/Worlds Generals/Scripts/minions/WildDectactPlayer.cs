using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class WildDectactPlayer : MonoBehaviour
{
    MinionWondering wildScript;
    [SerializeField] float runDelay;
    float rayLength;
    bool isRuning;
    // Start is called before the first frame update
    void Start()
    {
        wildScript = GetComponentInParent<MinionWondering>();
        rayLength = GetComponent<CircleCollider2D>().radius;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isRuning || collision.name != "Player")
            return;

        Vector2 posDif = transform.position - collision.transform.position;
        Vector3 rayDir = GetVector(Mathf.Rad2Deg * Mathf.Atan(posDif.y / -Mathf.Abs(posDif.x)));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, rayLength, ~256);
        //Debug.DrawRay(transform.position,rayDir*rayLength,Color.red);
        if (hit.collider == null || hit.collider.name != "Player")
            return;
        isRuning = true;
        wildScript.RunFrom(collision.transform);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name != "Player" || !gameObject.activeSelf)
            return;
        StartCoroutine(StopRunDelay());
    }
    IEnumerator StopRunDelay()
    {
        yield return new WaitForSeconds(runDelay);

        wildScript.StopRuning();
        isRuning = false;
    }
    Vector3 GetVector(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
