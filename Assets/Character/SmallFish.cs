using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFish : Character
{
    public Vector3 LastVelocity;

    private void Start()
    {
        moveDir = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
        Move(moveDir, ForceMode2D.Impulse, 10);
    }

    private void Update()
    {
        LastVelocity = Rb2d.velocity;
    }
}
