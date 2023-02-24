using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : MonoBehaviour
{
    public int speed = 50;
    Rigidbody2D _rigidbody2D;
    GameManager _gameManager;

    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.AddForce(new Vector2(0, -speed));
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            _gameManager.UpdateLives(1);
            Destroy(gameObject);
        }
    }
}
