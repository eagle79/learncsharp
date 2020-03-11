using System;
using System.Collections.Generic;

namespace Playground
{
    public static class Collections
    {
        public static void doStuff()
        {
            List<int> myCollection = new List<int>();

            ResetList(myCollection);
            PrintList(myCollection, "Initial");

            //So if we wanted to remove all integers that are divisible by 3
            // we cannot do it this way, because we're modifying the collection
            // we're iterating over (same as in Java)
            //resetList(myCollection);
            //foreach (int? n in myCollection)
            //{
            //    if (n % 3 == 0)
            //        myCollection.Remove(n);
            //}

            //But we cannot use the iterator itself to remove the element (as
            // we would in Java) because iterators in C# are read-only (they
            // do not modify the underlying collection)



            //One solution is to "iterate backwards" using a for-loop and use 
            // removeAt:
            ResetList(myCollection);
            for (int i=myCollection.Count-1; i>=0; i--)
            {
                if (myCollection[i] % 3 == 0)
                    myCollection.RemoveAt(i);
            }
            PrintList(myCollection, "Reversed for-loop");
            // this is clunky, and only works because all elements being removed
            // are either at the end of the list or followed in the list ONLY
            // by elements we're not going to remove. 



            //A somewhat more elegant solution is to perform such conditioned 
            // removals using the provided removeAll() method. This is
            // especially elegant when written with a lambda:
            ResetList(myCollection);
            myCollection.RemoveAll(n => (n % 3 == 0));
            PrintList(myCollection, "RemoveAll");
            // The argument for RemoveAll is a Predicate delegate, which has
            // the signature:
            //    public delegate bool Predicate<in T>(T obj);
            // You can write a method and pass it if preferred. In this case,
            //    private static bool IsDiv3(int? n) { return (n % 3 == 0); }



            //Of course, iteration also cannot survive adding elements;
            //ResetList(myCollection);
            //foreach (int n in myCollection)
            //{
            //    if (n % 3 == 0) {
            //        myCollection.Add(n + 100);
            //        myCollection.Remove(n);
            //    }
            //}
            //PrintList(myCollection, "Add while iterating");



            //But we can extend our RemoveAll solution a bit and make this 
            // work in an elegant fashion as well:
            ResetList(myCollection);
            List<int> toRemove = myCollection.FindAll(n => (n % 3 == 0));
            myCollection.RemoveAll(n => toRemove.Contains(n));
            foreach (int n in toRemove)
            {
                myCollection.Add(n + 100);
            }
            PrintList(myCollection, "Remove/add");
            // First we, find all the elements we want to remove and store them
            // in a new list. Then we remove all elements that we found. 
            // Finally, we add new elements based on the ones we found. This is
            // a bit of a convoluted example, but it illustrates how to work
            // in a clean, elegant way. One concern here is efficency. Each 
            // XxxAll(Predicate) call presumably iterates the entire list, which
            // can be ugly if the number of elements is very large.
        }

        public static void ResetList(List<int> list)
        {
            list.Clear();
            for (int i = 0; i < 25; i++)
            {
                list.Add(i);
            }
        }

        public static void PrintList(List<int> list, string prefix)
        {
            Console.Write($"{prefix}: ");
            foreach (int? n in list)
            {
                Console.Write($"{n} ");
            }
            Console.WriteLine();
        }
    }
}
