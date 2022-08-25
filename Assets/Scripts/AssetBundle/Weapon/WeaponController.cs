using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    public abstract void Init();

    public virtual bool CanChange(DetailType newWeaponType,string newItemName) { return false; }

    public abstract void ChangeWeapon();

    protected virtual void SetWeaponPos() { }
}
