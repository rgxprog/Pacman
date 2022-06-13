
using System.Collections;
using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    //-------------------------------------------

    public bool eaten { get; private set; }
    public SpriteRenderer body, eyes, blue, white;

    //-------------------------------------------

    public override void Enable(float duration)
    {
        base.Enable(duration);

        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        Invoke(nameof(Flash), duration / 2.0f);
    }

    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
        }
    }

    //-------------------------------------------

    public override void Disable()
    {
        base.Disable();

        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    //-------------------------------------------

    private void OnEnable()
    {
        ghost.movement.speedMultiplier = 0.5f;
        eaten = false;
    }

    //-------------------------------------------

    private void OnDisable()
    {
        ghost.movement.speedMultiplier = 1f;
        eaten = false;
    }

    //-------------------------------------------

    // Si pacman se come al fantasma
    private void Eaten()
    {
        eaten = true;

        ghost.home.Enable(duration);

        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;

        StartCoroutine(ReturnHomeTransition());
    }

    //-------------------------------------------

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled)
                Eaten();
        }
    }

    //-------------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Node node = collision.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition =
                    transform.position +
                    new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

    //-------------------------------------------

    // Lógica para regresar a la casita
    private IEnumerator ReturnHomeTransition()
    {
        ghost.movement.SetDirection(Vector2.up, true);
        ghost.movement.rb.isKinematic = true;
        ghost.movement.enabled = false;

        Vector3 position = transform.position;
        float duration = 0.5f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, ghost.home.inside.position, elapsed / duration);
            newPosition.z = position.z;
            ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;  // Para no seguir el while hasta el siguiente frame
        }

        ghost.movement.rb.isKinematic = false;
        ghost.movement.enabled = true;
    }

    //-------------------------------------------
}
