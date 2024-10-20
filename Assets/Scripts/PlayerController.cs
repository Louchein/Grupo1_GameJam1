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

    public Animator playerAnimator;
    private const string IS_WALKING = "IsWalking";
    private const string IS_SHOVELING = "IsShoveling";

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        playerAnimator = GetComponent<Animator>();

        speed = 10;
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
        move.y += Physics.gravity.y * Time.deltaTime;

        // Mover el personaje
        controller.Move(move * Time.deltaTime);
    }

    void Dig()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.Space))
        {
            playerAnimator.SetBool(IS_SHOVELING, true);

            StartCoroutine(CheckIfDirtPile());
        }
    }

    IEnumerator CheckIfDirtPile() {
        yield return new WaitForSeconds(.7f);

        GameObject pileOfDirt = currentCollider.gameObject;

        if (pileOfDirt.CompareTag("Dirt") && pileOfDirt.transform.childCount == 2) {
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
