using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

//Agent is reinforcement learning based
public class Move_To_Goal_Agent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, 0.0f, moveZ) * Time.deltaTime * moveSpeed;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            SetReward(1.0f);
            floorMeshRenderer.material = winMaterial;
        }
        else //if (other.tag == "Wall")
        {
            SetReward(-1.0f);
            floorMeshRenderer.material = loseMaterial;
        }
        EndEpisode();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
}
