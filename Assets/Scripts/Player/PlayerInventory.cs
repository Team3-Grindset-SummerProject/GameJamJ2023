using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int inventorySpace = 20;
    private int blackHoles = 0;
    private int ned = 0;
    private int laserGun = 0;
    private int unoReverseCard = 0;
    private int glue = 0;
    private int acid = 0;
    private int heavyItems;
    private int mediumItems;
    private int lightItems;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addItem(int itemWeight, string itemName)
    {
        if(inventorySpace - itemWeight < 0)
        {
            return;
        }
        if (itemName.Equals("acid"))
        {
            if(lightItems == 7)
            {
                return;
            }
            inventorySpace -= itemWeight;
            acid++;
            lightItems++;
        }
        if (itemName.Equals("glue"))
        {
            if (lightItems == 7)
            {
                return;
            }
            inventorySpace -= itemWeight;
            glue++;
            lightItems++;
        }
        if (itemName.Equals("laserGun"))
        {
            if (mediumItems == 4)
            {
                return;
            }
            inventorySpace -= itemWeight;
            laserGun++;
            mediumItems++;
        }
        if (itemName.Equals("unoCard"))
        {
            if (mediumItems == 4)
            {
                return;
            }
            inventorySpace -= itemWeight;
            unoReverseCard++;
            mediumItems++;
        }
        if (itemName.Equals("blackHole"))
        {
            if (heavyItems == 2)
            {
                return;
            }
            inventorySpace -= itemWeight;
            blackHoles++;
            heavyItems++;
        }
        if (itemName.Equals("NED"))
        {
            if (heavyItems == 2)
            {
                return;
            }
            inventorySpace -= itemWeight;
            ned++;
            heavyItems++;
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
        if (itemName.Equals("glue"))
        {
            if (glue == 0)
            {
                return;
            }
            glue--;
            lightItems--;
        }
        if (itemName.Equals("laserGun"))
        {
            if (laserGun == 0)
            {
                return;
            }
            laserGun--;
            mediumItems--;
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
        if (itemName.Equals("NED"))
        {
            if (ned == 0)
            {
                return;
            }
            ned--;
            heavyItems--;
        }
    }

}
