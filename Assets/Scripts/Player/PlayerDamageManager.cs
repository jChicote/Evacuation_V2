using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void OnDamage(float damage);
}

namespace PlayerSystems
{
    public class PlayerDamageManager : MonoBehaviour, IDamageable
    {
        public void OnDamage(float damage)
        {
            Debug.Log("Damage at: " + damage);
        }
    }
}