using UnityEngine;

public class PassAudioToNextScene : MonoBehaviour
{
    public static bool AudioOn = true;
    public AudioSource _audio;
    public AudioSource _audioPrincipal;
    public AudioClip _error;
    public AudioClip _acierto;
    public AudioClip _reparado;
    public AudioClip _win;
    public AudioClip _lose;
    public bool _pararMusica;
    // Start is called before the first frame update

    public static void ChangeAudio()
    {
        PassAudioToNextScene.AudioOn = !PassAudioToNextScene.AudioOn;
    }

    public void Play(int code)
    {
        switch(code)
        {
            case 0: _audio.clip = _error; break;
            case 1: _audio.clip = _acierto; break;
            case 2: _audio.clip = _reparado; break;
            case 3: _audio.clip = _win; _pararMusica = true; break;
            case 4: _audio.clip = _lose; _pararMusica = true; break;
        }
        if(PassAudioToNextScene.AudioOn)
            _audio.Play();
    }

    private void Update()
    {
        if (_pararMusica && _audio.isPlaying && PassAudioToNextScene.AudioOn)
        {
            _audioPrincipal.mute = true;
        }
        else if (_pararMusica && PassAudioToNextScene.AudioOn)
        {
            _pararMusica = false;
            _audioPrincipal.mute = false;
        }
    }

    //Play global
    private static PassAudioToNextScene instance = null;
    public static PassAudioToNextScene Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if(instance!= null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
