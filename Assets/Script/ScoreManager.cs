using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public enum GameState
    {
        WaitForStart = 0, Gametime, Overtime, Break, Pause, End
    };

    public enum GameStyle
    {
        Inter = 0, NBA 
    };

    public GameStyle gameStyle;

    public GameState gameState;

    public TeamManager team1;
    public TeamManager team2;

    public int gameTimer;
    public int breakTimer;
    public int quarter;
    public int restartCounter;
    public float restartResetTimer;

    public Text minuteText;
    public Text secondText;
    public Text quarterText;
    public Text gameStateText;

    public GameObject promtCanvas;
    public GameObject startPromt;
    public GameObject teamPromt;
    public InputField team1Field;
    public InputField team2Field;
    public GameObject sidePromt;

    // Use this for initialization
    void Start () {
        promtCanvas.SetActive(true);
        startPromt.SetActive(true);
        restartCounter = 0;
        quarter = 1;
        gameState = GameState.WaitForStart;
        StartCoroutine(GameTimer());
	}

    public void NameTeam()
    {
        if (team1Field.text == "" || team2Field.text == "")
            return;

        team1.teamText.text = team1Field.text;
        team2.teamText.text = team2Field.text;

        teamPromt.SetActive(false);
        sidePromt.SetActive(true);
    }

    void StartGame()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            sidePromt.SetActive(false);
            team2.OD(TeamManager.TeamState.Defense);
            team1.OD(TeamManager.TeamState.Offense);
            promtCanvas.SetActive(false);
            gameState = GameState.Gametime;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            sidePromt.SetActive(false);
            team1.OD(TeamManager.TeamState.Defense);
            team2.OD(TeamManager.TeamState.Offense);
            promtCanvas.SetActive(false);
            gameState = GameState.Gametime;
        }
    }

    void Update()
    {
        if (restartCounter > 0)
        {
            restartResetTimer += Time.deltaTime;

            if (restartResetTimer > 1.5f)
            {
                restartResetTimer = 0;
                restartCounter = 0;
            }
        }

        if (gameState == GameState.WaitForStart && startPromt.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                teamPromt.SetActive(true);
                startPromt.SetActive(false);
            }
        }
        else if (gameState == GameState.WaitForStart && sidePromt.activeInHierarchy)
        {
            StartGame();
        }
        else if (gameState == GameState.Gametime || gameState == GameState.Overtime)
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                gameState = GameState.Pause;
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                team1.Score(2);
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                team1.Score(3);
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                team2.Score(2);
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                team2.Score(3);
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                team1.Foul("Travelling");
            }
            if (Input.GetKeyDown(KeyCode.F6))
            {
                team2.Foul("Travelling");
            }
            if (Input.GetKeyDown(KeyCode.F7))
            {
                team1.Foul("Double Dribbing");
            }
            if (Input.GetKeyDown(KeyCode.F8))
            {
                team2.Foul("Double Dribbing");
            }
            if (Input.GetKeyDown(KeyCode.F9))
            {
                team1.Foul("Goaltending");
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                team2.Foul("Goaltending");
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                StartCoroutine(team1.OdSwitch());
                StartCoroutine(team2.OdSwitch());
            }


            if (Input.GetKeyDown(KeyCode.F12))
            {
                restartCounter += 1;

                if (restartCounter >= 3)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                restartCounter += 1;

                if (restartCounter >= 3)
                {
                    Application.Quit();
                }
            }
        }
        else if (gameState == GameState.Pause)
        {
            if (Input.GetKeyDown(KeyCode.F11))
            {
                gameState = GameState.Gametime;
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                team1.Score(1);
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                team2.Score(1);
            }
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                StartCoroutine(team1.OdSwitch());
                StartCoroutine(team2.OdSwitch());
            }

            if (Input.GetKeyDown(KeyCode.F12))
            {
                restartCounter += 1;

                if (restartCounter >= 3)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                restartCounter += 1;

                if (restartCounter >= 3)
                {
                    Application.Quit();
                }
            }
        }
        else if (gameState == GameState.Break)
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                restartCounter += 1;

                if (restartCounter >= 3)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                restartCounter += 1;

                if (restartCounter >= 3)
                {
                    Application.Quit();
                }
            }
        }
    }

	void FixedUpdate () {
        if (gameState != GameState.WaitForStart)
            UpdateGameState();
	}

    void SwitchTeam()
    {
        int tempScore = team1.score;
        string tempName = team1.teamText.text;

        team1.UpdateTeamSwitch(team2.score, team2.teamText.text);
        team2.UpdateTeamSwitch(tempScore, tempName);
    }

    IEnumerator GameTimer()
    {
        while (true)
        {
            yield return new WaitWhile(() => gameState == GameState.WaitForStart);

            if (gameState == GameState.Pause)
            {
                yield return new WaitForSecondsRealtime(1f);
                UpdateTime();
            }
            else if (gameState == GameState.Break)
            {
                yield return new WaitForSecondsRealtime(1f);
                if (quarter == 2)
                {
                    if (breakTimer < 900)
                    {
                        breakTimer += 1;
                    }
                    else
                    {
                        breakTimer = 0;
                        quarter += 1;
                        UpdateQuarter();
                        gameState = GameState.Gametime;
                    }
                }
                else if (quarter == 4)
                {
                    break;
                }
                else
                {
                    if (breakTimer < 120)
                    {
                        breakTimer += 1;
                    }
                    else
                    {
                        breakTimer = 0;
                        quarter += 1;
                        UpdateQuarter();
                        gameState = GameState.Gametime;
                    }
                }

                UpdateTime();
            }
            else if (gameState != GameState.End)
            {
                yield return new WaitForSecondsRealtime(1f);

                switch (gameStyle)
                {
                    case GameStyle.Inter:

                        if (gameState == GameState.Overtime)
                        {
                            if (gameTimer < 300)
                            {
                                gameTimer += 1;
                            }
                            else
                            {
                                gameStateText.text = "END GAME";
                                gameState = GameState.End;
                            }
                        }
                        else if (gameState != GameState.Pause)
                        {
                            if (gameTimer < 600)
                            {
                                gameTimer += 1;
                            }
                            else
                            {
                                if (quarter == 4)
                                {
                                    gameTimer = 0;
                                    gameState = GameState.Overtime;
                                }
                                else if (quarter == 2)
                                {
                                    SwitchTeam();
                                    gameTimer = 0;
                                    breakTimer = 0;
                                    gameState = GameState.Break;
                                }
                                else
                                {
                                    gameTimer = 0;
                                    breakTimer = 0;
                                    gameState = GameState.Break;
                                }
                            }
                        }
                        else
                        {
                            breakTimer = 0;
                        }


                        break;

                    case GameStyle.NBA:

                        if (gameState == GameState.Overtime)
                        {
                            if (gameTimer < 300)
                            {
                                gameTimer += 1;
                            }
                            else
                            {
                                gameStateText.text = "END GAME";
                                gameState = GameState.End;
                            }
                        }
                        else
                        {
                            if (gameTimer < 720)
                            {
                                gameTimer += 1;
                            }
                            else if (gameState != GameState.Pause)
                            {
                                if (quarter == 4)
                                {
                                    gameTimer = 0;
                                    gameState = GameState.Overtime;
                                }
                                else if (quarter == 2)
                                {
                                    SwitchTeam();
                                    gameTimer = 0;
                                    breakTimer = 0;
                                    gameState = GameState.Break;
                                }
                                else
                                {
                                    gameTimer = 0;
                                    breakTimer = 0;
                                    gameState = GameState.Break;
                                }
                            }
                            else
                            {
                                breakTimer = 0;
                            }
                        }

                        break;

                }

                UpdateTime();
            }
            else if (gameState == GameState.End)
            {
                break;
            }
        }
        StopCoroutine(GameTimer());
    }

    void UpdateGameState()
    {
        switch (gameState)
        {
            case GameState.Gametime:
                gameStateText.text = "GAME TIME";
                break;
            case GameState.Break:
                gameStateText.text = "BREAK TIME";
                break;
            case GameState.Pause:
                gameStateText.text = "PAUSE";
                break;
            case GameState.Overtime:
                gameStateText.text = "OVERTIME";
                break;
        }
    }

    void UpdateQuarter()
    {
        quarterText.text = "Queater " + quarter.ToString();
    }

    void UpdateTime()
    {
        if (gameState == GameState.Gametime || gameState == GameState.Overtime || gameState == GameState.Pause)
        {
            System.TimeSpan t = System.TimeSpan.FromSeconds(gameTimer);

            if (t.Seconds < 10 && t.Minutes < 10)
            {
                minuteText.text = "0" + t.Minutes;
                secondText.text = "0" + t.Seconds;
            }
            else if (t.Seconds > 9 && t.Minutes < 10)
            {
                minuteText.text = "0" + t.Minutes;
                secondText.text = t.Seconds.ToString();
            }
            else if (t.Seconds < 10 && t.Minutes > 9)
            {
                minuteText.text = t.Minutes.ToString();
                secondText.text = "0" + t.Seconds;
            }
            else if (t.Seconds > 9 && t.Minutes > 9)
            {
                minuteText.text = t.Minutes.ToString();
                secondText.text = t.Seconds.ToString();
            }
        }
        else if (gameState == GameState.Break)
        {
            System.TimeSpan t = System.TimeSpan.FromSeconds(breakTimer);

            if (t.Seconds < 10 && t.Minutes < 10)
            {
                minuteText.text = "0" + t.Minutes;
                secondText.text = "0" + t.Seconds;
            }
            else if (t.Seconds > 9 && t.Minutes < 10)
            {
                minuteText.text = "0" + t.Minutes;
                secondText.text = t.Seconds.ToString();
            }
            else if (t.Seconds < 10 && t.Minutes > 9)
            {
                minuteText.text = t.Minutes.ToString();
                secondText.text = "0" + t.Seconds;
            }
            else if (t.Seconds > 9 && t.Minutes > 9)
            {
                minuteText.text = t.Minutes.ToString();
                secondText.text = t.Seconds.ToString();
            }
        }
    }
}
