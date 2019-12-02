using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType
{
    Tutorial,
    Excercise,
    Positive,
    Negative,
    Neutral,
    Break,
}

public enum DeviceType
{
    Vive,
    Oculus,
}

public class PlayerPackage : MonoBehaviour
{
    public AppSettings appSettings;

    public static int LastScene = -1;
    public static string TutorialSceneName = "Tutorial";
    public static string ExcerciseSceneName = "Excercise";
    public static string PositiveSceneName = "Positive";
    public static string NegativeSceneName = "Negative";
    public static string NeutralSceneName = "Neutral";
    public static string BreakSceneName = "Break";

    public SceneType previousSceneType; //last scene; 
    public SceneType activeSceneType; //current scene  
    public SceneType nextSceneType; //scene to load 

    public GameObject jumpingJackObject;
    public List<Sign> signPosts;

    [Header("Transtion Fields")]
    public GameObject cameraOne;
    public GameObject cameraTwo;
    public GameObject targetToReach;

    [Header("Device Fields")]
    public GameObject ViveCamera;
    public GameObject OculusCamera;


    private float targetDistance = 20;
    private bool _isLoadingNextScene;
    private DataRecorder _dataRecorder;
    private float _levelTime;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        // ViveCamera = transform.GetChild(0).gameObject;
        // OculusCamera = transform.GetChild(1).gameObject;

        if (activeSceneType != SceneType.Excercise)
        {
            ViveCamera.SetActive(appSettings.deviceType == DeviceType.Vive);
            OculusCamera.SetActive(appSettings.deviceType == DeviceType.Oculus);
        }

        if (activeSceneType == SceneType.Neutral || activeSceneType == SceneType.Negative || activeSceneType == SceneType.Positive)
        {
            StartCoroutine("IStartTimer");
        }

        _dataRecorder = FindObjectOfType<DataRecorder>();
        jumpingJackObject.SetActive(activeSceneType == SceneType.Excercise);
        _isLoadingNextScene = false;
    }

    //Debug Method -> Remove after
    IEnumerator IWaitAndLoadNextScene()
    {
        yield return new WaitForSeconds(3f);
        LoadNextScene();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void LoadNextScene()
    {
        if (_isLoadingNextScene) return;

        if (_dataRecorder != null && activeSceneType != SceneType.Excercise && activeSceneType != SceneType.Break)
        {
            List<float> signTimes = new List<float>();
            for (int i = 0; i < signPosts.Count; i++)
            {
                signTimes.Add(signPosts[i].LookingTimer);
            }

            _dataRecorder.WriteSignInfo(signTimes, _levelTime, activeSceneType.ToString());
            StopCoroutine("IStartTimer");
        }

        IEnumerator iLoadScene = ILoadScene();
        StartCoroutine(iLoadScene);
        _isLoadingNextScene = true;
    }

    IEnumerator IStartTimer()
    {
        while (true)
        {
            _levelTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ILoadScene()
    {
        yield return new WaitForSeconds(1f);
        if (activeSceneType != SceneType.Excercise && activeSceneType != SceneType.Tutorial)
        {
            LastScene = (int)activeSceneType;
            previousSceneType = activeSceneType;
            Debug.Log("<color=red>" + activeSceneType + "</color>");
        }

        switch (nextSceneType)
        {
            case SceneType.Tutorial:
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(TutorialSceneName);
                    break;
                }
            case SceneType.Excercise:
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(ExcerciseSceneName);
                    break;
                }
            case SceneType.Positive:
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(PositiveSceneName);
                    break;
                }
            case SceneType.Negative:
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(NegativeSceneName);
                    break;
                }
            case SceneType.Neutral:
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(NeutralSceneName);
                    break;
                }
            case SceneType.Break:
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(BreakSceneName);
                    break;
                }
        }
    }

    private bool hasReachedToTarget()
    {
        if (cameraOne == null || cameraTwo == null || targetToReach == null)
        {
            return false;
        }
        float _distanceOne = (cameraOne.transform.position - targetToReach.transform.position).sqrMagnitude;
        float _distanceTwo = (cameraTwo.transform.position - targetToReach.transform.position).sqrMagnitude;
        return _distanceOne < targetDistance || _distanceTwo < targetDistance;
    }

    public SceneType NextSceneType
    {
        get { return nextSceneType; }
        set { this.nextSceneType = value; }
    }
}