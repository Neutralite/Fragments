using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShadowsGame : GameBase
{
    public GameObject tokenButton;
    public TextMeshProUGUI turnUI;
    public TextMeshPro winUI; 
    public TicTacToeSpace[] Spaces = new TicTacToeSpace[9];
    
    public int[] boardRepresentation = new int[9];
    public int[,,,,,,,,] stateScores = new int[3, 3, 3,
                                               3, 3, 3, 
                                               3, 3, 3];
    public List<int> availableMoves = new List<int>();

    public bool playerIsX = true, xTurn, P1Turn, P1TurnStarts, AITurnStarts, gameFinished;
    public string playerToken= "Chocolates", AIToken = "Marshmallows";
    public int playerTokenNum = 1, AITokenNum = 2, alpha, beta;
    
    // toggles which player is X, represented by chocolates
    // the player is x by default
    public void ToggleToken()
    {
        // if you want to change x to marshmallows,
        // you need to change the names here, and
        // switch what token shows up in PlaceToken()
        if (playerIsX)
        {
            playerToken = "Marshmallows";
            AIToken = "Chocolates";
        }
        else
        {
            playerToken = "Chocolates";
            AIToken = "Marshmallows";
        }
        tokenButton.GetComponentInChildren<TextMeshProUGUI>().text = "Token:\n" + playerToken;
        playerIsX = !playerIsX;
        // no rotating the board mid game
        Setup();
    }
    void Start()
    {
        tokenButton.GetComponentInChildren<TextMeshProUGUI>().text = "Token:\n" + playerToken;
        Setup();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();

        if (!paused && !P1Turn && AITurnStarts)
        {
            AITurnStarts = false;
            Invoke(nameof(AITurn), 1f);
            P1TurnStarts = true;
        }
    }
    void Setup()
    {
        // reset everything
        gameFinished = false;
        winUI.text = ")";
        availableMoves.Clear();
        for (int i = 0; i < 9; i++)
        {
            availableMoves.Add(i);
            Spaces[i].GetComponentInChildren<TicTacToeSpace>().button.enabled = true;
            Spaces[i].GetComponentInChildren<TicTacToeSpace>().chocolate.SetActive(false);
            Spaces[i].GetComponentInChildren<TicTacToeSpace>().marshmallow.SetActive(false);
            boardRepresentation[i] = 0;
        }

        // determine if x goes first, and thus, if the player or AI goes first
        xTurn = Random.value > 0.5f;
        if (playerIsX && xTurn || !playerIsX && !xTurn)
        {
            P1Turn = true;
        } else
        {
            P1Turn = false;
            
        }
        P1TurnStarts = P1Turn;
        AITurnStarts = !P1Turn;
        UpdateTurnUI();
    }
    void UpdateTurnUI()
    {
        if (P1Turn)
        {
            turnUI.text = playerToken + "' Turn";
        }
        else
        {
            turnUI.text = AIToken+ "' Turn";
        }
    }
    public void PlayerTurn(int move)
    {
        
        if (P1Turn && P1TurnStarts)
        {
            P1TurnStarts = false;
            PlaceToken(move, playerTokenNum);
            AITurnStarts = true;
        }
    }
    void AITurn()
    {
        int bestMove = availableMoves[0];
        int secondBest = availableMoves[0];
        int bestScore = -1000;
        alpha = -1000;
        beta = 1000;
        for (int x = 0; x < availableMoves.Count; x++)
        {
            int move = availableMoves[x];
            int score = minimax(move, 0, alpha, beta, false);
            //Debug.Log(move + ": " + score);
            if (score > bestScore)
            {
                bestScore = score;
                secondBest = bestMove;
                bestMove = move;
            }

            if (score == bestScore)
            {
                if (Random.value < 0.3f)
                {
                    Debug.Log("trigger");
                    bestMove = move;
                }
            }
        }

        // making the AI less unbeatable by having it sometimes not pick the best move
        if (Random.Range(0,10) < 2 && availableMoves.Count>2)
        {
            PlaceToken(secondBest, AITokenNum);
        } else
        {
            PlaceToken(bestMove, AITokenNum);
        }
    }
    public void PlaceToken(int move, int token)
    {
        // disable placing a token on given space
        availableMoves.Remove(move);
        Spaces[move].GetComponentInChildren<TicTacToeSpace>().button.enabled = false;
        // show appropriate token
        if (xTurn)
        {
            Spaces[move].GetComponentInChildren<TicTacToeSpace>().chocolate.SetActive(true);
        }
        else
        {
            Spaces[move].GetComponentInChildren<TicTacToeSpace>().marshmallow.SetActive(true);
        }
        // update record of current state of the game
        boardRepresentation[move] = token;
        // check to see if move was a winning one        
        int result = CheckWin(boardRepresentation);
        if (gameFinished)
        {
            WinHandling(result);
        }
        else
        {
            xTurn = !xTurn;
            P1Turn = !P1Turn;                
            UpdateTurnUI();
        }
    }
    int minimax(int move, int depth, int a, int b, bool maximizing)
    {
        // play simulated move
        int moveIndex = availableMoves.IndexOf(move);
        availableMoves.Remove(move);
        if (!maximizing)
        {
            boardRepresentation[move] = AITokenNum;
        }
        else
        {
            boardRepresentation[move] = playerTokenNum;
        }
        //see if the current node is a terminal node
        int result = CheckWin(boardRepresentation);
        // if the terminal node has been reached, end recursion of minimax function 
        if (gameFinished)
        {
            gameFinished = false;
        }
        else // evaluate the next node to evaluate current node
        {
            if (maximizing)
            {
                result = -1000;
                // for each available move, evaluate using minimax until the best score associated with the best move for AI is found at current depth
                for (int x = 0; x < availableMoves.Count; x++)
                {
                    int score = minimax(availableMoves[x], depth + 1, alpha, beta, false);
                    if (score > result)
                        result = score;
                }
            }
            else
            {
                result = 1000;
                // for each available move, evaluate using minimax until the worst score associated with the best move for the player is found at current depth
                for (int x = 0; x < availableMoves.Count; x++)
                {
                    int score = minimax(availableMoves[x], depth + 1, alpha, beta, true);
                    if (score < result)
                        result = score;
                }
            }
        }
        // undo simulated move, return its score
        boardRepresentation[move] = 0;
        availableMoves.Insert(moveIndex,move);
        return result;
    }
    int CheckWin(int[] state)
    {
        int result = 0;

        // check for player win
        if ((state[0] == playerTokenNum && state[1] == playerTokenNum && state[2] == playerTokenNum) ||
            (state[3] == playerTokenNum && state[4] == playerTokenNum && state[5] == playerTokenNum) ||
            (state[6] == playerTokenNum && state[7] == playerTokenNum && state[8] == playerTokenNum) ||
            (state[0] == playerTokenNum && state[3] == playerTokenNum && state[6] == playerTokenNum) ||
            (state[1] == playerTokenNum && state[4] == playerTokenNum && state[7] == playerTokenNum) ||
            (state[2] == playerTokenNum && state[5] == playerTokenNum && state[8] == playerTokenNum) ||
            (state[0] == playerTokenNum && state[4] == playerTokenNum && state[8] == playerTokenNum) ||
            (state[2] == playerTokenNum && state[4] == playerTokenNum && state[6] == playerTokenNum))
        {
            gameFinished = true;
            // +1 to state score for win
            result++;
            // and for every remaining empty space
            for (int x = 0; x < 9; x++)
            {
                if (state[x] == 0)
                {
                    result++;
                }
            }
            // times -1 for player win/AI loss
            result *= -1;
            return result;
        }

        // check for AI win
        if ((state[0] == AITokenNum && state[1] == AITokenNum && state[2] == AITokenNum) ||
            (state[3] == AITokenNum && state[4] == AITokenNum && state[5] == AITokenNum) ||
            (state[6] == AITokenNum && state[7] == AITokenNum && state[8] == AITokenNum) ||
            (state[0] == AITokenNum && state[3] == AITokenNum && state[6] == AITokenNum) ||
            (state[1] == AITokenNum && state[4] == AITokenNum && state[7] == AITokenNum) ||
            (state[2] == AITokenNum && state[5] == AITokenNum && state[8] == AITokenNum) ||
            (state[0] == AITokenNum && state[4] == AITokenNum && state[8] == AITokenNum) ||
            (state[2] == AITokenNum && state[4] == AITokenNum && state[6] == AITokenNum))
        {
            gameFinished = true;
            // +1 to state score for win
            result++;
            // and for every remaining empty space
            for (int x = 0; x < 9; x++)
            {
                if (state[x] == 0)
                {
                    result++;
                }
            }
            return result;
        }

        // check for tie
        if (state[0] != 0 && state[1] != 0 && state[2] != 0 &&
            state[3] != 0 && state[4] != 0 && state[5] != 0 &&
            state[6] != 0 && state[7] != 0 && state[8] != 0)
        {
            gameFinished = true;
            return 0;
        }

        return 0;
    }
    void WinHandling(int checkWinResult)
    {
        if (checkWinResult < 0) {
            winUI.text = "(";
        }
        if (checkWinResult > 0) {
            winUI.text = "D";
        } 
        Invoke(nameof(Setup), 1f);
    }

}
