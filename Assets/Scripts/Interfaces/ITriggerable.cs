using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerable
{
    bool isTriggered { get; set; }
    void SetEnemyTriggered();
}
