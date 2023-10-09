using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Obosolete
public class HealerController : PlayerController
{
    // Start is called before the first frame update

    protected override void DealDamage()
    {
        base.DealDamage();
        double damage = GetDamage() * -1;
        totalDamageDelt += damage;
        GameManager.instance.GetPlayerWithLowestHealth(true).TakeDamage(damage);
    }

    protected override void Update()
    {
        base.Update();
    }
}
