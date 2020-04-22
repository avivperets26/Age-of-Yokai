using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    [SerializeField]
    private LootButton[] lootButtons;

    private List<List<Item>> pages = new List<List<Item>>();

    private int pageIndex = 0;

    [SerializeField]
    private Text pageNumber;

    [SerializeField]
    private GameObject nextBtn, previousBtn;

    [SerializeField]//for debuging , will removed later
    private Item[] items;
    // Start is called before the first frame update
    void Start()
    {
        List<Item> tmp = new List<Item>();//for debuging

        for (int i = 0; i < items.Length; i++)
        {
            tmp.Add(items[i]);
        }
        CreatePages(tmp);
    }

    public void CreatePages(List<Item> items)
    {
        List<Item> page = new List<Item>();

        for (int i = 0; i < items.Count; i++)
        {
            page.Add(items[i]);

            if (page.Count == 4 || i == items.Count - 1)//1.Makes sure there is only 4 items in 1 LootWindow page, 2. make sure we added the last item to the Lootwindow page
            {
                pages.Add(page);

                page = new List<Item>();
            }
        }

        AddLoot();
    }

    private void AddLoot()
    {
        if (pages.Count > 0)
        {           
            pageNumber.text = pageIndex + 1 + "/" + pages.Count;//Handle page numbers

            //Handle next and prev buttons

            previousBtn.SetActive(pageIndex > 0);

            nextBtn.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);

            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    lootButtons[i].MyIcon.sprite = pages[pageIndex][i].MyIcon;//Seting the loot button icon

                    lootButtons[i].MyLoot = pages[pageIndex][i];

                    lootButtons[i].gameObject.SetActive(true);//making sure the loot button is visible

                    string title = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[pages[pageIndex][i].MyQuality], pages[pageIndex][i].MyTitle);

                    lootButtons[i].MyTitle.text = title;//make the title is correct
                }

            }
        }
    }

    public void ClearButtons()
    {
        foreach (LootButton btn in lootButtons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    public void NextPage()
    {
        if (pageIndex < pages.Count - 1)// We check if we have more pages
        {
            pageIndex++;

            ClearButtons();

            AddLoot();
        }
    }

    public void PreviousPage()
    {
        if (pageIndex >0)//we are checking if we have more pages in the backwards direction
        {
            pageIndex--;

            ClearButtons();

            AddLoot();
        }
    }
}
