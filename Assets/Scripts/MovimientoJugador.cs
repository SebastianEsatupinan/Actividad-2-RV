using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidadMovimiento = 5f; // Velocidad de movimiento
    public float sensibilidadMouse = 2f; // Sensibilidad del mouse para rotar la cámara
    public Transform camaraTransform; // Transform de la cámara

    private CharacterController controlador;
    private float rotacionX = 0f;

    // Start is called before the first frame update
    void Start()
    {
        controlador = GetComponent<CharacterController>();
        if (controlador == null)
        {
            Debug.LogError("No hay un 'CharacterController' adjunto al objeto.");
        }

        // Ocultar el cursor y bloquearlo en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlador == null) return; // Salir si no hay 'CharacterController'

        // Movimiento del jugador
        float movimientoHorizontal = Input.GetAxis("Horizontal"); // Movimiento lateral
        float movimientoVertical = Input.GetAxis("Vertical"); // Movimiento adelante y atrás
        Vector3 movimiento = transform.right * movimientoHorizontal + transform.forward * movimientoVertical;
        movimiento *= velocidadMovimiento;

        // Mover al jugador
        controlador.Move(movimiento * Time.deltaTime);

        // Rotación con el mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        // Rotar la cámara y el jugador
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f); // Limitar el ángulo de rotación vertical

        camaraTransform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
