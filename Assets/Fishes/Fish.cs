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
        SwimForward();
    }

    private void SwimForward()
    {
        _rb2d.AddForce(Speed * Time.fixedDeltaTime * transform.right);
    }

    public void Flip(int xDir = 90)
    {
        transform.Rotate(0, xDir == 90 ? 180 : xDir, 0);
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}
