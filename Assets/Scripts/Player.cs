using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _maxlives = 3;
    [SerializeField] private float _playerSpeed = 8;
    [SerializeField] private float _turboSpeed = 11;
    [SerializeField] private float _rotationSpeed = 150;
    [SerializeField] private float _turboRotationSpeed = 180;
    [SerializeField] private Animator _lightAnimator;
    private float _currentTurboSpeedTime = 0;
    private bool _runTurboClock = false;
    private Vector3 _direction = Vector3.forward;
    [SerializeField]
    private List<AudioClip> _shortFartSounds = new List<AudioClip>();
    // Use this for initialization
    void Start()
    {
        _lives = _maxlives;
    }

    // Update is called once per frame
    void Update()
    {
        var playerSpeed = 0f;
        var rotationSpeed = 0f;

        if (_runTurboClock)
        {
            if (_currentTurboSpeedTime <= 1)
            {
                _currentTurboSpeedTime += Time.deltaTime;
            }
            else
            {
                _runTurboClock = false;
                _currentTurboSpeedTime = 0;
            }
        }

        if (_runTurboClock)
        {
            playerSpeed = _turboSpeed;
            rotationSpeed = _turboRotationSpeed;
        }
        else
        {
            playerSpeed = _playerSpeed;
            rotationSpeed = _rotationSpeed;
        }


        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetMouseButton(0))
        {
            transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetMouseButton(1))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }

        transform.Translate(_direction * Time.deltaTime * playerSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            var playerRotation = transform.rotation.eulerAngles;
            playerRotation.y += 180;

            iTween.RotateTo(gameObject, playerRotation, 0.3f);
            _lightAnimator.SetTrigger("triggerFlash");
            NGUITools.PlaySound(_shortFartSounds[Random.Range(0, _shortFartSounds.Count - 1)]);

            _runTurboClock = true;
            _lives--;

            if (_lives==0)
            {
                _lives = _maxlives;
                GameController.LevelLost();
            }

        }
        else if (other.gameObject.tag == "EndCheckPoint")
        {
            GameController.LevelWin();
        }
    }
}

