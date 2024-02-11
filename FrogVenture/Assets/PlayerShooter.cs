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

    // get the tongue timer working
    public bool TongueShooting = false;
    public bool TongueRetreating = false;
    public bool YourTurn = true;
    private float TongueExtensionTime = 5f; // bepaalt de lengte/tijd onscreen van de tong
    private float TongueTimer = 0f; // de timer om naar de TongueTime te gaan

    void FixedUpdate()
    {
        if (Input.GetButton("Jump") && YourTurn)
        {
            Debug.Log("Tongue GO!");
            TongueShooting = true;   
            YourTurn = false;
        }

        if (TongueShooting)
        {
            TongueTimer += Time.deltaTime; 
            ShootTongue();
            if (TongueTimer > TongueExtensionTime)
            {
                TongueTimer = 0f;
                TongueShooting = false;
                TongueRetreating = true;
            }
        }

        if (TongueRetreating)
        {
            // retreat that tongue somehow
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!TongueShooting && !TongueRetreating)
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
    }

    private int Bounces = 0;

    private Quaternion deflectRotation;

    private Vector3 deflectDirection;

    private Vector3 dir;

    private RaycastHit2D previousHit;
    private Vector2 previousPoint;
    private RaycastHit2D nextHit;
    private Vector2 nextPoint;
    private RaycastHit2D firstHit;

    private float bounceDistance;

    private bool rayIsSet = false;
    private Ray2D ray;

    private void ShootTongue()
    {
        if (!rayIsSet)
        {
            ray = new Ray2D(transform.position, transform.right);
            rayIsSet = true;
            firstHit = Physics2D.Raycast(transform.position, transform.right);
        }

        

        if (Bounces % 2 == 0)
        {
            firstHit = TongueBouncing(firstHit);
            Debug.Log("PREBOUNCE: " + firstHit.point);

        } else if (Bounces % 2 == 1)
        {
            Debug.Log("Bounced Baby: " + firstHit.point);
        }

        //if (Bounces % 2 == 0)
        //{
        //    //    ObjTongue.transform.rotation = deflectRotation;
        //    if (!previousHit)
        //    {
        //        previousHit = firstHit;
        //        previousPoint = firstHit.point;
        //        CheckColliderPoint(firstHit);
        //    }

        //    dir = (ObjTongue.transform.position - new Vector3(previousPoint.x, previousPoint.y, 0f)).normalized;

        //    ObjTongue.transform.position = dir * TongueSpeed * Time.deltaTime;

        //    bounceDistance = Vector3.Distance(ObjTongue.transform.position, new Vector3(previousHit.point.x, previousHit.point.y, ObjTongue.transform.position.z));

        //    Debug.DrawRay(ObjTongue.transform.position, dir *1000, Color.red);

        //    if (bounceDistance < .1f)
        //    {
        //        Bounces++;
        //        nextHit = Physics2D.Raycast(previousPoint, transform.right);
        //        CheckColliderPoint(nextHit);
        //        dir = -deflectDirection;
        //        Debug.Log("BOING " + Bounces + dir);
        //    }
        //} else if (Bounces % 2 == 1)
        //{
        //    //    ObjTongue.transform.rotation = deflectRotation;
        //    dir = (ObjTongue.transform.position - new Vector3(nextHit.point.x, nextHit.point.y, 0f)).normalized;

        //    ObjTongue.transform.position = dir * TongueSpeed * Time.deltaTime;

        //    bounceDistance = Vector3.Distance(ObjTongue.transform.position, new Vector3(nextHit.point.x, nextHit.point.y, ObjTongue.transform.position.z));

        //    Debug.DrawRay(ObjTongue.transform.position, dir * 1000, Color.red);

        //    if (bounceDistance < .1f)
        //    {
        //        Bounces++;
        //        previousHit = Physics2D.Raycast(nextPoint, transform.right);
        //        CheckColliderPoint(previousHit);
        //        dir = -deflectDirection;
        //        Debug.Log("BOING " + Bounces + dir);
        //    }
        //}

        //if (Bounces == 1)
        //{
        //    ObjTongue.transform.position -= dir * TongueSpeed * Time.deltaTime;
        //}

    }

    private RaycastHit2D TongueBouncing(RaycastHit2D pHit)
    {
        dir = (ObjTongue.transform.position - new Vector3(pHit.point.x, pHit.point.y, 0f)).normalized;

        ObjTongue.transform.position -= dir * TongueSpeed * Time.deltaTime;

        bounceDistance = Vector3.Distance(ObjTongue.transform.position, new Vector3(pHit.point.x, pHit.point.y, 0f));

        Debug.DrawRay(ObjTongue.transform.position, dir * 1000, Color.red);
        
        if (bounceDistance < .1f)
        {
            var nHit = Physics2D.Raycast(pHit.point, transform.right);
            CheckColliderPoint(nHit);
            ObjTongue.transform.rotation = deflectRotation;
            ray = new Ray2D(ObjTongue.transform.position, transform.right);
            dir = -deflectDirection;
            Debug.Log("BOING " + Bounces + dir);
            Bounces++;
            Debug.Log("dabaunce pHit: " + pHit.point + "nHit" + nHit.point);
            return nHit;
        }

        return pHit;
    }

    void CheckColliderPoint(RaycastHit2D hit)
    {
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
            }
        }
    }
}
