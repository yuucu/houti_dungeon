using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpenButton : MonoBehaviour
{

    [SerializeField]
    private GameObject shopDataBaseObj;
    private ShopDataBase shopDataBase;

    void Start()
    {
        shopDataBase = shopDataBaseObj.GetComponent<ShopDataBase>();
    }

    public void OnClick()
    {
        shopDataBase.updateShopList();
    }
}
