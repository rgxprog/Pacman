using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    //-------------------------------------------

    public Transform inside, outside;

    //-------------------------------------------

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    //-------------------------------------------

    private void OnDisable()
    {
        if (gameObject.activeSelf)
            StartCoroutine(ExitTransition());
    }

    //-------------------------------------------

    // L�gica para salir de casita
    private IEnumerator ExitTransition()
    {
        ghost.movement.SetDirection(Vector2.up, true);
        ghost.movement.rb.isKinematic = true;
        ghost.movement.enabled = false;

        Vector3 position = transform.position;
        float duration = 0.5f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, inside.position, elapsed/duration);
            newPosition.z = position.z;
            ghost.transform.position = GameManager.Instance.GetState() == GameState.LevelWin ? ghost.transform.position : newPosition;
            elapsed += Time.deltaTime;
            yield return null;  // Para no seguir el while hasta el siguiente frame
        }

        elapsed = 0.0f;
        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(inside.position, outside.position, elapsed / duration);
            newPosition.z = position.z;
            ghost.transform.position = GameManager.Instance.GetState() == GameState.LevelWin ? ghost.transform.position : newPosition;
            elapsed += Time.deltaTime;
            yield return null;  // Para no seguir el while hasta el siguiente frame
        }

        ghost.movement.SetDirection(
            new Vector2(
                Random.value < 0.5f ? -1.0f : 1.0f,
                0.0f
                ),
            true);
        ghost.movement.rb.isKinematic = false;
        ghost.movement.enabled = true;
    }

    //-------------------------------------------

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            ghost.movement.SetDirection(-ghost.movement.direction, true);
    }

    //-------------------------------------------
}
