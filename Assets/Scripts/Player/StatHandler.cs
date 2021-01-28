using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IStatHandler {
    void InitialiseStats(ShipInfo shipInfo);
}

public interface IWeaponStats {
    List<string> GetForwardLoadout();
    List<string> GetTurrentLoadout();
}

public interface IShipData
{
    Modifier GetShipModifiers();
    ShipInfo GetShipStats();
}

public interface IShipHealth
{
    float GetShipHealth();
    void SetShipHealth(float newHealth);
}

public interface IShipShields 
{
    float GetShipShields();
    void SetShipShields(float newShields);
}


public class StatHandler : MonoBehaviour, IStatHandler, IWeaponStats, IShipData, IShipHealth, IShipShields
{
    [Header("Primary Hit Points")]
    public float currentHealth = 100;
    public float currentShield = 0;

    [Header("Ship Sub Stats")]
    public ShipInfo shipInfo;
    public Modifier shipModifier;

    [Header("Event Observers")]
    public ShipHitPoint OnHealthChanged;
    public ShipHitPoint OnShieldChanged;

    public void InitialiseStats(ShipInfo shipInfo)
    {
        this.shipInfo = shipInfo;

        SubscribeToEvents();
    }

    /// <summary>
    /// Subscribes class events to external entities or UI.
    /// </summary>
    private void SubscribeToEvents()
    {
        OnHealthChanged = new ShipHitPoint();
        OnShieldChanged = new ShipHitPoint();

        //TODO: Subscribe UI items
    }

    public List<string> GetForwardLoadout()
    {
        return shipInfo.forwardWeapons;
    }

    public List<string> GetTurrentLoadout()
    {
        return shipInfo.turrentWeapons;
    }

    public Modifier GetShipModifiers()
    {
        return shipModifier;
    }

    public ShipInfo GetShipStats()
    {
        return shipInfo;
    }

    public float GetShipHealth()
    {
        return currentHealth;
    }

    public void SetShipHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public float GetShipShields()
    {
        return currentShield;
    }

    public void SetShipShields(float newShields)
    {
        currentShield = newShields;
    }
}

[System.Serializable]
public class ShipHitPoint : UnityEvent<float> { }

[System.Serializable]
public struct Modifier
{
    public float speedModifier;
    public float shieldModifier;
    public float healthModifier;
    public float fireRateModifier;
    public float damageModifier;
}
