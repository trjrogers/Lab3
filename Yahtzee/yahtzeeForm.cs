using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Example Yahtzee website if you've never played
// https://cardgames.io/yahtzee/

namespace Yahtzee
{
    public partial class yahtzeeForm : Form
    {
        public yahtzeeForm()
        {
            InitializeComponent();
        }

        // you may find these helpful in manipulating the scorecard and in other places in your code
        private const int NONE = -1;
        private const int ONES = 0;
        private const int TWOS = 1;
        private const int THREES = 2;
        private const int FOURS = 3;
        private const int FIVES = 4;
        private const int SIXES = 5;
        private const int THREE_OF_A_KIND = 6;
        private const int FOUR_OF_A_KIND = 7;
        private const int FULL_HOUSE = 8;
        private const int SMALL_STRAIGHT = 9;
        private const int LARGE_STRAIGHT = 10;
        private const int CHANCE = 11;
        private const int YAHTZEE = 12;

        // tracks how many rolls have happened on person's turn
        private int rollCount = 0;
        // counts how many user score card items are full
        private int uScoreCardCount = 0;
        // array holds user's scores
        private int[] scoreCard = new int[13];
        // holds the number of dice need to be rolled when roll button is clicked
        private int numDie;
        // array that holds the count of each die value in player's "hand"
        private int[] counts = new int[5];
        string EMPTY = "";
        
        // random class constructor
        Random random = new Random();
        // holds boolean value of whether or not the player's turn has ended or if they can keep rolling
        bool turnOver;

        // you'll need an instance variable for the user's scorecard - an array of 13 ints
        private int[] scorecard = new int[13];
        // as well as an instance variable for 0 to 5 dice as the user rolls - array or list<int>?
        List<int> dice = new List<int>(5);
        // as well as an instance variable for 0 to 5 dice that the user wants to keep - array or list<int>? 
        private List<int> keep = new List<int>(5);


        // this is the list of methods that I used

        // START WITH THESE 2
        // This method rolls numDie and puts the results in the list
        public void Roll(int numDie, List<int> dice)
        {

            for (int i = 0; i < numDie; i++)
            {
                int rnd = random.Next(1, 7);
                dice.Add(rnd);
            }
        }

        // This method moves all of the rolled dice to the keep dice before scoring.  All of the dice that
        // are being scored have to be in the same list 
        public void MoveRollDiceToKeep(List<int> roll, List<int> keep)
        {
            for (int i = 0; i < dice.Count; i++)
            {
                if (dice[i] != -1)
                {
                    int index = GetFirstAvailablePB(keep);
                    keep[index] = dice[i];
                }
            }
        }

        #region Scoring Methods
        /* This method returns the number of times the parameter value occurs in the list of dice.
         * Calling it with 5 and the following dice 2, 3, 5, 5, 6 returns 2.
         */
        private int Count(int value, List<int> counts)
        {
            int count = 0;
            for (int i = 0; i < keep.Count; i++)
            {
                if (keep[i] == value) {
                    count++;
                }
            }
            return count;
        }

        /* This method counts how many 1s, 2s, 3s ... 6s there are in a list of ints that represent a set of dice
         * It takes a list of ints as it's parameter.  It should create an array of 6 integers to store the counts.
         * It should then call Count with a value of 1 and store the result in the first element of the array.
         * It should then repeat the process of calling Count with 2 - 6.
         * It returns the array of counts.
         * All of the rest of the scoring methods can be "easily" calculated using the array of counts.
         */
        private int[] GetCounts(List<int> keep)
        {
            //counts still 6 at this point
            int[] counts = new int[6];
            for (int i = 0; i <= keep.Count; i++)
            {
                int value = (i + 1);
                counts[i] = Count(value, keep);     
            }
            return counts;
        }

        /* Each of these methods takes the array of counts as a parameter and returns the score for a dice value.
         */
        private int ScoreOnes(int[] counts)
        {
            return counts[0];
        }

        private int ScoreTwos(int[] counts)
        {
            return counts[1];
        }

        private int ScoreThrees(int[] counts)
        {
            return counts[2];
        }

        private int ScoreFours(int[] counts)
        {
            return counts[3];
        }

        private int ScoreFives(int[] counts)
        {
            return counts[4];
        }

        private int ScoreSixes(int[] counts)
        {
            return counts[5];
        }

