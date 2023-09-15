using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Blast,
        Bomb,
        Speed,
        Kick
    }
    public ItemType type;

    private void ItemPickup(GameObject player)
    {
        switch(type)
        {
            case ItemType.Blast:
                player.GetComponent<SetBomb>().blastLength++;
                break;
            case ItemType.Bomb:
                player.GetComponent<SetBomb>().AddBomb();
                break;
            case ItemType.Speed:
                player.GetComponent<Movement>().speed++;
                break;
            case ItemType.Kick:
                player.GetComponent<Movement>().kick = true;
                break;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player"))
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ItemPickup(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Destroy(gameObject);
        }
    }
}