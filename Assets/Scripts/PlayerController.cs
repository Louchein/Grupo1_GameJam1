using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;

    // Coordenadas de movimiento
    Vector3 movement;
    float movZ, movX;
    public float speed;
    public int familyCount = 0;

    // Variables para deteccion de colisiones.
    bool canInteract = false;
    private Collider currentCollider;

    // Variables para acceder a las imagenes
    public GameObject inactiveAxe;
    public GameObject activeAxe;
    public GameObject inactiveHammer;
    public GameObject activeHammer;
    public GameObject inactiveHoe;
    public GameObject activeHoe;
    public GameObject inactivePickaxe;
    public GameObject activePickaxe;
    public bool gameOver;


    void Start()
    {
        controller = GetComponent<CharacterController>();
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
        // Combina los vectores de movimiento vertical y horizontal
        Vector3 move = transform.forward * movZ + transform.right * movX;
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
            GameObject pileOfDirt = currentCollider.gameObject;
            if (pileOfDirt.CompareTag("Dirt") && pileOfDirt.transform.childCount == 2)
            {
                Transform childDirt = currentCollider.transform.Find("PileOfDirt");
                if (childDirt != null)
                {
                    Destroy(childDirt.gameObject);
                    currentCollider.GetComponent<Collider>().enabled = false;
                }
                if(currentCollider.transform.GetChild(1) != null)
                {
                    currentCollider.transform.GetChild(1).gameObject.SetActive(true); //Activa el familiar
                }
            }
        }
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
            familyCount = 0;
            gameOver = true;

        }

        if(other.gameObject.CompareTag("Axe"))
        {
            familyCount++;
            inactiveAxe.SetActive(false);
            activeAxe.SetActive(true);
        }

        if(other.gameObject.CompareTag("Hammer"))
        {
            familyCount++;
            inactiveHammer.SetActive(false);
            activeHammer.SetActive(true);

        }

        if(other.gameObject.CompareTag("Hoe"))
        {
            familyCount++;
            inactiveHoe.SetActive(false);
            activeHoe.SetActive(true);
        }

        if(other.gameObject.CompareTag("Pickaxe"))
        {
            familyCount++;
            inactivePickaxe.SetActive(false);
            activePickaxe.SetActive(true);

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
