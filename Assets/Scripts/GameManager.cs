using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random=UnityEngine.Random;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text timerText;
    public bool stopWatchActive;
    private float currentTime;

    public GameObject winScreeen;
    public GameObject appearParticles;
    public GameObject goobPrefab;
    public Transform[] spawnPoints;
    public GameObject[] goobs;
    public TMP_Text goobCountText;
    public int goobCount;
    public float waitTime;
    private int spawnIndex;

    void Start()
    {
        stopWatchActive = true;
        StartCoroutine(SpawnGoob());
    }

    void Update()
    {
        goobs = GameObject.FindGameObjectsWithTag("Goob");
        goobCount = goobs.Length;
        goobCountText.text = $"{goobCount}";

        if (stopWatchActive) {
            currentTime = currentTime + Time.deltaTime;
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
        timerText.text = "Total Time: " + timeSpan.Minutes.ToString() + ":" + timeSpan.Seconds.ToString() + ":" + timeSpan.Milliseconds.ToString();

        if (goobCount == 0) {
            stopWatchActive = false;
            winScreeen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    IEnumerator SpawnGoob() {
        spawnIndex = Random.Range(0, spawnPoints.Length);
        if (spawnPoints[spawnIndex].GetComponent<SpawnPoint>().goobIn) {
            spawnIndex = Random.Range(0, spawnPoints.Length);
        }

        else {
            Instantiate(goobPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
            Instantiate(appearParticles, spawnPoints[spawnIndex].position, Quaternion.identity);
        }

        yield return new WaitForSeconds(waitTime);
        StartCoroutine(SpawnGoob());
    }
}
