using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 0;
    [HideInInspector]public bool IsFinished = false;
    private Input_Controls _input;
    private bool _isGameStart = false;
    private Vector3 _positionBuffer;

    void Move()
    {
        _positionBuffer.z += speed * Time.deltaTime;
        gameObject.transform.position = _positionBuffer;
    }

    private void Awake()
    {
        _input = new Input_Controls();
        _input.Action_Map.Tap.performed += context =>
        {
            if (_isGameStart == false)
            {
                StartCoroutine(this.GetComponent<MoveScript>().Braking());
            }
            _isGameStart = true;
        };
        _positionBuffer = gameObject.transform.position;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
    
    void FixedUpdate()
    {
        if (_isGameStart)
        {
            if(IsFinished == false)
            {
                Move();
            }
            else
            {

            }
        }
    }
}
