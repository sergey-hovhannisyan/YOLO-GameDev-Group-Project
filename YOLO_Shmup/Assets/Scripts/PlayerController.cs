using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Characteristics
    float speed = 10f;
    float speedMultiplier = 2f;
    float angleSpeed = 50f;
    float bulletSpeed = 1000f;
    float timeInterval = 0.07f;
    bool hitTimeFlag = true;
    public int bulletLives = 30;

    Rigidbody2D _rigidbody2D;
    Renderer renderer;
    Collider2D collider;
    public GameObject selfExplosion;
    public GameObject bulletSpikes;
    public AudioClip selfExplosionClip;

    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public GameObject respawn_PS;
    public Transform spawn;
    public AudioClip gun_shot_clip;
    public AudioClip buzzer_clip;
    //Teleporting
    public GameObject teleportPrefab;
    public AudioClip teleport_clip;
    float period = 2f;
    float nextTeleport = 0f;

    public EnemySpawner _enemySpawner;

    private bool isShooting = false;
    private bool isTeleporting = false;

    public static string bulletType = "bullet";

    GameManager _gameManager;

    // Initialization

    public static void BulletType(string type){
        bulletType = type;
    }
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider2D>();
    }

    // Updates for every frame 
    void Update()
    {
        float xSpeed = Input.GetAxis("Horizontal") * speed;
        float ySpeed = Input.GetAxis("Vertical") * speed;
        _rigidbody2D.velocity = new Vector2(xSpeed, ySpeed);

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, angleSpeed * Time.deltaTime);
        // Fire check
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;
            StartCoroutine(FireGun());
        }
        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Rect screenBounds = new Rect(-8.25f, -4.5f, 16.5f, 9f);
            if (screenBounds.Contains(mousePos) && !isTeleporting)
            {
                isTeleporting = true;
                AudioSource.PlayClipAtPoint(teleport_clip, transform.position, 1f);
                Instantiate(teleportPrefab, mousePos, Quaternion.identity);
                renderer.enabled = false;
                collider.enabled = false;
                StartCoroutine(Teleport(mousePos));
            }
            else
            {
                AudioSource.PlayClipAtPoint(buzzer_clip, transform.position, 1f);
                StartCoroutine(WaitSeconds(1f));
            }
        }
    }

    private IEnumerator Teleport(Vector2 destination)
    {
        yield return new WaitForSeconds(period);
        transform.position = destination;
        renderer.enabled = true;
        collider.enabled = true;
        isTeleporting = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        print("collid");
        if (other.gameObject.CompareTag("Enemy") && hitTimeFlag)
        {
            hitTimeFlag = false;
            _gameManager.UpdateLives(-1);
            StartCoroutine(WaitSeconds(2f));
            hitTimeFlag = true;
        }
        if(other.gameObject.CompareTag("Heart") && hitTimeFlag){
            hitTimeFlag = false;
            _gameManager.UpdateLives(1);
            hitTimeFlag = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Hit with bullet
        if (other.CompareTag("EnemyBullet"))
        {
            Destroy(other.gameObject);
            Instantiate(bulletSpikes, transform.position, Quaternion.identity);
            if (bulletLives == 0)
            {
                AudioSource.PlayClipAtPoint(selfExplosionClip, transform.position, 1f);
                _gameManager.UpdateLives(-1);
            }
            else
            {
                AudioSource.PlayClipAtPoint(selfExplosionClip, transform.position, 0.3f);
                bulletLives--;
            }
        }
    }


    // Player respawn: 
    public void Respawn()
    {
        StartCoroutine(WaitSeconds(2f));
        Vector2 previousPosition = transform.position;
        Vector2 spawnPos = previousPosition;

        while (Vector2.Distance(previousPosition, spawnPos) < 2.0f)
        {
            spawnPos = new Vector2(Random.Range(-8.25f, 8.25f), Random.Range(-4.5f, 4.5f));
        }
        Instantiate(respawn_PS, spawnPos, Quaternion.identity);
        transform.position = spawnPos;
        _enemySpawner.enabled = false;
        StartCoroutine(WaitSeconds(3f));
        _enemySpawner.enabled = true;

    }
    private IEnumerator FireGun()
    {
        while (isShooting)
        {
            if(bulletType == "bullet"){
                GameObject newBullet = Instantiate(bulletPrefab, spawn.position, Quaternion.identity);
                newBullet.transform.rotation = transform.rotation;
                newBullet.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
                AudioSource.PlayClipAtPoint(gun_shot_clip, spawn.position, 1f);
                yield return new WaitForSeconds(timeInterval);
            }
            if(bulletType == "missile"){
                GameObject newMissile = Instantiate(missilePrefab, spawn.position, Quaternion.identity);
                newMissile.transform.rotation = transform.rotation;
                newMissile.GetComponent<Rigidbody2D>().AddForce(transform.up * missileSpeed);
                AudioSource.PlayClipAtPoint(missile_shot_clip, spawn.position, 1f);
                print("missile");
                yield return new WaitForSeconds(timeInterval);
                yield break;
            }
        }
    }

    // Wait coroutine
    private IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void UpdateBulletLives(int newLives)
    {
        bulletLives = newLives;
    }
}