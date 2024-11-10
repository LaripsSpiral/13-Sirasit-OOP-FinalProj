using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFish : Character
{
    private void FixedUpdate()
    {
        Move(transform.right);
    }
}
