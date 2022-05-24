using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int shmeklsAmount = 5;

    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.ShowText("+ " + shmeklsAmount + " Shmekls!",25, Color.yellow, transform.position, Vector3.up * 30, 0.5f);
            
        }
    }
}
