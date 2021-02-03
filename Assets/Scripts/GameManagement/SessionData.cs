using System;
using System.Linq;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;


public class SessionData : MonoBehaviour
{
    // the session data will be the main model class that interfaces with the files.
    // Should be responsible for saving and extracting data.

    public static SessionData instance = null;

    public HangarInventory hangarCurrentSave;
    public UserStatus userStatus;

    [Header("Data Events")]
    public UnityEvent OnUserTransaction; 

    // Contains variables that relate to the active variables used for the game
    [Header("Session State")]
    public ShipInfo selectedShip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Saves current state of game into device storage.
    /// </summary>
    public void Save()
    {
        // This can be called multiple times throughout the session.
        // 1. Create new game save
        GameSaveState newGameSave = new GameSaveState();

        // 2. Write saved instances
        newGameSave.SaveHangar(hangarCurrentSave);
        newGameSave.SaveUserStatus(userStatus);

        // 3. Serialise file and Save
        string jsonData = JsonUtility.ToJson(newGameSave, false);
        string jsonPath = Application.persistentDataPath + "/gamesave.save";

        File.WriteAllText(jsonPath, jsonData);
        Debug.Log("Finihsed Writing");
    }

    /// <summary>
    /// Loads the stored game state to session.
    /// </summary>
    public void Load()
    {
        // Designed to be only called once without recalls to file data.
        // Incorrect calls could overwrite data.
        string jsonPath = Application.persistentDataPath + "/gamesave.save";

        if (File.Exists(jsonPath))
        {
            // 1. Fetch from path
            GameSaveState saveState = JsonUtility.FromJson<GameSaveState>(File.ReadAllText(jsonPath));
            Debug.Log("State has been loaded");

            // 2. Read saved instances
            hangarCurrentSave = saveState.GetHangarSave();
            userStatus = saveState.GetUserStatus();

            if (hangarCurrentSave == null || saveState.userStatus == null)
            {
                Debug.LogWarning("Detecting missing or data loss");
                SetupDefaultPlayer();
            }
        }
        else
        {
            SetupDefaultPlayer();
        }
    }

    /// <summary>
    /// Loads default items and ship when state returns null
    /// </summary>
    private void SetupDefaultPlayer()
    {
        Debug.LogWarning("No game save state or data was found. Creating a new default state");

        HangarInventory hangarSaveState = new HangarInventory();

        // 2. Load default items
        List<WeaponInfo> weaponList = new List<WeaponInfo>();
        weaponList.Add(GameManager.Instance.weaponSettings.turrentWeapons[0].ConvertToWeaponInfo());

        hangarSaveState.UpdateHangarWeapons(weaponList);
        hangarSaveState.ResetAllShips();

        // 3. Save to global
        hangarCurrentSave = hangarSaveState;

        UserStatus newuser = new UserStatus();
        userStatus = newuser;
    }

    /// <summary>
    /// Resets current save state and loads defaults.
    /// </summary>
    public void ResetAllSaves()
    {
        GameSaveState newGameSave = new GameSaveState();
        string jsonData = JsonUtility.ToJson(newGameSave, false);
        string jsonPath = Application.persistentDataPath + "/gamesave.save";

        File.WriteAllText(jsonPath, jsonData);
        Debug.Log("Finihsed Reset");

        SetupDefaultPlayer();
    }

    public WeaponInfo GetWeaponItem(string weaponID)
    {
        // To limit the clunckiness of passing weapon info around, each vessel will instead store only the string reference.

        return hangarCurrentSave.hangarWeapons.Where(x => x.stringID == weaponID).First();
    }

    public int GetWeaponInstanceCount(string name)
    {
        int count =  hangarCurrentSave.hangarWeapons.Where(x => x.name == name).Count();
        return count;
    }

    public void RemoveWeaponInstance(string name)
    {
        WeaponInfo removedObject = hangarCurrentSave.hangarWeapons.Where(x => x.name == name && x.isAttached == false).First();
        hangarCurrentSave.hangarWeapons.Remove(removedObject);
    }

    public void AddWeaponInstance(WeaponAsset asset)
    {
        WeaponInfo info = asset.ConvertToWeaponInfo();
        hangarCurrentSave.hangarWeapons.Add(info);
    }

    public ShipInfo GetShipItem(string shipID) 
    {
        // As there are no duplicates storing ship identifiers through string, hangar will only store id for simplification.
        // Only will fetch and store ship info of vessels that are either in use or unlocked.

        return hangarCurrentSave.hangarShips.Where(x => x.stringID == shipID).First();
    }
}

[System.Serializable]
public class UserStatus
{
    public int userLevel = 0;
    public int credits = 4000;

    public int goalsCollected = 0;
    public int totalKills = 0;
    public int peopleSaved = 0;

}