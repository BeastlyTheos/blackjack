using System.Collections.Generic;

public class  CardGame
{
     protected const  int NUM_RANKS = 13, NUM_SUITS = 4;
     protected enum Rank { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }; //ranks with 1-based indexing
    protected enum Suit { Club = 1, Diamond, Heart, Spade }; //suits with 1-based indexing     


    protected class Card
    {
        private Rank rank;
        private Suit suit;
private string stringForm;
private string icon;

        //constructer
        //initialises all card properties
        public Card(Rank r, Suit s)
        {
            this.rank = r;
            this.suit = s;
            this.stringForm = this.rank + " of " + this.suit + "s";

            //create icon string and assign value
            switch ((int)rank)
            {
                case 1: this.icon = "A";
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    this.icon = System.Convert.ToString((int)this.rank);
                    break;
                case 11: this.icon = "J";
                    break;
                case 12: this.icon = "Q";
                    break;
                case 13: this.icon = "K";
                    break;
                default: this.icon = "";
                    break;
            }

            //add the suit character to the icon
            switch ((int)this.suit)
            {
                case 1: this.icon += (char)9827;
                    break;
                case 2: this.icon += (char)9830;
                    break;
                case 3: this.icon += (char)9829;
                    break;
                case 4: this.icon += (char)9824;
                    break;
                default: this.icon += "";
                    break;
            }//end of suit switch
        }//end of constructer

        //Rank readonly property
        public Rank Rank
        { get { return rank; } }

        //Suit readonly property
        public Suit Suit
        { get { return suit; } }

        //Icon property
        public string Icon
        { get { return this.icon; } }

        //ToString method
        //returns string representation in the form of [rank] of [suit]
        public override string ToString()
        { return stringForm; }
    }//end of class card 
    

    protected class DealingShoe
    {
        private Card[] deck; //index 0 indicates bottom of deck
        private int count; //deck[count] indicates top of deck
        private int capacity;


        //constructor
        //builds a dealing shoe with the given number of decks
        //args: number of decks
        public DealingShoe(int n)
        {//do nothing if non-positive number of decks is specified
            if (0 >= n)
                return;

            //create array of cards large enough to hold 52 times the number of decks
            deck = new Card[capacity = NUM_RANKS * NUM_SUITS * n];

            //fill deck
            int r, s; //indicies for rank and suit
            while (0 != n--) //for every deck in the set of decks
                for (r = NUM_RANKS; 0 != r; r--) // for every rank
                    for (s = NUM_SUITS; 0 != s; s--)//for every suit
                        deck[count++] = new Card((Rank)r, (Suit)s);
        }


//add card method
        //adds a card to the top of the deck
//returns true if card was added, false otherwise
        public bool AddCard(Card c)
        {//if there is room in the deck
            if (this.count < this.capacity)
            {
                deck[this.count++] = c;
                return true;
            }
            else //deck is full
                return false;
        }

        //DealTo method
        //moves the top card from the dealing shoe into a player's given hand    
        //returns the card delt
        public Card DealTo(Player p, int handIndex)
        {
            if (0 != count && 0 <= handIndex)
            {//if there is a card in the dealing shoe
                return p.AddCard(RemoveCard(), handIndex);
            }
            else
                return null;
        }

        //DealTo method for default handIndex
        public Card DealTo(Player p)
        { return DealTo(p, 0); }

        //RemoveCard method
        //removes the top card from the deck
        //returns the removed card if there is one, null otherwise
        private Card RemoveCard()
        {//if there is a card in the deck
            if (0 != count)
                return deck[--this.count];
            else //deck is empty
                return null;
        }

        //shuffle function
        public void Shuffle()
        {
            Card temp;
            int j;
            System.Random randNum = new System.Random();

            //for every card in the deck, swap with a random card equal or lower to it
            for (int i = this.capacity - 1; 0 < i; i--)
            {
                j = randNum.Next(0, i); //pick a random index equal or lower than i

                //swap cards at the selected indicies
                temp = this.deck[i];
                this.deck[i] = this.deck[j];
                this.deck[j] = temp;
            }
        }//end of shuffle function
    }//end of class DealingShoe



