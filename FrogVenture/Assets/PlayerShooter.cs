using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditorInternal.VersionControl.ListControl;
using static UnityEngine.UI.Image;

public class PlayerShooter : MonoBehaviour
{
    public GameObject ObjTongue;
    public GameObject ShootPosition;

    private float TongueSpeed = 5f;

    private int Bounces = 0;

    private Quaternion deflectRotation;

    private Vector3 deflectDirection;

    private Vector3 dir;

    void FixedUpdate()
    {
        if (Input.GetKey("space"))
        {
            ShootTongue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void ShootTongue()
    {
        Ray2D ray = new Ray2D(transform.position, transform.right);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

        // getting tongue tip into rotation and transformation
        

        if (Bounces == 0)
        {
            dir = (ObjTongue.transform.position - new Vector3(hit.point.x, hit.point.y, 0f)).normalized;

            ObjTongue.transform.position -= dir * TongueSpeed * Time.deltaTime;

            float distance = Vector3.Distance(ObjTongue.transform.position, new Vector3(hit.point.x, hit.point.y, ObjTongue.transform.position.z));
            if (distance < .1f)
            {
                Bounces++;
                dir = -deflectDirection;
                Debug.Log("BOING " + dir);
            }
        }

        if (Bounces == 1)
        {
            ObjTongue.transform.position -= dir * TongueSpeed * Time.deltaTime;
            Debug.Log("Schmovin " + dir);
            //float distance = Vector3.Distance(ObjTongue.transform.position, new Vector3(hit.point.x, hit.point.y, ObjTongue.transform.position.z));
            //Debug.Log(distance);
            //if (distance < .1f)
            //{
            //    Bounces++;
            //    Debug.Log("BOING");
            //}
        }

        if (hit.collider)
        {
            if (hit.collider.tag == "Wall")
            {
                // Get a rotation to go from our ray direction (negative, so coming from the wall),
                // to the normal of whatever surface we hit.
                deflectRotation = Quaternion.FromToRotation(-ray.direction, hit.normal);

                // We then take that rotation and apply it to the same normal vector to basically
                // mirror that angle difference.
                deflectDirection = deflectRotation * hit.normal;

                Debug.DrawRay(hit.point, deflectDirection *100, Color.magenta);
            }
        }
    }
}
