﻿using System.Collections;
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
    private Vector3 _laserOffset = new Vector3(0, 1.05f, 0);
    [SerializeField] 
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager spawnManager;
   
    private bool isTripleShotActive = false;
    private bool isSpeedIncreaseEnabled = false;
    
    void Start()
    {
        // Starting Position
        transform.position = new Vector3(0, 0, 0);
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
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
        _lives -= 1;
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
}
