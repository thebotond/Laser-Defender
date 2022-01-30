using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{

    public static int score;
    private Text myText;

    void Start()
    {
        myText = GetComponent<Text>();
    }

    public void UpdateScore(int points)
    {
        score += points;
        myText.text = score.ToString();
    }

    public static void ResetScore()
    {
        score = 0;
    }
}