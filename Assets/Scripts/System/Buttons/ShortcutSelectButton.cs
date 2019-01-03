using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShortcutSelectButton : MonoBehaviour
{

    [SerializeField]
    private GameObject content;
    [SerializeField]
    private GameObject panel;


    [SerializeField]
    private Sprite defSprite;
    [SerializeField]
    private GameObject[] button;

    [SerializeField]
    private bool[] useItem = { false, false, false, false, false };
    [SerializeField]
    private int[] itemID = new int[5];




    public void OnClick(int num)
    {
        if (!useItem[num])
        {
            foreach (Transform n in content.transform)
                GameObject.Destroy(n.gameObject);
            panel.SetActive(true);
            ItemDataBase.Instance.createSelectShortcutNode(content);

            foreach (Transform t in content.transform)
            {
                Button b = t.GetComponent<Button>();
                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(() =>
                {
                    // name id value
                    string[] item = b.name.Split(',');
                    int id = int.Parse(item[1]);
                    for (int i = 0; i < 5; i++)
                        if (itemID[i] == id && useItem[i])
                            resetClick(i);

                    itemID[num] = id;
                    useItem[num] = true; ;
                    button[num].transform.Find("itemName").GetComponent<Text>().text = item[0];
                    button[num].transform.Find("itemImage").GetComponent<Image>().sprite = ItemDataBase.Instance.getItemStatus(id).itemSprite;
                    button[num].transform.Find("itemValue").GetComponent<Text>().text = "×\t" + ItemDataBase.Instance.getItemValue(id);
                    panel.SetActive(false);
                });
            }
        }

        if (useItem[num])
        {
            ItemDataBase.Instance.use(itemID[num], 1);
            button[num].transform.Find("itemValue").GetComponent<Text>().text = "×\t" + ItemDataBase.Instance.getItemValue(itemID[num]);
            if (ItemDataBase.Instance.getItemValue(itemID[num]) <= 0)
            {
                resetClick(num);
            }
        }
    }

    public void resetClick(int num)
    {
        useItem[num] = false;
        button[num].transform.Find("itemName").GetComponent<Text>().text = "未登録";
        button[num].transform.Find("itemImage").GetComponent<Image>().sprite = defSprite;
        button[num].transform.Find("itemValue").GetComponent<Text>().text = "";
    }

    public void valueUpdate()
    {
        for (int i = 0; i < 5; i++)
        {
            if (useItem[i])
            {
                button[i].transform.Find("itemValue").GetComponent<Text>().text = "×\t" + ItemDataBase.Instance.getItemValue(itemID[i]);
            }
        }
    }

    public void save()
    {
        List<int> s1 = new List<int>();
        s1.AddRange(itemID);

        List<bool> s2 = new List<bool>();
        s2.AddRange(useItem);


        SaveData.SetList<int>("shortcutItemID", s1);
        SaveData.SetList<bool>("shortcutItemBool", s2);
    }

    public void load()
    {
        List<int> s1 = SaveData.GetList<int>("shortcutItemID", null);
        List<bool> s2 = SaveData.GetList<bool>("shortcutItemBool", null);

        if (s1 != null && s2 != null)
        {
            itemID = s1.ToArray();
            useItem = s2.ToArray();

            for (int num = 0; num < 5; num++)
            {
                if (useItem[num])
                {
                    ItemStatus itemStatus = ItemDataBase.Instance.getItemStatus(itemID[num]);
                    button[num].transform.Find("itemName").GetComponent<Text>().text = itemStatus.itemName;
                    button[num].transform.Find("itemImage").GetComponent<Image>().sprite = itemStatus.itemSprite;
                    button[num].transform.Find("itemValue").GetComponent<Text>().text = "×\t" + ItemDataBase.Instance.getItemValue(itemID[num]);
                }
            }
        }
    }
}

