using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMovement : MonoBehaviour
{
    Transform player; // Referencia al transform jugador
    public float followDistance = 5f; // Distancia a mantener
    public float followSpeed = 5f; // Velocidad de seguimiento
    public float rotationSpeed = 5f;
    private Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Rigidbody>().velocity.magnitude > 0) {
            // Calcula la posición objetivo manteniendo la distancia
            targetPosition = player.position - player.forward * followDistance;

            // Mueve el GameObject hacia la posición objetivo
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            Quaternion targetRotation = Quaternion.Lerp(transform.rotation, player.rotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    }
}
