using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
    public Transform RocketContainer;
    [HideInInspector]
    public List<RaceCarScript> RaceCars;
    //[HideInInspector] public Text[] PlayerNameText;
    public Text MaxLapText;
    //public GameObject TextContainer;
    public ColorBoxContainer colorBoxContainer;
    public CameraManager cameraManager;
    private ApplicationMain applicationMain;
    public GameObject OpenFinishWindow;
    public GamePlayFinish GamePlayFinish;
    public List<string> NameFinishRockets;
    public List<Color> ColorFinishRockets;
    public Dictionary<Color, bool> ALLColorRockets = new Dictionary<Color, bool>();
    public Dictionary<string, bool> AllNameRockets = new Dictionary<string, bool>();
    public Text startCountText;

    public void SetAllNameRockets(string carName)
    {

    }

    public void OnFinish(RaceCarScript raceCarScript)
    {
        string playerName = raceCarScript.PlayerName;
        
        AllNameRockets[playerName] = true;
        ALLColorRockets[raceCarScript.CarColor] = true;

        NameFinishRockets.Add(playerName);
        ColorFinishRockets.Add(raceCarScript.CarColor);

        applicationMain.GameIsPlaying = false;

        if (NameFinishRockets.Count == ApplicationMain.RaceCars.Count - 1)
        {
            for (int i = 0; i < NameFinishRockets.Count; i++)
            {
                GamePlayFinish.PlayersWon[i].gameObject.SetActive(true);
                GamePlayFinish.PlayersWon[i].text = NameFinishRockets[i] + " " + (i + 1);
                GamePlayFinish.PlayersWon[i].color = ColorFinishRockets[i];
            }

            var myKey = AllNameRockets.FirstOrDefault(x => x.Value == false).Key;
            var myColorKey = ALLColorRockets.FirstOrDefault(x => x.Value == false).Key;

            GamePlayFinish.PlayersWon[NameFinishRockets.Count].gameObject.SetActive(true);
            GamePlayFinish.PlayersWon[NameFinishRockets.Count].text = myKey + " " + (NameFinishRockets.Count + 1);
            GamePlayFinish.PlayersWon[NameFinishRockets.Count].color = myColorKey;

            OpenFinishWindow.SetActive(true);

            Time.timeScale = 0;
        }
    }

    private void Awake()
    {
        applicationMain = ApplicationMain.Instance;
        applicationMain.GamePlayManager = this;
        cameraManager.GamePlayManager = this;

        cameraManager.ColorBoxContainer = colorBoxContainer;

        //for (int i = 0; i < colorBoxContainer.transform.childCount; i++)
        //{
        //    cameraManager.ColorBoxContainer[i] = colorBoxContainer.transform.GetChild(i);
        //}
    }

    void Start()
    {
        if (CarsIsEmpty())
            return;

        var MainRaceCars = ApplicationMain.RaceCars;
        Debug.Log("MainRaceCars: " + MainRaceCars.Count);
        DisableAllCars();
  
        //PlayerNameText = new Text[RaceCars.Count];
        MaxLapText.text = "Total laps: " + CarLapCounter.MaxLapCount;

        RaceCars = new List<RaceCarScript>();

        for (int i = 0; i < MainRaceCars.Count; i++)
        {
            Debug.Log("---MainRaceCars: " + MainRaceCars.Count);
      
            RaceCars.Add(RocketContainer.GetChild(i).GetComponent<RaceCarScript>());
            RaceCars[i].gameObject.SetActive(true);
            RaceCars[i].CurrentCarLapCounter.onGameEnded += OnFinish;

            var texture = MainRaceCars[i].CarTexture;
            //Debug.Log("texture width: " + texture.width + "  texture heigth: " + texture.height);
            //var withCoeff = texture.width / 208.0f;
            //var heightCoeff = texture.height / 208.0f;
            //Debug.Log("withCoeff: " + withCoeff + "  heightCoeff: " + heightCoeff);
            //var width = texture.height > texture.width ? texture.width / heightCoeff : texture.width / withCoeff;
            //var height = texture.width > texture.height ? texture.height / withCoeff : texture.height / heightCoeff;
            //Debug.Log("width: " + width + "  height: " + height);
            //TextureScale.Bilinear(texture, (int)width, (int)height);
            //Debug.Log("ITOG texture width: " + texture.width + "  ITOG texture heigth: " + texture.height);

            RaceCars[i].CarSpriteRenderer.sprite =
                Sprite.Create(MainRaceCars[i].CarTexture, new Rect(0, 0, MainRaceCars[i].CarTexture.width, MainRaceCars[i].CarTexture.height), new Vector2(0.5f, 0.5f));

            RaceCars[i].CarSpriteRenderer.sprite.name = MainRaceCars[i].PlayerName + "_sprite";
            //RaceCars[i].CarSpriteRenderer.color = MainRaceCars[i].CarColor;
            //RaceCars[i].spriteOutline.SetColor(MainRaceCars[i].CarColor);
            RaceCars[i].CarSpriteRenderer.material.mainTexture = MainRaceCars[i].CarTexture;
            RaceCars[i].CarSpriteRenderer.material.shader = Shader.Find("Sprites/Outline");
            RaceCars[i].SubstanceName = MainRaceCars[i].SubstanceName/*ApplicationMain.makePhotoButtonData[i].PlayerNumber.text*/;
            RaceCars[i].CarColorName = MainRaceCars[i].CarColorName;
            RaceCars[i].PlayerName = MainRaceCars[i].PlayerName;
            RaceCars[i].CarTexture = MainRaceCars[i].CarTexture;
            RaceCars[i].PlayerFace = MainRaceCars[i].PlayerFace;
            RaceCars[i].CarColor = MainRaceCars[i].CarColor;
            RaceCars[i].cameraManager = cameraManager;
            RaceCars[i].MyPlayerFaceTransform = colorBoxContainer.transform.GetChild(i).transform;
            var coll = RaceCars[i].CarSpriteRenderer.gameObject.AddComponent<CapsuleCollider2D>();
            coll.size = new Vector2(coll.size.x - 0.4f, coll.size.y);

            //var secondColl = RaceCars[i].CarSpriteRenderer.gameObject.AddComponent<CapsuleCollider2D>();
            //secondColl.size = coll.size;
            //secondColl.isTrigger = true;

            //PlayerNameText[i] = TextContainer.transform.GetChild(i).GetComponent<Text>();
            //PlayerNameText[i].text = RaceCars[i].PlayerName + ". Lap 1";

            colorBoxContainer.SetColorBox(RaceCars[i].PlayerFace, RaceCars[i].CarColor, i, i, false);
            colorBoxContainer.EnableColorBox(i);

            ALLColorRockets.Add(RaceCars[i].CarColor, false);
            AllNameRockets.Add(RaceCars[i].PlayerName, false);
        }

        StartCoroutine(CheckStart());
    }

    private IEnumerator CheckStart()
    {
        var startCount = 3;

        startCountText.gameObject.SetActive(true);
        startCountText.text = startCount.ToString();

        while (startCount > 0)
        {
            yield return new WaitForSeconds(1);
            startCount--;
            startCountText.text = startCount.ToString();
        }

        for (int i = 0; i < RaceCars.Count; i++)
        {
            RaceCars[i].CurrentCarLapCounter.currentAiCarMovement.Start = true;
        }
        applicationMain.GameIsPlaying = true;
        startCountText.text = "GO!";

        yield return new WaitForSeconds(1);

        startCountText.gameObject.SetActive(false);

    }

    private void DisableAllCars()
    {
        //if (CarsIsEmpty())
        //    return;

        //for (int i = 0; i < RaceCars.Count; i++)
        //{
        //    RaceCars[i].gameObject.SetActive(false);
        //}

        for (int i = 0; i < RocketContainer.childCount; i++)
        {
            RocketContainer.GetChild(i).gameObject.SetActive(false);
        }
    }

    private bool CarsIsEmpty()
    {
        return RaceCars == null || RaceCars.Count == 0 ? true : false;
    }
}
