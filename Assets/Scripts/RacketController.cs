using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketController : MonoBehaviour
{
    public static RacketController Instance { get; private set; }

    [SerializeField] private float racketSpeed = 10f;
    [SerializeField] private float stopDistance = 0.1f; // Minimum ditance for a racket to move
    [SerializeField] private float defaultRotation = 0f; // Default rotation of racket, when not moving
    [SerializeField] private float rotationSmoothTime = 0.1f; // Time of smooth transition to a basic rotation of racket
    [SerializeField] private float tiltAngleMax = 30f; // Angle amount of racket, when moving
    [SerializeField] private float racketSize; // Size of a racket
    [Range(0, 1)]
    [SerializeField] private float correctionAmount; // How good is racket hits are going to be. 0 - no help, 1 - every shot is perfect
    //[SerializeField] private float racketOffset;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private float currentRotation;
    private float rotationVelocity;
    private Vector2 direction;

    private void Awake()
    {
        Instance = this;

        rb = GetComponent<Rigidbody2D>();


        racketSpeed = GameAssets.Instance.racketSpeed.levelUpgrades[PlayerPrefs.GetInt(GameAssets.Instance.racketSpeed.levelSaveName, 0)];
        racketSize = GameAssets.Instance.racketSize.levelUpgrades[PlayerPrefs.GetInt(GameAssets.Instance.racketSize.levelSaveName, 0)];
        correctionAmount = GameAssets.Instance.racketAccuracy.levelUpgrades[PlayerPrefs.GetInt(GameAssets.Instance.racketAccuracy.levelSaveName, 0)];

        transform.localScale = Vector3.one * racketSize;
    }

    private void Start()
    {
        targetPosition = rb.position;

        UIControllerZone.OnPlayingZoneClicked += SetTargetPosition;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            float distance = Vector2.Distance(rb.position, targetPosition);

            if (distance > stopDistance)
            {
                direction = (targetPosition - rb.position).normalized;
                Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, racketSpeed * Time.fixedDeltaTime);
                rb.MovePosition(newPosition);
                UpdatePaddleRotation(direction);
            }
            else
            {
                rb.MovePosition(targetPosition);
                isMoving = false;
            }
        }

        // Smooth come back in basic direction of racket
        if (!isMoving)
        {
            float targetRotation = defaultRotation;
            currentRotation = Mathf.SmoothDampAngle(currentRotation, targetRotation, ref rotationVelocity, rotationSmoothTime);
            rb.rotation = currentRotation;
        }
    }

    public void SetTargetPosition(Vector2 targetPosition)
    {
        //targetPosition.y += racketOffset;
        this.targetPosition = targetPosition;
        isMoving = true;
    }

    private void UpdatePaddleRotation(Vector2 direction)
    {
        // If racket move left - mirror it
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(-racketSize, racketSize, racketSize);
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector3(racketSize, racketSize, racketSize);
        }
        float angle = Mathf.Sign(direction.x) * -tiltAngleMax;
        float angleLerped = Mathf.Lerp(rb.rotation, angle, 10 * Time.deltaTime);
        rb.rotation = angleLerped; // Angle of rotation for moving right

        currentRotation = rb.rotation;
    }

    public Vector3 GetRacketPosition()
    {
        return transform.position;
    }
    public float GetCorrectionAmount()
    {
        return correctionAmount;
    }
}