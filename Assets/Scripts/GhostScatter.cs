
using UnityEngine;

public class GhostScatter : GhostBehavior
{
    //-------------------------------------------

    private void OnDisable()
    {
        ghost.chase.Enable();
    }

    //-------------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Node node = collision.GetComponent<Node>();

        if (node != null && enabled && !ghost.frightened.enabled)
        {
            int index = Random.Range(0, node.availableDirections.Count);

            if (node.availableDirections[index] == -ghost.movement.direction && node.availableDirections.Count > 1)
                index = index + 1 >= node.availableDirections.Count ? 0 : index + 1;

            ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }

    //-------------------------------------------
}
