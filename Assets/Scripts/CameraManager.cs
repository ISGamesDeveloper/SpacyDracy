using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [HideInInspector] public Transform Target;

    public Button ChangeCameraButton;
    public Camera MainCamera;
    public RaceCarScript[] raceCarScripts;
    public GamePlayManager GamePlayManager;
    public Transform[] ColorBoxContainer;
    public Text CurrentPlayerName;

    private int currentCameraNumber;
    private ApplicationMain applicationMain; //сдеалть несколько камер (основная и переключаемая)
                                             //private Text[] playerTextUI;
    private bool autoUpdateCamera;
    private int currentIndexForCamera;
    private const int CameraHeight = -5;
    private readonly Vector3 MainCameraDefaultPosition = new Vector3(26, 0, CameraHeight);
    private float orthographicSize = 3f;
    private float defaultOrthographicSize = 25;

    private void Awake()
    {
        applicationMain = ApplicationMain.Instance;
        applicationMain.CameraManager = this;
    }

    void Start()
    {
        //playerTextUI = applicationMain.GamePlayManager.PlayerNameText;
        ChangeCameraButton.onClick.AddListener(ChangeStateCameras);

        raceCarScripts = new RaceCarScript[ApplicationMain.RaceCars.Count];
        Debug.Log("ApplicationMain.RaceCars: " + ApplicationMain.RaceCars.Count);
        for (var i = 0; i < raceCarScripts.Length; i++)
        {
            Debug.Log("i: " + i);
            raceCarScripts[i] = GamePlayManager.RaceCars[i];

            ChangeColorBackgroundCar(raceCarScripts[i].CarColor, /*playerTextUI[i], */raceCarScripts[i]);
        }

        MainCamera.orthographicSize = orthographicSize;

        autoUpdateCamera = true;

        if (autoUpdateCamera == false)
        {
            currentRaceCarTransform = raceCarScripts[0].transform;
        }
        else
        {
            ChangeStateCameras();
        }
    }

    private void FindFirstRocket()
    {
        for (int i = 0; i < raceCarScripts.Length; i++)
        {
            if (raceCarScripts[i].SubstanceRank > myRank)
            {
                myRank = raceCarScripts[i].SubstanceRank;
                currentIndexForCamera = i;
                ColorBoxContainer[i].SetSiblingIndex(0);
            }
        }
    }

    public float myRank = 0;

    public void CheckCurrentRankin()
    {
        FindFirstRocket();

        var color = Color.clear;

        for (int i = 0; i < ColorBoxContainer.Length; i++)
        {
            ColorBoxContainer[i].GetComponent<Outline>().effectColor = color;
        }

        if (autoUpdateCamera)
        {
            color = Color.black;
            ColorBoxContainer[currentIndexForCamera].GetComponent<Outline>().effectColor = color;
            //currentRaceCarTransform = raceCarScripts[currentIndexForCamera].transform;
            //CurrentPlayerName.text = raceCarScripts[currentIndexForCamera].PlayerName;
            //CurrentPlayerName.color = raceCarScripts[currentIndexForCamera].CarColor;
            UpdateRocketInfo(currentIndexForCamera);
        }

    }

    private void UpdateRocketInfo(int index)
    {
        var rocket = raceCarScripts[index];
        var currentLap = rocket.CurrentCarLapCounter.CurrentLap;

        currentRaceCarTransform = rocket.transform;
        CurrentPlayerName.text = rocket.PlayerName + ". Lap: " + currentLap;
        CurrentPlayerName.color = rocket.CarColor;
    }

    public void ChangeStateCameras()
    {
        currentCameraNumber++;

        for (int i = 0; i < ColorBoxContainer.Length; i++)
        {
            var color = ColorBoxContainer[i].GetComponent<Outline>().effectColor;
            color.a = 0;
        }

        if (currentCameraNumber == ApplicationMain.RaceCars.Count + 1)
        {
            currentCameraNumber = 0;
            autoUpdateCamera = true;
            MainCamera.orthographicSize = orthographicSize + 4;
        }
        else
        {
            var color = ColorBoxContainer[currentCameraNumber - 1].GetComponent<Outline>().effectColor;
            color.a = 1;

            //currentRaceCarTransform = raceCarScripts[currentCameraNumber - 1].transform;
            //CurrentPlayerName.text = raceCarScripts[currentCameraNumber - 1].PlayerName;
            //CurrentPlayerName.color = raceCarScripts[currentCameraNumber - 1].CarColor;
            UpdateRocketInfo(currentCameraNumber - 1);
            autoUpdateCamera = false;
            MainCamera.orthographicSize = orthographicSize;
        }
    }

    private Transform currentRaceCarTransform;

    private void Update()
    {
        if (raceCarScripts.Length == 0/* || currentCameraNumber == 0*/)
            return;

        var position = currentRaceCarTransform.position;
        var newPosition = new Vector3(position.x, position.y, CameraHeight);
        MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, newPosition, Time.deltaTime * 5);
        //MainCamera.transform.localPosition = new Vector3(position.x, position.y, CameraHeight);
    }

    private void ChangeColorBackgroundCar(Color color, /*Text textPlayerUi, */RaceCarScript raceCarScript)
    {
        if (raceCarScripts == null)
            return;

        var main = raceCarScript.Fire.main;
        main.startColor = color;
        //textPlayerUi.color = color;
    }
}

public static class Program
{
    //метод для обмена элементов массива
    static void Swap(ref int x, ref int y)
    {
        var t = x;
        x = y;
        y = t;
    }

    //метод возвращающий индекс опорного элемента
    static int Partition(int[] array, int minIndex, int maxIndex)
    {
        var pivot = minIndex - 1;
        for (var i = minIndex; i < maxIndex; i++)
        {
            if (array[i] < array[maxIndex])
            {
                pivot++;
                Swap(ref array[pivot], ref array[i]);
            }
        }

        pivot++;
        Swap(ref array[pivot], ref array[maxIndex]);
        return pivot;
    }

    //быстрая сортировка
    public static int[] QuickSort(int[] array, int minIndex, int maxIndex)
    {
        if (minIndex >= maxIndex)
        {
            return array;
        }

        var pivotIndex = Partition(array, minIndex, maxIndex);
        QuickSort(array, minIndex, pivotIndex - 1);
        QuickSort(array, pivotIndex + 1, maxIndex);

        return array;
    }

    public static int[] QuickSort(int[] array)
    {
        return QuickSort(array, 0, array.Length - 1);
    }
}
