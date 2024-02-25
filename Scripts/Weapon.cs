using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable
{
    // Damage struct
    public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7 };
    public float[] pushForce = {2.0f, 2.2f, 2.5f, 3f, 3.2f, 3.6f, 4f};


    //Upgrade
    public int weaponLavel = 0;
    public  SpriteRenderer spriteRenderer;

    //Swing
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter") 
        {
            if (coll.name == "Player")
                return;

            //Create a new damage object, then we'll send it to the fighter we've hit
            Damage dmg = new Damage
            {
                damageAmount = damagePoint[weaponLavel],
                origin = transform.position,
                pushForce = pushForce[weaponLavel]
            };
            coll.SendMessage("ReceiveDamage", dmg);
        }
    }
    protected void Swing()
    {
        anim.SetTrigger("Swing");
    }
    public void UpgradeWeapon()
    {
        weaponLavel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLavel];

    }
    public void SetWeaponLevel(int level)
    {
        weaponLavel= level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLavel];
    }
}
