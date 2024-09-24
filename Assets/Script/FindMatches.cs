using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{

    public List<GameObject> currentMatches = new List<GameObject>();
    public void FindMatch()
    {

        for (int i = 0; i < Manage.instance.width; i++)
        {

            for (int j = 0; j < Manage.instance.height; j++)
            {
                GameObject dotCurrent = Manage.instance.board.allDots[i, j];
                

                if (dotCurrent != null)
                {

                    if (i > 0 && i < Manage.instance.width - 1)
                    {
                        GameObject dotLeft = Manage.instance.board.allDots[i - 1, j];
                        GameObject dotRight = Manage.instance.board.allDots[i + 1, j];

                        if (dotLeft != null && dotRight != null)
                        {
                            if (dotCurrent.tag == dotLeft.tag && dotCurrent.tag == dotRight.tag)
                            {
                                dotCurrent.GetComponent<Dot>().isMatched = true;
                                dotLeft.GetComponent<Dot>().isMatched = true;
                                dotRight.GetComponent<Dot>().isMatched = true;
                            }
                        }
                    }

                    if (j > 0 && j < Manage.instance.height - 1)
                    {
                        GameObject dotUp = Manage.instance.board.allDots[i, j + 1];
                        GameObject dotDown = Manage.instance.board.allDots[i, j - 1];

                        if (dotUp != null && dotDown != null)
                        {
                            if (dotCurrent.tag == dotUp.tag && dotCurrent.tag == dotDown.tag)
                            {
                                
                                dotCurrent.GetComponent<Dot>().isMatched = true;
                                dotUp.GetComponent<Dot>().isMatched = true;
                                dotDown.GetComponent<Dot>().isMatched = true;
                            }
                        }
                    }
                    //if (dotCurrent.GetComponent<Dot>().isMatched)
                    //{
                    //    if (dotCurrent.GetComponent<Dot>().isExplodeHorizontal)
                    //    {
                    //        Debug.Log("thuc hien ngang");
                    //        GetHorizontalPieces(j);

                    //    }
                    //    if (dotCurrent.GetComponent<Dot>().isExplodeVertical)
                    //    {
                    //        Debug.Log("thuc hien docs");

                    //        GetVerticalPieces(i);
                    //    }
                      
                    //}
                }
            }
        }

    }



    IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(0.5f);
    }


    //public void CheckToCreateBomb()
    //{
    //    if(Manage.instance.currentDot != null)
    //    {
    //        //current dot dot dot
    //        GameObject rigthDot1 = null;
    //        GameObject rigthDot2 = null;
    //        GameObject rigthDot3 = null;
    //        if(Manage.instance.currentDot.GetComponent<Dot>().positionX>0 && Manage.instance.currentDot.GetComponent<Dot>().positionX < Manage.instance.width)

    //    }
    //    if (Manage.instance.currentDot.otherDot != null)
    //    {

    //    }
    //}

    public void PracticalVertical()
    {

        for (int i = 0; i < Manage.instance.width; i++)
        {
            CheckAmountVertical(i);
        }
    }


    private void CheckAmountVertical(int y)
    {
        List<Dot> check = new List<Dot>();

        for (int i = 0; i < Manage.instance.height; i++)
        {
            if (Manage.instance.board.allDots[y, i] != null)
            {
                Dot currentDot = Manage.instance.board.allDots[y, i].GetComponent<Dot>();

                if (check.Count == 0 || currentDot.tag == check[check.Count - 1].tag)
                {
                    check.Add(currentDot);
                }
                else
                {
                    if (check.Count >= 4)
                    {
                        MakeBomb(check);
                    }
                    check.Clear();
                    check.Add(currentDot);
                }
            }
        }


    }




    public void PracticalHorizontal()
    {
        //CheckFalseHorizontal();
        Debug.Log("practice ngang");

        for (int i = 0; i < Manage.instance.height - 1; i++)
        {
            CheckAmountHorizontal(i);
        }
    }


    private void CheckAmountHorizontal(int x)
    {
        List<Dot> check = new List<Dot>();
        for (int i = 0; i < Manage.instance.width; i++)
        {
            if (Manage.instance.board.allDots[i, x] != null)
            {
                Dot currentDot = Manage.instance.board.allDots[i, x].GetComponent<Dot>();

                if (check.Count == 0 || currentDot.tag == check[check.Count - 1].tag)
                {
                    check.Add(currentDot);
                }
                else
                {
                    if (check.Count >= 4)
                    {
                        MakeBomb(check);
                    }
                    check.Clear();
                    check.Add(currentDot);
                }
            }
        }
        //if (check.Count >= 4)
        //{
        //    MakeBomb(check);
        //}


    }

    public void MakeBomb(List<Dot> check)
    {
        //float value = UnityEngine.Random.Range(0f, 100f);
        //if (value < 40)
        //{
        //    Debug.Log("Vo color");
        //        foreach (Dot item in check)
        //        {
        //            if (item != null && Manage.instance.currentDot != null)
        //            {
        //                if (item == Manage.instance.currentDot)
        //                {
        //                    Manage.instance.currentDot.isMatched = false;
        //                    Manage.instance.currentDot.isBombColor = true;
        //                    Manage.instance.currentDot.MakeBombColor();

        //                 }
        //             }
        //                    //Manage.instance.board.DestroyDot();
        //        }
            
        //}
        //else
        //{

            foreach (Dot item in check)
                {
                    if (Manage.instance.currentDot != null && item != null)
                    {
                        if (Manage.instance.currentDot == item)
                        {
                            if (Manage.instance.currentDot.isMatched == true)
                            {
                                Manage.instance.currentDot.isMatched = false;
                                if (Manage.instance.currentDot.swipeAngle > -45 && Manage.instance.currentDot.swipeAngle <= 45 || Manage.instance.currentDot.swipeAngle < -135 || Manage.instance.currentDot.swipeAngle >= 135)
                                {
                                Manage.instance.currentDot.isBombColor = false;
                                Manage.instance.currentDot.isExplodeVertical = false;

                                Manage.instance.currentDot.isExplodeHorizontal = true;
                                    Manage.instance.currentDot.MakeHorizontalBomb();
                                }
                                else
                                {
                                Manage.instance.currentDot.isBombColor = false;
                                Manage.instance.currentDot.isExplodeHorizontal = false;

                                Manage.instance.currentDot.isExplodeVertical = true;
                                    Manage.instance.currentDot.MakeVerticalBomb();
                                }
                                //Manage.instance.board.DestroyDot();

                        }
                        if (Manage.instance.currentDot.otherDot.GetComponent<Dot>().isMatched == true && Manage.instance.currentDot.otherDot != null && Manage.instance.currentDot.otherDot==item)
                            {

                                Manage.instance.currentDot.otherDot.GetComponent<Dot>().isMatched = false;
                                if (Manage.instance.currentDot.otherDot.GetComponent<Dot>().swipeAngle > -45 && Manage.instance.currentDot.otherDot.GetComponent<Dot>().swipeAngle <= 45 || Manage.instance.currentDot.otherDot.GetComponent<Dot>().swipeAngle < -135 || Manage.instance.currentDot.otherDot.GetComponent<Dot>().swipeAngle >= 135)
                                {
                                    Manage.instance.currentDot.otherDot.GetComponent<Dot>().isExplodeHorizontal = true;
                                    Manage.instance.currentDot.otherDot.GetComponent<Dot>().isBombColor = false;
                                    Manage.instance.currentDot.otherDot.GetComponent<Dot>().isExplodeVertical = false;

                                    Manage.instance.currentDot.otherDot.GetComponent<Dot>().MakeHorizontalBomb();
                                }
                                else
                                {
                                    Manage.instance.currentDot.otherDot.GetComponent<Dot>().isExplodeVertical = true;
                                    Manage.instance.currentDot.otherDot.GetComponent<Dot>().isBombColor = false;
                                    Manage.instance.currentDot.otherDot.GetComponent<Dot>().isExplodeHorizontal = false;
                                    Manage.instance.currentDot.otherDot.GetComponent<Dot>().MakeVerticalBomb();
                                }



                            }


                         }

                    }
                }



        //}

    }
   

    public void GetVerticalPieces(int y)
    {
        for (int i = 0; i < Manage.instance.height; i++)
        {
            if (Manage.instance.board.allDots[y, i] != null)
            {
                Manage.instance.board.allDots[y, i].GetComponent<Dot>().isMatched = true;

            }
        }
    }
    public void GetHorizontalPieces(int x)
    {
        for (int i = 0; i < Manage.instance.width; i++)
        {
            if (Manage.instance.board.allDots[i, x] != null)
            {
                Manage.instance.board.allDots[i,x].GetComponent<Dot>().isMatched = true;

            }
        }
    }
    public void GetColorPieces(string tag)
    {
        for(int i = 0; i < Manage.instance.width; i++)
        {
            for(int j = 0; j<Manage.instance.height; j++)
            {
                if (Manage.instance.board.allDots[i,j]!=null && Manage.instance.board.allDots[i,j].tag == tag)
                {
                    Manage.instance.board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                }
            }
        }
    }

}
