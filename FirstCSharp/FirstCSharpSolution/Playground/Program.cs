using System;
using Playground;

class MainClass
{
    static void Main(string[] args)
    {
        SimpleEventAdder simpleAdder = new SimpleEventAdder();
        simpleAdder.OnMultipleOfFiveReached += simpleAdder_MulitpleOfFiveReached;

        int iAnswer = simpleAdder.Add(4, 2);
        Console.WriteLine($"iAnswer = {iAnswer}");
        iAnswer = simpleAdder.Add(4, 6);
        Console.WriteLine($"iAnswer = {iAnswer}");

        DotNetEventAdder eventAdder = new DotNetEventAdder();
        eventAdder.OnMultipleOfFiveReached += eventAdder_MultipleOfFiveReached;
        iAnswer = eventAdder.Add(4, 2);
        Console.WriteLine($"iAnswer = {iAnswer}");
        iAnswer = eventAdder.Add(4, 6);
        Console.WriteLine($"iAnswer = {iAnswer}");
    }

    static void simpleAdder_MulitpleOfFiveReached()
    {
        Console.WriteLine("This is a multiple of five:");
    }

    static void eventAdder_MultipleOfFiveReached(object sender, EventArgs args)
    {
        Console.WriteLine("This is a multiple of five:");
    }
}
