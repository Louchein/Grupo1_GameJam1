using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;

    // Coordenadas de movimiento
    Vector3 movement;
    float movZ, movX;
    public float speed;
    bool canInteract = false;
    private Collider currentCollider;

    [HideInInspector] public Animator playerAnimator;
    private const string IS_WALKING = "IsWalking";
    private const string IS_SHOVELING = "IsShoveling";

    public ParticleSystem dirtAway;

    // Nueva variable para la velocidad vertical (gravedad)
    private float velocityY;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        speed = 10;

        velocityY = 0f; // Inicialmente la velocidad vertical es 0
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
        MovePlayer();
        Dig();
    }

    void InputManager()
    {
        // Se obtiene el axis para mover el personaje 
        movZ = Input.GetAxis("Vertical");
        movX = Input.GetAxis("Horizontal");
    }


    void MovePlayer()
    {
        if (movX > 0 || movZ > 0) {
            playerAnimator.SetBool(IS_WALKING, true);
        } else {
            playerAnimator.SetBool(IS_WALKING, false);
        }

        // Combina los vectores de movimiento vertical y horizontal
        Vector3 move = transform.forward * -movZ + transform.right * -movX;
        move = move.normalized * speed; // Normaliza el vector y aplica la velocidad

        // Se aplica gravedad
        // Check if player is grounded
        if (controller.isGrounded) {
            // Si está en el suelo, la velocidad en Y es 0
            velocityY = 0;
        } else {
            // Si no está en el suelo, aplicar la gravedad
            velocityY += Physics.gravity.y * Time.deltaTime;
        }

        // Aplicar la velocidad en Y (gravedad)
        move.y = velocityY * 0.1f;

        // Mover el personaje
        controller.Move(move * Time.deltaTime);
    }

    void Dig()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.Space))
        {
            playerAnimator.SetBool(IS_SHOVELING, true);

            StartCoroutine(CheckIfDirtPile());

            GameObject pileOfDirt = currentCollider.gameObject;
            if (pileOfDirt.CompareTag("Dirt") && pileOfDirt.transform.childCount == 2) {
                dirtAway.gameObject.SetActive(true);

                StartCoroutine(StartDirtParticlesAnimation());
            }
        }
    }

    IEnumerator CheckIfDirtPile() {
        yield return new WaitForSeconds(.7f);

        if (currentCollider.transform.childCount == 2 ) { 
            Transform childDirt = currentCollider.transform.Find("PileOfDirt");
        
            if (childDirt != null) {
                Destroy(childDirt.gameObject);
                currentCollider.GetComponent<Collider>().enabled = false;
            }
            if (currentCollider.transform.GetChild(1) != null) {
                currentCollider.transform.GetChild(1).gameObject.SetActive(true); //Activa el familiar
            }
        }
        

        playerAnimator.SetBool(IS_SHOVELING, false);
    }

    IEnumerator StartDirtParticlesAnimation() {
        yield return new WaitForSeconds(1.7f);

        dirtAway.gameObject.SetActive(false);
    }

        //Dectector de colisionadores de la tierra.
        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dirt"))
        {
            canInteract = true;
            currentCollider = other;
        }

        if (other.gameObject.CompareTag("Bomb"))
        {
            Debug.Log("GameOver");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dirt"))
        {
            canInteract = false;
            currentCollider = null;
        }
    }
}
