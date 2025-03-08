using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        progressAmmount = 0;
        progressSlider.value = 0;
        Gema.OnGemCollect += IncreaseProgressAmmount;
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
        LoadCanvas.SetActive(false);
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
        LoadCanvas.SetActive(false);

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[nextLevelIndex].gameObject.SetActive(true);

        player.transform.position = new Vector3(0, 0, 0);
        progressAmmount = 0;
        progressSlider.value = 0;
    }
}
