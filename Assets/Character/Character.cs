using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Character : MonoBehaviour
{
    public float Speed = 10;
    protected Rigidbody2D rb2d;

    virtual protected void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    virtual public void Move(Vector2 moveDir)
    {
        rb2d.AddForce(Speed * Time.fixedDeltaTime * moveDir);
    }

    public void Flip(int xScale = 0)
    {
        Vector3 flipScale = transform.localScale;

        if (xScale == 0)
            flipScale.x *= -1;
        else
            flipScale.x = xScale;

        transform.localScale = flipScale;
    }
}
