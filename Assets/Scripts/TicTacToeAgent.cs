using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Net.Sockets;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class TicTacToeAgent : Agent
{
    public GameController gameController;

    public BoardManager boardManager;

    public int player;

    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (var field in gameController.Fields)
        {
            sensor
                .AddOneHotObservation(field
                    .GetComponent<FieldManager>()
                    .getField(player),
                3);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        gameController.playField(actionBuffers.DiscreteActions[0], player);
    }

    public void Win()
    {
        AddReward(0.8f);
        //EndEpisode();
    }

    public void Lose()
    {
        AddReward(-1.0f);
        //EndEpisode();
    }

    public void Draw(bool isLast)
    {
        if (isLast)
        {
            AddReward(0.8f);
        }
        else
        {
            AddReward(0.8f);
        }
        //EndEpisode();
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        int[] playedFields = (int[]) gameController.GetOccupiedFields();
        foreach (var field in playedFields)
        {
            actionMask.SetActionEnabled(0, field, false);
        }
    }
}
