using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeHit(Attack attack, RaycastHit hit);

    void TakeAttack(Attack attack);

    void BaseHit();
}
