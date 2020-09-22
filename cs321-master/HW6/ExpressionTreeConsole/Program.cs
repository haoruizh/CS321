using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS321;

namespace ExpressionTreeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            bool endProgram = false;
            string userInput;
            ExpressionTree expressionTree = new ExpressionTree("A1-B1-C1");
            while(endProgram!=true)
            {
                Console.WriteLine("Menu (Current Expression: {0})\n" +
                "1=Enter a new Expression\n" +
                "2=Set a variable value\n" +
                "3=Evaluate tree\n" +
                "4=Quit", expressionTree.Expression);
                userInput = Console.ReadLine();
                if(userInput=="4")
                {
                    endProgram = true;
                }
                else if(userInput == "3")
                {
                    Console.WriteLine(expressionTree.Evaluate());
                }
                else if(userInput=="2")
                {
                    Console.WriteLine("Enter variable name:");
                    string variable = Console.ReadLine();
                    Console.WriteLine("Enter new variable value");
                    expressionTree.SetVariable(variable, Convert.ToDouble(Console.ReadLine()));
                }
                else if (userInput == "1")
                {
                    Console.WriteLine("Enter new expression");
                    expressionTree.Expression = Console.ReadLine();
                }
            }
        }
    }
}
