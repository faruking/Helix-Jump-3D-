using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject gamePlayPanel;
    public GameObject startMenuPanel;
    public GameObject levelCompletedPanel;
    public GameObject PlayButton;
    public GameObject TimeAttackButton;
    public GameObject nightButton;
    public GameObject dayButton;

    public static bool gameOver;
    public static bool levelCompleted;
    public static bool isGameStarted;
    public static bool mute = false;
    private  bool timerMode = false;
    public static int currentLevelIndex;
    public static int score = 0;
    private float timeLeft = 60f; 

    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI timerText;

    public static int numberOfPassedRings;
    public Slider gameProgressSlider;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        numberOfPassedRings  = 0;
        highScoreText.text = "Best Score\n" + PlayerPrefs.GetInt("HighScore",0);
        isGameStarted = levelCompleted =  gameOver = false;
          if (currentLevelIndex > 1)
        {
            Play();
            if(PlayerPrefs.GetInt("timerMode") == 1){
                // timerText.SetActive(true);
                StartCoroutine(StartCountdown(timeLeft));
            }
        }
    }

    private void Awake(){
        currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex",1);
        Debug.Log(currentLevelIndex);
    }
    // Update is called once per frame
    void Update()
    {
        // update UI
        currentLevelText.text = currentLevelIndex.ToString();
        nextLevelText.text = (currentLevelIndex+1).ToString();

        int progress = (numberOfPassedRings * 100) / FindObjectOfType<HelixManager>().numberOfRings;
        gameProgressSlider.value = progress;

        scoreText.text = score.ToString();

        // Start level
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !isGameStarted)
        {
            if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            isGameStarted = true;
            gamePlayPanel.SetActive(true);
            startMenuPanel.SetActive(false);
        }
    
        // Game Over
        if(gameOver){
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
            PlayerPrefs.SetInt("CurrentLevelIndex",1);
            if (Input.GetButtonDown("Fire1"))
            {
                if (score > PlayerPrefs.GetInt("HighScore",0))
                {
                    PlayerPrefs.SetInt("HighScore",score);
                }
                score = 0;
                SceneManager.LoadScene(0);
            }
        
        }
        if(levelCompleted){
            levelCompletedPanel.SetActive(true);
            if (Input.GetButtonDown("Fire1"))
            {
                PlayerPrefs.SetInt("CurrentLevelIndex",currentLevelIndex+1);
                SceneManager.LoadScene(0);
            }
        }
    }
    public void Play(){
        isGameStarted = true;
        PlayButton.SetActive(false);
        TimeAttackButton.SetActive(false);
        gamePlayPanel.SetActive(true);
        startMenuPanel.SetActive(false);
    }
     // Countdown timer that ends the game when it gets to zero. 
    public IEnumerator StartCountdown(float countdownValue)
    {
        timerText.text = "Timer: " + countdownValue;
        while (countdownValue > 0 && isGameStarted && !levelCompleted)
        {
            yield return new WaitForSeconds(1.0f);
            countdownValue--;
            timerText.text = "Timer: " + countdownValue;
        }
        if(countdownValue <= 0){
            gameOver = true;
        }
    }

    public void TimerPlayMode(){
        Play();
        timerMode = true;
        PlayerPrefs.SetInt("timerMode",1);
        StartCoroutine(StartCountdown(timeLeft));
    }
    public void NightMode(){
        Camera.main.backgroundColor = Color.black;
        dayButton.SetActive(true);
        nightButton.SetActive(false);
    }
    public void DayMode(){
        Camera.main.backgroundColor = Color.blue;
        dayButton.SetActive(false);
        nightButton.SetActive(true);
    }
}
