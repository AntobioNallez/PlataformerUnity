using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressAmmount;
    public Slider progressSlider;
    public GameObject player;
    public GameObject LoadCanvas;
    public List<GameObject> levels;
    private int currentLevelIndex = 0;

    public GameObject gameOverScreen;
    public TMP_Text survivedText;
    private int survivedLevelsCount;

    public static event Action OnReset;

    // Start is called before the first frame update
    void Start()
    {
        progressAmmount = 0;
        progressSlider.value = 0;
        Gema.OnGemCollect += IncreaseProgressAmmount;
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
        PlayerHealth.OnPlayerDie += GameOverScreen;
        LoadCanvas.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        MusicManager.PauseBackgroundMusic();
        survivedText.text = "YOU SURVIVED " + survivedLevelsCount + " LEVEL";
        if (survivedLevelsCount != 1) survivedText.text += "S";
        Time.timeScale = 0;
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        MusicManager.PlayBackgroundMusic(true);
        survivedLevelsCount = 0;
        LoadLevel(0, false);
        OnReset.Invoke();
        Time.timeScale = 1;
    }

    void LoadLevel(int level, bool wantSurvivedIncrease) {
        LoadCanvas.SetActive(false);

        levels[currentLevelIndex].SetActive(false);
        levels[level].SetActive(true);

        currentLevelIndex = level;
        player.transform.position = new Vector3(0, 0, 0);
        progressAmmount = 0;
        progressSlider.value = 0;
        
        if(wantSurvivedIncrease) survivedLevelsCount++;
    }

    void IncreaseProgressAmmount(int amount)
    {
        progressAmmount += amount;
        progressSlider.value = progressAmmount;
        if (progressAmmount >= 100)
        {
            //LEVEL FINISHED
            LoadCanvas.SetActive(true);
            Debug.Log("Has terminado el nivel");
        }
    }

    void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        LoadLevel(nextLevelIndex, true);
    }
}
