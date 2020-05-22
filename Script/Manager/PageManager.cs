using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    //按鈕路徑
    private string path = "Prefab/PageButton/";

    //按鈕物件
    private GameObject lastPageBtn;
    private GameObject nextPageBtn;
    private GameObject pageBtn;
    private GameObject space;

    //目前頁面索引
    private int currentPage = 1;
    //頁數計數
    private int pageCount = 1;
    //顯示最後兩頁
    private bool isShowLastPage = false;

    //儲存資料
    public Dictionary<int, List<GameObject>> recordData = new Dictionary<int, List<GameObject>>();
    //分頁按鈕物件
    private Dictionary<int, GameObject> recordPage = new Dictionary<int, GameObject>();
    //換頁按鈕物件
    private Dictionary<int, GameObject> recordChangePage = new Dictionary<int, GameObject>();

    /// <summary>
    /// 生成分頁及換頁按鈕
    /// </summary>
    /// <param name="dataTotal">資料總數</param>
    /// <param name="interval">每頁資料筆數</param>
    /// <param name="pagePos">生成位置</param>
    public void PageButtonSpawn(int dataTotal, int interval, Transform pagePos)
    {
        //載入按鈕資源
        if (lastPageBtn == null)
        {
            lastPageBtn = Resources.Load<GameObject>(path + "LastPageButton");
            nextPageBtn = Resources.Load<GameObject>(path + "NextPageButton");
            pageBtn = Resources.Load<GameObject>(path + "PageButton");
            space = Resources.Load<GameObject>(path + "Space");
        }

        //總頁數
        int pageTotal = Convert.ToInt16(Math.Ceiling(float.Parse(dataTotal.ToString()) / interval));
        pageTotal = pageTotal + 1; //因為頁數索引從1開始

        //頁數計數
        pageCount = 1;
        //生成上一頁按鈕
        GameObject lastBtn = Instantiate(lastPageBtn, pagePos);
        bool isLastPage = false; //參數需配置不同記憶空間
        lastBtn.GetComponent<Button>().onClick.AddListener(() => ChangePage(isLastPage));
        recordChangePage.Add(0, lastBtn);
        for (int i = 1; dataTotal >= i; i++)
        {
            //生成間隔於第三頁
            if (pageCount == 3 && recordChangePage.ContainsKey(1) == false)
            {
                //生成間隔(...)物件(左)
                GameObject spaceBtn_left = Instantiate(space, pagePos);
                spaceBtn_left.SetActive(false);
                recordChangePage.Add(1, spaceBtn_left);
            }
            //生成間隔於倒數第三頁
            if (pageCount == pageTotal - 2 && recordChangePage.ContainsKey(2) == false)
            {
                //生成間隔(...)物件(右)
                GameObject spaceBtn_right = Instantiate(space, pagePos);
                spaceBtn_right.SetActive(false);
                recordChangePage.Add(2, spaceBtn_right);
            }
            //達到當頁資料數最大值或是已達頁尾
            if (i % interval == 0 && i != 0 || i == dataTotal)
            {
                int page = pageCount;
                //生成分頁按鈕
                GameObject pageObj = Instantiate(pageBtn, pagePos);
                pageObj.GetComponent<Button>().onClick.AddListener(() => PageClcik(page));
                pageObj.GetComponentInChildren<Text>().text = page.ToString();
                recordPage.Add(page, pageObj);
                //如果大於顯示頁數(頁數 * 頁數 + 1)
                if (i >= interval * 6)
                {
                    pageObj.SetActive(false);
                    isShowLastPage = true;
                }
                pageCount++;
            }
        }
        //總資料小於第一頁數量時
        if (dataTotal < interval)
        {
            //移除上一頁按鈕監聽
            lastBtn.GetComponent<Button>().onClick.RemoveListener(() => ChangePage(false));
            //生成下一頁按鈕
            Instantiate(nextPageBtn, pagePos);
            //預設顯示第一頁
            PageClcik(1);
            return;
        }
        //生成下一頁按鈕
        GameObject nextBtn = Instantiate(nextPageBtn, pagePos);
        bool isNextPage = true; //參數需配置不同記憶空間
        nextBtn.GetComponent<Button>().onClick.AddListener(() => ChangePage(isNextPage));
        recordChangePage.Add(3, nextBtn);

        //預設顯示第一頁
        PageClcik(1);
        ChangePage(false);
    }

    /// <summary>
    /// 點選分頁
    /// </summary>
    /// <param name="pageNumber">頁數(從1開始)</param>
    private void PageClcik(int pageNumber)
    {
        int max = recordPage.Aggregate((l, r) => l.Key > r.Key ? l : r).Key;
        currentPage = pageNumber;
        foreach (int key in recordData.Keys)
        {
            foreach (GameObject value in recordData[key])
            {
                Image pageBtn = recordPage[key].GetComponent<Image>();
                Text pageText = recordPage[key].GetComponentInChildren<Text>();
                //顯示前後五頁
                if (key + 2 >= pageNumber && key - 2 <= pageNumber ||
                    key <= 2 || key >= max - 1)
                {
                    if (recordPage.ContainsKey(key) == true)
                    {
                        recordPage[key].SetActive(true);
                    }
                }
                else
                {
                    
                    if (key <= 5 && pageNumber <= 5 ||
                        key > max - 5 && pageNumber > max - 5)
                    {
                        recordPage[key].SetActive(true);
                    }
                    else
                    {
                        recordPage[key].SetActive(false);
                    }
                }
                //顯示該頁資料
                if (key == pageNumber)
                {
                    value.SetActive(true);
                    pageBtn.color = Color.grey;
                    pageText.color = Color.white;
                }
                else
                {
                    value.SetActive(false);
                    pageBtn.color = Color.white;
                    pageText.color = Color.black;
                }
            }
        }
        //檢查點選狀態
        ChangePageCheck();
    }

    /// <summary>
    /// 點選換頁
    /// </summary>
    /// <param name="isNextPage">是否為下一頁</param>
    private void ChangePage(bool isNextPage)
    {
        if (isNextPage == true)
        {
            //切換分頁到尾端
            if (pageCount - 1 == currentPage)
            {
                ChangePageColor(3);
                return;
            }
            currentPage++;
            //當分頁沒有顯示
            if (recordPage.ContainsKey(currentPage + 2) == true && recordPage[currentPage + 2].activeInHierarchy == false)
            {
                recordPage[currentPage + 2].SetActive(true);
                if (recordPage.ContainsKey(currentPage - 3) == true)
                {
                    recordPage[currentPage - 3].SetActive(false);
                }
            }
        }
        else
        {
            //切換分頁到始端
            if (currentPage == 1)
            {
                ChangePageColor(0);
                return;
            }
            currentPage--;
            //當分頁沒有顯示
            if (recordPage.ContainsKey(currentPage - 2) == true && recordPage[currentPage - 2].activeInHierarchy == false)
            {
                recordPage[currentPage - 2].SetActive(true);
                if (recordPage.ContainsKey(currentPage + 3) == true)
                {
                    recordPage[currentPage + 3].SetActive(false);
                }
            }
        }

        //檢查點選狀態(計算後再次檢查)
        ChangePageCheck();
        //分頁選擇
        PageClcik(currentPage);
    }

    /// <summary>
    /// 檢查分頁點選狀態
    /// </summary>
    private void ChangePageCheck()
    {
        if (pageCount - 1 == currentPage)
        {
            ChangePageColor(3);
        }
        else if (currentPage == 1)
        {
            ChangePageColor(0);
        }
        else
        {
            ChangePageColor(-1);
        }
    }

    /// <summary>
    /// 分頁選擇狀態
    /// </summary>
    /// <param name="buttonID"></param>
    private void ChangePageColor(int buttonID)
    {
        //四個換頁元件
        for (int index = 0; 4 > index; index++)
        {
            if (buttonID == index)
            {
                if (recordChangePage.ContainsKey(buttonID) == true &&
                    recordChangePage[buttonID].GetComponent<Image>() != null)
                {
                    recordChangePage[buttonID].GetComponent<Image>().color = Color.grey;
                    recordChangePage[buttonID].GetComponentInChildren<Text>().color = Color.white;
                }
            }
            else
            {
                if (recordChangePage.ContainsKey(index) == true &&
                    recordChangePage[index].GetComponent<Image>() != null)
                {
                    recordChangePage[index].GetComponent<Image>().color = Color.white;
                    recordChangePage[index].GetComponentInChildren<Text>().color = Color.grey;
                }
            }
        }

        //顯示最後幾頁按鈕
        if (isShowLastPage == true)
        {
            ShowLastPage();
        }
    }

    /// <summary>
    /// 顯示最後兩頁
    /// </summary>
    private void ShowLastPage()
    {
        var max = recordPage.Aggregate((l, r) => l.Key > r.Key ? l : r).Key;
        if (currentPage >= 7)
        {
            recordChangePage[1].SetActive(true);
        }
        else
        {
            recordChangePage[1].SetActive(false);
        }
        if (currentPage <= max - 6)
        {
            recordChangePage[2].SetActive(true);
        }
        else
        {
            recordChangePage[2].SetActive(false);
        }
    }
}
