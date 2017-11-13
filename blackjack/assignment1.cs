using System;
using System.IO;

public class assignment1
{
    public static void Main()
    {
//StreamWriter sw = new StreamWriter(@"..\..\..\testing.txt", true); sw.AutoFlush = true;
        //Console.SetOut(sw); Console.WriteLine("\n"); Console.WriteLine();
//StreamReader sr = new StreamReader(@"..\..\..\input.txt");
//Console.SetIn(s
Blackjack game = new Blackjack();
Console.Title = "Blackjack";
game.initialise();

        //repetitively play rounds until no players left
while (0 != game.playHand())
    ;//Console.WriteLine("\n\n-------- New Round --------\n\n");//continue playing until no players left

Console.Write("No players left.\nGame Over");
Console.ReadLine();
    }//end of main
}//end of assignment1 class