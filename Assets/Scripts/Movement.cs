using UnityEngine;

//-----------------------------------------------

[RequireComponent(typeof(Rigidbody2D))]

//-----------------------------------------------

public class Movement : MonoBehaviour
{
    //-------------------------------------------

    public Rigidbody2D rb { get; private set; }

    public float speed = 8.0f;
    public float speedMultiplier = 1.0f;

    public Vector3 startPosition { get; private set; }

    public Vector2 initialDirection;
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }

    public LayerMask obstacleLayer;

    private Vector3 lastPosition;

    //-------------------------------------------

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    //-------------------------------------------

    private void Start()
    {
        ResetState();
    }

    //-------------------------------------------

    private void Update()
    {
        if (nextDirection != Vector2.zero)
            SetDirection(nextDirection);
    }

    //-------------------------------------------

    private void FixedUpdate()
    {
        if (GameManager.Instance.GetState() != GameState.InGame)
            return;

        if (GameManager.Instance.stopMovement)
            return;
        
        lastPosition = transform.position;
        Vector2 position = rb.position;
        Vector2 translation = speed * speedMultiplier * Time.deltaTime * direction;
        rb.MovePosition(position + translation);
    }

    //-------------------------------------------

    public void ResetState()
    {
        speedMultiplier = 1.0f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startPosition;
        rb.isKinematic = false;     // Que afecte físicas y colisiones
        enabled = true;
    }

    //-------------------------------------------

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }

    //-------------------------------------------

    // Para verificar si un espacio en una dirección está ocupado
    private bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position,
            Vector2.one * 0.35f,
            0.0f,
            direction,
            0.5f,
            obstacleLayer
            );

        return hit.collider != null;
    }

    //-------------------------------------------

    // Para consultar si el objeto se ha movido
    public bool DidMove()
    {
        return lastPosition != transform.position;
    }

    //-------------------------------------------
}
