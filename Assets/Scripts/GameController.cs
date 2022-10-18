using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using UnityEngine;
using Unity.MLAgents.Actuators;
using miniMax;

public class GameController : MonoBehaviour
{
    public List<GameObject> Fields = new List<GameObject>();

    public bool isPlayerOneTurn = true;

    public bool humanplayer = false;

    public BoardManager board;

    public TicTacToeAgent

            playerX,
            playerO;

    MiniMax miniMax;

    // Start is called before the first frame update
    void Start()
    {
        board = GetComponentInChildren<BoardManager>();
        foreach (Transform t in transform.GetChild(1))
        {
            Fields.Add(t.gameObject);
        }
        initializeField();

        //setupField();
        miniMax = new MiniMax();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOneTurn)
        {
            if (humanplayer)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.gameObject.tag == "Field")
                        {
                            if (
                                !hit
                                    .transform
                                    .gameObject
                                    .GetComponent<FieldManager>()
                                    .isPlayed()
                            )
                            {
                                hit
                                    .transform
                                    .gameObject
                                    .GetComponent<FieldManager>()
                                    .setField("X");
                                board
                                    .board[hit
                                        .transform
                                        .gameObject
                                        .GetComponent<FieldManager>()
                                        .row,
                                    hit
                                        .transform
                                        .gameObject
                                        .GetComponent<FieldManager>()
                                        .column] = 1;
                                isPlayerOneTurn = false;
                            }
                        }
                    }
                }
            }
            else
            {
                playerX.RequestDecision();
                isPlayerOneTurn = false;
            }
            if (checkWinner() == 1)
            {
                Debug.Log("Player X wins");
                playerX.Win();
                playerO.Lose();
                EndEpisodes();
                initializeField();
                isPlayerOneTurn = false;
                return;
            }
            if (checkWinner() == 2)
            {
                Debug.Log("Player O wins");
                playerX.Lose();
                playerO.Win();
                EndEpisodes();
                initializeField();
                isPlayerOneTurn = true;
                return;
            }
            if (checkWinner() == 3)
            {
                Debug.Log("Draw");
                if (isPlayerOneTurn)
                {
                    playerX.Draw(true);
                    playerO.Draw(false);
                    isPlayerOneTurn = true;
                }
                else if (!isPlayerOneTurn)
                {
                    playerX.Draw(false);
                    playerO.Draw(true);
                    isPlayerOneTurn = false;
                }
                EndEpisodes();
                initializeField();
                return;
            }
        }
        else
        {
            var bestMove = miniMax.findBestMove(board.getBoard());

            //var bestMove = miniMax.findBestMove(board.getBoard());
            Debug.Log(bestMove.row + " " + bestMove.col);
            playField(((bestMove.row * 3) + bestMove.col), 2); // 2 = O
            playerO.RequestDecision();
            isPlayerOneTurn = true;
        }
        if (checkWinner() == 1)
        {
            Debug.Log("Player X wins");
            playerX.Win();
            playerO.Lose();
            EndEpisodes();
            initializeField();
            isPlayerOneTurn = false;
            return;
        }
        if (checkWinner() == 2)
        {
            Debug.Log("Player O wins");
            playerX.Lose();
            playerO.Win();
            EndEpisodes();
            initializeField();
            isPlayerOneTurn = true;
            return;
        }
        if (checkWinner() == 3)
        {
            Debug.Log("Draw");
            if (isPlayerOneTurn)
            {
                playerX.Draw(true);
                playerO.Draw(false);
                isPlayerOneTurn = true;
            }
            else if (!isPlayerOneTurn)
            {
                playerX.Draw(false);
                playerO.Draw(true);
                isPlayerOneTurn = false;
            }
            EndEpisodes();
            initializeField();
            return;
        }
    }

    public void initializeField()
    {
        foreach (GameObject f in Fields)
        {
            f.GetComponent<FieldManager>().clearField();
        }
        board.resetBoard();
        //Debug.Log("Start Game");
    }

    public IEnumerable<int> GetAvailableFields()
    {
        List<int> availableFields = new List<int>();

        for (int i = 0; i < Fields.Count; i++)
        {
            if (!Fields[i].GetComponent<FieldManager>().isPlayed())
                availableFields.Add(i);
        }

        return availableFields.ToArray();
    }

    public IEnumerable<int> GetOccupiedFields()
    {
        List<int> impossibleFields = new List<int>();

        for (int i = 0; i < Fields.Count; i++)
        {
            if (Fields[i].GetComponent<FieldManager>().isPlayed())
                impossibleFields.Add(i);
        }
        return impossibleFields.ToArray();
    }

    public int checkWinner()
    {
        // check rows
        for (int i = 0; i < 3; i++)
        {
            if (
                board.board[i, 0] == board.board[i, 1] &&
                board.board[i, 1] == board.board[i, 2]
            )
            {
                if (board.board[i, 0] == 1)
                {
                    //Debug.Log("Player 1 Wins");
                    return 1;
                }
                else if (board.board[i, 0] == 2)
                {
                    //Debug.Log("Player 2 Wins");
                    return 2;
                }
            }
        }

        // check columns
        for (int i = 0; i < 3; i++)
        {
            if (
                board.board[0, i] == board.board[1, i] &&
                board.board[1, i] == board.board[2, i]
            )
            {
                if (board.board[0, i] == 1)
                {
                    //Debug.Log("Player 1 Wins");
                    return 1;
                }
                else if (board.board[0, i] == 2)
                {
                    //Debug.Log("Player 2 Wins");
                    return 2;
                }
            }
        }

        // check diagonals
        if (
            board.board[0, 0] == board.board[1, 1] &&
            board.board[1, 1] == board.board[2, 2]
        )
        {
            if (board.board[0, 0] == 1)
            {
                //Debug.Log("Player 1 Wins");
                return 1;
            }
            else if (board.board[0, 0] == 2)
            {
                //Debug.Log("Player 2 Wins");
                return 2;
            }
        }
        if (
            board.board[0, 2] == board.board[1, 1] &&
            board.board[1, 1] == board.board[2, 0]
        )
        {
            if (board.board[0, 2] == 1)
            {
                //Debug.Log("Player 1 Wins");
                return 1;
            }
            else if (board.board[0, 2] == 2)
            {
                //Debug.Log("Player 2 Wins");
                return 2;
            }
        }
        if (GetAvailableFields().Count() == 0)
        {
            //Debug.Log("Draw");
            return 3;
        }
        return 0;
    }

    public void playField(int fieldnumber, int player)
    {
        Fields[fieldnumber]
            .GetComponent<FieldManager>()
            .setField(player == 1 ? "X" : "O");
        board
            .board[Fields[fieldnumber].GetComponent<FieldManager>().row,
            Fields[fieldnumber].GetComponent<FieldManager>().column] = player;

        // if (player == 1)
        // {
        //     Debug.Log("PlayerX, answer: " + fieldnumber);
        //     Debug.Log("Set Field " + fieldnumber + " to X");
        // }
        // else
        // {
        //     //Debug.Log("PlayerO, answer: " + fieldnumber);
        //     //Debug.Log("Set Field " + fieldnumber + " to O");
        // }
        // if (checkWinner() == 1)
        // {
        //     //Debug.Log("Check Winner() = 1)");
        //     //initializeField();
        // }
        // else if (checkWinner() == 2)
        // {
        //     playerX.Lose();
        //     playerO.Win();

        //     //Debug.Log("CheckWinner() =  2");
        //     //initializeField();
        // }
        // else if (checkWinner() == 3)
        // {
        //     playerX.Draw(true);
        //     playerO.Draw(false);

        //     //Debug.Log("CheckWinner() = 3");
        //     //initializeField();
        // }
        // else
        //     return;
        // if (!Fields[fieldnumber].GetComponent<FieldManager>().isPlayed())
        // {
        //     Fields[fieldnumber]
        //         .GetComponent<FieldManager>()
        //         .setField(player == 1 ? "X" : "O");
        //     board
        //         .board[Fields[fieldnumber].GetComponent<FieldManager>().row,
        //         Fields[fieldnumber].GetComponent<FieldManager>().column] =
        //         player;
        //     Debug.Log("Updated board");

        //     if (player == 1)
        //     {
        //         Debug.Log("PlayerX, answer: " + fieldnumber);
        //         Debug.Log("Set Field " + fieldnumber + " to " + player);
        //         if (checkWinner() == 1)
        //         {
        //             playerX.Win();
        //             playerO.Lose();
        //             initializeField();
        //         }
        //         else if (checkWinner() == 3)
        //         {
        //             playerX.Draw(true);
        //             playerO.Draw(false);
        //             initializeField();
        //         }
        //         else
        //         {
        //             playerO.RequestDecision();
        //             Debug.Log("Player O, request decision");
        //         }
        //     }
        //     else
        //     {
        //         Debug.Log("PlayerO, answer: " + fieldnumber);
        //         if (checkWinner() == 2)
        //         {
        //             playerX.Lose();
        //             playerO.Win();
        //             initializeField();
        //         }
        //         else if (checkWinner() == 3)
        //         {
        //             playerX.Draw(false);
        //             playerO.Draw(true);
        //             initializeField();
        //         }
        //         else
        //         {
        //             playerX.RequestDecision();
        //             Debug.Log("Player X, request decision");
        //         }
        //     }
        // }
        // else
        // {
        //     if (player == 1)
        //     {
        //         Debug.Log("PlayerX, field " + fieldnumber + " already taken");
        //         playerX.RequestDecision();
        //         playerX.RequestAction();
        //     }
        //     else
        //     {
        //         Debug.Log("PlayerO, field " + fieldnumber + " already taken");

        //         playerO.RequestDecision();
        //         playerO.RequestAction();
        //     }
        // }
    }

    public void EndEpisodes()
    {
        playerX.EndEpisode();
        playerO.EndEpisode();
    }
}
