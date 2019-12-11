using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BreakController : MonoBehaviour
{
    private float _timeLeft = 300;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI instructionText;

    private PlayerPackage _playerPackage;
    private SceneType _lastSceneLoaded;

    [Header("UI Panel Fields")]
    public Canvas canvas;
    public Camera viveCameraEye;
    public Camera oculusCameraEye;

    void Start()
    {
        _lastSceneLoaded = (SceneType)PlayerPackage.LastScene;
        if (_lastSceneLoaded != SceneType.Positive)
        {
            StartCoroutine("IStartTimer", _timeLeft);
        }

    }

    void Update()
    {
        canvas.worldCamera = _playerPackage.appSettings.deviceType == DeviceType.Oculus ? oculusCameraEye : viveCameraEye;
        _lastSceneLoaded = (SceneType)PlayerPackage.LastScene;
        //if (_lastSceneLoaded == SceneType.Positive)
        //{
        timeText.enabled = false;
        instructionText.text = "Please take off the headset.";
        return;
        //}
        //timeText.text = GetDisplayTime();
    }

    IEnumerator IStartTimer(int timer)
    {
        _playerPackage = FindObjectOfType<PlayerPackage>();
        _playerPackage.NextSceneType = getNextScene();
        _timeLeft = timer;
        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            yield return null;
        }


        _playerPackage.LoadNextScene();
    }

    public string GetDisplayTime()
    {
        int t = (int)_timeLeft;
        int min = t / 60;
        int sec = t % 60;
        int hr = min / 60;
        if (min >= 60)
        {
            min = min % 60;
        }

        return getTime(min) + ":" + getTime(sec);
    }

    private string getTime(int t)
    {
        return t < 10 ? "0" + (int)(t) : "" + (int)t;
    }

    private SceneType getNextScene()
    {
        Debug.Log("Last Scene Loaded -> " + (SceneType)_lastSceneLoaded);
        switch (_lastSceneLoaded)
        {
            case SceneType.Tutorial:
                {
                    return SceneType.Neutral;
                }
            case SceneType.Neutral:
                {
                    return SceneType.Negative;
                }
            case SceneType.Negative:
                {
                    return SceneType.Positive;
                }
            case SceneType.Positive:
                {
                    return SceneType.Break;
                }
        }
        return SceneType.Tutorial;
    }
}