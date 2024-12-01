using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBoat : Enemy
{
    [SerializeField] List<FishingBait> fishingBaits;

    [SerializeField] Player _target;

    public Player Target 
    {
        get => _target;
        private set => _target = value; 
    }


    void Update()
    {
        Behavior();
    }

    protected override void Behavior()
    {
        //Find
        Target = FindAnyObjectByType<Player>();

        //Not Exist
        if (_target == null)
            return;

        //Do
        MoveTowardPlayer(_target);
    }

    private void MoveTowardPlayer(Player target)
    {
        Vector2 moveTargetPos = new(target.transform.position.x, transform.position.y);
        Vector2 lerpTargetPos = Vector2.Lerp(transform.position, moveTargetPos, Speed * Time.deltaTime);

        moveDir = Vector2.MoveTowards(transform.position, moveTargetPos, float.MaxValue);
        moveDir.y = 0;

        Rb2d.velocity = moveDir;

        Rb2d.MovePosition(lerpTargetPos);
    }

    public void HookUp(FishingBait hookedBait, Player player)
    {
        player.Rb2d.velocity = default;
        player.Rb2d.isKinematic = true;

        player.Mouth.parent = transform;
        player.Mouth.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 0));

        player.transform.parent = player.Mouth;
        player.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 90));

        StartCoroutine(hookedBait.AnimHookup());
    }

}
