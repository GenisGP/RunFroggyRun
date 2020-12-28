using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu, optionsMenu, levelOne, levelLocked;
    public TMP_Text maxDistanceTraveledText, distancePercentageText, maxFliesText, commpleteText;

    public Animator transition;

    public Level level;
    private void Start()
    {
        ReadMaxDistance();
        ReadMaxFlies();

        Time.timeScale = 1f;
    }

    public void Play()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);

        transition.SetTrigger("WaveUp");



        yield return null;


    }

    public void EnterOptions()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void ExitOptions()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    //Level Select
    public void NextLevel()
    {
        levelOne.SetActive(false);
        levelLocked.SetActive(true);
    }
    public void PreviousLevel()
    {
        levelOne.SetActive(true);
        levelLocked.SetActive(false);
    }
    public void ReadMaxDistance()
    {
        float maxDistance = PlayerPrefs.GetFloat("MaxDistance", 0);
        float maxDistancePercentage = (maxDistance / level.distance) * 100;
        maxDistanceTraveledText.text = maxDistance.ToString("F0");
        distancePercentageText.text = " (" + maxDistancePercentage.ToString("F0") + "%)";

        if (Mathf.Ceil(maxDistance) >= level.distance)
        {
            distancePercentageText.color = new Color(65f / 255f, 246f / 255f, 32f / 255f);
            commpleteText.text = "COMPLETE";
            commpleteText.color = new Color(31f / 255f, 236f / 255f, 45f / 255f);
        }
    }
    public void ReadMaxFlies()
    {
        float maxFlies = PlayerPrefs.GetFloat("MaxFlies", 0);
        float totalFlies = level.flies;
        maxFliesText.text = maxFlies.ToString() + " / " + totalFlies.ToString();
        
        if(maxFlies >= totalFlies)
        {
            maxFliesText.color = new Color(31f / 255f, 236f / 255f, 45f / 255f);
        }
        
    }

}
