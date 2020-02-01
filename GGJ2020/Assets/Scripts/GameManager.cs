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

    public enum GameStateEnum { PREGAME, COUNTDOWN, GAME, PAUSE, GAMEOVER }

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
        }
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
    }
}
