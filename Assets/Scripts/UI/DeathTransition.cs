using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathTransition : MonoBehaviour
{
    public Text yourScoreText;
    public Text timerText;
    public Animator animator;

    private float timer;
    private bool animationStarted = false;
    private float maxTimer = 10f;

    private void Update()
    {
        yourScoreText.text = "YOUR SCORE:  " + ScoreManager.score;

        if (animationStarted)
        {
            timer += Time.deltaTime;
            int value = (int) (maxTimer - timer) + 1;
            timerText.text = "Returning in...  " + value;

            if (timer >= maxTimer)
            {
                MapGenerator.ReturnToMainManuFromDeath();
                animationStarted = false;
                animator.enabled = false;
            }
        }
        
    }

    public void activateDeathTransition()
    {
        animator.SetTrigger("playerIsDead");
        animationStarted = true;
    }
}
