using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
public enum GameState
{
    wait,
    move
}
public class Board : MonoBehaviour
{
    public List<GameObject> listSwareSpecialBoard;
    public GameState currentState = GameState.move;
    private int width;
    private int height;
    public GameObject[,] allDots;
    public int offset;
    // Start is called before the first frame update
    void Start()
    {
        listSwareSpecialBoard = new List<GameObject>();
        width = Manage.instance.width;
        height = Manage.instance.height;
        allDots = new GameObject[width, height];
        CreateBoard();
    }


    private void CreateBoard()
    {
        // Lấy kích thước của ô theo cả chiều rộng và chiều cao

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(Manage.instance.tilePrefab, tempPosition, Quaternion.identity);
                backgroundTile.transform.parent = this.transform;
                backgroundTile.transform.name = "( " + i + ", " + j + " )";
                int countRandom = Random.Range(0, Manage.instance.dots.Length);

                int maxIerations = 0;
                while (MatchesAt(i, j, Manage.instance.dots[countRandom]) && maxIerations < 100)
                {
                    countRandom = Random.Range(0, Manage.instance.dots.Length);
                    maxIerations++;
                }

                Vector2 tempPositionDot = new Vector2(i, j + offset);

                GameObject dot = Instantiate(Manage.instance.dots[countRandom], tempPositionDot, Quaternion.identity);
                dot.GetComponent<Dot>().positionX = i;
                dot.GetComponent<Dot>().positionY = j;
                dot.transform.parent = this.transform;


                dot.transform.name = "( " + i + ", " + j + " )";
                allDots[i, j] = dot;
                SetUpCold(i, j);
                SetUpDotSnow(i, j);

            }
        }
    }

    private void SetUpDotSnow(int i, int j)
    {
        if (Manage.instance.dotSnow.Count > 0)
        {
            Debug.Log("Amount " + Manage.instance.dotSnow.Count);
            foreach (SwareSnow item in Manage.instance.dotSnow)
            {
                if (i == item.positionX && j == item.positionY)
                {


                    item.transform.position = new Vector2(i, j);
                    item.transform.parent = this.transform;
                    item.transform.name = "snow ( " + i + ", " + j + " )";
                    item.gameObject.SetActive(true);

                }
            }
        }
    }

    private void SetUpCold(int i, int j)
    {
        if (Manage.instance.dotCold.Count > 0)
        {
            foreach (SwareSpecial item in Manage.instance.dotCold)
            {
                if (i == item.positionX && j == item.positionY)
                {


                    item.transform.position = new Vector2(i, j);
                    item.transform.parent = this.transform;
                    item.transform.name = "cold ( " + i + ", " + j + " )";
                    item.gameObject.SetActive(true);

                }
            }

        }
    }

    public void DestroyDot()
    {
        CheckExplodeBomb();
        SetStatusSwareCold();
        SetStatusDotSnow();

        for (int i = 0; i < Manage.instance.width; i++)
        {
            for (int j = 0; j < Manage.instance.height; j++)
            {
                if (allDots[i,j] != null)
                {
                    
                        Explode(allDots[i, j], i, j);

                }
            }
        }
        DestroySwareCold();
        DestroySwareSnow();
        StartCoroutine(DecreaseColumn());
    }

    public void DestroySwareCold()
    {
        foreach(SwareSpecial item in Manage.instance.dotCold)
        {
            if (!item.status)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    public void DestroySwareSnow()
    {
        foreach (SwareSnow item in Manage.instance.dotSnow)
        {
            if (!item.status)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    public void CheckExplodeBomb()
    {
        for (int i = 0; i < Manage.instance.width; i++)
        {
            for (int j = 0; j < Manage.instance.height; j++)
            {
                if (allDots[i, j] != null)
                {

                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        if (allDots[i, j].GetComponent<Dot>().isExplodeHorizontal)
                        {
                            Manage.instance.findMatches.GetHorizontalPieces(j);

                        }
                        if (allDots[i, j].GetComponent<Dot>().isExplodeVertical)
                        {

                            Manage.instance.findMatches.GetVerticalPieces(i);
                        }

                    }
                }
            }
        }
    }
    public void SetStatusDotSnow()
    {
        foreach (SwareSnow item in Manage.instance.dotSnow)
        {
            GameObject leftDot = null;
            GameObject rightDot = null;
            GameObject currentDot = null;
            GameObject upDot = null;
            GameObject downDot = null;

            int maxX = allDots.GetLength(0);
            int maxY = allDots.GetLength(1);

            if (item.positionX >= 0 && item.positionX < maxX && item.positionY >= 0 && item.positionY < maxY)
            {
                currentDot = allDots[item.positionX, item.positionY];
            }
            if (item.positionX - 1 >= 0 && item.positionX - 1 < maxX)
            {
                leftDot = allDots[item.positionX - 1, item.positionY];
            }
            if (item.positionX + 1 >= 0 && item.positionX + 1 < maxX)
            {
                rightDot = allDots[item.positionX + 1, item.positionY];
            }
            if (item.positionY - 1 >= 0 && item.positionY - 1 < maxY)
            {
                downDot = allDots[item.positionX, item.positionY - 1];
            }
            if (item.positionY + 1 >= 0 && item.positionY + 1 < maxY)
            {
                upDot = allDots[item.positionX, item.positionY + 1];
            }

            if (item.scenceThree == true && item.scenceTwo==true)
            {

                if (currentDot != null && currentDot.GetComponent<Dot>().isMatched)
                {

                    item.SetStatus();
                }
                if (leftDot != null && leftDot.GetComponent<Dot>().isMatched)
                {

                    item.SetStatus();
                }
                if (rightDot != null && rightDot.GetComponent<Dot>().isMatched)
                {

                    item.SetStatus();
                }
                if (downDot != null && downDot.GetComponent<Dot>().isMatched)
                {

                    item.SetStatus();
                }
                if (upDot != null && upDot.GetComponent<Dot>().isMatched)
                {

                    item.SetStatus();
                }
            }

            if (item.scenceTwo == true && item.scenceThree==false)
            {

                if (currentDot != null && currentDot.GetComponent<Dot>().isMatched)
                {
                    item.SetAnimationThree();
                }
                if (leftDot != null && leftDot.GetComponent<Dot>().isMatched)
                {
                    item.SetAnimationThree();
                }
                if (rightDot != null && rightDot.GetComponent<Dot>().isMatched)
                {
                    item.SetAnimationThree();
                }
                if (downDot != null && downDot.GetComponent<Dot>().isMatched)
                {
                    item.SetAnimationThree();
                }
                if (upDot != null && upDot.GetComponent<Dot>().isMatched)
                {
                    item.SetAnimationThree();
                }
            }

            if (item.scenceTwo == false && item.scenceThree == false)
            {
                if (currentDot != null && currentDot.GetComponent<Dot>().isMatched)
                {
                    item.SetAnimationTwo();
                }
                if (leftDot != null && leftDot.GetComponent<Dot>().isMatched)
                {
                    item.SetAnimationTwo();
                }
                if (rightDot != null && rightDot.GetComponent<Dot>().isMatched)
                {
                    item.SetAnimationTwo();
                }
                if (downDot != null && downDot.GetComponent<Dot>().isMatched)
                {
                    item.SetAnimationTwo();
                }
                if (upDot != null && upDot.GetComponent<Dot>().isMatched)
                {
                    item.SetAnimationTwo();
                }
            }
        }
    }
    public void SetStatusSwareCold()
    {
        foreach (SwareSpecial item in Manage.instance.dotCold)
        {
            GameObject leftDot = null;
            GameObject rightDot = null;
            GameObject currentDot = null;
            GameObject upDot = null;
            GameObject downDot = null;

            int maxX = allDots.GetLength(0);
            int maxY = allDots.GetLength(1);

            if (item.positionX >= 0 && item.positionX < maxX && item.positionY >= 0 && item.positionY < maxY)
            {
                currentDot = allDots[item.positionX, item.positionY];
            }
            if (item.positionX - 1 >= 0 && item.positionX - 1 < maxX)
            {
                leftDot = allDots[item.positionX - 1, item.positionY];
            }
            if (item.positionX + 1 >= 0 && item.positionX + 1 < maxX)
            {
                rightDot = allDots[item.positionX + 1, item.positionY];
            }
            if (item.positionY - 1 >= 0 && item.positionY - 1 < maxY)
            {
                downDot = allDots[item.positionX, item.positionY - 1];
            }
            if (item.positionY + 1 >= 0 && item.positionY + 1 < maxY)
            {
                upDot = allDots[item.positionX, item.positionY + 1];
            }

            if (item.status == true)
            {
                if (currentDot != null && currentDot.GetComponent<Dot>().isMatched)
                {
                    item.status = false;
                }
                if (leftDot != null && leftDot.GetComponent<Dot>().isMatched)
                {
                    item.status = false;
                }
                if (rightDot != null && rightDot.GetComponent<Dot>().isMatched)
                {
                    item.status = false;
                }
                if (downDot != null && downDot.GetComponent<Dot>().isMatched)
                {
                    item.status = false;
                }
                if (upDot != null && upDot.GetComponent<Dot>().isMatched)
                {
                    item.status = false;
                }
            }
        }
    }


    private void TurnOffDotCold(SwareSpecial item)
    {
        foreach (GameObject itemCold in listSwareSpecialBoard)
        {
            if (itemCold.transform.position.x == item.positionX && itemCold.transform.position.y == item.positionY)
            {
                Destroy(itemCold);
            }
        }
    }

    public void Explode(GameObject gameObject, int i, int j)
    {
        if (allDots[i, j].GetComponent<Dot>().isMatched)
        {
            GameObject explode = Instantiate(Manage.instance.explode, allDots[i,j].transform.position, Quaternion.identity);
            Destroy(explode,0.4f);
            Destroy(gameObject);
            allDots[i, j] = null;
            Manage.instance.amountCurrent++;
        }

    }

    public IEnumerator DecreaseColumn()
    {
        for (int i = 0; i < Manage.instance.width; i++)
        {
            int countNull = 0;
            for (int j = 0; j < Manage.instance.height; j++)
            {
                if (allDots[i, j] == null)
                {
                    countNull++;
                }
                else if (countNull > 0)
                {
                    allDots[i, j].GetComponent<Dot>().positionY -= countNull;
                    allDots[i, j] = null;
                }
            }
        }
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(FillBoardCo());


    }

    private bool MatchesAt(int i, int j, GameObject piece)
    {
        if(i>1 && j>1)
        {
            if (allDots[i-1,j].tag== piece.tag && piece.tag == allDots[i - 2, j].tag)
            {
                return true;
            }
            if (allDots[i,j-1].tag == piece.tag && allDots[i, j - 2].tag==piece.tag)
            {
                return true;
            }
        }
        else if(i<=1 || j <= 1)
        {
            if (j > 1)
            {
                if (allDots[i,j-1].tag==piece.tag && allDots[i, j - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (i > 1)
            {
                if (allDots[i-1,j].tag==piece.tag && piece.tag == allDots[i - 2, j].tag)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ReFillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j +offset);
                    int randomDot = Random.Range(0, Manage.instance.dots.Length);
                    GameObject gameObject = Instantiate(Manage.instance.dots[randomDot], tempPosition, Quaternion.identity);
                    allDots[i, j] = gameObject;
                    gameObject.GetComponent<Dot>().positionX = i;
                    gameObject.GetComponent<Dot>().positionY = j;

                    int number = Random.Range(0, 1000); // Increase range for finer control
                    int bombChance = 50; // Set probability for bomb (50 means 5%)

                    if (Manage.instance.amountCurrent >= Manage.instance.amountToCreateBombColor)
                    {
                        if (number < bombChance)
                        {
                            gameObject.GetComponent<Dot>().isMatched = false;
                            gameObject.GetComponent<Dot>().isBombColor = true;
                            gameObject.GetComponent<Dot>().MakeBombColor();
                            Manage.instance.amountCurrent = 0;
                        }
                    }
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j  = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    

    private IEnumerator FillBoardCo()
    {
        ReFillBoard();
        yield return new WaitForSeconds(0.5f);
        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(0.5f);
            DestroyDot();
        }
        yield return new WaitForSeconds(0.5f);
        currentState = GameState.move;
    }
}
