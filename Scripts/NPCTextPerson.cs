using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTextPerson : Collidable
{
    public string massage;
    private float cooldown = 0.1f; //4.0f
    private float lastShout;

    protected override void Start()
    {
        base.Start();
        lastShout = -cooldown;
    }
    protected override void OnCollide(Collider2D coll)
    {
        if (Time.time - lastShout > cooldown)
        {
            lastShout = Time.time;
            GameManager.instance.ShowText(massage, 25, Color.white, transform.position + new Vector3(0,0.25f,0), Vector3.zero, cooldown);
        }
    }
}
