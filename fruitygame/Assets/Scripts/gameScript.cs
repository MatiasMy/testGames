using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; //MAKE HOLD COST MONEY OR POINTS!!!!!!!!!!!!

public class gameScript : MonoBehaviour //HOLD BUTTONS WORK VISUALLY, SOMETIMES DOESNT HOLD, NOT SURE WHY. AFTER FIXING HOLD BUTTONS CONSIDER DOING SPRITES, ANIMATIONS, ADDING SOUNDS OR STARTING SHOP/TOKENS ||ACTUALLY START DOING THE INFINITE SPINNING ROLLS, DOUBLE BUTTON, CASH IN BUTTON ADN THE LEVER
{
    public List<GameObject> fruits;
    public List<GameObject> fruitPositions;
    public List<GameObject> holdButtons;
    private List<GameObject> spawnedFruits = new List<GameObject>();
    public TMP_Text winTxt;
    public TMP_Text moneyTxt;
    public int money = 100;
    private float pointsWon = 0;
    private List<Vector2Int> wins = new List<Vector2Int>();
    private bool cashIn = false;
    public int amountOfHolds = 2;
    public GameObject playButton;
    public GameObject cashInButton;
    private bool spadeMultiply = false;
    private int tempID = 0;
    private int fristHold = 0;
    private int secondHold = 0;

    void Start()
    {
        winTxt.text = "";
        moneyTxt.text = "points " + pointsWon;
    }
    public void play()  //called when the play button or cash in button is pressed, checks if the player is cashing in or playing and does the appropriate actions
    {
        if (cashIn == true) // Cash in
        {
            checkForWinConditions(); //check for win conditions after the fruits have been spawned
            cashIn = false;
            amountOfHolds = 0;
            moneyTxt.text = "points " + pointsWon;
            winTxt.text = "";
            cashInButton.transform.Find("cash-in_button_pressed").gameObject.SetActive(true);
            playButton.transform.Find("play_button").gameObject.SetActive(true);
            cashInButton.transform.Find("cash-in_button").gameObject.SetActive(false);
            playButton.transform.Find("play_button_pressed").gameObject.SetActive(false);
            cashInButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
            playButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
            return;
        }
        else // play
        {
            cashInButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
            playButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
            cashInButton.transform.Find("cash-in_button_pressed").gameObject.SetActive(false);
            playButton.transform.Find("play_button").gameObject.SetActive(false);
            cashInButton.transform.Find("cash-in_button").gameObject.SetActive(true);
            playButton.transform.Find("play_button_pressed").gameObject.SetActive(true);
            cashIn = true;
            amountOfHolds = 2;
            foreach (GameObject button in holdButtons)
            {
                button.transform.Find("hold_red").gameObject.SetActive(false);
                button.transform.Find("hold_pressed").gameObject.SetActive(false);
                button.transform.Find("hold").gameObject.SetActive(true);
            }
        }

        if (fristHold == 0 && secondHold == 0) //if nothing is being held, clear the previous fruits from the screen
        {
            foreach (GameObject fruit in spawnedFruits) //clear the previous fruits from the screen
            {
                Destroy(fruit);
            }

            spawnedFruits.Clear();
        }

        spawnFruits(); //checks if any columns want to be held and spawns fruits
    }
    public void spawnFruits()
    {
        if (fristHold == 0 && secondHold == 0) // if nothing want to be held
        {
            foreach (GameObject fruitPosition in fruitPositions)    //spawn new fruits
            {
                GameObject randFruit = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPosition.transform.position, transform.rotation);
                fruitInfo fruit = randFruit.GetComponent<fruitInfo>();
                spawnedFruits.Add(randFruit);
            }
            money = money - 1;
        }
        else
        {
            for (int i = 0; i < 15; i++)
            {
                fruitInfo FruitInfo = spawnedFruits[i].GetComponent<fruitInfo>();
                if (FruitInfo.column == fristHold || FruitInfo.column == secondHold)
                {
                    continue;
                }
                else
                {
                    Destroy(spawnedFruits[i]);
                    spawnedFruits[i] = Instantiate(fruits[Random.Range(0, fruits.Count)], fruitPositions[i].transform.position, transform.rotation);
                }
            }
        }
        foreach (GameObject fruity in spawnedFruits)
        {
            fruitInfo fruit = fruity.GetComponent<fruitInfo>();
            fruit.fruitID = tempID;
            tempID++;
        }
        tempID = 0;

        foreach (GameObject fruity in spawnedFruits)
        {
            fruitInfo fruit = fruity.GetComponent<fruitInfo>();
            if (fruit.fruitID == 0 || fruit.fruitID == 5 || fruit.fruitID == 10)
            {
                fruit.column = 1;
            }
            if (fruit.fruitID == 1 || fruit.fruitID == 6 || fruit.fruitID == 11)
            {
                fruit.column = 2;
            }
            if (fruit.fruitID == 2 || fruit.fruitID == 7 || fruit.fruitID == 12)
            {
                fruit.column = 3;
            }
            if (fruit.fruitID == 3 || fruit.fruitID == 8 || fruit.fruitID == 13)
            {
                fruit.column = 4;
            }
            if (fruit.fruitID == 4 || fruit.fruitID == 9 || fruit.fruitID == 14)
            {
                fruit.column = 5;
            }
        }

