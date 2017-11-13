using System;

public class Blackjack : CardGame
{
    private const int STARTING_NUM_CHIPS = 10;
    private DealingShoe deck;
    private LinkedList<Player> players;
    private Player dealer;
    
    public void initialise()
    {
        players = new LinkedList<Player>();
         int numDecks = 0;
        bool isInputInvalid;
        string inputString;
        
        Console.WriteLine("Welcome to Blackjack.\n" +
                    "First, select how many cards to place in the dealing shoe,\n" +
                   "then enter your names as prompted.\n" +
                       "You will start with " + STARTING_NUM_CHIPS + " betting chips.\n" +
                       "After the cards have been delt, press 'h' or 's' to hit or stand\n" +
"Once you lose all your chips, you will be eliminated from the table.\n" +
                   "Play continues until everybody has been eliminated.\n" +
                   "\n" +
                   "Gamble responsibly.");

        //Step 1: The BlackJack deck of cards is created. 
        isInputInvalid = true;
        do
        {//prompt for number of decks
            Console.Write("Enter the number of decks you want to play with> ");
            try
            {
                numDecks = Convert.ToInt32(Console.ReadLine());
                if (0 >= numDecks)
                    Console.WriteLine("Sorry. There cannot be a non-positive number of decks.\n");
                else
                    isInputInvalid = false;
            }
            catch (FormatException)
            { Console.WriteLine("Sorry. That is not a whole number.\n"); }
        } while (isInputInvalid);
        deck = new DealingShoe(numDecks);
        Console.WriteLine("");


        //Step 0: The players and dealer are initialized. 
        dealer = new Player("dealer", 0);
do//add players until user requests to stop
        {
            do//get player name
            {
                Console.Write("Enter the name of player #{0}> ", players.Count + 1);
                inputString = Console.ReadLine();
            } while (0 >= inputString.Length);
            players.Add(new Player(inputString, STARTING_NUM_CHIPS));
            Console.WriteLine("{0} Has been added to the table.\n", inputString);

            do//ask user if more players shall be added
            {
                Console.Write("Press 'p' to add another player, 's' to start the game> ");
                inputString = Console.ReadLine();
            } while (!(0 < inputString.Length && ('p' == inputString[0] || 's' == inputString[0]))); //while input is not valid
        } while ('p' == inputString[0]); //while adding player
Console.WriteLine("");

        //announce the creation of the table
        Console.Write("A blackjack table has been created with ");
        for (LinkedList<Player>.Node pn = players.First; null != pn; pn = pn.Next)
            Console.Write(pn.Item.Name + ", ");
        Console.WriteLine(" and myself, the dealer, who will be using {0} decks of cards for this game.\n", numDecks);
    }//end of initialise blackjack method

