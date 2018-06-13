using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{

    public enum TeamSide
    {
        Left, Right
    };

    public enum TeamState
    {
        Offense, Defense
    };

    public BeheviorManager beheviorManager;

    public TeamState teamState;

    public TeamSide teamSide;
    public int score;

    public Text scoreText;
    public Text teamText;
    public Image teamStateImage;
    public Image teamStatBar;

    public Sprite offenseSprite;
    public Sprite defenseSprite;

    public Animator animator;

    public Color defense;
    public Color attack;

    public void UpdateTeamSwitch(int score, string teamName)
    {
        this.score = score;
        teamText.text = teamName;
        Score(0);
        StartCoroutine(OdSwitch());
    }

    void Awake()
    {
        if (!animator)
            animator = GetComponent<Animator>();

        if (!beheviorManager)
            beheviorManager = GetComponentInChildren<BeheviorManager>();
    }

    void OnEnable()
    {
        if (!animator)
            animator = GetComponent<Animator>();

        if (!beheviorManager)
            beheviorManager = GetComponentInChildren<BeheviorManager>();
    }

    public void Score(int points)
    {
        score += points;

        if (score < 10)
        {
            scoreText.text = "0" + score.ToString();
        }
        else
        {
            scoreText.text = score.ToString();
        }

        beheviorManager.Points(points, teamSide);
    }

    public void Foul(string foulRule)
    {
        beheviorManager.Foul(foulRule, teamSide);
    }

    public void OD(TeamState teamState)
    {
        switch (teamState)
        {
            case (TeamState.Offense):
                this.teamState = TeamState.Offense;
                teamStateImage.sprite = offenseSprite;
                teamStateImage.color = attack;
                teamStatBar.color = attack;
                animator.enabled = true;
                break;
            case (TeamState.Defense):
                this.teamState = TeamState.Defense;
                teamStateImage.sprite = defenseSprite;
                teamStateImage.color = defense;
                teamStatBar.color = defense;
                animator.enabled = true;
                break;
        }
    }

    internal IEnumerator OdSwitch()
    {
        if (teamSide == TeamSide.Left)
        {
            animator.Play("TeamStateLout");
        }
        else
        {
            animator.Play("TeamStateRout");
        }

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        switch (teamState)
        {
            case (TeamState.Offense):
                teamState = TeamState.Defense;
                teamStateImage.sprite = defenseSprite;
                teamStateImage.color = defense;
                teamStatBar.color = defense;

                break;
            case (TeamState.Defense):
                teamState = TeamState.Offense;
                teamStateImage.sprite = offenseSprite;
                teamStateImage.color = attack;
                teamStatBar.color = attack;
                break;
        }
    }


}
