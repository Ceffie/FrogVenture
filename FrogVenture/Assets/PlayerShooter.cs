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

    private float TongueSpeed = 7f;

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

    private RaycastHit2D firstHit;

    private float bounceDistance;

    private bool rayIsSet = false;
    private Ray2D ray;

    private float wallDist = .2f;

    private void ShootTongue()
    {
        if (!rayIsSet)
        {
            rayIsSet = true;
            ray = new Ray2D(ObjTongue.transform.position, transform.right);
            firstHit = Physics2D.Raycast(ObjTongue.transform.position, transform.right);
        }

        if (Bounces % 2 == 0)
        {
            //Debug.Log("PREBOUNCE: " + firstHit.point);
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
            firstHit = TongueBouncing(firstHit);
        } else if (Bounces % 2 == 1)
        {
            //Debug.Log("Bounced Baby: " + firstHit.point);
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);
            firstHit = TongueBouncing(firstHit);
        }
    }

    private RaycastHit2D TongueBouncing(RaycastHit2D pHit)
    {
        dir = (ObjTongue.transform.position - new Vector3(pHit.point.x, pHit.point.y, 0f)).normalized;

        ObjTongue.transform.position -= dir * TongueSpeed * Time.deltaTime;

        bounceDistance = Vector3.Distance(ObjTongue.transform.position, new Vector3(pHit.point.x, pHit.point.y, ObjTongue.transform.position.z));

        if (bounceDistance < wallDist)
        {

            //CheckCollider pHit and edit nHit accordingly
            CheckColliderPoint(pHit);
            dir = (deflectDirection).normalized;
            // Debug.Log("objtongue pos: " + ObjTongue.transform.position + " deflDir: " +  deflectDirection + " dir: " + dir);
            var nHit = Physics2D.Raycast(ObjTongue.transform.position, dir);

            //create new ray
            ray = new Ray2D(ObjTongue.transform.position, deflectDirection);

            //rot tonguye
            ObjTongue.transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: nHit.point); // AAAA DOET HET NIET

            ////check with logs
            //Debug.Log("deflectRot: " + deflectRotation);
            //Debug.Log("BOING " + Bounces + dir);
            //Debug.Log("dabaunce pHit: " + pHit.point + "nHit" + nHit.point);

            //finish off checks and returns
            Bounces++;
            return nHit;
        }

        return pHit;
    }

    void CheckColliderPoint(RaycastHit2D hit)
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
