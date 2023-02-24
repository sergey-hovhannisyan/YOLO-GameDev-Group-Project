using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class spawner1 : MonoBehaviour
{
    public int timerLimit = 1;
    float timer=0;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        //Random rnd = new Random();
    }

    // Update is called once per frame
    void Update()
    {
        System.Random random = new System.Random();
        timerLimit = random.Next(1,4);
        timer+=Time.deltaTime;
        if(timer>timerLimit){
            Instantiate(enemy, transform.position,Quaternion.identity);
            timer=0;
        }
    }
}