        /* This method can be used to determine if you have 3 of a kind (or 4? or  5?).  The output parameter
         * whichValue tells you which dice value is 3 of a kind.
         */ 
        private bool HasCount(int howMany, int[] counts, out int whichValue)
        {

            int index = ONES;
            foreach (int count in counts)
            {
                if (howMany == count)
                {
                    whichValue = index;
                    return true;
                }
            }
            whichValue = NONE;
            return false;
        }

        /* This method returns the sum of the dice represented in the counts array.
         * The sum is the # of 1s * 1 + the # of 2s * 2 + the number of 3s * 3 etc
         */ 
        private int Sum(int[] counts)
        {
            int sum = 0;

            for (int i = 0; i < counts.Length; i++)
            {
                int x = (counts[i] * (i + 1));
                sum = sum + x;
            }
            return sum;
        }

        /* This method calls HasCount(3...) and if there are 3 of a kind calls Sum to calculate the score.
         */ 
        private int ScoreThreeOfAKind(int[] counts)
        {
            /*int score;
            HasCount(3, counts, out int whichValue);
            score = counts[whichValue] * 3;
            return score;*/

            int score = 0;
            if (HasCount(3, counts, out int whichValue))
            {
                for (int i = 0; i < 6; i++)
                {
                    score = score + (counts[i] * (i + 1));
                }
            }
            return score;
        }

        // Calls has count to see if there are 4 of a type of die in keep
        private int ScoreFourOfAKind(int[] counts)
        {
            int score = 0;
            if (HasCount(4, counts, out int whichValue))
            {
                for (int i = 0; i < 6; i++)
                {
                    score = score + (counts[i] * (i + 1));
                }
            }
            return score;
        }

        // calls has count to see if there are 5 of a type of die in keep
        private int ScoreYahtzee(int[] counts)
        {
            int score = 0;
            if (HasCount(5, counts, out int whichValue) == true)
            {
                score = score + 50;
            }
            return score;
        }

