using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] public int _lives = 3;
    [SerializeField] public int _maxlives = 3;
    [SerializeField] private float _playerSpeed = 8;
    [SerializeField] private float _turboSpeed = 11;
    [SerializeField] private float _rotationSpeed = 150;
    [SerializeField] private float _turboRotationSpeed = 180;
    [SerializeField] private Animator _lightAnimator;
    private float _currentTurboSpeedTime = 0;
    private bool _runTurboClock = false;
    private Vector3 _direction = Vector3.forward;
    public Action onHitWall;
    [SerializeField] private List<AudioClip> _shortFartSounds = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _secretSounds = new List<AudioClip>();
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

//    [MenuItem("GameObject/Create Other/Box collider")]
//    static void AddBoxCollider()
//    {
//        var selectedTransforms = Selection.transforms;
//        if (selectedTransforms!=null)
//        {
//            foreach (var selectedTransform in selectedTransforms)
//            {
//                CreateBoxCollider(selectedTransform.gameObject);
//            }
//        }
//        
//    }

    private static void CreateBoxCollider(GameObject gameObject)
    {
        gameObject.AddComponent<BoxCollider>();
        BoxCollider collider = (BoxCollider)gameObject.collider;
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        MeshFilter[] filters = gameObject.GetComponentsInChildren<MeshFilter>();

        foreach (MeshFilter f in filters)
        {
            bounds.Encapsulate(f.sharedMesh.bounds);
            bounds.center = Vector3.zero;

        }

        collider.size = bounds.size;
        collider.center = bounds.center;
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
            Invoke("PlayDelaySound", 0.3f);
            

            _runTurboClock = true;
            _lives--;

            if(onHitWall!=null)
            {
                onHitWall();
            }
            //onHitWall?.Invoke();

            if (_lives == 0)
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

    public void PlayDelaySound()
    {
        NGUITools.PlaySound(_secretSounds[Random.Range(0, _secretSounds.Count - 1)]);
    }
}

