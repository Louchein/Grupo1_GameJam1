using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ParticleSystem moveParticle; //  Particulas cuando el player se mueve
    public ParticleSystem explosionParticle;
    
    CharacterController controller;

    // Coordenadas de movimiento
    Vector3 movement;
    float movZ, movX;
    public float speed;


    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Asegúrate de que el sistema de partículas no siga al player
        var mainModule = moveParticle.main;
        mainModule.simulationSpace = ParticleSystemSimulationSpace.World; // Partículas en el espacio del mundo
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
        MovePlayer();
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

        // Si el player está en movimiento, se activan las partículas
        if (movX != 0 || movZ != 0)
        {
            if (!moveParticle.isPlaying) // Verifica si las partículas ya no están reproduciéndose
            {
                moveParticle.Play(); // Reproduce las partículas
            }
        }
        else
        {
            if (moveParticle.isPlaying) // Verifica si las partículas están reproduciéndose
            {
                moveParticle.Stop(); // Detiene las partículas
            }
        }
    }

    //Dectector de colisionadores de la tierra.
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Dirt"))
        {
            Transform childDirt = other.transform.Find("PileOfDirt");
            if (childDirt != null) 
            {
                Destroy(childDirt.gameObject);
                other.GetComponent<Collider>().enabled = false;
            }
            Transform childTool = other.transform.GetChild(1);
            if (childTool != null) 
            {
                childTool.gameObject.SetActive(true); // Activa el familiar
            }
        }

        if(other.gameObject.CompareTag("Bomb"))
            {
                Debug.Log("GameOver");

                explosionParticle.Play();
            }
    }
}
