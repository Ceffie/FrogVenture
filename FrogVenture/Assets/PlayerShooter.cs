using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject ObjTongue;
    public GameObject ShootPosition;

    public float TongueSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ShootTongue();
        }

        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void ShootTongue()
    {
        Instantiate(ObjTongue, ShootPosition.transform.position, ShootPosition.transform.rotation);
    }
}