        /* This method calls HasCount(2 and HasCount(3 to determine if there's a full house.  It calls sum to 
         * calculate the score.
         */ 
        private int ScoreFullHouse(int[] counts)
        {
            if (HasCount(3, counts, out int whichValue) == true)
            {
                if (HasCount(2, counts, out int whichValue2) == true)
                {
                    return 25;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }

        // there are 3 possible permutations of a small straight {(1234) (2345) (3456)}
        // this method goes through counts in 3 loops starting at 1, 2, and 3 respectively and sees if the following indices have at least 1 count 
        private int ScoreSmallStraight(int[] counts)
        {
            bool small = false;

            if (counts[0] >= 1)
            {
                if (counts[1] >= 1)
                {
                    if (counts[2] >= 1)
                    {
                        if (counts[3] >= 1)
                        {
                            small = true;
                        }
                        else
                        {
                            small = false;
                        }
                    }
                }
            }
            else if (counts[0] < 1)
            {
                if (counts[1] >= 1)
                {
                    if (counts[2] >= 1)
                    {
                        if (counts[3] >= 1)
                        {
                            if (counts[4] >= 1)
                            {
                                small = true;
                            }
                        }
                    }
                }
                else if (counts[1] < 1)
                {
                    if (counts[2] >= 1)
                    {
                        if (counts[3] >= 1)
                        {
                            if (counts[4] >= 1)
                            {
                                if (counts[5] >= 1)
                                {
                                    small = true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                small = false;
            }

            if (small == true)
            {
                return 30;
            }
            else
            {
                return 0;
            }

        }

        // there are 2 possible permutations of a large straight {(12345) (23456)}
        // this method goes through counts in 2 loops starting at 1 and 2 respectively and sees if the following indices have at least 1 count
        private int ScoreLargeStraight(int[] counts)
        {
            bool large = false;

            if (counts[0] == 1)
            {
                if (counts[1] == 1)
                {
                    if (counts[2] == 1)
                    {
                        if (counts[3] == 1)
                        {
                            if (counts[4] == 1)
                            {
                                large = true;
                            }
                            else
                            {
                                large = false;
                            }
                        }
                        else
                        {
                            large = false;
                        }
                    }
                    else
                    {
                        large = false;
                    }
                }
                else
                {
                    large = false;
                }
            }
            else if (counts[1] == 1)
            {
                if (counts[2] == 1)
                {
                    if (counts[3] == 1)
                    {
                        if (counts[4] == 1)
                        {
                            if (counts[5] == 1)
                            {
                                large = true;
                            }
                            else
                            {
                                large = false;
                            }
                        }
                        else
                        {
                            large = false;
                        }
                    }
                    else
                    {
                        large = false;
                    }
                }
                else
                {
                    large = false;
                }
            }

            if (large == true)
            {
                return 40;
            }
            else
            {
                return 0;
            }
        }

        private int ScoreChance(int[] counts)
        {
            int chance = Sum(counts);
            return chance;
        }

        /* This method makes it "easy" to call the "right" scoring method when you click on an element
         * in the user score card on the UI.
         */ 
        private int Score(int whichElement, List<int> keep)
        {
            //counts is 6 ind at this point
            int[] counts = new int[6];
            counts = GetCounts(keep);
            switch (whichElement)
            {
                case ONES:
                    return ScoreOnes(counts);
                case TWOS:
                    return ScoreTwos(counts);
                case THREES:
                    return ScoreThrees(counts);
                case FOURS:
                    return ScoreFours(counts);
                case FIVES:
                    return ScoreFives(counts);
                case SIXES:
                    return ScoreSixes(counts);
                case THREE_OF_A_KIND:
                    return ScoreThreeOfAKind(counts);
                case FOUR_OF_A_KIND:
                    return ScoreFourOfAKind(counts);
                case FULL_HOUSE:
                    return ScoreFullHouse(counts);
                case SMALL_STRAIGHT:
                    return ScoreSmallStraight(counts);
                case LARGE_STRAIGHT:
                    return ScoreLargeStraight(counts);
                case CHANCE:
                    return ScoreChance(counts);
                case YAHTZEE:
                    return ScoreYahtzee(counts);
                default:
                    return 0;
            }
        }
        #endregion

        // set each value to some negative number because 
        // a 0 or a positive number could be an actual score
        private void ResetScoreCard(int[] scoreCard, int scoreCardCount)
        {  
            for (int i = 0; i < scoreCard.Length; i++)
            {
                scoreCard[i] = -1;
            }
        }

        // this set has to do with user's scorecard UI
        private void ResetUserUIScoreCard()
        {
            foreach (var l in new Label[] { user0, user1, user2, user3, user4, user5, user6, user7, user8, user9, user10, user11, user12 })
            {
                l.Text = "";
            }
        }

        // this method adds the subtotals as well as the bonus points when the user is done playing
        public void UpdateUserUIScoreCard()
        {
            // update the sum(s) and bonus parts of the score card
            int smallSum = 0;
            int totalSum = 0;
            for (int i = 0; i < 6; i++)
            {
                smallSum = smallSum + scoreCard[i];
            }
            userSum.Text = smallSum.ToString();

            if (smallSum >= 63)
            {
                totalSum = totalSum + 35;
                userBonus.Text = 35.ToString();
            }

            for (int i = 0; i < 12; i++)
            {
                totalSum = totalSum + scoreCard[i];
            }
            userTotalScore.Text = totalSum.ToString();
        }

        /* When I move a die from roll to keep, I put a -1 in the spot that the die used to be in.
         * This method gets rid of all of those -1s in the list.
         */
        private void CollapseDice(List<int> dice)
        {
            int numDice = dice.Count;
            for (int count = 0, i = 0; count < numDice; count++)
            {
                if (dice[i] == -1)
                    dice.RemoveAt(i);
                else
                    i++;
            }
        }

        /* When I move a die from roll to keep, I need to know which pb I can use.  It's the first spot with a -1 in it
         */
        public int GetFirstAvailablePB(List<int> keep)
        {
            return keep.IndexOf(-1);
        }

        #region UI Dice Methods
        /* These are all UI methods */
        private PictureBox GetKeepDie(int i)
        {
            PictureBox die = (PictureBox)this.Controls["keep" + i];
            return die;
        }

        public void HideKeepDie(int i)
        {
            GetKeepDie(i).Visible = false;
        }
        public void HideAllKeepDice()
        {
            for (int i = 0; i < 5; i++)
                HideKeepDie(i);
        }

        public void ShowKeepDie(int i)
        {
            PictureBox die = GetKeepDie(i);
            die.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\..\\..\\Dice\\die" + keep[i] + ".png");
            die.Visible = true;
        }

        public void ShowAllKeepDie()
        {
            for (int i = 0; i < keep.Count; i++)
            {
                if (keep[i] != -1) {
                    ShowKeepDie(i);
                }
            } 
        }

        private PictureBox GetComputerKeepDie(int i)
        {
            PictureBox die = (PictureBox)this.Controls["computerKeep" + i];
            return die;
        }

        public void HideComputerKeepDie(int i)
        {
            GetComputerKeepDie(i).Visible = false;
        }

        public void HideAllComputerKeepDice()
        {
            for (int i = 0; i < 5; i++)
                HideComputerKeepDie(i);
        }

        public void ShowComputerKeepDie(int i)
        {
            PictureBox die = GetComputerKeepDie(i);
            //die.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\..\\..\\Dice\\die" + keep[i] + ".png");
            die.Visible = true;
        }

        public void ShowAllComputerKeepDie()
        {
            for (int i = 0; i < 5; i++)
                ShowComputerKeepDie(i);
        }

        private PictureBox GetRollDie(int i)
        {
            PictureBox die = (PictureBox)this.Controls["roll" + i];
            return die;
        }

        public void HideRollDie(int i)
        {
            GetRollDie(i).Visible = false;
        }

        public void HideAllRollDice()
        {
            for (int i = 0; i < 5; i++)
                HideRollDie(i);
        }

        public void ShowRollDie(int i)
        {
                PictureBox die = GetRollDie(i);
            if (dice[i] != -1)
            {
                die.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\..\\..\\Dice\\die" + dice[i] + ".png");
                die.Visible = true;
            }
            else die.Visible = false;
        }

        public void ShowAllRollDie()
        {
            for (int i = 0; i < dice.Count; i++)
            {
                ShowRollDie(i);
            }
        }
        #endregion

        #region Event Handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            //hides all dice pb
            HideAllComputerKeepDice();
            HideAllKeepDice();
            HideAllRollDice();

            //keep list full of -1, later used for the FindFirstAvailablePB method
            keep.Insert(0, -1);
            keep.Insert(1, -1);
            keep.Insert(2, -1);
            keep.Insert(3, -1);
            keep.Insert(4, -1);

            for (int i = 0; i < 13; i++)
            {
                scoreCard[i] = -1;
            }

        }

        private void rollButton_Click(object sender, EventArgs e)
        {
            // hide all of the keep picture boxes
            HideAllKeepDice();
            
            //clears dice list

            // hide all of thhe roll picture boxes
            HideAllRollDice();
            numDie = 0;

            for (int i = 0; i < keep.Count; i++)
            {
                PictureBox die = GetKeepDie(i);
                die.Enabled = true;
            }

            //reenables empty score card items
            foreach (var l in new Label[] { user0, user1, user2, user3, user4, user5, user6, user7, user8, user9, user10, user11, user12 })
            {
                if (l.Text == EMPTY)
                {
                    l.Enabled = true;
                }
            }

            //logic for new-turn rolls
            #region
            if (turnOver == true)
            {
                rollCount = 0;
                keep.Clear();
                keep.Insert(0, -1);
                keep.Insert(1, -1);
                keep.Insert(2, -1);
                keep.Insert(3, -1);
                keep.Insert(4, -1);

                rollButton.Enabled = true;
                //figures out how many dice need re-rolled by looking at number of keep indices with the value -1
                for (int i = 0; i < 5; i++)
                {
                    if (keep[i] == -1)
                    {
                        numDie++;
                    }
                }

                int numEmpty = (5 - numDie);
                dice.Clear();

                //rolls X number of times based on numDie value
                for (int i = 0; i < keep.Count; i++)
                {
                    if (i < numDie)
                    {
                        Roll(1, dice);
                    }
                }

                for (int i = 0; i < numEmpty; i++)
                {
                    dice.Add(-1);
                }

                // show the roll picture boxes
                ShowAllRollDie();
                // increment the number of rolls
                rollCount++;

                for (int i = 0; i < keep.Count; i++)
                {
                    PictureBox die = GetKeepDie(i);
                    die.Enabled = true;
                }

                //disable the button if you've rolled 3 times or if there are no dice in roll area
                if (rollCount == 3 || !keep.Contains(-1))
                {
                    rollButton.Enabled = false;
                }
                else
                {
                    rollButton.Enabled = true;
                }

                //shows keep dice
                ShowAllKeepDie();
                turnOver = false;
            }
            #endregion
            // logic for non-new-turn  rolls
            #region
            else
            {
                //figures out how many dice need re-rolled by looking at number of keep indices with the value -1
                for (int i = 0; i < 5; i++)
                {
                    if (keep[i] == -1)
                    {
                        numDie++;
                    }
                }

                int numEmpty = (5 - numDie);
                dice.Clear();

                //rolls X number of times based on numDie value
                for (int i = 0; i < keep.Count; i++)
                {
                    if (i < numDie)
                    {
                        Roll(1, dice);
                    }
                }

                for (int i = 0; i < numEmpty; i++)
                {
                    dice.Add(-1);
                }

                // show the roll picture boxes
                ShowAllRollDie();
                // increment the number of rolls
                rollCount++;
                //disable the button if you've rolled 3 times or if there are no dice in roll area
                if (rollCount == 3 || !keep.Contains(-1))
                {
                    rollButton.Enabled = false;
                }
                else
                {
                    rollButton.Enabled = true;
                }

                //shows keep dice
                ShowAllKeepDie();
                turnOver = false;

                for (int i = 0; i < keep.Count; i++)
                {
                    PictureBox die = GetKeepDie(i);
                    die.Enabled = true;
                }
            }
            #endregion
        }

        private void userScorecard_DoubleClick(object sender, EventArgs e)
        {
            //moves any rolled die into the keep dice
            MoveRollDiceToKeep(dice, keep);
            //shows dice in keep area
            ShowAllKeepDie();
            //hides dice in roll area
            HideAllRollDice();
            //disables dice so they can't be clicked on
            for (int i = 0; i < keep.Count; i++)
            {
                PictureBox die = GetKeepDie(i);
                die.Enabled = false;
            }

            // determine which element in the score card was clicked
            Label clicked = (Label)sender;
            int clickedLabel = int.Parse(clicked.Name.Substring(4));

            int[] counts = GetCounts(keep);


            // score that element
            switch (clickedLabel)
            {
                case 0:
                    clickedLabel = ONES;
                    // score that element
                    scoreCard[0] = (Score(ONES, keep));
                    // put the score in the scorecard and the UI
                    user0.Text = (scoreCard[0].ToString());
                    uScoreCardCount++;
                    break;
                case 1:
                    clickedLabel = TWOS;
                    scoreCard[1] = (Score(TWOS, keep) * 2);
                    user1.Text = (scoreCard[1]).ToString();
                    uScoreCardCount++;
                    break;
                case 2:
                    clickedLabel = THREES;
                    scoreCard[2] = (Score(THREES, keep) * 3);
                    user2.Text = (scoreCard[2]).ToString();
                    uScoreCardCount++;
                    break;
                case 3:
                    clickedLabel = FOURS;
                    scoreCard[3] = (Score(FOURS, keep) * 4);
                    user3.Text = (scoreCard[3]).ToString();
                    uScoreCardCount++;
                    break;
                case 4:
                    clickedLabel = FIVES;
                    scoreCard[4] = (Score(FIVES, keep) * 5);
                    user4.Text = (scoreCard[4]).ToString();
                    uScoreCardCount++;
                    break;
                case 5:
                    clickedLabel = SIXES;
                    scoreCard[5] = (Score(SIXES, keep) * 6);
                    user5.Text = (scoreCard[5]).ToString();
                    uScoreCardCount++;
                    break;
                case 6:
                    clickedLabel = THREE_OF_A_KIND;
                    scoreCard[6] = ScoreThreeOfAKind(counts);
                    user6.Text = (scoreCard[6].ToString());
                    uScoreCardCount++;
                    break;
                case 7:
                    clickedLabel = FOUR_OF_A_KIND;
                    scoreCard[7] = ScoreFourOfAKind(counts);
                    user7.Text = (scoreCard[7].ToString());
                    uScoreCardCount++;
                    break;
                case 8:
                    clickedLabel = FULL_HOUSE;
                    scoreCard[8] = ScoreFullHouse(counts);
                    user8.Text = (scoreCard[8].ToString());
                    uScoreCardCount++;
                    break;
                case 9:
                    clickedLabel = SMALL_STRAIGHT;
                    scoreCard[9] = ScoreSmallStraight(counts);
                    user9.Text = (scoreCard[9].ToString());
                    uScoreCardCount++;
                    break;
                case 10:
                    clickedLabel = LARGE_STRAIGHT;
                    scoreCard[10] = ScoreLargeStraight(counts);
                    user10.Text = (scoreCard[10].ToString());
                    uScoreCardCount++;
                    break;
                case 11:
                    clickedLabel = CHANCE;
                    scoreCard[11] = ScoreChance(counts);
                    user11.Text = (scoreCard[11].ToString());
                    uScoreCardCount++;
                    break;
                case 12:
                    clickedLabel = YAHTZEE;
                    scoreCard[12] = ScoreYahtzee(counts);
                    user12.Text = (scoreCard[12].ToString());
                    uScoreCardCount++;
                    break;
            }

            // disable filled score card items and re-enable any empty ones
            foreach (var l in new Label[] { user0, user1, user2, user3, user4, user5, user6, user7, user8, user9, user10, user11, user12 })
            {
                l.Enabled = false;
            }


            // clear the keep dice
            // increment the number of elements in the score card that are full
            // enable/disable buttons
            if (clicked.Text == "0")
            {
                clicked.Enabled = true;
                clicked.Text = "";
                scoreCard[clickedLabel] = -1;
                uScoreCardCount--;
                for (int i = 0; i < keep.Count; i++)
                {
                    PictureBox die = GetKeepDie(i);
                    die.Enabled = false;
                }

                //move keep to roll
                for (int i = 0; i < keep.Capacity; i++)
                {
                    dice[i] = keep[i];
                    ShowAllRollDie();
                }
                keep.Clear();

                for (int i = 0; i < 5; i++)
                {
                    keep.Add(-1);
                    HideAllKeepDice();
                }
                rollButton.Enabled = true;
                turnOver = false;

                if (rollCount == 3)
                {
                    turnOver = true;
                }
            }
            else
            {
                turnOver = true;
                rollButton.Enabled = true;
            }

            // when it's the end of the game
            if (uScoreCardCount == 13)
            {
                UpdateUserUIScoreCard();
                MessageBox.Show("Game over!");
            }

        }

        private void roll_DoubleClick(object sender, EventArgs e)
        {
            //declares which pb was clicked
            PictureBox clickedRollDie = (PictureBox)sender;
            
            //logs which number pb was clicked 0-4
            int rollIndex= int.Parse(clickedRollDie.Name.Substring(4));
            //gets die value of the clicked pb
            int i = dice[rollIndex];
            
            //first available pb in keep area
            int availableKeep = GetFirstAvailablePB(keep);
            //keep index at availableKeep equal to die value of clicked pb
            keep[availableKeep] = i;
            
            //reuses i to the first available keep pb
            i = availableKeep;
            //using this method leads to the die image popping up in the keep area
            ShowKeepDie(i);

            //-1 for empty indices to note that it is empty
            dice[rollIndex] = -1;
            // hide the picture box
            clickedRollDie.Visible = false;
        }

        private void keep_DoubleClick(object sender, EventArgs e)
        {
            //declares which pb was clicked
            PictureBox clickedKeepDie = (PictureBox)sender;

            //logs which number pb was clicked 0-4
            int keepIndex = int.Parse(clickedKeepDie.Name.Substring(4));
            //gets die value of clicked pb
            int j = keep[keepIndex];

            //finds first available pb in roll area
            int availableRoll = GetFirstAvailablePB(dice);
            //roll index at availableKeep equal to die value of clicked pb
            dice[availableRoll] = j;

            //reuses j to first available roll pb 
            j = availableRoll;
            //using this method leads to die image popping up in roll area
            ShowRollDie(j);

            //hide the picture box
            clickedKeepDie.Visible = false;
            keep[keepIndex] = -1;
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            // Clears the UI
            HideAllComputerKeepDice();
            HideAllKeepDice();
            HideAllRollDice();
            ResetUserUIScoreCard();

            // Clears arrays and initializes keep to be full of -1s
            dice.Clear();
            keep.Clear();
            keep.Insert(0, -1);
            keep.Insert(1, -1);
            keep.Insert(2, -1);
            keep.Insert(3, -1);
            keep.Insert(4, -1);
            ResetScoreCard(scoreCard, uScoreCardCount);

            // Resets roll count
            rollCount = 0;

            // Re-enables roll button
            rollButton.Enabled = true;

        }
        #endregion
    }
}
