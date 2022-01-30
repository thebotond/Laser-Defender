using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthCheck : MonoBehaviour
{

    public static int health;
    private Text myText;

    void Start()
    {
        myText = GetComponent<Text>();
    }

    public void UpdateHealth(int points)
    {
        myText.text = points.ToString();
    }

    public static void ResetHealth()
    {
        health = 3;
    }
}