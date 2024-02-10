using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TongueMover : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Just for debugging, adds some velocity during OnEnable")]
    public Vector2 initialVelocity;

    [SerializeField]
    private float minVelocity = 10f;

    private Vector3 lastFrameVelocity;
    
    public Rigidbody2D rb;

    private Vector2 direction;
    
    private float shootSpeed = 15f;
    void Awake()
    {
        // getting your components
        direction = -transform.right;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialVelocity;
    }

    void Start()
    {
        // initiliaze your components
        rb.velocity = direction * shootSpeed;
        rb.angularVelocity = 0;
        rb.gravityScale = 0;
    }

    //private void OnEnable()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    rb.velocity = initialVelocity;
    //}

    void Update()
    {
        //transform.position -= transform.right * Time.deltaTime * shootSpeed;

        lastFrameVelocity = rb.velocity;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Bounce(collision.contacts[0].normal);
    }

    private void Bounce(Vector2 collisionNormal)
    {
        var speed = lastFrameVelocity.magnitude;
        var direction = Vector2.Reflect(lastFrameVelocity.normalized, collisionNormal);

        Debug.Log("Out Direction: " + direction);
        rb.velocity = direction * Mathf.Max(speed, minVelocity);
    }
}