    protected class Player
    {
        private string name;
        private List<Card> hand;
        private int handValue; // value of a hand of cards 
                private int numHighAces;
                private int numLowAces;
        private int winnings; // the number of chips the player has remaining 
        private int bet; // how many chips are bet on the current hand 


        //constructer
        //initialises name to the given name, and rest of attributes to default values
        public Player(string n, int w)
        {
            name = n;
            winnings = w;
            handValue = 0;
            hand = new List<Card>(8); //hand large enough to contain most blackjack hands, but will resize as required
                        numHighAces = 0;
                        numLowAces = 0;
            bet = 0;
        }

        //Name readonly property
    public string Name
        { get { return this.name; } }

        //Hand method
        //returns the card at the given index
        public Card Hand(int i)
        {
            if (i < hand.Count && 0 <= i)
                return hand[i];
            else
                return null;
        }

        //add card method
        //adds the given card to the player's hand
        public Card AddCard(Card c, int handIndex)
        {//add the card
            hand.Add(c);

            switch ((int)c.Rank)
            {
                case 1:
                    if (10 < handValue) //if high ace would cause bust
                    {
                        numLowAces++;
                        handValue += 1;
                    }
                    else
                    {
                        numHighAces++;
                        handValue += 11;
                    }
                                                            break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    handValue += (int)c.Rank;
                    break;
                case 11:
                case 12:
                case 13:
                    handValue += 10;
                    break;
                default:
                    break;
            }

            if (0 < numHighAces && 21 < handValue) //if has high ace and is bust
            {
                handValue -= 10;
                numHighAces--;
                numLowAces++;
            }

            return c;
        }//end of add card

        //AddCard method for no specified hand
        //returns card added, or null if fail
        public Card AddCard(Card c)
        { return this.AddCard(c, 0); }

        //EmptyHand method
        //empties a hand into a DealingShoe
//returns void
        public void EmptyHand(DealingShoe d)
        {//while there is a card remaining in the player's hand
            while (0 < hand.Count)
            if(null != hand[0])
                d.AddCard(RemoveCard());
        }

        //remove card method
        //removes a card from the player's hand
        //returns the removed card if there is one, null otherwise
        private Card RemoveCard()
        {
            if (0 < this.hand.Count)//if there is a card to be removed
            {
                Card c = hand[0];
                hand.RemoveAt(0);

                //decrement handValue
                switch ((int)c.Rank)
                {
                    case 1:
                        if (0 < numLowAces)
                        {
                            numLowAces--;
                            handValue -= 1;
                        }
                        else
                        {
                            numHighAces--;
                            handValue -= 11;
                        }
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        handValue -= (int)c.Rank;
                        break;
                    case 11:
                    case 12:
                    case 13:
                        handValue -= 10;
                        break;
                    default:
                        break;
                }

                if(0 < numLowAces && 11 >= handValue) //if there is a low ace, and there's room to make it high
                {
                    handValue += 10;
                    numHighAces++;
                    numLowAces--;
                }

                return c;
            }
            else //hand is empty
                return null;
        }//end of remove card method

        //handValue readonly property 
        public int HandValue
        {
            get { return handValue; }
        }

        //HandValueAsString
        //returns a string representing the player's possible point totals, accounting for ace variability
        public string HandValueAsString()
        {
            if (21 > handValue && 0 != numHighAces)
                return handValue.ToString() + " or " + (handValue - 10).ToString();
            else
                return handValue.ToString();
        }//end of HandValueAsString method

        //handIcons method
        //returns a string containing all the icons for the player's cards\
        public string handIcons()
        {
            string icons = "";
            for (int i = 0; this.hand.Count > i; i++)
                icons += this.hand[i].Icon + " ";

            return icons.TrimEnd();
        }

