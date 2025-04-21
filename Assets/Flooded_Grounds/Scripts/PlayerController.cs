using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public PlayerHUD playerHUD; // Ссылка на HUD
    private float maxHealth = 100f;//Максимальное здоровье 
    private float currentHealth;
    public float speed = 5f;
    public float sensitivity = 2f;
    public float gravity = 20f; //Значение притяжения персонажа к земле
    public float jumpHeight = 2f;
    public float sprintSpeed = 20f; // Скорость при беге
    public float stamina = 100f; // Текущая выносливость
    public float maxStamina = 100f; // Максимальная выносливость
    public float staminaDrain = 20f; // Расход стамины при беге
    public float staminaRegen = 10f; // Восстановление стамины
    public float interactDistance = 3f; // Максимальная дистанция для подбора предметов
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
        cameraTransform = GetComponentInChildren<Camera>().transform; // Берём камеру из дочерних объектов
                                                                         
        // Проверяем, не стоит ли игра на паузе, перед блокировкой курсора
        if (!PauseMenu.isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        

        currentHealth = maxHealth;
        playerHUD.UpdateHealth(currentHealth, maxHealth);
        currentSpeed = speed;
        playerHUD.UpdateStamina(stamina, maxStamina); // Обновляем стамину в HUD
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
            // Проверяем, стоит ли персонаж на земле
            isGrounded = controller.isGrounded;


            // Движение персонажа
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * speed * Time.deltaTime);

            if (Time.timeScale == 0f) return; // Если пауза, игнорируем ввод

            // Вращение камеры и персонажа
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            // Вертикальное вращение (камера)
            rotationX += mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

            // Горизонтальное вращение (персонаж)
            transform.Rotate(Vector3.up * mouseX);

            // Гравитация
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = 0f; // Небольшое прижатие к земле, чтобы не "парил"
            }

            // Прыжок (проверка, что персонаж на земле)
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
            }

            // Применение гравитации
            velocity.y -= gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            // Проверяем, жмёт ли игрок Shift и есть ли стамина
            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            {
                currentSpeed = sprintSpeed;
                stamina -= staminaDrain * Time.deltaTime;

            }
            else
            {
                currentSpeed = speed;
                stamina += staminaRegen * Time.deltaTime; // Восстановление
            }

            if (stamina <= 0)
            {
                stamina = 0;
                currentSpeed = speed;
            }
            // Ограничиваем стамину
            stamina = Mathf.Clamp(stamina, 0, maxStamina);

            // Обновляем HUD
            playerHUD.UpdateStamina(stamina, maxStamina);

            // Движение
            controller.Move(move * currentSpeed * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.E)) // Нажатие E
            {
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 3f)) // Проверка объекта перед игроком
                {
                    Door door = hit.collider.GetComponentInParent<Door>();
                    if (door != null)
                    {
                        door.ToggleDoor();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) // Проверяем нажатие "E"
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
        //int layerMask = LayerMask.GetMask("Interactable"); // Проверяем только слой "Interactable"
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            ItemPickup item = hit.collider.GetComponent<ItemPickup>();

            if (item != null) // Если это предмет, вызываем его метод Interact()
            {
                item.Interact();
            }
        }
    }*/
     
    


}
