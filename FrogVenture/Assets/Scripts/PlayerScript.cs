using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody2D body;
    
    float vertical;

    public float runSpeed = 20.0f;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");

    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(0, vertical * runSpeed);
    }
}
