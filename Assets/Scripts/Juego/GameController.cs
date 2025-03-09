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

    /// <summary>
    /// Detiene el juego y muestra la pantalla de fin indicando cuantos niveles sobrevivio
    /// </summary>
    void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        MusicManager.PauseBackgroundMusic();
        survivedText.text = "YOU SURVIVED " + survivedLevelsCount + " LEVEL";
        if (survivedLevelsCount != 1) survivedText.text += "S";
        Time.timeScale = 0;
    }

    /// <summary>
    /// Devuelve el juego a su estado original y hace que las cosas vuelvan a moverse
    /// </summary>
    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        MusicManager.PlayBackgroundMusic(true);
        survivedLevelsCount = 0;
        LoadLevel(0, false);
        OnReset.Invoke();
        Time.timeScale = 1;
    }

    /// <summary>
    /// Carga un nivel
    /// </summary>
    /// <param name="level">Nivel a cargar</param>
    /// <param name="wantSurvivedIncrease">Indica si debe aumentarse la cantidad de niveles sobrevividos, puede ser false en caso de reset del juego</param>
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

    /// <summary>
    /// Incrementa el valor de la barra de progreso del juego
    /// Al llegar al tope habilita la posibilidad de cambiar de nivel
    /// </summary>
    /// <param name="amount"></param>
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

    /// <summary>
    /// Carga el siguiente nivel, en caso de no haber más niveles volvera al nivel de inicio
    /// </summary>
    void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        LoadLevel(nextLevelIndex, true);
    }
}
