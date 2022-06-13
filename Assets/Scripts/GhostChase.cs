using UnityEngine;

public class GhostChase : GhostBehavior
{
    //-------------------------------------------

    private void OnDisable()
    {
        ghost.scatter.Enable();
    }

    //-------------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Node node = collision.GetComponent<Node>();

        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            Vector2 alternateDirection = Vector2.zero;      // Para no regresar por donde venía
            float minDistance = float.MaxValue;
                
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition =
                    transform.position +
                    new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    alternateDirection = direction;
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            if (direction == -ghost.movement.direction && alternateDirection != Vector2.zero)
                direction = alternateDirection;

            ghost.movement.SetDirection(direction);
        }
    }

    //-------------------------------------------
}
