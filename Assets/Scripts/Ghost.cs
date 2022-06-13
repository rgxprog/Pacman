using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    //-------------------------------------------

    public int points = 200;

    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }

    public GhostBehavior initialBehavior;
    public Transform target { get; private set; }

    //-------------------------------------------

    private void Awake()
    {
        movement = GetComponent<Movement>();
        target = FindObjectOfType<Pacman>().transform;

        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    //-------------------------------------------

    private void Start()
    {
        ResetState();
    }

    //-------------------------------------------

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior)
            home.Disable();

        if (initialBehavior != null)
            initialBehavior.Enable();
    }

    //-------------------------------------------

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled)
                GameManager.Instance.GhostEaten(this);
            else
                GameManager.Instance.PacmanEaten();
        }
    }

    //-------------------------------------------
}
