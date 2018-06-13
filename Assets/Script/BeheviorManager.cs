using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeheviorManager : MonoBehaviour
{

    [SerializeField] Text BeheviorText;
    Animator animator;

    void Awake()
    {
        if (!animator)
            animator = GetComponent<Animator>();

        if (!BeheviorText)
            BeheviorText = GetComponentInChildren<Text>();
    }

    internal void Points(int score, TeamManager.TeamSide teamSide)
    {
        if (score == 1)
        {
            BeheviorText.text = ("Score " + score + " point").ToString();
        }
        else if (score == 0)
        {
            BeheviorText.text = "Team Switched";
        }
        else
        {
            BeheviorText.text = ("Scores " + score + " points").ToString();
        }

        if (teamSide == TeamManager.TeamSide.Left)
        {
            animator.Play("Behevior-Point-L");
        }
        else
        {
            animator.Play("Behevior-Point-R");
        }
    }

    void OnEnable()
    {
        if (!animator)
            GetComponent<Animator>();

        if (!BeheviorText)
            GetComponentInChildren<Text>();
    }

    internal void Foul(string foulRule, TeamManager.TeamSide teamSide)
    {
        BeheviorText.text = foulRule;

        if (teamSide == TeamManager.TeamSide.Left)
        {
            animator.Play("Behevior-Foul-L");
        }
        else
        {
            animator.Play("Behevior-Foul-R");
        }
    }

}
