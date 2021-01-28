using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarMenu : MonoBehaviour
{
    public GameObject shopMenu;
    public GameObject inventoryMenu;
    public GameObject shipMenu;
    public GameObject equipmentMenu;

    public void RevealInventory()
    {
        shipMenu.SetActive(false);
        shopMenu.SetActive(false);
        equipmentMenu.SetActive(false);

        inventoryMenu.SetActive(true);
    }

    public void RevealShop()
    {
        shipMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        equipmentMenu.SetActive(false);

        shopMenu.SetActive(true);

        ShopMenu menu = shopMenu.GetComponent<ShopMenu>();
        menu.PopulateWeaponList();
    }

    public void RevealShips()
    {
        shopMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        equipmentMenu.SetActive(false);

        shipMenu.SetActive(true);

        IEquipmentMenu menu = shipMenu.GetComponent<IEquipmentMenu>();
        menu.PopulateInventoryList();
    }

    public void RevealEquipmentMenu()
    {
        shipMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        shopMenu.SetActive(false);

        equipmentMenu.SetActive(true);

        IEquipmentMenu menu = equipmentMenu.GetComponent<IEquipmentMenu>();
        menu.InitialiseMenu();
        menu.PopulateInventoryList();
    }

    public void HideHangarMenu()
    {
        gameObject.SetActive(false);
    }
}
