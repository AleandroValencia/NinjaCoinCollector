using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject Coin;
    public GameObject boot;
    public GameObject lightning;
    public GameObject obstacle;
    public GameObject Player;
    public float timeLeft = 62.0f;
    public Text timerText;
    public Text tenText;
    public Text twentyText;
    public Text fiftyText;
    public Text oneText;
    public Text twoText;
    public GameObject pauseMenu;
    public int bootCount = 0;
    public int maxObstacles = 3;

    public int maxCoins = 50;
    private int numCoins = 0;
    private bool paused = false;
    private float delay = 0.0f;

    private int lightningChance = 500;
    public int obstacleCount = 0;

    private float timePassed = 0.0f;
    public float startupTimer = 1.75f;

    public AudioSource music;
    public Text readyText;

    private float volumeIncrement = -0.5f;

    // Use this for initialization
    void Start()
    {
        pauseMenu.SetActive(paused);
        GenerateCoins();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed > startupTimer)
        {
            readyText.text = "GO!";

            if (timePassed > 2.15f)
            {
                if (readyText.enabled == true)
                {
                    readyText.enabled = false;
                }
            }

            if (!paused)
            {
                timeLeft -= Time.deltaTime;
                timerText.text = timeLeft.ToString();

                if (bootCount < 1)
                {
                    if (Random.Range(0, 500) == 0)
                    {
                        Instantiate(boot);
                        bootCount++;
                    }
                }

                if (Random.Range(0, lightningChance) == 0)
                {
                    Instantiate(lightning);
                    if (lightningChance > 100)
                    {
                        lightningChance -= 50;
                    }
                }

                if (obstacleCount < maxObstacles)
                {
                    if (Random.Range(0, 500) == 0)
                    {
                        Instantiate(obstacle);
                        obstacleCount++;
                    }
                }
            }
        }

        SetUIText();
        SetDataManagerValues();

        if (timeLeft <= 0.0f)
        {
            timeLeft = 0.0f;
            paused = true;
            Player.GetComponent<PlayerScript>().SetMoving(!paused);
            delay += Time.deltaTime;
            if (delay > 0.30f)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
        else if (timeLeft <= 5.0f)
        {
            timerText.color = new Color(1.0f, 0.0f, 0.0f);
        }
    }

    public void PauseGame()
    {
        paused = !paused;
        Player.GetComponent<PlayerScript>().SetMoving(!paused);
        pauseMenu.SetActive(paused);
        volumeIncrement *= -1;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void SetUIText()
    {
        tenText.text = Player.GetComponent<PlayerScript>().tenCents.ToString();
        twentyText.text = Player.GetComponent<PlayerScript>().twentyCents.ToString();
        fiftyText.text = Player.GetComponent<PlayerScript>().fiftyCents.ToString();
        oneText.text = Player.GetComponent<PlayerScript>().oneDollars.ToString();
        twoText.text = Player.GetComponent<PlayerScript>().twoDollars.ToString();
    }

    private void SetDataManagerValues()
    {
        GameObject.Find("DataManager").GetComponent<DataManagerScript>().tens = Player.GetComponent<PlayerScript>().tenCents;
        GameObject.Find("DataManager").GetComponent<DataManagerScript>().twenties = Player.GetComponent<PlayerScript>().twentyCents;
        GameObject.Find("DataManager").GetComponent<DataManagerScript>().fifties = Player.GetComponent<PlayerScript>().fiftyCents;
        GameObject.Find("DataManager").GetComponent<DataManagerScript>().ones = Player.GetComponent<PlayerScript>().oneDollars;
        GameObject.Find("DataManager").GetComponent<DataManagerScript>().twos = Player.GetComponent<PlayerScript>().twoDollars;
    }

    private void GenerateCoins()
    {
        while (numCoins < maxCoins)
        {
            Instantiate(Coin);
            numCoins++;
        }
    }

    public float GetTimeRemaining()
    {
        return timeLeft;
    }
}
