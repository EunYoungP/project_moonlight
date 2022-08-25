using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseController
{ 
    Transform GetTarget();
    void SetTarget(Transform target);
    void GetDamage();
    void ChangeState(MonsterState state);
}
