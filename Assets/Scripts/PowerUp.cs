using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    
    
    [SerializeField ]
    private int powerupId; //0 = Powerup, 1 = Speed, 2 = Shields
    
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerupId)
                {
                    case 0: 
                        player.TripleShotActive();
                        break;
                    case 1: 
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldsActive();
                        break;
                    default:
                        Debug.Log("Welcome to the game");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
