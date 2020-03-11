using System;
using System.Collections.Generic;
using Cards;

namespace GinRummy
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            CardStack deck = CardStack.GetShuffledStandardDeck(
                    CardStackOrientation.FaceDown);
            PrintCardStack(deck);
            Console.WriteLine();

            CardStack discards = CardStack.GetEmptyStack(
                    CardStackOrientation.FaceUp);

            List<Card> drawnCards = deck.Draw(5);
            Console.WriteLine("Drawn face down:");
            PrintCardList(drawnCards);
            Console.WriteLine("Deck status:");
            PrintCardStack(deck);
            Console.WriteLine();

            //flip the stack over
            Console.WriteLine("Flipped deck.");
            deck.Orientation = CardStackOrientation.FaceUp;
            PrintCardStack(deck);

            //PrintCardStack(discards);
            //foreach (Card card in drawnCardsDown)
            //{
            //    discards.AddCardFaceDown(card);
            //}
            //PrintCardStack(discards);

            //foreach (Card card in drawnCardsUp)
            //{
            //    discards.AddCardFaceDown(card);
            //}
            //PrintCardStack(discards);
        }

        private static void PrintCardStack(CardStack deck)
        {
            for (int i = 0; i<deck.Count; i++)
            {
                Console.Write($"{deck.Contents[i]} ");
                if (i % 13 == 12)
                    Console.WriteLine();
            }
            if (deck.Count % 12 != 0)
                Console.WriteLine();
        }

        private static void PrintCardList(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                Console.Write($"{card} ");
            }
            Console.WriteLine();
        }
    }
}
