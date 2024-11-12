using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Character : MonoBehaviour
{

    public float Speed = 10;

    [SerializeField] protected Vector2 moveDir;

    private Rigidbody2D _rb2d;
    public Rigidbody2D Rb2d
    {
        get => _rb2d;
        private set => _rb2d = value;
    }


    virtual protected void Awake()
    {
        Rb2d = GetComponent<Rigidbody2D>();
    }
     
    virtual protected void FixedUpdate()
    {
        RotateAlongVelocity();
    }


    protected void Move(Vector2 moveDir, ForceMode2D forceMode2D = ForceMode2D.Force, float multiplier = 1)
    {
        Rb2d.AddForce(Speed * multiplier * Time.fixedDeltaTime * moveDir, forceMode2D);
    }

    void RotateAlongVelocity()
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
