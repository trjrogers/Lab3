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

        private int rollCount = 0;
        private int uScoreCardCount = 0;
        private int value;
        private int[] counts = new int[5];
        int howMany;
        int whichValue;
        Random random = new Random();

        // you'll need an instance variable for the user's scorecard - an array of 13 ints
        private int[] scorecard = new int[13];
        // as well as an instance variable for 0 to 5 dice as the user rolls - array or list<int>?
        //private List<int> roll = new List<int>(5);
        // as well as an instance variable for 0 to 5 dice that the user wants to keep - array or list<int>? 
        List<int> dice = new List<int>(5);


        private List<int> keep = new List<int>(5);
        int numDie;

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
        }

        #region Scoring Methods
        /* This method returns the number of times the parameter value occurs in the list of dice.
         * Calling it with 5 and the following dice 2, 3, 5, 5, 6 returns 2.
         */
        private int Count(int value, List<int> dice)
        {
            return 0;
        }

        /* This method counts how many 1s, 2s, 3s ... 6s there are in a list of ints that represent a set of dice
         * It takes a list of ints as it's parameter.  It should create an array of 6 integers to store the counts.
         * It should then call Count with a value of 1 and store the result in the first element of the array.
         * It should then repeat the process of calling Count with 2 - 6.
         * It returns the array of counts.
         * All of the rest of the scoring methods can be "easily" calculated using the array of counts.
         */
        private int[] GetCounts(List<int> dice)
        {
            return null;
        }

        /* Each of these methods takes the array of counts as a parameter and returns the score for a dice value.
         */
        private int ScoreOnes(int[] counts)
        {
            return 0;
        }

        private int ScoreTwos(int[] counts)
        {
            return 0;
        }

        private int ScoreThrees(int[] counts)
        {
            return 0;
        }

        private int ScoreFours(int[] counts)
        {
            return 0;
        }

        private int ScoreFives(int[] counts)
        {
            return 0;
        }

        private int ScoreSixes(int[] counts)
        {
            return 0;
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
            return 0;
        }

        /* This method calls HasCount(3...) and if there are 3 of a kind calls Sum to calculate the score.
         */ 
        private int ScoreThreeOfAKind(int[] counts)
        {
            return 0;
        }

        private int ScoreFourOfAKind(int[] counts)
        {
            return 0;
        }

        private int ScoreYahtzee(int[] counts)
        {
            return 0;
        }

        /* This method calls HasCount(2 and HasCount(3 to determine if there's a full house.  It calls sum to 
         * calculate the score.
         */ 
        private int ScoreFullHouse(int[] counts)
        {
            return 0;
        }

        private int ScoreSmallStraight(int[] counts)
        {
            return 0;
        }

        private int ScoreLargeStraight(int[] counts)
        {   
            return 0;
        }

        private int ScoreChance(int[] counts)
        {
            return 0;
        }

        /* This method makes it "easy" to call the "right" scoring method when you click on an element
         * in the user score card on the UI.
         */ 
        private int Score(int whichElement, List<int> dice)
        {
            int[] counts = GetCounts(dice);
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
        }

        // this set has to do with user's scorecard UI
        private void ResetUserUIScoreCard()
        {
        }

        // this method adds the subtotals as well as the bonus points when the user is done playing
        public void UpdateUserUIScoreCard()
        {
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
                die.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\..\\..\\Dice\\die" + dice[i] + ".png");
                die.Visible = true;
        }

        public void ShowAllRollDie()
        {
            for (int i = 0; i < numDie; i++)
            {
                ShowRollDie(i);
            }
        }
        #endregion

        #region Event Handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            /* reset the user's scorecard
            * Hide the roll dice
            * Hide the keep dice
            * Hide the computer's dice
            */
            HideAllComputerKeepDice();
            HideAllKeepDice();
            HideAllRollDice();
            keep.Insert(0, -1);
            keep.Insert(1, -1);
            keep.Insert(2, -1);
            keep.Insert(3, -1);
            keep.Insert(4, -1);

        }

        private void rollButton_Click(object sender, EventArgs e)
        {
            // DON'T WORRY ABOUT ROLLING MULTIPLE TIMES UNTIL YOU CAN SCORE ONE ROLL
            // hide all of the keep picture boxes
            HideAllKeepDice();
            // any of the die that were moved back and forth from roll to keep by the user
            // are "collapsed" in the keep data structure
            // show the keep dice again
            ShowAllKeepDie();

            // clear the roll data structure
            dice.Clear();
            // hide all of thhe roll picture boxes
            HideAllRollDice();
            // roll the right number of dice
            numDie = dice.Capacity - dice.Count;

            Roll(numDie, dice);
            // show the roll picture boxes
            ShowAllRollDie();
            // increment the number of rolls
            rollCount++;
            // disable the button if you've rolled 3 times or if there are no dice in roll area
            if (rollCount == 3 || keep.Count == 5) {
                rollButton.Enabled = false;
            }

            //roll counter
            foreach (int num in keep)
            {
                numDie++;
            }
        }

        private void userScorecard_DoubleClick(object sender, EventArgs e)
        {
            // move any rolled die into the keep dice
            // hide picture boxes for both roll and keep

            // determine which element in the score card was clicked
            // score that element
            // put the score in the scorecard and the UI
            // disable this element in the score card

            // clear the keep dice
            // reset the roll count
            // increment the number of elements in the score card that are full
            // enable/disable buttons

            // when it's the end of the game
            // update the sum(s) and bonus parts of the score card
            // enable/disable buttons
            // display a message box?
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


            // clear the die in the roll data structure
            dice[rollIndex] = -1;
            // hide the picture box
            clickedRollDie.Visible = false;
        }

        private void keep_DoubleClick(object sender, EventArgs e)
        {
            // figure out which die you clicked on

            // figure out where in the set of roll picture boxes there's a "space"
            // move the roll die value from this die to the roll data structure in the "right place"
            // sometimes that will be at the end but if the user is moving dice back and forth
            // it may be in the middle somewhere

            // clear the die in the keep data structure
            // hide the picture box
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
