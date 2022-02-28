using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class RollerAgent : Agent
{
    public Transform target;
    Rigidbody rBody;


    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        //When the agent falls below the floor
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        //At the beginning of the episode, the target position is changed to random
        target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }
}
