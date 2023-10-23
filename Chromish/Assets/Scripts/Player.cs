using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [SerializeField] GameInput gameInput;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 100f;
    [SerializeField] private float sprintSpeedBoost = 5f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private Transform bulletPrefab;

    public float currentSpeed;
    private Vector3 mouseWorldPosition;

    private Rigidbody rb;
    private HealthSystem playerHealth;
    //private AudioSource playerAmbience;

    public static Player Instance { get; private set; }

    private void Awake() {
        Instance = this;

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        GameInput.jumpInput += HandleJumping;
        GameInput.sprintInput += StartSprinting;
        GameInput.sprintInputCanceled += StopSprinting;

        playerHealth = transform.GetComponent<HealthSystem>();
        playerHealth.OnDamaged += PlayerHealth_OnDamaged;
        playerHealth.OnDied += PlayerHealth_OnDied;

        currentSpeed = moveSpeed;
    }

    private void OnDisable() {
        GameInput.jumpInput -= HandleJumping;
        GameInput.sprintInput -= StartSprinting;
        GameInput.sprintInputCanceled -= StopSprinting;

        playerHealth.OnDamaged -= PlayerHealth_OnDamaged;
        playerHealth.OnDied -= PlayerHealth_OnDied;
    }

    private void PlayerHealth_OnDied(object sender, System.EventArgs e) {
        GameOverUI.Instance.Show();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    private void PlayerHealth_OnDamaged(object sender, System.EventArgs e) {
        ChromaticAbberationEffect.Instance.SetIntensity(0.2f);
        CameraShakeEffect.Instance.ShakeCamera(3f, .1f);
    }

    private void FixedUpdate() {
        HandleCameraRotation();
        HandleMovement();
    }

    private void Update() {

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderMask)) {
            mouseWorldPosition = hit.point;
        }

    }
    private void HandleCameraRotation() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        if (inputVector != Vector2.zero) {
            float rotationSpeed = 4f;
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f; 
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cameraForward), Time.deltaTime * rotationSpeed);
        }

    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 localDirection = new Vector3(inputVector.x, 0, inputVector.y).normalized;
        Vector3 worldDirection = transform.TransformDirection(localDirection);

        Vector3 moveDir = worldDirection;

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.8f;
        float playerHeight = 3f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (canMove) {
            transform.position += moveDir * currentSpeed * Time.deltaTime;
        }

    }

    private void StartSprinting() {
        currentSpeed += sprintSpeedBoost;
    }
    private void StopSprinting() {
        currentSpeed -= sprintSpeedBoost;
    }

    private void HandleJumping() {
        float distanceToGround = 1.8f;
        RaycastHit hit;

        bool canJump = Physics.Raycast(transform.position, Vector3.down, out hit, distanceToGround, groundLayerMask);

        if (canJump && hit.collider != null) {
             rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
        }
        
    }


}
