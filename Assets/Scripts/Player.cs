using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    private float _speedIncrease = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldsVisualizer;
    // [SerializeField]
    // private GameObject _shieldsPrefab;
    private Vector3 _laserOffset = new Vector3(0, 1.05f, 0);
    [SerializeField] 
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private int _score;
   
    private bool isTripleShotActive = false;
    private bool isSpeedIncreaseEnabled = false;
    private bool isShieldsActive = false;
    private bool shieldsVisualizer = false;
    
    void Start()
    {
        // Starting Position
        transform.position = new Vector3(0, 0, 0);
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UIManager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            SpawnLaser();
        }
    }

    void SpawnLaser()
    { 
        
        _canFire = Time.time + _fireRate;
        if (isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
        }
        
        
    }
    
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");
        
                Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
                transform.Translate(direction * _speed * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.5f, 0), 0);
                if (transform.position.x >= 9.24f)
                {
                    transform.position = new Vector3(-9.24f, transform.position.y, 0);
                } else if (transform.position.x <= -9.24f)
                {
                    transform.position = new Vector3(9.24f, transform.position.y, 0);
                }
    }

    public void Damage()
    {
        if (isShieldsActive == true)
        {
            isShieldsActive = false;
            _shieldsVisualizer.SetActive(false);
            return;
        }
        else
        {
            
             _lives -= 1;
        }
       
        if (_lives < 1)
        {
            spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        isTripleShotActive = false;

    }

    public void SpeedBoostActive()
    {
        isSpeedIncreaseEnabled = true;
        _speed *= _speedIncrease;
        StartCoroutine(SpeedBoostCooldown());
    }

    IEnumerator SpeedBoostCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        isSpeedIncreaseEnabled = false;
        _speed /= _speedIncrease;
    }

    public void ShieldsActive()
    {
        isShieldsActive = true;
        _shieldsVisualizer.SetActive(true);
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.AppendScore(_score);
    }
    
}
