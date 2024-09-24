using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int positionX;
    public int positionY;
    public int targetX;
    public int targetY;
    public int previousPositionX;
    public int previousPositionY;
    public float swipeAngle = 0;
    public float swipResist = 0.5f;
    private Vector2 firstTouch;
    private Vector2 finallTouch;
    public GameObject otherDot;
    public bool isMatched = false;
    public bool isExplodeHorizontal=false;
    public bool isExplodeVertical = false;
    public bool isBombColor = false;
    private bool isDelay = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        targetX = positionX;
        targetY = positionY;
        if(Mathf.Abs(targetX - transform.position.x) > 0.1f)
        {
            Vector2 tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.6f);
            if (Manage.instance.board.allDots[positionX, positionY] != this.gameObject)
            {
                Manage.instance.board.allDots[positionX,positionY]= this.gameObject;
            }
            //StartCoroutine(DelayTime());
            //Manage.instance.findMatches.PracticalVertical();
            //Manage.instance.findMatches.PracticalHorizontal();



        }
        else
        {
            Vector2 tempPosition = new Vector2(positionX, transform.position.y);
            transform.position = tempPosition;
            if (!isDelay)
            {
                StartCoroutine(DelayTimeHorizontal());
            }
            //Manage.instance.findMatches.FindMatch();
        }


        if (Mathf.Abs(targetY - transform.position.y) > 0.1f)
        {
            Vector2 tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.6f);
            if (Manage.instance.board.allDots[positionX, positionY] != this.gameObject)
            {
                Manage.instance.board.allDots[positionX, positionY] = this.gameObject;
            }
            //Manage.instance.findMatches.PracticalVertical();
            //Manage.instance.findMatches.PracticalHorizontal();

            //StartCoroutine(DelayTime());




        }
        else
        {
            Vector2 tempPosition = new Vector2(transform.position.x, positionY);
            transform.position = tempPosition;
            if (!isDelay)
            {
                StartCoroutine(DelayTimeVertical());
            }
            //Manage.instance.findMatches.FindMatch();
        }
    }

  private IEnumerator DelayTimeHorizontal()
    {

        isDelay = true;
        yield return new WaitForSeconds(0.5f);
        Manage.instance.findMatches.FindMatch();
        //yield return new WaitForSeconds(0.04f);
        Manage.instance.findMatches.PracticalHorizontal();
        yield return new WaitForSeconds(0.04f);
        Manage.instance.findMatches.PracticalVertical();

        isDelay = false;

    }

    private IEnumerator DelayTimeVertical()
    {

        isDelay = true;
        yield return new WaitForSeconds(0.5f);
        Manage.instance.findMatches.FindMatch();
        //yield return new WaitForSeconds(0.04f);
        Manage.instance.findMatches.PracticalVertical();
        yield return new WaitForSeconds(0.04f);
        Manage.instance.findMatches.PracticalHorizontal();

        isDelay = false;

    }

        public void OnPointerDown(PointerEventData eventData)
    {
        firstTouch = Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Manage.instance.board.currentState == GameState.move)
        {
        finallTouch = Camera.main.ScreenToWorldPoint(eventData.position);
        CalculateAngle();

        }
    }
    private void CalculateAngle()
    {
        if (Mathf.Abs(finallTouch.x - firstTouch.x) > swipResist || Mathf.Abs(finallTouch.y - firstTouch.y) > swipResist)
        {
            Vector2 point = finallTouch - firstTouch;
            swipeAngle = Mathf.Atan2(point.y, point.x) * 180 / Mathf.PI;
            Manage.instance.board.currentState = GameState.wait;
            MoveSpiece();
            Manage.instance.currentDot = this;
          

        }
        else
        {
            Manage.instance.board.currentState = GameState.move;
        }
    }
    private void MoveSpiece()
    {
        if((swipeAngle>-45&& swipeAngle<=45) && positionX < Manage.instance.width -1)
        {
            previousPositionX = positionX;
            previousPositionY = positionY;
            otherDot = Manage.instance.board.allDots[positionX + 1, positionY];
            otherDot.GetComponent<Dot>().positionX -= 1;
            positionX += 1;
        }
        else if((swipeAngle>45 &&  swipeAngle<135) && positionY< Manage.instance.height - 1)
        {
            previousPositionX = positionX;
            previousPositionY = positionY;
            otherDot = Manage.instance.board.allDots[positionX, positionY + 1];
            otherDot.GetComponent<Dot>().positionY -= 1;
            positionY += 1;
        }
        else if((swipeAngle >135 || swipeAngle <= -135) && positionX>0) {
            previousPositionX = positionX;
            previousPositionY = positionY;
            otherDot = Manage.instance.board.allDots[positionX-1, positionY];
            otherDot.GetComponent<Dot>().positionX += 1;
            positionX -= 1;
        }
        else if((swipeAngle<=-45 && swipeAngle >= -135) &&  positionY>0)
        {
            previousPositionX = positionX;
            previousPositionY = positionY;
            otherDot = Manage.instance.board.allDots[positionX, positionY - 1];
            otherDot.GetComponent<Dot>().positionY += 1;
            positionY -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }

    private IEnumerator CheckMoveCo()
    {
        if (isBombColor)
        {
            isMatched = true;
            if(otherDot!= null)
            {
                Manage.instance.findMatches.GetColorPieces(otherDot.tag);

            }
        }

        yield return new WaitForSeconds(0.5f);
        if (otherDot != null)
        {
            if(!isMatched && !otherDot.GetComponent<Dot>().isMatched && !isExplodeVertical && !isExplodeHorizontal && !isBombColor) {
                otherDot.GetComponent<Dot>().positionX = positionX;
                otherDot.GetComponent<Dot>().positionY = positionY;
                positionX = previousPositionX;
                positionY = previousPositionY;
                Manage.instance.currentDot = null;
                Manage.instance.board.currentState = GameState.move;
            
            }
            else
            {

                Manage.instance.board.DestroyDot();
            }
        }
    }
    public void MakeHorizontalBomb()
    {
        isExplodeHorizontal = true;
        GameObject arow = Instantiate(Manage.instance.explodeHorizontal, transform.position, Quaternion.identity);
        arow.transform.parent = this.transform;
    }
    public void MakeVerticalBomb()
    {
        isExplodeVertical = true;
        GameObject arow = Instantiate(Manage.instance.explodeVertical, this.transform.position, Quaternion.identity);
        arow.transform.parent = this.transform;
    }
    public void MakeBombColor()
    {
        isBombColor = true;
        GameObject bomb = Instantiate(Manage.instance.explodeColor, this.transform.position, Quaternion.identity);
        bomb.transform.parent= this.transform;
    }
}
