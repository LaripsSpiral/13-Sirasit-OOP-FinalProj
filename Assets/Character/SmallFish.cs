using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFish : Character
{
    private void Start()
    {
        moveDir = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
        Move(moveDir, ForceMode2D.Impulse, 10);
    }
}