        fristHold = 0;
        secondHold = 0;
    }

    public void checkForWinConditions()
    {
        foreach (GameObject fruit in spawnedFruits)
        {
            fruit.name = fruit.name.Replace("(Clone)", "");
        }
        checkForSpades();
        checkForLinesOfFive();
        checkForLinesOfThree();
        checkForDiagonals();
        checkForZigZag();
        checkForColumns();
        checkForCoins();
        calculateWonPoints();
    }
    public void checkForSpades()
    {
        int spades = 0;
        foreach (GameObject coin in spawnedFruits)
        {
            if (coin.name == "spade")
            {
                spades++;
            }
        }
        if (spades >= 3)
        {
            spadeMultiply = true;
        }
    }
    public void checkForLinesOfFive() // check for lines of five
    {
        //line of five times the value of the fruit by five
        //check for top line
        if (spawnedFruits[0].name == spawnedFruits[1].name && spawnedFruits[0].name == spawnedFruits[2].name && spawnedFruits[0].name == spawnedFruits[3].name && spawnedFruits[0].name == spawnedFruits[4].name)
        {
            Debug.Log("line of five");
            int value = fruitValue(spawnedFruits[0].name);
            wins.Add(new Vector2Int(value, 15));
        }
        //check for middle line
        if (spawnedFruits[5].name == spawnedFruits[6].name && spawnedFruits[5].name == spawnedFruits[7].name && spawnedFruits[5].name == spawnedFruits[8].name && spawnedFruits[5].name == spawnedFruits[9].name)
        {
            Debug.Log("line of five");
            int value = fruitValue(spawnedFruits[5].name);
            wins.Add(new Vector2Int(value, 15));
        }
        //check for bottom line
        if (spawnedFruits[10].name == spawnedFruits[11].name && spawnedFruits[10].name == spawnedFruits[12].name && spawnedFruits[10].name == spawnedFruits[13].name && spawnedFruits[10].name == spawnedFruits[14].name)
        {
            Debug.Log("line of five");
            int value = fruitValue(spawnedFruits[10].name);
            wins.Add(new Vector2Int(value, 15));
        }
    }
    public void checkForLinesOfThree() // check for lines of three, four of the same in line counts as two lines
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
    public void checkForDiagonals() // check for diagonal patterns
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
    public void checkForZigZag() // check for zigzag patterns
    {
        //check for zigzag pattern
        if (spawnedFruits[5].name == spawnedFruits[1].name && spawnedFruits[5].name == spawnedFruits[7].name && spawnedFruits[5].name == spawnedFruits[3].name && spawnedFruits[5].name == spawnedFruits[9].name)
        {
            Debug.Log("zigzag");
            int value = fruitValue(spawnedFruits[5].name);
            wins.Add(new Vector2Int(value, 30));
        }
        if (spawnedFruits[5].name == spawnedFruits[11].name && spawnedFruits[5].name == spawnedFruits[7].name && spawnedFruits[5].name == spawnedFruits[13].name && spawnedFruits[5].name == spawnedFruits[9].name)
        {
            Debug.Log("zigzag");
            int value = fruitValue(spawnedFruits[5].name);
            wins.Add(new Vector2Int(value, 30));
        }
        if (spawnedFruits[10].name == spawnedFruits[6].name && spawnedFruits[10].name == spawnedFruits[12].name && spawnedFruits[10].name == spawnedFruits[8].name && spawnedFruits[10].name == spawnedFruits[14].name)
        {
            Debug.Log("zigzag");
            int value = fruitValue(spawnedFruits[10].name);
            wins.Add(new Vector2Int(value, 30));
        }
        if (spawnedFruits[0].name == spawnedFruits[6].name && spawnedFruits[0].name == spawnedFruits[2].name && spawnedFruits[0].name == spawnedFruits[8].name && spawnedFruits[0].name == spawnedFruits[4].name)
        {
            Debug.Log("zigzag");
            int value = fruitValue(spawnedFruits[0].name);
            wins.Add(new Vector2Int(value, 30));
        }
    }
    public void checkForColumns() //check for matching columns
    {
        if (spawnedFruits[0].name == spawnedFruits[5].name && spawnedFruits[0].name == spawnedFruits[10].name)
        {
            Debug.Log("frist column");
            int value = fruitValue(spawnedFruits[0].name);
            wins.Add(new Vector2Int(value, 4));
        }
        if (spawnedFruits[1].name == spawnedFruits[6].name && spawnedFruits[1].name == spawnedFruits[11].name)
        {
            Debug.Log("frist column");
            int value = fruitValue(spawnedFruits[1].name);
            wins.Add(new Vector2Int(value, 4));
        }
        if (spawnedFruits[2].name == spawnedFruits[7].name && spawnedFruits[2].name == spawnedFruits[12].name)
        {
            Debug.Log("frist column");
            int value = fruitValue(spawnedFruits[2].name);
            wins.Add(new Vector2Int(value, 4));
        }
        if (spawnedFruits[3].name == spawnedFruits[8].name && spawnedFruits[3].name == spawnedFruits[13].name)
        {
            Debug.Log("frist column");
            int value = fruitValue(spawnedFruits[3].name);
            wins.Add(new Vector2Int(value, 4));
        }
        if (spawnedFruits[4].name == spawnedFruits[9].name && spawnedFruits[4].name == spawnedFruits[14].name)
        {
            Debug.Log("frist column");
            int value = fruitValue(spawnedFruits[4].name);
            wins.Add(new Vector2Int(value, 4));
        }
    }
    public void checkForCoins() // check if theres coins, theyre worth one even if not being in any patterns
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
    public void checkForFreeSpins() // gives back 1 money "free spin"
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
        if (fruit == "cherry" || fruit == "watermelon")
        {
            return 10;
        }
        if (fruit == "lucky7")
        {
            return 20;
        }
        return 0;
    }
    public void calculateWonPoints()    //wins are stored into the "wins" list and counted here, done so that the player can win multiple times in one round
    {
        int points = 0;
        foreach (Vector2Int win in wins)
        {
            points = points + (win.x * win.y);
        }
        if (spadeMultiply)
        {
            points = points * 2;
        }
        pointsWon = pointsWon + points;
        spadeMultiply = false;
        wins.Clear();
    }

    #region hold columns
    public void holdFirstColumn()
    {
        if (amountOfHolds <= 0 || cashIn == false)
        {
            return;
        }
        GameObject parent = holdButtons[0];
        parent.transform.Find("hold_pressed").gameObject.SetActive(true);
        parent.transform.Find("hold").gameObject.SetActive(false);
        if (fristHold == 0)
        {
            fristHold = 1;
            amountOfHolds--;
            return;
        }
        if (secondHold == 0)
        {
            secondHold = 1;
            amountOfHolds--;
            turnButtonsRed();
        }
    }
    public void holdSecondColumn()
    {
        if (amountOfHolds <= 0 || cashIn == false)
        {
            return;
        }
        GameObject parent = holdButtons[1];
        parent.transform.Find("hold_pressed").gameObject.SetActive(true);
        parent.transform.Find("hold").gameObject.SetActive(false);
        if (fristHold == 0)
        {
            fristHold = 2;
            amountOfHolds--;
            return;
        }
        if (secondHold == 0)
        {
            secondHold = 2;
            amountOfHolds--;
            turnButtonsRed();
        }
    }
    public void holdThirdColumn()
    {
        if (amountOfHolds <= 0 || cashIn == false)
        {
            return;
        }
        GameObject parent = holdButtons[2];
        parent.transform.Find("hold_pressed").gameObject.SetActive(true);
        parent.transform.Find("hold").gameObject.SetActive(false);
        if (fristHold == 0)
        {
            fristHold = 3;
            amountOfHolds--;
            return;
        }
        if (secondHold == 0)
        {
            secondHold = 3;
            amountOfHolds--;
            turnButtonsRed();
        }
    }
    public void holdFourthColumn()
    {
        if (amountOfHolds <= 0 || cashIn == false)
        {
            return;
        }
        GameObject parent = holdButtons[3];
        parent.transform.Find("hold_pressed").gameObject.SetActive(true);
        parent.transform.Find("hold").gameObject.SetActive(false);
        if (fristHold == 0)
        {
            fristHold = 4;
            amountOfHolds--;
            return;
        }
        if (secondHold == 0)
        {
            secondHold = 4;
            amountOfHolds--;
            turnButtonsRed();
        }
    }
    public void holdFifthColumn()
    {
        if (amountOfHolds <= 0 || cashIn == false)
        {
            return;
        }
        GameObject parent = holdButtons[4];
        parent.transform.Find("hold_pressed").gameObject.SetActive(true);
        parent.transform.Find("hold").gameObject.SetActive(false);
        if (fristHold == 0)
        {
            fristHold = 5;
            amountOfHolds--;
            return;
        }
        if (secondHold == 0)
        {
            secondHold = 5;
            amountOfHolds--;
            turnButtonsRed();
        }
    }
    public void turnButtonsRed()
    {
        foreach (GameObject button in holdButtons)
        {
            string buttonName = button.name.Replace("reroll", ""); //remove the "reroll" from the name so that it can be compared to the hold numbers that are turned into strings
            string firstHoldStr = fristHold.ToString();
            string secondHoldStr = secondHold.ToString();
            if (buttonName == firstHoldStr || buttonName == secondHoldStr)
            {
                continue;
            }
            else
            {
                button.transform.Find("hold_red").gameObject.SetActive(true);
                button.transform.Find("hold").gameObject.SetActive(false);
            }
        }
    }
    #endregion
}