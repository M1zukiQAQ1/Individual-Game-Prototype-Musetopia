using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Obosolete
public class WarriorController : PlayerController
{
    // Start is called before the first frame update
    private int monsterIndex = 0;

    protected override void DealDamage()
    {
        positionToMoveTo = GameManager.instance.GetMobList()[monsterIndex].transform.position + GameManager.instance.GetMobList()[monsterIndex].positionOffsetValue;
        monsterIndex = Random.Range(0, GameManager.instance.GetMobList().Count - 1);
        StartCoroutine(nameof(MoveAndAttack));
    }

    private IEnumerator MoveAndAttack()
    {
        yield return new WaitUntil(() => positionToMoveTo == transform.position);
        yield return new WaitForSeconds(0.5f);

        base.DealDamage();
        yield return new WaitForSeconds(0.6f);

        double damage = GetDamage();
        totalDamageDelt += damage;
        GameManager.instance.GetMobList()[monsterIndex].TakeDamage(damage);
        yield return new WaitForSeconds(1f);

        positionToMoveTo = transform.position - new Vector3(Random.Range(2, 4), 0, 0);
    }

    protected override void Update()
    {
        base.Update();
        MoveToPosition(positionToMoveTo);
    }
}
