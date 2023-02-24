using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    private GameObject player;
    public GameObject selfExplosion;
    public AudioClip selfExplosionClip;
    public int levelEnemies = 1;
    public GameObject enemyPrefab;
    public GameObject blackHolePrefab;
    public int timerLimit = 15;

    float timer=0;


    GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer=0;
        print("start");
        while(timer<=timerLimit){
            timer+=Time.deltaTime;
            print(timer);
        }   
        print("done");
        timer=0;
    }
    
    void spawnEnemy(){
        for (int i = 0; i < levelEnemies; i++)
        {
            print("spawning");
            while(timer<timerLimit){
                timer+=Time.deltaTime;
            }
            timer=0;
            Vector2 spawnPos = new Vector2(Random.Range(-8.25f, 8.25f), Random.Range(-4.5f, 4.5f));
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            Instantiate(blackHolePrefab, spawnPos, Quaternion.identity);
            
        }
    }
}
