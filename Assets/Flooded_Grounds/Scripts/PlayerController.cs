using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public PlayerHUD playerHUD; // ������ �� HUD
    private float maxHealth = 100f;//������������ �������� 
    private float currentHealth;
    public float speed = 5f;
    public float sensitivity = 2f;
    public float gravity = 20f; //�������� ���������� ��������� � �����
    public float jumpHeight = 2f;
    public float sprintSpeed = 20f; // �������� ��� ����
    public float stamina = 100f; // ������� ������������
    public float maxStamina = 100f; // ������������ ������������
    public float staminaDrain = 20f; // ������ ������� ��� ����
    public float staminaRegen = 10f; // �������������� �������
    public float interactDistance = 3f; // ������������ ��������� ��� ������� ���������
    [SerializeField] public GameObject inventoryUIObject;
    private float currentSpeed;
    private CharacterController controller;
    private Transform cameraTransform;
    private float rotationX = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isInventoryOpen = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform; // ���� ������ �� �������� ��������
                                                                         
        // ���������, �� ����� �� ���� �� �����, ����� ����������� �������
        if (!PauseMenu.isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        

        currentHealth = maxHealth;
        playerHUD.UpdateHealth(currentHealth, maxHealth);
        currentSpeed = speed;
        playerHUD.UpdateStamina(stamina, maxStamina); // ��������� ������� � HUD
    }
    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        playerHUD.UpdateHealth(currentHealth, maxHealth);
    }

    void Update()
    {
        


        

        if (!isInventoryOpen)
        {
            // ���������, ����� �� �������� �� �����
            isGrounded = controller.isGrounded;


            // �������� ���������
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * speed * Time.deltaTime);

            if (Time.timeScale == 0f) return; // ���� �����, ���������� ����

            // �������� ������ � ���������
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            // ������������ �������� (������)
            rotationX += mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

            // �������������� �������� (��������)
            transform.Rotate(Vector3.up * mouseX);

            // ����������
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = 0f; // ��������� �������� � �����, ����� �� "�����"
            }

            // ������ (��������, ��� �������� �� �����)
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            }

            // ���������� ����������
            velocity.y -= gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            // ���������, ��� �� ����� Shift � ���� �� �������
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            {
                currentSpeed = sprintSpeed;
                stamina -= staminaDrain * Time.deltaTime;

            }
            else
            {
                currentSpeed = speed;
                stamina += staminaRegen * Time.deltaTime; // ��������������
            }

            if (stamina <= 0)
            {
                stamina = 0;
                currentSpeed = speed;
            }
            // ������������ �������
            stamina = Mathf.Clamp(stamina, 0, maxStamina);

            // ��������� HUD
            playerHUD.UpdateStamina(stamina, maxStamina);

            // ��������
            controller.Move(move * currentSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.E)) // ������� E
            {
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 3f)) // �������� ������� ����� �������
                {
                    Door door = hit.collider.GetComponentInParent<Door>();
                    if (door != null)
                    {
                        door.ToggleDoor();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) // ��������� ������� "E"
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 3f))
                {
                    var pickup = hit.collider.GetComponent<ItemPickup>();
                    if (pickup != null)
                    {
                        pickup.Interact();
                    }
                }
            }

            

        }

    }

    /*void TryInteract()
    {
        //int layerMask = LayerMask.GetMask("Interactable"); // ��������� ������ ���� "Interactable"
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            ItemPickup item = hit.collider.GetComponent<ItemPickup>();

            if (item != null) // ���� ��� �������, �������� ��� ����� Interact()
            {
                item.Interact();
            }
        }
    }*/
     
    


}
