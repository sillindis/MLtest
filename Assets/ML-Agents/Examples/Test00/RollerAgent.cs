using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class RollerAgent : Agent
{

    Rigidbody rBody;

    public Transform target;
    public float forceMultiplier = 10;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    //When the new episode starts, the agent's position is reset again.
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

    //Delivering observation information to the Machine learning program
    public override void CollectObservations(VectorSensor sensor)
    {
        //Deliver Target and Agent position
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        //Deliver the current agent's velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);

    }

    //
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //Move agent for learning.
        //Actions size=2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        rBody.AddForce(controlSignal * forceMultiplier);

        //Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);

        if (distanceToTarget < 1.42f) //When finding a target, reward it and end the episode.
        {
            SetReward(1.0f);
            EndEpisode();
        }
        else if (this.transform.localPosition.y < 0) //If it falls below the floor, it ends learning.
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continousActionsOut = actionsOut.ContinuousActions;
        continousActionsOut[0] = Input.GetAxis("Horizontal");
        continousActionsOut[1] = Input.GetAxis("Vertical");
    }

}
