using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private JoystickController joystickController;
    private CharacterController characterController;
    [SerializeField] private PlayerAnimator playerAnimator;
    private Transform cameraTransform;
    Vector3 moveVector;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float referenceResolution;
    
    [Header("Joystick Settings")]
    [SerializeField] private float joystickMaxRadius = 100f;

    private float gravity = -9.81f;
    private float gravityMultiplier = 3f;
    private float gravityVelocity;
    void Start()
    {
        characterController = GetComponentInChildren<CharacterController>();
        if (characterController == null)
            Debug.LogError("CharacterController bulunamadı!");
        
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        MovePlayer();
    }
    
    private void MovePlayer()
    {
        Vector2 rawInput = joystickController.GetMovePosition();
        Vector2 normalizedInput = NormalizeJoystickInput(rawInput);
        
        // Kameranın yönüne göre hareket vektörü oluştur
        Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
        
        Vector3 moveDirection = (cameraForward * normalizedInput.y + cameraRight * normalizedInput.x);
        
        // 3. Ekran yönüne göre ölçeklendirme faktörü
        float aspectRatioFactor = GetAspectRatioCorrection();
        
        // 4. Hareket vektörünü hesapla
        moveVector = moveDirection * moveSpeed * Time.deltaTime * aspectRatioFactor;
        
        playerAnimator.ManageAnimations(moveVector);
        
        ApplyGravity();
        characterController.Move(moveVector);
    }
    
    private Vector2 NormalizeJoystickInput(Vector2 input)
    {
        // Joystick inputunu normalize et [-1, 1]
        float magnitude = Mathf.Clamp01(input.magnitude / joystickMaxRadius);
        Vector2 direction = input.normalized;
        return direction * magnitude;
    }
    
    private float GetAspectRatioCorrection()
    {
        // Ekran yönüne göre düzeltme faktörü
        float currentAspect = (float)Screen.width / Screen.height;
        float referenceAspect = 9f / 16f; // 16:9 referans
        
        // Portre modu düzeltmesi
        if (currentAspect < 1f)
            return Mathf.Sqrt(currentAspect / referenceAspect);
        
        return 1f;
    }
    
    private void ApplyGravity()
    {
        if (characterController.isGrounded && gravityVelocity < 0.0f)
        {
            gravityVelocity = -1f;
        }
        else
        {
            gravityVelocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        moveVector.y = gravityVelocity;
    }
}
