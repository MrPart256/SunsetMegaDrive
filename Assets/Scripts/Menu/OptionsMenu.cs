using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class OptionsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public Dropdown resDrop;
    private int currentRes;
    private void Start()
    {
       resolutions= Screen.resolutions;
        resDrop.ClearOptions();
        List<string> res = new List<string>();
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            res.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentRes = i;
            }

        }
        resDrop.AddOptions(res);
        resDrop.value = currentRes;
        resDrop.RefreshShownValue();
    }
    public AudioMixer mixer;
    public void setVolume(float volume)
    {
        mixer.SetFloat("masterVolume",volume);
    }
    public void setQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }
    public void setFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }
    public void setResolution(int res)
    {
        Resolution resolution = resolutions[res];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }
    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
