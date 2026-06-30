using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameScript : MonoBehaviour
{
    public List<GameObject> fruits;
    public List<GameObject> fruitPositions;
    private List<GameObject> spawnedFruits = new List<GameObject>();
    public TMP_Text winTxt;
    public TMP_Text moneyTxt;
    public int money = 100;
    private float pointsWon = 0;
    private int tempID = 1;
    private List<Vector2Int> wins = new List<Vector2Int>();
    private bool cashIn = false;
    public int amountOfRerolls = 2;
    public GameObject unpressed;
    public GameObject pressed;

    void Start()
    {
        winTxt.text = "";
        moneyTxt.text = "points " + pointsWon;
    }

    public void play()  //called when the play button is pressed
    {
        if (cashIn == true)
        {
            checkForWinConditions(); //check for win conditions after the fruits have been spawned
            cashIn = false;
            amountOfRerolls = 0;
            moneyTxt.text = "points " + pointsWon;
            winTxt.text = "";
            unpressed.SetActive(true);
            pressed.SetActive(false);
            return;
        }
        else
        {
            pressed.SetActive(true);
            unpressed.SetActive(false);
            cashIn = true;
            amountOfRerolls = 2;
        }
        foreach (GameObject fruit in spawnedFruits) //clear the previous fruits from the screen
        {
            Destroy(fruit);
        }

        spawnedFruits.Clear();

        foreach (GameObject fruitPosition in fruitPositions)    //spawn new fruits
        {
            GameObject randFruit = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPosition.transform.position, transform.rotation);
            fruitInfo fruit = randFruit.GetComponent<fruitInfo>();
            fruit.fruitID = tempID;
            spawnedFruits.Add(randFruit);
            tempID++;
        }
        tempID = 1;
        money = money - 1;

        checkForMatchingColumns();

        //if (fruitID1 == fruitID2 && fruitID1 == fruitID3)
        //{
        //    winTxt.text = "MEGA VICTORY";
        //    money = money + 10;
        //}
    }

    public void checkForWinConditions()
    {
        foreach (GameObject fruit in spawnedFruits)
        {
            fruit.name = fruit.name.Replace("(Clone)", "");
        }
        lineOfFive();
        linesOfThree();
        checkForDiagonals();
        checkForZigZag();
        checkForCoins();
        calculateWonPoints();
    }
    public void checkForMatchingColumns() //check for matching columns and calls functions to reroll them
    {
        if (spawnedFruits[0].name == spawnedFruits[5].name && spawnedFruits[0].name == spawnedFruits[10].name)
        {
            rerollFirstColumn();
        }
        if (spawnedFruits[1].name == spawnedFruits[6].name && spawnedFruits[1].name == spawnedFruits[11].name)
        {
            rerollSecondColumn();
        }
        if (spawnedFruits[2].name == spawnedFruits[7].name && spawnedFruits[2].name == spawnedFruits[12].name)
        {
            rerollThirdColumn();
        }
        if (spawnedFruits[3].name == spawnedFruits[8].name && spawnedFruits[3].name == spawnedFruits[13].name)
        {
            rerollFourthColumn();
        }
        if (spawnedFruits[4].name == spawnedFruits[9].name && spawnedFruits[4].name == spawnedFruits[14].name)
        {
            rerollFifthColumn();
        }
    }
    public void lineOfFive()
    {
        //line of five times the value of the fruit by five
        //check for top line
        if (spawnedFruits[0].name == spawnedFruits[1].name && spawnedFruits[0].name == spawnedFruits[2].name && spawnedFruits[0].name == spawnedFruits[3].name && spawnedFruits[0].name == spawnedFruits[4].name)
        {
            Debug.Log("line of five");
            int value = fruitValue(spawnedFruits[0].name);
            wins.Add(new Vector2Int(value, 5));
        }
        //check for middle line
        if (spawnedFruits[5].name == spawnedFruits[6].name && spawnedFruits[5].name == spawnedFruits[7].name && spawnedFruits[5].name == spawnedFruits[8].name && spawnedFruits[5].name == spawnedFruits[9].name)
        {
            Debug.Log("line of five");
            int value = fruitValue(spawnedFruits[5].name);
            wins.Add(new Vector2Int(value, 5));
        }
        //check for bottom line
        if (spawnedFruits[10].name == spawnedFruits[11].name && spawnedFruits[10].name == spawnedFruits[12].name && spawnedFruits[10].name == spawnedFruits[13].name && spawnedFruits[10].name == spawnedFruits[14].name)
        {
            Debug.Log("line of five");
            int value = fruitValue(spawnedFruits[10].name);
            wins.Add(new Vector2Int(value, 5));
        }
    }
    public void linesOfThree()
    {
        //lines of three times the value of the fruit by three
        //check for top line
        if (spawnedFruits[0].name == spawnedFruits[1].name && spawnedFruits[0].name == spawnedFruits[2].name)
        {
            Debug.Log("line of three");
            int value = fruitValue(spawnedFruits[0].name);
            wins.Add(new Vector2Int(value, 3));
        }
        if (spawnedFruits[1].name == spawnedFruits[2].name && spawnedFruits[1].name == spawnedFruits[3].name)
        {
            Debug.Log("line of three");
            int value = fruitValue(spawnedFruits[1].name);
            wins.Add(new Vector2Int(value, 3));
        }
        if (spawnedFruits[2].name == spawnedFruits[3].name && spawnedFruits[2].name == spawnedFruits[4].name)
        {
            Debug.Log("line of three");
            int value = fruitValue(spawnedFruits[2].name);
            wins.Add(new Vector2Int(value, 3));
        }
        //check for middle line
        if (spawnedFruits[5].name == spawnedFruits[6].name && spawnedFruits[5].name == spawnedFruits[7].name)
        {
            Debug.Log("line of three");
            int value = fruitValue(spawnedFruits[7].name);
            wins.Add(new Vector2Int(value, 3));
        }
        if (spawnedFruits[6].name == spawnedFruits[7].name && spawnedFruits[6].name == spawnedFruits[8].name)
        {
            Debug.Log("line of three");
            int value = fruitValue(spawnedFruits[6].name);
            wins.Add(new Vector2Int(value, 3));
        }
        if (spawnedFruits[7].name == spawnedFruits[8].name && spawnedFruits[7].name == spawnedFruits[9].name)
        {
            Debug.Log("line of three");
            int value = fruitValue(spawnedFruits[7].name);
            wins.Add(new Vector2Int(value, 3));
        }
        //check for bottom line
        if (spawnedFruits[10].name == spawnedFruits[11].name && spawnedFruits[10].name == spawnedFruits[12].name)
        {
            Debug.Log("line of three");
            int value = fruitValue(spawnedFruits[12].name);
            wins.Add(new Vector2Int(value, 3));
        }
        if (spawnedFruits[11].name == spawnedFruits[12].name && spawnedFruits[11].name == spawnedFruits[13].name)
        {
            Debug.Log("line of three");
            int value = fruitValue(spawnedFruits[11].name);
            wins.Add(new Vector2Int(value, 3));
        }
        if (spawnedFruits[12].name == spawnedFruits[13].name && spawnedFruits[12].name == spawnedFruits[14].name)
        {
            Debug.Log("line of three");
            int value = fruitValue(spawnedFruits[12].name);
            wins.Add(new Vector2Int(value, 3));
        }
    }
    public void checkForDiagonals()
    {
        //check for diagonals where it starts with 0
        if (spawnedFruits[0].name == spawnedFruits[6].name && spawnedFruits[0].name == spawnedFruits[12].name)
        {
            Debug.Log("diagonal");
            int value = fruitValue(spawnedFruits[0].name);
            wins.Add(new Vector2Int(value, 6));
        }
        //check for diagonals where it starts with 1
        if (spawnedFruits[1].name == spawnedFruits[7].name && spawnedFruits[1].name == spawnedFruits[13].name)
        {
            Debug.Log("diagonal");
            int value = fruitValue(spawnedFruits[1].name);
            wins.Add(new Vector2Int(value, 6));
        }
        //check for diagonals where it starts with 2
        if (spawnedFruits[2].name == spawnedFruits[6].name && spawnedFruits[2].name == spawnedFruits[10].name)
        {
            Debug.Log("diagonal");
            int value = fruitValue(spawnedFruits[2].name);
            wins.Add(new Vector2Int(value, 6));
        }
        if (spawnedFruits[2].name == spawnedFruits[8].name && spawnedFruits[2].name == spawnedFruits[14].name)
        {
            Debug.Log("diagonal");
            int value = fruitValue(spawnedFruits[2].name);
            wins.Add(new Vector2Int(value, 6));
        }
        //check for diagonals where it starts with 3
        if (spawnedFruits[3].name == spawnedFruits[7].name && spawnedFruits[3].name == spawnedFruits[11].name)
        {
            Debug.Log("diagonal");
            int value = fruitValue(spawnedFruits[3].name);
            wins.Add(new Vector2Int(value, 6));
        }
        //check for diagonals where it starts with 4
        if (spawnedFruits[4].name == spawnedFruits[8].name && spawnedFruits[4].name == spawnedFruits[12].name)
        {
            Debug.Log("diagonal");
            int value = fruitValue(spawnedFruits[4].name);
            wins.Add(new Vector2Int(value, 6));
        }
    }
    public void checkForZigZag()
    {
        //check for zigzag pattern
        if (spawnedFruits[5].name == spawnedFruits[1].name && spawnedFruits[5].name == spawnedFruits[7].name && spawnedFruits[5].name == spawnedFruits[3].name && spawnedFruits[5].name == spawnedFruits[9].name)
        {
            Debug.Log("zigzag");
            int value = fruitValue(spawnedFruits[5].name);
            wins.Add(new Vector2Int(value, 10));
        }
        if (spawnedFruits[5].name == spawnedFruits[11].name && spawnedFruits[5].name == spawnedFruits[7].name && spawnedFruits[5].name == spawnedFruits[13].name && spawnedFruits[5].name == spawnedFruits[9].name)
        {
            Debug.Log("zigzag");
            int value = fruitValue(spawnedFruits[5].name);
            wins.Add(new Vector2Int(value, 10));
        }
        if (spawnedFruits[10].name == spawnedFruits[6].name && spawnedFruits[10].name == spawnedFruits[12].name && spawnedFruits[10].name == spawnedFruits[8].name && spawnedFruits[10].name == spawnedFruits[14].name)
        {
            Debug.Log("zigzag");
            int value = fruitValue(spawnedFruits[10].name);
            wins.Add(new Vector2Int(value, 10));
        }
        if (spawnedFruits[0].name == spawnedFruits[6].name && spawnedFruits[0].name == spawnedFruits[2].name && spawnedFruits[0].name == spawnedFruits[8].name && spawnedFruits[0].name == spawnedFruits[4].name)
        {
            Debug.Log("zigzag");
            int value = fruitValue(spawnedFruits[0].name);
            wins.Add(new Vector2Int(value, 10));
        }
    }
    public void checkForCoins()
    {
        int amountOfCoins = 0;
        foreach (GameObject coin in spawnedFruits)
        {
            if (coin.name == "coin")
            {
                wins.Add(new Vector2Int(1, 1));
                amountOfCoins++;
            }
        }
        if (amountOfCoins > 0)
        {
            Debug.Log(amountOfCoins + " coins");   
        }
    }
    public void checkForFreeSpins()
    {
        foreach (GameObject free in spawnedFruits)
        {
            if (free.name == "free")
            {
                money++;
                Debug.Log("free spin");
            }
        }
    }
    public int fruitValue(string fruit) //assing values to each fruit
    {
        if (fruit == "clover" || fruit == "coin")
        {
            return 1;
        }
        if (fruit == "banana" || fruit == "orange")
        {
            return 2;
        }
        else if (fruit == "bar")
        {
            return 3;
        }
        if (fruit == "star" || fruit == "strawberry")
        {
            return 5;
        }
        if (fruit == "lucky7")
        {
            return 7;
        }
        if (fruit == "cherry")
        {
            return 10;
        }
        if (fruit == "free" || fruit == "spade")
        {
            return 20;
        }
        return 0;
    }
    public void calculateWonPoints()    //wins are stored into the "wins" list and counted here, done so that the player can win multiple times in one round
    {
        foreach (Vector2Int win in wins)
        {
            pointsWon = pointsWon + (win.x * win.y);
        }
        wins.Clear();
    }

    #region checking columns
    public void rerollFirstColumn()
    {
        if (amountOfRerolls <= 0)
        {
            return;
        }
        wait();
        Destroy(spawnedFruits[0]);
        spawnedFruits[0] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[0].transform.position, transform.rotation);

        Destroy(spawnedFruits[5]);
        spawnedFruits[5] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[5].transform.position, transform.rotation);

        Destroy(spawnedFruits[10]);
        spawnedFruits[10] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[10].transform.position, transform.rotation);
    }
    public void rerollSecondColumn()
    {
        if (amountOfRerolls <= 0)
        {
            return;
        }
        wait();
        Destroy(spawnedFruits[1]);
        spawnedFruits[1] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[1].transform.position, transform.rotation);

        Destroy(spawnedFruits[6]);
        spawnedFruits[6] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[6].transform.position, transform.rotation);

        Destroy(spawnedFruits[11]);
        spawnedFruits[11] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[11].transform.position, transform.rotation);
    }
    public void rerollThirdColumn()
    {
        if (amountOfRerolls <= 0)
        {
            return;
        }
        wait();
        Destroy(spawnedFruits[2]);
        spawnedFruits[2] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[2].transform.position, transform.rotation);

        Destroy(spawnedFruits[7]);
        spawnedFruits[7] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[7].transform.position, transform.rotation);

        Destroy(spawnedFruits[12]);
        spawnedFruits[12] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[12].transform.position, transform.rotation);
    }
    public void rerollFourthColumn()
    {
        if (amountOfRerolls <= 0)
        {
            return;
        }
        wait();
        Destroy(spawnedFruits[3]);
        spawnedFruits[3] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[3].transform.position, transform.rotation);

        Destroy(spawnedFruits[8]);
        spawnedFruits[8] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[8].transform.position, transform.rotation);

        Destroy(spawnedFruits[13]);
        spawnedFruits[13] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[13].transform.position, transform.rotation);
    }
    public void rerollFifthColumn()
    {
        if (amountOfRerolls <= 0)
        {
            return;
        }
        wait();
        Destroy(spawnedFruits[4]);
        spawnedFruits[4] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[4].transform.position, transform.rotation);

        Destroy(spawnedFruits[9]);
        spawnedFruits[9] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[9].transform.position, transform.rotation);

        Destroy(spawnedFruits[14]);
        spawnedFruits[14] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[14].transform.position, transform.rotation);
    }
    IEnumerator wait()
    {
        int i = 1;
        yield return new WaitForSeconds(0.5f);
        i++;
    }
    #endregion
    public void rerollButtonPressed()
    {
        amountOfRerolls -= 1;
    }
}