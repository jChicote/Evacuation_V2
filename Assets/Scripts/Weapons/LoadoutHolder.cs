using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Placed within loadout position that both sets and gets the weapon in the loadout
/// </summary>
public class LoadoutHolder : MonoBehaviour
{
    public int loadoutNum;
    public LoadoutPosition loadoutType;
    public Loadout weaponLoadout;
    public GameObject weapon;

    /// <summary>
    /// 
    /// </summary>
    public IWeapon SetWeapon(Loadout loadout)
    {
        WeaponInfo weaponInfo = loadout.weaponInformation;
        weaponLoadout = loadout;

        //TODO: Shortening the weapon constructing to be centric to the weapon prefab instead of a seperate list
        //Spawn and Initialise Weapon
        GameObject weapon = Instantiate(weaponInfo.weaponPrefab, transform);
        IWeapon weaponInterface = weapon.GetComponent<IWeapon>();
        weaponInterface.InitialiseWeapon(weaponInfo.weaponData, weaponInfo.projectileShell);
        weaponInterface.ConfigureWeaponPositioning(loadoutType);

        return weaponInterface;
    }

    /// <summary>
    /// 
    /// </summary>
    public void GetWeapon()
    {

    }
}


//   MOVE TO A SEPERATE FILE        

public class Loadout
{
    public int positionNum;
    public WeaponInfo weaponInformation;
}

public enum LoadoutPosition
{
    Fixed,
    Pivot
}

