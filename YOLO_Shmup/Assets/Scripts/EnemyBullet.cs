using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager _gameManager;
    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            _gameManager.UpdateLives(-1);
            Destroy(gameObject);
        }
    }
}
