using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int[,] board;

    // Start is called before the first frame update
    void Awake()
    {
        board = new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
    }

    public void playField(int x, int y, int player)
    {
        board[x, y] = player;
    }

    public void resetBoard()
    {
        board = new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
    }

    public int[,] getBoard()
    {
        return board;
    }

    public void printBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Debug.Log(board[i, j]);
            }
        }
    }
}
