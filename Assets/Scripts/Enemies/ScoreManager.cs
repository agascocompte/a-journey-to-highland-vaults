using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;

    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }


    void Update()
    {
        text.text = "Score:  " + score;
    }
}
