using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class FishingBait : MonoBehaviour, IEatable
{
    public FishingBoat OwnedBoat;

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

    void FixedUpdate()
    {
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

        OwnedBoat.HookUp(this, player);
    }

    public IEnumerator AnimHookup()
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

