using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;

    //public float horizontalInput;
    
    // Start is called before the first frame update
    void Start()
    {
        // Starting Position
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        
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
}
