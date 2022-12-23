using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SoundManager : MonoBehaviour
{

    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioClip buttonClickAudioSource;


    private static System.Random r = new System.Random();
    private static float lastShotSound = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 1);
        }

        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        Debug.Log("sadasdasdasdsdasd");
    }

    public static float GetRandomVolume(int min, int max) {
        return (float)(r.Next(min, max) / 100.0);
    }


    public static bool AllowNextShotSound()
    {
        if (Time.time > lastShotSound + 0.10)
        {
            lastShotSound = Time.time;
            return true;
        }
        return false;
    }

    public void PlayButtonSound() {
        AudioSource.PlayClipAtPoint(buttonClickAudioSource, Camera.main.transform.position);
    }

}
