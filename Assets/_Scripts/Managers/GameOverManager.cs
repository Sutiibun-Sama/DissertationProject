﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public float RestartDelay;
    private bool gameRunning;
    private float restartTimer;
    private float timeToRestart;
    private bool isOver;
    private GameObject player;
    private GameObject enemy;
    private GameObject gAManager;
    private Animator anim;
    private Text restartText;
    private AIController aIController; 
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;
    private FireBullets fireBullets;
    
    // One of the first functions to be called
    private void Awake()
    {
        anim = GetComponent<Animator>();
        restartText = GameObject.Find("RestartCountdown").GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        gAManager = GameObject.FindGameObjectWithTag("GAManager");
        aIController = gAManager.GetComponent<AIController>();
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = enemy.GetComponent<EnemyHealth>();
        fireBullets = enemy.GetComponent<FireBullets>();
        isOver = false;
        timeToRestart = RestartDelay;
        SetRestartText();
    }

    private void Start()
    {
        gameRunning = true;
    }

    private void SetRestartText()
    {
        restartText.text = timeToRestart.ToString();
    }

    private void Update()
    {
        if (IsGameOver())
        {
            if (gameRunning)
            {
                OnDeath();
            }

            DeathScreenAnimation();
        }
    }

    public bool IsGameOver()
    {
        if (playerHealth.currentHealth <= 0 || enemyHealth.currentHealth <= 0)
        {
            return true;
        }

        return false;
    }

    private void ShutGameDown()
    {
        if (aIController.Save.NumOfGames == aIController.Save.Population.Count)
        {
            Application.Quit();
        }
    }

    private void OnDeath()
    {
        gameRunning = false;
        //aIController.HealthFitnessFunction(aIController.Save.NumOfGames);
        //aIController.TimeFitnessFuntion(aIController.Save.NumOfGames);
        aIController.HealthAndTimeFitnessFunction(aIController.Save.NumOfGames);
        aIController.SaveGeneration(aIController.FullPath);
        anim.SetTrigger("GameOver");
        isOver = true;
        fireBullets.CeaseFire(isOver);
        ShutGameDown();
    }

    private void DeathScreenAnimation()
    {
        restartTimer += Time.deltaTime;
        timeToRestart -= Time.deltaTime;
        SetRestartText();
        if (restartTimer >= RestartDelay)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
