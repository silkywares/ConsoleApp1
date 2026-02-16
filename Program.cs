using System.Collections;
using System.Drawing;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace PokerGame;

class Program
{
    static void Main()
    {
        Player p1 = new Player("Luis",1);
        Player p2 = new Player("Arny",2);
        Player p3 = new Player("GMan",3);
        Player p4 = new Player("Monk",4);
        Player p5 = new Player("Niga",5);

        // (initialize names/chips if you want)
        List<Player> players = new List<Player> { p1,p2,p3,p4,p5 };
        
        Table table = new Table(players);
        //table.Dealer.TestBoard();
        //table.Dealer.TestHand();
        bool flag = true;
        while (flag)
        {
            foreach (Player p in players)
                p.Hand.Clear();
            table.Dealer.ClearBoard();

            table.Dealer.DealBoardCards();
            table.Dealer.DealBoardCards();
            table.Dealer.DealBoardCards();
            table.Dealer.DealPlayerCards();
            table.PrintTable();
            
            Evaluator.EvaluateBoard(players,table.Dealer.Board);  

            
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.Backspace)
                flag = false; 
        }

    }   
} 

