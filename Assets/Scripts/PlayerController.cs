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


    void Start()
    {
        controller = GetComponent<CharacterController>();
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
    }
}
