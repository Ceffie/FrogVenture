using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueSlurp : MonoBehaviour
{
    public int BugOnTongueCounter = 0;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.gameObject.SetActive(false);
            BugOnTongueCounter++;
        }
    }
}
