using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))] // dont need probably
public class SwapButtonPlayerUI : MonoBehaviour
{
    public PlayerUI playerUI;
    public float[] xSplitLines;
    public int index;
    public int startIndex;
    public string minionName;
    public GameObject minion;
    Image image;
    bool buttonDown;
    GameObject copyButton;
    private void Start()
    {
        startIndex = index;
        image = GetComponent<Image>();
        copyButton = Instantiate(gameObject);
        copyButton.transform.SetParent(transform, false);
        copyButton.GetComponent<Button>().interactable = false;
        copyButton.SetActive(false);
    }

    public void ButtonDown()
    {
        image.enabled = false;
        buttonDown = true;

        StopCoroutine(folw());
        StartCoroutine(folw());
    }
    IEnumerator folw()
    {
        copyButton.SetActive(true);
        Vector3 mousePos = Vector3.zero;
        while (buttonDown)
        {
            mousePos = Input.mousePosition;
            copyButton.transform.position = mousePos;
            if (index != xSplitLines.Length && mousePos.x > xSplitLines[xSplitLines.Length - 1])
                playerUI.UppdateButtonsPos(index, xSplitLines.Length);
            else
            {
                for (int i = 0; i < xSplitLines.Length; i++)
                {

                    if (mousePos.x < xSplitLines[i])
                    {
                        if (index != i)
                            playerUI.UppdateButtonsPos(index, i);
                        break;
                    }
                }
            }

            yield return null;
        }
        copyButton.SetActive(false);
    }
    public void ButtonUp()
    {
        image.enabled = true;
        buttonDown = false;
    }
    IEnumerator moveCoroutine;
    public void MoveTo(float xPoint, float speed)
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = Move(xPoint,speed);
        StartCoroutine(moveCoroutine);

    }
    float distance(float x, float x2)
    {
        return Mathf.Abs(x - x2);
    }
    IEnumerator Move(float xPoint,float speed)
    {
        if (xPoint < transform.position.x)
            speed *= -1;

        while (distance(xPoint, transform.position.x) >= Mathf.Abs(speed*2))
        {
            transform.position = new Vector2 (transform.position.x+speed, transform.position.y);
            yield return null;
        }
        transform.position = new Vector2(xPoint, transform.position.y);
    }
}
