using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static int _lives = 3;
    private static float _playerSpeed = 8;
    private static float _turboSpeed = 11;
    private static float _rotationSpeed = 150;
    private static float _turboRotationSpeed = 180;
    [SerializeField] private Animator _lightAnimator;
    private float _currentTurboSpeedTime = 0;
    private bool _runTurboClock = false;
    private Vector3 _direction = Vector3.forward;
    [SerializeField]
    private List<AudioClip> _shortFartSounds = new List<AudioClip>();
    // Use this for initialization
    void Start()
    {

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
                _lives = 3;

            }

        }
        else if (other.gameObject.tag == "EndCheckPoint")
        {
            GameController.LevelWin();
        }
    }
}

