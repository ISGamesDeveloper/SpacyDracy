using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [HideInInspector] public Transform Target;

    public Camera MainCamera;
    public List<RaceCarScript> RCS = new List<RaceCarScript>();
    public GamePlayManager GamePlayManager;
    public ColorBoxContainer ColorBoxContainer;
    public Text CurrentPlayerName;

    private ApplicationMain applicationMain;

    private bool autoUpdateCamera;

    private float orthographicSize = 3f;

    public double GlobalTime = 0;

    private void Awake()
    {
        applicationMain = ApplicationMain.Instance;
        applicationMain.CameraManager = this;
    }

    void Start()
    {
        for (var i = 0; i < GamePlayManager.RaceCars.Count; i++)
        {
            RCS.Add(GamePlayManager.RaceCars[i]);
            RCS[i].RcsID = i;
            ChangeColorBackgroundCar(RCS[i].CarColor, RCS[i]);
        }

        MainCamera.orthographicSize = orthographicSize;

        currentRaceCarTransform = RCS[0].transform;

        AutoCamera();

        ColorBoxContainer.OnAutoCamera += AutoCamera;
        ColorBoxContainer.OnChangeStateCamera += ManualCamera;

        StartCoroutine(FindFirstRocket());
    }

    private IEnumerator FindFirstRocket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (applicationMain.GameIsPlaying)
            {
                RCS.Sort(delegate (RaceCarScript b, RaceCarScript a)
                {
                    return a.SubstanceRank.CompareTo(b.SubstanceRank);
                });

                if (autoUpdateCamera)
                {
                    SetFocus(0);
                    UpdateRocketInfo(0);
                }

                for (int i = 0; i < RCS.Count; i++)
                {
                    GamePlayManager.colorBoxContainer.SetColorBox(RCS[i].PlayerFace, RCS[i].CarColor, i, i, RCS[i].Focused);
                }
            }
        }
    }

    private void AutoCamera()
    {
        currentClickedPlayer = 0;
        autoUpdateCamera = true;
        MainCamera.orthographicSize = orthographicSize + 4;
    }
    private int currentClickedPlayer = 0;
    private void ManualCamera(int id)
    {
        Debug.Log("ID RCS: " + id);

        autoUpdateCamera = false;
        currentClickedPlayer = id;
        MainCamera.orthographicSize = orthographicSize;

        SetFocus(id);

        UpdateRocketInfo(id);
    }

    private Transform currentRaceCarTransform;

    private void Update()
    {
        if (applicationMain.GameIsPlaying)
        {
            GlobalTime += Time.deltaTime;
        }

        var position = currentRaceCarTransform.position;
        var newPosition = new Vector3(position.x, position.y, -5);
        MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, newPosition, Time.deltaTime * 5);
    }

    private void ChangeColorBackgroundCar(Color color, RaceCarScript raceCarScript)
    {
        if (RCS == null)
            return;

        var main = raceCarScript.Fire.main;
        main.startColor = color;
    }

    private void UpdateRocketInfo(int index)
    {
        var rocket = RCS[index];

        var currentLap = rocket.CurrentCarLapCounter.CurrentLap;

        if (currentLap > CarLapCounter.MaxLapCount)
        {
            currentLap -= 1;
        }

        currentRaceCarTransform = rocket.transform;
        CurrentPlayerName.text = rocket.PlayerName + ". Lap: " + currentLap;
        CurrentPlayerName.color = rocket.CarColor;
        //ColorBoxContainer.ColorBoxItems[index].MyRcsID = index;
        //ColorBoxContainer.EnableArc(rocket.RcsID);
    }

    private void SetFocus(int index)
    {
        for (int i = 0; i < RCS.Count; i++)
        {
            RCS[i].Focused = false;
        }

        RCS[index].Focused = true;
    }
}
