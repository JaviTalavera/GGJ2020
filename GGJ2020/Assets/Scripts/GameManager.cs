using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int _rounds = 0;
    public Text _txtCountdown;
    public Text _txtTimer;
    public Text _txtMessage;
    public Text _txtStart;
    public Text _txtResult;
    public Text _txtRounds;

    public Transform _panelPause;
    public Transform _panelGameOver;
    public Transform _panelVictory;
    public Transform _panelGame;
    public Transform _panelControles;

    Level level;

    public static bool IsPause = false;

    public enum GameStateEnum { PREGAME, COUNTDOWN, GAME, POSGAME, PAUSE, GAMEOVER }

    private TimeSpan _time;

    private double _milliseconds;
    private readonly double _maxMilliseconds = 1 * 60;

    bool stopClock = false;

    private GameStateEnum _gameState;
    public bool _mainGame = false;

    public void SubtractTime (double t)
    {
        _milliseconds -= t;
        _txtTimer.GetComponent<Animator>().Play("TimeSubtract");
    }

    private void Start()
    {
        if (_mainGame)
            InitializeGame();
        level = GameObject.FindWithTag("LevelManager").GetComponent<Level>();
    }

    public void ShowText(string text)
    {
        _txtMessage.enabled = true;
        _txtMessage.text = text;
        _txtMessage.GetComponent<Animator>().SetTrigger("show");
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            _gameState = GameStateEnum.PAUSE;
            GameManager.IsPause = true;
            _panelPause.gameObject.SetActive(true);
            _panelGame.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            _gameState = GameStateEnum.GAME;
            GameManager.IsPause = false;
            _panelPause.gameObject.SetActive(false);
            _panelGame.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GameOver();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            EndGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameState == GameStateEnum.PAUSE)
            {
                Pause(false);
            }

            else if (_gameState == GameStateEnum.GAME)
            {
                Pause(true);
            }
        }
        if (_gameState == GameStateEnum.PREGAME)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(StartGame());
            }
        }
        else if (_gameState == GameStateEnum.GAME)
        {
            _time = new TimeSpan(0, 0, 0, 0, (int)(_milliseconds * 1000));
            _txtTimer.text = _time.ToString(@"mm\:ss\.fff");

            if (!stopClock) _milliseconds -= Time.deltaTime;
            if (_milliseconds <= 0)
                GameOver();
        }
    }

    public void GameOver()
    {
        level.SetRobots(3);
        _txtRounds.text = "Rounds: " + _rounds;
        _rounds = 0;
        GameObject.FindWithTag("Audio").GetComponent<PassAudioToNextScene>().Play(4);
        _milliseconds = 0;
        _gameState = GameStateEnum.GAMEOVER;
        Time.timeScale = 0;
        var robots = GameObject.FindGameObjectsWithTag("Robot");
        foreach (var r in robots) Destroy(r);
        var pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (var p in pieces) Destroy(p);
        GameObject.FindWithTag("LevelManager").GetComponent<Level>().RestartHook();
        _panelGameOver.gameObject.SetActive(true);
        _panelGame.gameObject.SetActive(false);
    }

    public void EndGame()
    {
        _rounds++;        
        GameObject.FindWithTag("Audio").GetComponent<PassAudioToNextScene>().Play(3);
        _gameState = GameStateEnum.POSGAME;
        Time.timeScale = 0;
        var robots = GameObject.FindGameObjectsWithTag("Robot");
        foreach (var r in robots) Destroy(r);
        var pieces = GameObject.FindGameObjectsWithTag("Piece");
        foreach (var p in pieces) Destroy(p);
        level.RestartHook();
        level.SetRobots(level.GetRobots() + 2);
        var time = _maxMilliseconds - _milliseconds;
        _time = new TimeSpan(0, 0, 0, 0, (int)(time * 1000));
        _txtResult.text = _time.ToString(@"mm\:ss\.fff");
        _panelVictory.gameObject.SetActive(true);
        _panelGame.gameObject.SetActive(false);
    }

    public void InitializeGame()
    {
        _txtStart.enabled = true;
        _txtCountdown.enabled = false;
        _gameState = GameStateEnum.PREGAME;
        _milliseconds = _maxMilliseconds;
        _time = new TimeSpan(0, 0, 0, 0, (int)(_milliseconds * 1000));
        _txtTimer.text = _time.ToString(@"mm\:ss\.fff");
        Time.timeScale = 1;
        _panelControles.gameObject.SetActive(true);
        _panelPause.gameObject.SetActive(false);
        _panelGameOver.gameObject.SetActive(false);
        _panelVictory.gameObject.SetActive(false);
        _panelGame.gameObject.SetActive(false);
    }

    public IEnumerator StartGame()
    {
        _panelControles.gameObject.SetActive(false);
        _panelGame.gameObject.SetActive(true);
        _gameState = GameStateEnum.COUNTDOWN;
        int countdown = 3;
        _txtMessage.enabled = false;
        _txtStart.enabled = false;
        _txtCountdown.enabled = true;
        _txtCountdown.text = countdown.ToString();
        yield return new WaitForSeconds(0.75f);
        _txtCountdown.text = (--countdown).ToString();
        yield return new WaitForSeconds(0.75f);
        _txtCountdown.text = (--countdown).ToString();
        yield return new WaitForSeconds(0.75f);
        _txtCountdown.enabled = false;
        ShowText("GO!");
        _gameState = GameStateEnum.GAME;
        GameObject.FindWithTag("LevelManager").GetComponent<Level>()?.Initialize();
    }

    public void LoadScene(string scene) => SceneManager.LoadScene(scene);

    public void Quit() => Application.Quit();
}
