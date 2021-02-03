using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IHangarActions
{
    void RevealInventory();
    void RevealShop();
    void RevealShips();
    void RevealEquipmentMenu();
}

namespace UserInterfaces
{
    public class HangarMenu : MonoBehaviour, IInfoPanel, IHangarActions
    {
        public GameObject shopMenu;
        public GameObject inventoryMenu;
        public GameObject shipMenu;
        public GameObject equipmentMenu;

        [Space]
        public GameObject informationPanel;
        public TextMeshProUGUI creditText;

        public void InitialiseHangar()
        {
            IEquipmentMenu equipment = equipmentMenu.GetComponent<IEquipmentMenu>();
            equipment.InitialiseMenu();

            ShopMenu shop = shopMenu.GetComponent<ShopMenu>();
            shop.InitialiseMenu(this);

            IShipMenu shipInterface = shipMenu.GetComponent<IShipMenu>();
            shipInterface.InitialiseMenu(this);

            SessionData.instance.OnUserTransaction.AddListener(UpdateCreditInfo);
            UpdateCreditInfo();
        }

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

            IShipMenu menu = shipMenu.GetComponent<IShipMenu>();
            menu.OpenMenu();
        }

        public void RevealEquipmentMenu()
        {
            shipMenu.SetActive(false);
            inventoryMenu.SetActive(false);
            shopMenu.SetActive(false);

            equipmentMenu.SetActive(true);

            IEquipmentMenu menu = equipmentMenu.GetComponent<IEquipmentMenu>();
            menu.PopulateInventoryList();
        }

        /// <summary>
        /// Sets the information on the information panel based on the information set on both the info and assets.
        /// </summary>
        public void SetInfoPanel(WeaponType type, string name)
        {
            WeaponAsset asset = GameManager.Instance.weaponSettings.RetrieveFromSettings(type, name);

            informationPanel.SetActive(true);
            InformationPanel infoPanel = informationPanel.GetComponent<InformationPanel>();
            infoPanel.SetPanelInfo(name, asset.description, null);
        }

        /// <summary>
        /// Sets the information on the information panel based on the information set on both the info and assets.
        /// </summary>
        public void SetInfoPanel(string equipmentID)
        {
            WeaponInfo info = SessionData.instance.GetWeaponItem(equipmentID);
            WeaponAsset asset = GameManager.Instance.weaponSettings.RetrieveFromSettings(info.weaponType, info.name);

            informationPanel.SetActive(true);
            InformationPanel infoPanel = informationPanel.GetComponent<InformationPanel>();
            infoPanel.SetPanelInfo(info.name, asset.description, null);
        }

        public void UpdateCreditInfo()
        {
            Debug.Log(SessionData.instance.userStatus.credits.ToString());
            creditText.text = SessionData.instance.userStatus.credits.ToString();
        }

        public void HideHangarMenu()
        {
            gameObject.SetActive(false);
        }
    }

}