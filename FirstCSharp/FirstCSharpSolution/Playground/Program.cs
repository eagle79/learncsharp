using System;
using Playground;

class MainClass
{
    static void Main(string[] args)
    {
        Collections.doStuff();
        //EventsAndStuff();
    }

    private static void EventsAndStuff()
    {
        WrapHandler wrapHandler1 = new WrapHandler("wrapHandler1");
        WrapHandler wrapHandler2 = new WrapHandler("wrapHandler2");

        SimpleEventAdder simpleAdder = new SimpleEventAdder();
        simpleAdder.OnMultipleOfFiveReached += wrapHandler1.simpleAdder_MultipleOfFiveReached;
        simpleAdder.OnMultipleOfFiveReached += wrapHandler2.simpleAdder_MultipleOfFiveReached;

        int iAnswer = simpleAdder.Add(4, 2);
        Console.WriteLine($"iAnswer = {iAnswer}");
        iAnswer = simpleAdder.Add(4, 6);
        Console.WriteLine($"iAnswer = {iAnswer}");

        wrapHandler2 = null;

        DotNetEventAdder eventAdder = new DotNetEventAdder();
        eventAdder.OnMultipleOfFiveReached += eventAdder_MultipleOfFiveReached;
        iAnswer = eventAdder.Add(4, 2);
        Console.WriteLine($"iAnswer = {iAnswer}");
        iAnswer = eventAdder.Add(4, 6);
        Console.WriteLine($"iAnswer = {iAnswer}");

        try
        {
            iAnswer = eventAdder.Add(short.MaxValue, 1);
            Console.WriteLine($"iAnswer = {iAnswer}");
        }
        catch (MyException e)
        {
            Console.WriteLine($"\"Handled\" MyException: {e.Message}\n{e.StackTrace}");
        }
    }

    static void eventAdder_MultipleOfFiveReached(object sender, EventArgs args)
    {
        Console.WriteLine("This is a multiple of five:");
    }

    public class WrapHandler
    {
        public string Id { get; set; }

        public WrapHandler(string Id)
        {
            this.Id = Id;
        }

        public void simpleAdder_MultipleOfFiveReached()
        {
            Console.WriteLine($"{Id} This is a multiple of five:");
        }
    }
}
