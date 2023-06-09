using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FSMState
{
    private Vector3 _destinationPosition;
    private float _currentRotationSpeed;
    private float _currentSpeed = 100.0f;

    public ChaseState()
    {
        StateID = FSMStateID.Chasing;
    }
    
    public override void CheckTransitionRules(Transform player, GameObject npc)
    {
        // Check health
        var controller = npc.GetComponent<NPCTankController>();
        if (controller.health < 50)
        {
            controller.SetTransition(Transition.Damaged);
            return;
        }
        // Check distance
        _destinationPosition = player.position;
        float distance = Vector3.Distance(npc.transform.position, _destinationPosition);
        switch (distance)
        {
            case <= 200.0f:
                controller.SetTransition(Transition.ReachPlayer);
                break;
            case >= 300.0f:
                controller.SetTransition(Transition.LostPlayer);
                break;
        }
    }

    public override void RunState(Transform player, GameObject npc)
    {
        _destinationPosition = player.position;
        Quaternion targetRotation = Quaternion.LookRotation(_destinationPosition - npc.transform.position);
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, targetRotation, Time.deltaTime * _currentRotationSpeed);
        npc.transform.Translate(Vector3.forward * (Time.deltaTime * _currentSpeed));
    }
}
