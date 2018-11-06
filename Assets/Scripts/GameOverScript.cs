using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public Text tensText, twentyText, fiftyText, oneText, twoText, totalText, highScoreText;
    public Button playButton, exitButton;
    public GameObject particles;

    private int tens = 0, twenties = 0, fifties = 0, ones = 0, twos = 0;
    private float total, highScore;
    private GameObject dataManager;
    private float timer = 0.3f;
    private bool skip = false;

    // Use this for initialization
    void Start()
    {
        dataManager = GameObject.Find("DataManager");
        playButton.enabled = false;
        exitButton.enabled = false;
        highScore = PlayerPrefs.GetFloat("HighScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        CountCoins();
        SetUIText();
    }

    private void CountCoins()
    {
        if (Input.GetMouseButtonDown(0))
        {
            skip = true;
        }

        if (timer <= 0.0f)
        {
            if (twos < dataManager.GetComponent<DataManagerScript>().twos)
            {
                twos++;
                total += 2.0f;
                timer = 0.1f;
                if (skip)
                {
                    total -= twos;
                    twos = dataManager.GetComponent<DataManagerScript>().twos;
                    total += twos;
                    skip = false;
                }
            }
            else if (ones < dataManager.GetComponent<DataManagerScript>().ones)
            {
                ones++;
                total += 1.0f;
                timer = 0.06f;
                if (skip)
                {
                    total -= ones;
                    ones = dataManager.GetComponent<DataManagerScript>().ones;
                    total += ones;
                    skip = false;
                }
            }
            else if (fifties < dataManager.GetComponent<DataManagerScript>().fifties)
            {
                fifties++;
                total += 0.50f;
                timer = 0.03f;
                if (skip)
                {
                    total -= fifties * 0.50f;
                    fifties = dataManager.GetComponent<DataManagerScript>().fifties;
                    total += fifties * 0.50f;
                    skip = false;
                }
            }
            else if (twenties < dataManager.GetComponent<DataManagerScript>().twenties)
            {
                twenties++;
                total += 0.20f;
                timer = 0.02f;
                if (skip)
                {
                    total -= twenties * 0.20f;
                    twenties = dataManager.GetComponent<DataManagerScript>().twenties;
                    total += twenties * 0.20f;
                    skip = false;
                }
            }
            else if (tens < dataManager.GetComponent<DataManagerScript>().tens)
            {
                tens++;
                total += 0.10f;
                timer = 0.01f;
                if (skip)
                {
                    tens = dataManager.GetComponent<DataManagerScript>().tens;
                    total += tens * 0.10f;
                    skip = false;
                }
            }
            else
            {
                if (total > highScore)
                {
                    highScore = total;
                    PlayerPrefs.SetFloat("HighScore", highScore);
                    particles.GetComponent<ParticleSystem>().Play();
                }
                playButton.enabled = true;
                exitButton.enabled = true;
            }
        }

        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
    }

    private void SetUIText()
    {
        tensText.text = tens.ToString();
        twentyText.text = twenties.ToString();
        fiftyText.text = fifties.ToString();
        oneText.text = ones.ToString();
        twoText.text = twos.ToString();
        totalText.text = string.Format("{0:C}", total);
        highScoreText.text = string.Format("{0:C}", highScore);
    }
}
