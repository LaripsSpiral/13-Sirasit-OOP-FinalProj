using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class FishingBait : Enemy_Fish, IEatable
{
    [Header("Settings")]
    [SerializeField] float _minRange, _maxRange;


    Vector3 spawnPos;
    float hookRange;

    LineRenderer _lineRenderer;

    private void Start()
    {
        spawnPos = transform.position;

        hookRange = Random.Range(_minRange, _maxRange);
        transform.position += Vector3.down * hookRange;

        _lineRenderer = GetComponent<LineRenderer>();
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();

        //Hook line
        _lineRenderer.SetPosition(0, spawnPos + Vector3.back);
        _lineRenderer.SetPosition(1, transform.position + Vector3.back);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(!col.gameObject.TryGetComponent(out Player player))
            return;

        EatenBy(player);
    }

    public void EatenBy(Player player)
    {
        player.TakeDamage(1);

        if (player.Heart > 0)
            return;

        PlayerDeath(player);
    }
    protected override void PlayerDeath(Player player)
    {
        player.Rb2d.velocity = default;
        player.Rb2d.isKinematic = true;

        player.Mouth.parent = transform;
        player.Mouth.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 0));

        player.transform.parent = player.Mouth;
        player.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 90));

        StartCoroutine(AnimHookup());
    }

    IEnumerator AnimHookup()
    {
        while (transform.position.y < spawnPos.y)
        {
            transform.position = Vector3.Lerp(transform.position, spawnPos, hookRange * Time.deltaTime * .05f);

            yield return new WaitForEndOfFrame();
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + Vector3.down * _minRange, transform.position + Vector3.down * _maxRange);
    }
}

