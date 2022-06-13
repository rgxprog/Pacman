using UnityEngine;

public class Pacman : MonoBehaviour
{
    //-------------------------------------------

    public enum State
    {
        Idle,
        Eat,
        Hurt
    }

    //-------------------------------------------

    public Movement movement { get; private set; }

    private State state;
    private Animator animator;

    //-------------------------------------------

    private void Awake()
    {
        movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
    }

    //-------------------------------------------

    private void Start()
    {
        ResetState();
    }

    //-------------------------------------------

    private void Update()
    {
        if (GameManager.Instance.GetState() != GameState.InGame)
            return;
        
        if (state != State.Hurt)
        {
            CheckControl();
            CheckRotation();
            ChangeState(movement.DidMove() ? State.Eat : State.Idle);
        }
    }

    //-------------------------------------------

    public void ResetState()
    {
        ChangeState(State.Idle);
        movement.ResetState();
        gameObject.SetActive(true);
    }

    //-------------------------------------------

    // Para cambiar estado de Pacman y sus efectos
    private void ChangeState(State newState)
    {
        state = newState;

        animator.SetBool("Moving", state == State.Eat);
        animator.SetBool("Hurt", state == State.Hurt);
    }

    //-------------------------------------------

    // Verificar que jugador mueve a Pacman
    private void CheckControl()
    {
        if (Input.GetAxis("Horizontal") > 0)
            movement.SetDirection(Vector2.right);
        else if (Input.GetAxis("Horizontal") < 0)
            movement.SetDirection(Vector2.left);
        else if (Input.GetAxis("Vertical") > 0)
            movement.SetDirection(Vector2.up);
        else if (Input.GetAxis("Vertical") < 0)
            movement.SetDirection(Vector2.down);
    }

    //-------------------------------------------

    private void CheckRotation()
    {
        if (movement.direction.x > 0)
            SetRotation(0, new Vector3(1, 1, 1));
        else if (movement.direction.x < 0)
            SetRotation(0, new Vector3(-1, 1, 1));
        else if (movement.direction.y > 0)
            SetRotation(90, new Vector3(1, 1, 1));
        else if (movement.direction.y < 0)
            SetRotation(-90, new Vector3(1, -1, 1));
    }

    private void SetRotation(int angle, Vector3 scale)
    {
        transform.eulerAngles = angle == 0 ? Vector3.forward : Vector3.forward * angle;
        transform.localScale = scale;
    }

    //-------------------------------------------

    // Pacman toca un fantasma (pierde)
    public void TouchLiveGhost()
    {
        ChangeState(State.Hurt);
    }

    //-------------------------------------------
}
