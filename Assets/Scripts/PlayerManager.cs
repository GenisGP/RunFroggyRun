using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public PlayerController player;

    public static bool gamePaused;
    public static bool gameOver;
    private bool inGameOverMenu = false;
    public GameObject gameOverPanel;
    public GameObject distanceNoRecord, distanceRecord; //Para cuando el jugador logre un record de distancia se mostrará el objeto de record
    public TMP_Text distanceTraveledText, distanceTraveledRecord;
    public TMP_Text maxDistanceTraveledText;

    public int totalFliesInLevel;
    public static bool levelCompleted;
    public GameObject levelCompletedPanel;
    public TMP_Text distanceEndText, fliesEndText;

    public static bool isGameStarted;
    public GameObject startingText;

    public static int numberOfCoins;
    public TMP_Text coinsText;

    public static float distanceScore;
    public TMP_Text disanceText;

    public GameObject inGameUI;

    public AudioManager aManager;

    public bool resetScore;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; //Ya que sinó al reiniciar el juego se queda la escala a 0

        gameOver = false;
        levelCompleted = false;
        isGameStarted = false;
        gamePaused = false;
        numberOfCoins = 0;
        distanceScore = 0;

        if (resetScore)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver && !inGameOverMenu)
        {
            GameOver();
        }

        if (levelCompleted)
        {
            LevelCompleted();
        }
        coinsText.text = (numberOfCoins.ToString());
        disanceText.text = (distanceScore.ToString("F0"));

        if (SwipeManager.tap)
        {
            isGameStarted = true;
            startingText.SetActive(false);
        }
    }

    private void GameOver()
    {
        //Se escala el tiempo a 0
        Time.timeScale = 0;

        //Audio
        aManager.StopAllSounds();
        aManager.PlaySound("GameOver");

        //Leemos el record o distancia máxima que el player ha llegado a lograr y se guardan datos
        float maxDistance = PlayerPrefs.GetFloat("MaxDistance", 0);
        maxDistanceTraveledText.text = maxDistance.ToString("F0");
        if (distanceScore > maxDistance)
        {
            distanceTraveledRecord.text = distanceScore.ToString("F0");
            distanceNoRecord.SetActive(false);
            distanceRecord.SetActive(true);
            PlayerPrefs.SetFloat("MaxDistance", distanceScore);
        }

        //Leemos el record de moscas comidas que el player ha llegado a lograr y se guardan datos
        float maxFlies = PlayerPrefs.GetFloat("MaxFlies", 0);
        if (numberOfCoins > maxFlies)
        {
            PlayerPrefs.SetFloat("MaxFlies", numberOfCoins);
        }
        PlayerPrefs.Save();

        distanceTraveledText.text = disanceText.text;
        gameOverPanel.SetActive(true);
        inGameUI.SetActive(false);

        inGameOverMenu = true;

    }

    private void LevelCompleted()
    {
        //Se escala el tiempo a 0
        Time.timeScale = 0;

        //Audio
        aManager.StopAllSounds();      

        //Leemos el record o distancia máxima que el player ha llegado a lograr y se guardan datos
        float maxDistance = PlayerPrefs.GetFloat("MaxDistance", 0);

        if (distanceScore > maxDistance)
        {
            PlayerPrefs.SetFloat("MaxDistance", distanceScore);
        }

        //Leemos el record de moscas comidas que el player ha llegado a lograr y se guardan datos
        float maxFlies = PlayerPrefs.GetFloat("MaxFlies", 0);
        if (numberOfCoins > maxFlies)
        {
            PlayerPrefs.SetFloat("MaxFlies", numberOfCoins);
        }
        PlayerPrefs.Save();

        distanceEndText.text = disanceText.text;
        fliesEndText.text = coinsText.text + "/" + totalFliesInLevel;

        levelCompletedPanel.SetActive(true);
        inGameUI.SetActive(false);
    }
}
