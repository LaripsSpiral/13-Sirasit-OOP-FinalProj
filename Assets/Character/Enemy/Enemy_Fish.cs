using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Fish : Character
{
    protected abstract void OnCollisionEnter2D(Collision2D col);
    protected abstract void OnTriggerEnter2D(Collider2D col);
}
