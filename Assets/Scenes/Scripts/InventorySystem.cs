using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class InventorySystem : MonoBehaviour
{

    public GameObject ItemInfoUI;
    public static InventorySystem Instance { get; set; } //singleton

    public GameObject inventoryScreenUI;
    public bool isOpen;

    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    //public bool isFull;


    // Pickup Pop Up

    public GameObject pickupAlert;
    public TextMeshProUGUI pickupName;
    public UnityEngine.UI.Image pickupImage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        

        isOpen = false;
        PopulateSlotList();
    }

    private void PopulateSlotList()
    {
        foreach(Transform child in inventoryScreenUI.transform)
        {
            if(child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(slotList);
        }

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            isOpen = true;
            

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (!CraftingSystem.Instance.isOpen)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
            isOpen = false;
            
        }
    }
    public void AddToInventory(string itemName)
    {
        
            
            whatSlotToEquip = FindNextEmptySlot();
        Debug.Log(whatSlotToEquip);
            

            itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            itemList.Add(itemName);   

            Sprite sprite = itemToAdd.GetComponent<UnityEngine.UI.Image>().sprite;

            TriggerPickupPopUp(itemName, sprite);


            ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
        
    }

    void TriggerPickupPopUp(string itemName,Sprite itemSprite)
    {
        pickupAlert.SetActive(true);

        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;
        StartCoroutine(DisplayPickup());

    }
    IEnumerator DisplayPickup ()
    {
        yield return new WaitForSeconds(2f);
        pickupAlert.SetActive(false);
    }

    public GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
            
        }
        return new GameObject();
    }




    public bool CheckifFull()
    {
        int counter = 0;
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                counter += 1;
            }          
        }
        if(counter == 21)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    public void RemoveItem(string nameToRemove ,int amountToRemove)
    {

        int counter = amountToRemove;

        for(var i =slotList.Count -1;i>=0;i--)
        {
            if (slotList[i].transform.childCount>0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove +"(Clone)" && counter !=0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;

                }
            }
        }
        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }
    public void ReCalculateList()
    {
        itemList.Clear();   

        foreach(GameObject slot in slotList )
        {
            if(slot.transform.childCount > 0)
            {

                string name = slot.transform.GetChild(0).name;

                string str2 = "(Clone)";

                string result = name.Replace(str2, "");

                itemList.Add(result);
            }
        }
    }

}