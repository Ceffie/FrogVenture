using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueSlurp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Splat");
            Instantiate(collision.GetComponent<Sprite>(), transform.position, transform.rotation, transform);
            collision.gameObject.SetActive(false);
        }
    }
}
