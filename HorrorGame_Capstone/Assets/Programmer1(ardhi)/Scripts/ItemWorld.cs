using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item) {
       Transform transform = Instantiate(ItemAssets.Instance.pf_ItemWorld, position, Quaternion.identity);

       ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
       itemWorld.SetItem(item);

       return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetItem (Item item) {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
    }
}