    public int playHand()
    {
        int inputInt = 0;
        char inputChar;
        bool isInputInvalid;
        LinkedList<Player>.Node pn; //player node
        Player p; //current player

        //Step 2: The deck is shuffled.
        deck.Shuffle();
        
        //Step 3: Each player places a bet. 
for(int i = 0 ; i < players.Count ; i++)
        {//p = pn.Item;
            p = players[i];
            Console.Write("{0}, You have {1} chips. How many would you like to bet?> ", p.Name, p.Winnings);
            isInputInvalid = true;
            do
            {
                try
                {
                    inputInt = Convert.ToInt32(Console.ReadLine());
                    if (0 >= inputInt)
                        Console.WriteLine("Sorry. you cannot place a non-positive bet.\n");
                    else if (p.Winnings < inputInt)
                        Console.WriteLine("Sorry, you only have {0} chips. Please enter a lower number", p.Winnings);
                    else
                        isInputInvalid = false;
                }
                catch (FormatException)
                { Console.WriteLine("Sorry. That is not a whole number.\n"); }
            } while (isInputInvalid);

            //place the bet
            p.PlaceBet(inputInt);
            if (0 == p.Winnings)
                Console.WriteLine("You go all in with your {0} chips.\n", p.Bet);
            else
                Console.WriteLine("You bet {0} chips.\n", p.Bet);
        }//end of placing bets

        
//Step 4: Two cards are dealt to each player in a round robin fashion from the top of the deck. 
        for(int i = 0 ; 2 > i ; i++)
        {for (pn = players.First; null != pn; pn = pn.Next)
        {
            p = pn.Item;
            deck.DealTo(p);
            Console.WriteLine("{0}, You're delt a {1}.", p.Name, p.Hand(i).ToString());
}//end of each player
        
            //deal to the dealer
        deck.DealTo(dealer);
            if( 0 == i)
Console.WriteLine("I'm delt a card face down.");
            else
                Console.WriteLine("I'm delt a {0}.", dealer.Hand(i).ToString());
}//end of dealing
        Console.WriteLine("");
        
        
        //Step 5: Each player requests cards in turn until he or she stands or busts. 
        for (pn = players.First; null != pn; pn = pn.Next)
        {p = pn.Item;
        Console.WriteLine("{0}, your cards are: {1}, for {2} points.", p.Name, p.handIcons(), p.HandValueAsString());

        if (p.HasBlackjack())
            Console.WriteLine("You have a blackjack!");
else //player does not have blackjack
        {do//add cards until user requests to stand
        {do//get player's request
    {
            Console.Write("Would you like to {0}hit (h), or stand (s)?> ", p.canDoubleDown()? "double (d), ": "");
        inputChar = Console.ReadKey().KeyChar;
        Console.WriteLine(""); //for formatting

        inputChar = char.ToLower(inputChar);
    } while (!('h' == inputChar || 's' == inputChar || ('d' == inputChar && p.canDoubleDown()))); //while request is invalid
    
                        if('h' == inputChar || 'd' == inputChar)
            {if('d' == inputChar)
            {p.PlaceBet(2 * p.Bet);
                Console.WriteLine("You double your bet.");
            }
            Console.WriteLine("You're delt a {0}, which makes {1} points.",
            deck.DealTo(p), p.HandValueAsString());
        if (p.HandValue == 21) //if handValue = 21, autamatically stand
            inputChar = 's';
        else if (p.HandValue > 21) //busts
              {
        inputChar = 's';
          p.EmptyHand(deck);
          Console.WriteLine("Bust!");
          Console.WriteLine("\nBUST!\n");
            Console.WriteLine("The dealer collects your cards");
       }//end of if bust
            }//end of if hit or double
          } while ('s' != inputChar && 'd' != inputChar); //while not standing
}//end of if not blackjack
Console.WriteLine("");
        }//end of players taking their turns
        
    
        //Step 6: The dealer deals cards to himself/herself until he/she stands on 17 or higher. 
        Console.WriteLine("My cards are: {0}, for {1} points.", dealer.handIcons(), dealer.HandValue);
        if (dealer.HasBlackjack())
            Console.WriteLine("I have a blackjack.");

                while(17 > dealer.HandValue)
Console.WriteLine("I deal myself a {0}, which makes {1} points.", deck.DealTo(dealer), dealer.HandValueAsString());
                
        //if the dealer busts
        if(21 < dealer.HandValue)
        {Console.WriteLine("I bust");
        dealer.EmptyHand(deck);
        }
        Console.WriteLine("");
  

//hand termination
for (pn = players.First; null != pn; pn = pn.Next)
{p = pn.Item;

    //Step 7: Winnings are calculated. 
                    if(p.HasBlackjack())
                        if (dealer.HasBlackjack())
                        {
                            p.Payout(0);
                            Console.WriteLine("{0}, We tied with our blackjacks. {1}.", p.Name, p.Winnings);
                        }
                        else //if player has blackjack, but dealer not have blackjack
                        {
                            p.Payout(1.5);
                            Console.WriteLine("{0}, you won with your blackjack. Your winnings increase to {1}.", p.Name, p.Winnings);
                        }
                    else if (dealer.HasBlackjack()) //if player not have blackjack, but dealer has blackjack
                    {
                        p.LoseBet();
                        Console.WriteLine("{0}, I won with my blackjack. {1}.", p.Name, p.Winnings);
                    }
                    else if (p.HandValue > dealer.HandValue)
                    {//player wins
                        p.Payout(1);
Console.WriteLine("{0}, you won with your {1}. Your winnings increase to {2}.", p.Name, p.HandValue, p.Winnings);
                    }
                    else if (p.HandValue == dealer.HandValue)
                    {//tie
                        p.Payout(0);
                        if (0 == p.HandValue) //if tied on bust
                            Console.WriteLine("{0}, we both bust. Your winnnings stay at {1}.", p.Name, p.Winnings);
                        else
                        Console.WriteLine("{0}, we tied with {1}. Your winnings stay at {2}.", p.Name, p.HandValue, p.Winnings);
                    }
                    else//player loses
                    {
                        p.LoseBet();
                        if (0 == p.HandValue)
                            Console.WriteLine("{0}, you lost your bet and now have {1} winnings.", p.Name, p.Winnings);
                        else
                        Console.WriteLine("{0}, you lost with {1} and now have {2} winnings.", p.Name,  p.HandValue, p.Winnings);
                    }
            

            //(cards may need to be gathered). 
                    p.EmptyHand(deck);
    
            //Step 8: Players with zero winnings are eliminated. 
            if (0 == p.Winnings)
            {
                players.Remove(p);
                Console.WriteLine("Sorry, {0}, you have no more chips and have been eliminated from the game.", p.Name);
            }
}//end of hand termination

        //clear the dealer's hand
dealer.EmptyHand(deck);

        //Step 9: Play continues at Step 2 until all players are eliminated or the game ends upon request. 

return players.Count;
    }//end of play method
}//end of Blackjack class
        