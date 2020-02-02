using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Text _txtCountdown;
    public Text _txtTimer;
    public Text _txtMessage;
    public Text _txtStart;
    public Text _txtResult;

    public Transform _panelPause;
    public Transform _panelGameOver;
    public Transform _panelVictory;
    public Transform _panelGame;

    public enum GameStateEnum { PREGAME, COUNTDOWN, GAME, POSGAME, PAUSE, GAMEOVER }

    private TimeSpan _time;

    private double _milliseconds;
    private readonly double _maxMilliseconds = 3 * 60;

    bool stopClock = false;

    private GameStateEnum _gameState;

    private void Start()
    {
        InitializeGame();
    }

    public void ShowText(string text)
    {
        _txtMessage.enabled = true;
        _txtMessage.text = text;
        _txtMessage.GetComponent<Animator>().SetTrigger("show");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_gameState == GameStateEnum.PAUSE)
            {
                Time.timeScale = 1;
                _gameState = GameStateEnum.GAME;
                _panelPause.gameObject.SetActive(false);
                _panelGame.gameObject.SetActive(true);
            }

            else if (_gameState == GameStateEnum.GAME)
            {
                Time.timeScale = 0;
                _gameState = GameStateEnum.PAUSE;
                _panelPause.gameObject.SetActive(true);
                _panelGame.gameObject.SetActive(false);
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
            {
                _milliseconds = 0;
                _gameState = GameStateEnum.GAMEOVER;
                Time.timeScale = 0;
                _panelGameOver.gameObject.SetActive(true);
                _panelGame.gameObject.SetActive(false);
            }
        }
    }

    public void EndGame()
    {
        _gameState = GameStateEnum.POSGAME;
        Time.timeScale = 0;
        var time = _maxMilliseconds - _milliseconds;
        _time = new TimeSpan(0, 0, 0, 0, (int)(time * 1000));
        _txtResult.text = _time.ToString(@"mm\:ss\.fff");
        _panelVictory.gameObject.SetActive(true);
        _panelGame.gameObject.SetActive(false);
    }

    public void InitializeGame()
    {
        _txtCountdown.enabled = false;
        _gameState = GameStateEnum.PREGAME;
        _milliseconds = _maxMilliseconds;
        _time = new TimeSpan(0, 0, 0, 0, (int)(_milliseconds * 1000));
        _txtTimer.text = _time.ToString(@"mm\:ss\.fff");
    }

    public IEnumerator StartGame()
    {
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

    public void Quit() => Application.Quit();
}
