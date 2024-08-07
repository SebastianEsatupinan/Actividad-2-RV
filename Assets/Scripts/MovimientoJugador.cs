using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidadMovimiento = 5f; // Velocidad de movimiento
    public float sensibilidadMouse = 2f; // Sensibilidad del mouse para rotar la cámara
    public Transform camaraTransform; // Transform de la cámara
    public float gravedad = -9.81f; // Fuerza de la gravedad
    public Transform sueloComprobador; // Objeto para comprobar si el jugador está en el suelo
    public float distanciaSuelo = 0.4f; // Distancia para comprobar si el jugador está en el suelo
    public LayerMask sueloMask; // Capa que representa el suelo

    private CharacterController controlador;
    private float rotacionX = 0f;
    private Vector3 velocidad;
    private bool enSuelo;

    // Variables para el movimiento de la cámara
    public float amplitudMovimiento = 0.1f; // Amplitud del movimiento de la cámara
    public float frecuenciaMovimiento = 1.0f; // Frecuencia del movimiento de la cámara
    private float tiempoMovimiento;

    // Variables para el sonido de caminar
    public AudioSource audioSource; // Componente AudioSource
    public AudioClip sonidoCaminar; // Clip de sonido para caminar
    private bool caminando = false;

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

        // Comprobar si el jugador está en el suelo
        enSuelo = Physics.CheckSphere(sueloComprobador.position, distanciaSuelo, sueloMask);

        // Movimiento del jugador
        float movimientoHorizontal = Input.GetAxis("Horizontal"); // Movimiento lateral
        float movimientoVertical = Input.GetAxis("Vertical"); // Movimiento adelante y atrás
        Vector3 movimiento = transform.right * movimientoHorizontal + transform.forward * movimientoVertical;
        movimiento *= velocidadMovimiento;

        if (enSuelo && velocidad.y < 0)
        {
            velocidad.y = -2f; // Mantener al jugador pegado al suelo
        }

        // Aplicar movimiento
        controlador.Move(movimiento * Time.deltaTime);

        // Aplicar gravedad
        velocidad.y += gravedad * Time.deltaTime;
        controlador.Move(velocidad * Time.deltaTime);

        // Rotación con el mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        // Rotar la cámara y el jugador
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f); // Limitar el ángulo de rotación vertical

        camaraTransform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Movimiento de la cámara
        tiempoMovimiento += Time.deltaTime * frecuenciaMovimiento;
        camaraTransform.localPosition = new Vector3(0f, Mathf.Sin(tiempoMovimiento) * amplitudMovimiento, 0f);

        // Sonido de caminar
        if (enSuelo && (movimientoHorizontal != 0 || movimientoVertical != 0))
        {
            if (!caminando)
            {
                audioSource.clip = sonidoCaminar;
                audioSource.loop = true;
                audioSource.Play();
                caminando = true;
            }
        }
        else
        {
            if (caminando)
            {
                audioSource.Stop();
                caminando = false;
            }
        }
    }
}
