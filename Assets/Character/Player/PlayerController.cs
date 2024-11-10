using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Character
{
    [SerializeField] Vector2 moveDir;

    [SerializeField] float rotationSpeed = 5;

    private void Update()
    {
        moveDir = GetMoveDir();
        RotateAlongVelocity(moveDir);
    }

    private void FixedUpdate()
    {
        Move(moveDir);
    }

    Vector2 GetMoveDir()
    {
        Vector2 moveDir;
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.y = Input.GetAxis("Vertical");

        return moveDir;
    }

    void RotateAlongVelocity(Vector2 moveDir)
    {
        if (moveDir != Vector2.zero)
        {
            //Get Rotate Angle
            Vector2 dir = rb2d.velocity;
            float rotAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            //Flip Rotate Angle
            rotAngle -= dir.x < 0 ? -180 : 0;

            transform.rotation = Quaternion.AngleAxis(rotAngle, Vector3.forward);

            //Flip Character
            if (dir.x != 0)
            {
                Flip(dir.x < 0 ? -1 : 1);
            }
        }
    }
}
