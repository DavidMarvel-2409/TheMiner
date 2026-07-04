using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class AntController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Key k_up, k_down, k_left, k_right, k_excavar;
    [SerializeField] private float escala_cam, tiempoEspera, speed = 2;
    private Vector3 direct = Vector3.zero, directbody = Vector3.zero;

    [Header("Excavación")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tileVacio, tilePared;

    private Vector3Int? celdaColision = null;
    void Start()
    {
        
    }

    void Update()
    {
        if (tiempoEspera > 0) tiempoEspera -= Time.deltaTime;
        if (tiempoEspera <= 0)
        {
            cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, cam.transform.position.z);
            cam.orthographicSize = escala_cam;
        }
        movimiento();
        MirarAlMouse();
        //rotatess();
        if (Keyboard.current[k_excavar].isPressed) Excavar();
    }

    private void movimiento()
    {
        if (Keyboard.current[k_up].isPressed) direct.y = 1;
        else if (Keyboard.current[k_down].isPressed) direct.y = -1;
        else direct.y = 0;
        if (Keyboard.current[k_left].isPressed) direct.x = -1;
        else if (Keyboard.current[k_right].isPressed) direct.x = 1;
        else direct.x = 0;

        transform.position += direct * speed * Time.deltaTime;
    }
    private void rotatess()
    {
        if (Keyboard.current[k_up].isPressed) directbody.y = 1;
        else if (Keyboard.current[k_down].isPressed) directbody.y = -1;
        else directbody.y = 0;
        if (Keyboard.current[k_left].isPressed) directbody.x = -1;
        else if (Keyboard.current[k_right].isPressed) directbody.x = 1;
        else directbody.x = 0;
        transform.rotation = Quaternion.LookRotation(directbody, Vector3.zero);
    }
    private void MirarAlMouse()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseScreenPos.z = -cam.transform.position.z; // distancia entre cámara y el plano 2D
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);

        Vector3 direccion = mouseWorldPos - transform.position;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angulo - 90f);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject != tilemap.gameObject) return;

        ContactPoint2D contacto = collision.GetContact(0);
        // nos metemos un poco hacia adentro del tile para no quedar justo en el borde
        Vector3 puntoAjustado = contacto.point - contacto.normal * 0.1f;
        celdaColision = tilemap.WorldToCell(puntoAjustado);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject == tilemap.gameObject) celdaColision = null;
    }

    private void Excavar()
    {
        if (!celdaColision.HasValue) return;
        TileBase tileActual = tilemap.GetTile(celdaColision.Value);
        if (tileActual == tilePared) return;
        tilemap.SetTile(celdaColision.Value, tileVacio);
        Debug.Log("Excavado");
    }

}
