using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    // Variables para acceder al TMPro
    TextMeshProUGUI axeText;
    TextMeshProUGUI hammerText;
    TextMeshProUGUI hoeText;
    TextMeshProUGUI pickaxeText;

    // Audio
    public AudioClip gameMusic;
    public AudioClip notFoundAudio, foundAudio, bombAudio;

    public bool gameOver;


    void Start()
    {
        // Reproducir musica de fondo
        AudioManager.Instance.PlayMusic(gameMusic);

        controller = GetComponent<CharacterController>();

        axeText = GameObject.Find("AxeText").GetComponent<TextMeshProUGUI>();
        hammerText = GameObject.Find("HammerText").GetComponent<TextMeshProUGUI>();
        hoeText = GameObject.Find("HoeText").GetComponent<TextMeshProUGUI>();
        pickaxeText = GameObject.Find("PickaxeText").GetComponent<TextMeshProUGUI>();

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
                if (currentCollider.transform.GetChild(1) != null)
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
            AudioManager.Instance.PlaySFX(notFoundAudio);
        }

        if (other.gameObject.CompareTag("Bomb"))
        {
            familyCount = 0;
            gameOver = true;
            AudioManager.Instance.PlaySFX(bombAudio);

        }

        if (other.gameObject.CompareTag("Axe"))
        {
            familyCount++;
            axeText.text = "Axe: Found";
            axeText.color = Color.green;
            AudioManager.Instance.PlaySFX(foundAudio);

        }

        if (other.gameObject.CompareTag("Hammer"))
        {
            familyCount++;
            hammerText.text = "Hammer: Found";
            hammerText.color = Color.green;
            AudioManager.Instance.PlaySFX(foundAudio);

        }

        if (other.gameObject.CompareTag("Hoe"))
        {
            familyCount++;
            hoeText.text = "Hoe: Found";
            hoeText.color = Color.green;
            AudioManager.Instance.PlaySFX(foundAudio);

        }

        if (other.gameObject.CompareTag("Pickaxe"))
        {
            familyCount++;
            pickaxeText.text = "Pickaxe: Found";
            pickaxeText.color = Color.green;
            AudioManager.Instance.PlaySFX(foundAudio);

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
