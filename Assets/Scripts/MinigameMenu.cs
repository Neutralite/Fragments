using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinigameMenu : MonoBehaviour
{
    public TextMeshProUGUI gameName, instructions;
    public List<string> gamesList;
    [TextArea(15, 20)]
    public string[] gameInstructions;

    public void UpdateText(int game)
    {
        gameName.text = gamesList[game];
        instructions.text = gameInstructions[game];
    }
}
