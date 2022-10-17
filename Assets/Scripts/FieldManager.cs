using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FieldState
{
    non = 0,
    X = 1,
    O = 2
}

public class FieldManager : MonoBehaviour
{
    public int

            row,
            column,
            index;

    public bool isplayed = false;

    public FieldState currentState = FieldState.non;

    public void clearField()
    {
        GameObject child_O = gameObject.transform.GetChild(0).gameObject;
        child_O.SetActive(false);
        GameObject child_X = gameObject.transform.GetChild(1).gameObject;
        child_X.SetActive(false);
        isplayed = false;
    }

    public void setField(String s)
    {
        if (s == "O")
        {
            GameObject child_O = transform.GetChild(0).gameObject;
            child_O.SetActive(true);
            isplayed = true;
            currentState = FieldState.O;
        }
        else if (s == "X")
        {
            GameObject child_X = transform.GetChild(1).gameObject;
            child_X.SetActive(true);
            isplayed = true;
            currentState = FieldState.X;
        }
    }

    public int getField(int player)
    {
        if (player == 1)
        {
            if (currentState == FieldState.X) return 1;
            if (currentState == FieldState.O) return 2;
        }
        if (player == 2)
        {
            if (currentState == FieldState.X) return 2;
            if (currentState == FieldState.O) return 1;
        }
        return 0;
    }

    public bool isPlayed()
    {
        return isplayed;
    }

    public void setRowColumn(int row, int column)
    {
        this.row = row;
        this.column = column;
    }
}