        //Winnings readonly property
        public int Winnings
        { get { return this.winnings; } }

        //Bet readonly property
        public int Bet
        { get { return this.bet; } }

        //PlaceBet method
        //moves given number of chips from winnings to bet
        //if given number is greater than winnings, all winnings are bet
        //returns: number of chips bet
        public int PlaceBet(int b)
        {
            if (this.winnings > b)
                this.bet = b;
            else
                this.bet = this.winnings; //player goes all in

            this.winnings -= this.bet; //remove the bet chips from winnings
            return this.bet;
        }//end of PlaceBet method

        //PayOut method
        //returns a player's bet to their winnings, along with their given payout
        public void Payout(double rate)
        {//return bet to player along with their payout
            this.winnings += this.bet + (int)((this.bet + 0.5) * rate); //the +0.5 is to force a rounding up when rate is 1.5
            this.bet = 0;
        }

        //LoseBet method
        //removes a player's bet
        public void LoseBet()
        { this.bet = 0; }

        //has blackjack method
        //returns: true if the player has a blackjack, false otherwise
        public bool HasBlackjack()
        { return 21 == this.handValue && 2 == this.hand.Count; }

        public bool canDoubleDown()
        { return 2 == hand.Count && 7 <= handValue && 11 >= handValue; }
    } //end of player class



    protected class LinkedList<T>
    {
        private Node first, last;
        private int count;
        private Node lastAccessedNode;
        private int lastAccessedIndex;

        public T this[int index]
        {
            get
            {
                if (0 > index || index >= this.count)
                    throw new System.ArgumentOutOfRangeException(index.ToString());
                            Node n = first;
                            if (lastAccessedIndex <= index)
                                lastAccessedIndex =  index -= lastAccessedIndex;
            while(0 !=  index--)
                n = n.Next;
            lastAccessedNode = n;
            return n.Item;}
            set
            {
                if (0 < index || index <= this.count)
                    throw new System.ArgumentOutOfRangeException(index.ToString());
                        Node n = first;
            while(0!=index--)
                n = n.Next;
            n.Item = value;}}

        //constructer
        //initialises an empty LinkedList
        public LinkedList()
        {MakeEmpty();}

        //First readonly property
        //returns the first node
        public Node First
        { get { return this.first; } }

               //Count readonly property
        public int Count
        { get { return this.count; } }

        //Add method
        //appends a new node at the end of the list with the given item
        public void Add(T item)
        {
            Node newNode = new Node(item, null);
            if (count == 0)
                first = last = newNode;
            else
                last = last.Next = newNode;

            count++;
}

        //Remove method
        //searches the list for the first node with the given item, removing it when found
        public void Remove(T item)
        {
            if (item.Equals(first.Item))
            {
                lastAccessedNode = first = first.Next; //change first to point to the second element
                lastAccessedIndex = 0;
                count--;
                return;
            }
            else
            {
                Node prev = first, curr = first.Next;
                while (null != curr)
                    if (item.Equals(curr.Item))
                    {
                        prev.Next = curr.Next;
                        count--;
                        lastAccessedNode = first;
                        lastAccessedIndex = 0;
                        return;
                    }//end of if found

                prev = curr;
                curr = curr.Next;
            }//end of looping through the list
        }//end of remove method

        //MakeEmpty method
        public void MakeEmpty()
        {
            first = last = null;
            count = 0;
            lastAccessedNode = first;
            lastAccessedIndex = 0;
        }

        //Empty method
        public bool Empty()
        {
            return count == 0;
        }

        //Node subclass
        //declared as public so other classses can index the list
        public class Node
        {
            private T item;
            private Node next;

            //constructer
public Node(T item, Node next)
            {
                this.Item = item;
                this.Next = next;
            }

            //Item property
            public T Item
            {
                get { return item; }
                set { item = value; }
            }

            //Next property
            public Node Next
            {
                get { return next; }
                set { next = value; }
            }
        }//end of node class
    }//end of playerList class
       }//end of cardGame class
