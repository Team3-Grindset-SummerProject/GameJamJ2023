using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int inventorySpace = 20;
    private int blackHoles = 0;
    private int unoReverseCard = 0;
    private int acid = 0;
    private int heavyItems = 0;
    private int mediumItems = 0;
    private int lightItems = 0;
    private int totalWeight = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addItem(string itemName)
    {
        int itemWeight = 1;
        if(itemName.Equals("acid"))
        {
            itemWeight = 2;
        }
        if (itemName.Equals("laserGun"))
        {
            itemWeight = 4;
        }
        if (itemName.Equals("blackHole"))
        {
            itemWeight = 7;
        }

        if (inventorySpace - itemWeight < 0)
        {
            return;
        }
        if (itemName.Equals("acid"))
        {
            if(lightItems >= 7)
            {
                return;
            }
            inventorySpace -= itemWeight;
            acid++;
            lightItems++;
            totalWeight += itemWeight;
        }
        if (itemName.Equals("unoCard"))
        {
            if (mediumItems >= 4)
            {
                return;
            }
            inventorySpace -= itemWeight;
            unoReverseCard++;
            mediumItems++;
            totalWeight += itemWeight;
        }
        if (itemName.Equals("blackHole"))
        {
            if (heavyItems >= 2)
            {
                return;
            }
            inventorySpace -= itemWeight;
            blackHoles++;
            heavyItems++;
            totalWeight += itemWeight;
        }
    }

    public void placeItem(string itemName)
    {
        if (itemName.Equals("acid"))
        {
            if(acid == 0)
            {
                return;
            }
            acid--;
            lightItems--;
        }

        if (itemName.Equals("unoCard"))
        {
            if (unoReverseCard == 0)
            {
                return;
            }
            unoReverseCard--;
            mediumItems--;
        }
        if (itemName.Equals("blackHole"))
        {
            if (blackHoles == 0)
            {
                return;
            }
            blackHoles--;
            heavyItems--;
        }

    }

    public int GetTotalWeight()
    {
        return totalWeight;
    }

    public int GetAcid()
    {
        return acid;
    }

    public int GetUnoCard()
    {
        return unoReverseCard;
    }

    public int GetBlackHole()
    {
        return blackHoles;
    }
}
