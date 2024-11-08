using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float Speed = 10;

    Rigidbody2D _rb2d;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Swim();
    }

    void Swim()
    {
        _rb2d.AddForce(transform.right * Speed * Time.fixedDeltaTime);
    }

    public void Flip(int xDir = 90)
    {
        transform.Rotate(0, xDir == 90 ? 180 : xDir, 0);
    }
}
