using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Character
{

    private void Start()
    {
        Move(Vector2.right, ForceMode2D.Impulse, .5f);
    }

    private void Update()
    {
        moveDir = GetMoveDir();
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
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
            Vector2 dir = Rb2d.velocity;
            float rotAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            //Flip Rotate Angle
            rotAngle -= dir.x < 0 ? -180 : 0;

            transform.rotation = Quaternion.AngleAxis(rotAngle, Vector3.forward);

            //Flip Character
            if (dir.x != 0)
            {
                Vector3 flipScale = transform.localScale;
                flipScale.x = dir.x < 0 ? -1 : 1;

                transform.localScale = flipScale;
            }
        }
    }
}
