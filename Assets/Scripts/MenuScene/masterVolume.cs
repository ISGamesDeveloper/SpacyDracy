using UnityEngine;
using UnityEngine.UI;

public class masterVolume : MonoBehaviour
{
    public Slider mySlider;

    void Start()
    {
        mySlider.onValueChanged.AddListener(myVolume);
    }

    public void myVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
