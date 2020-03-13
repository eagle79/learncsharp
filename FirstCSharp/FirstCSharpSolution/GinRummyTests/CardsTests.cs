using System;
using System.Collections.Generic;
using Cards;
using NUnit.Framework;

namespace CardsTests
{
    [TestFixture()]
    public class TestClass_Card
    { 
        [Test()]
        public void TestInstantiation()
        {
            //Card suit out of range
            Assert.Throws<ArgumentOutOfRangeException>(
                    () => new Card((CardSuit) (-1), CardRank.Ace));
            Assert.Throws<ArgumentOutOfRangeException>(
                    () => new Card((CardSuit) 4, CardRank.Ace));

            //Card rank out of range
            Assert.Throws<ArgumentOutOfRangeException>(
                    () => new Card(CardSuit.Clubs, (CardRank) 0));
            Assert.Throws<ArgumentOutOfRangeException>(
                    () => new Card(CardSuit.Clubs, (CardRank) 14));
        }

        [Test()]
        public void TestEquality()
        {
            Card card1 = new Card(CardSuit.Clubs, CardRank.Ace);
            Card card2 = new Card(CardSuit.Clubs, CardRank.Ace);

            Assert.True(card1.Equals(card2));
            Assert.True(card1.GetHashCode() == card2.GetHashCode());

            card2 = new Card(CardSuit.Diamonds, CardRank.Deuce);

            Assert.False(card1.Equals(card2));
        }

        [Test()]
        public void TestComparisons()
        {
            Card aceOfClubs = new Card(CardSuit.Clubs, CardRank.Ace);
            Card deuceOfClubs = new Card(CardSuit.Clubs, CardRank.Deuce);
            Card aceOfHearts = new Card(CardSuit.Hearts, CardRank.Ace);
            Card deuceOfHearts = new Card(CardSuit.Hearts, CardRank.Deuce);

            //COMPARE_SUIT_RANK
            Assert.AreEqual(0, Card.COMPARE_SUIT_RANK(aceOfClubs, aceOfClubs));
            //same suit, different rank
            Assert.Less(Card.COMPARE_SUIT_RANK(aceOfClubs, deuceOfClubs), 0);
            Assert.Greater(Card.COMPARE_SUIT_RANK(deuceOfClubs, aceOfClubs), 0);

            //same rank, different suit
            Assert.Less(Card.COMPARE_SUIT_RANK(aceOfHearts, aceOfClubs), 0);
            Assert.Greater(Card.COMPARE_SUIT_RANK(aceOfClubs, aceOfHearts), 0);

            //different suit and rank
            Assert.Greater(
                    Card.COMPARE_SUIT_RANK(aceOfClubs, deuceOfHearts), 0);
            Assert.Less(Card.COMPARE_SUIT_RANK(deuceOfHearts, aceOfClubs), 0);

            //same card
            Assert.Zero(Card.COMPARE_SUIT_RANK(aceOfClubs, aceOfClubs));

            //COMPARE_RANK_SUIT
            //same suit, different rank
            Assert.Less(Card.COMPARE_RANK_SUIT(aceOfClubs, deuceOfClubs), 0);
            Assert.Greater(Card.COMPARE_RANK_SUIT(deuceOfClubs, aceOfClubs), 0);

            //same rank, different suit
            Assert.Less(Card.COMPARE_RANK_SUIT(aceOfHearts, aceOfClubs), 0);
            Assert.Greater(Card.COMPARE_RANK_SUIT(aceOfClubs, aceOfHearts), 0);

            //different suit and rank
            Assert.Less(Card.COMPARE_RANK_SUIT(aceOfClubs, deuceOfHearts), 0);
            Assert.Greater(
                    Card.COMPARE_RANK_SUIT(deuceOfHearts, aceOfClubs), 0);

            //same card
            Assert.Zero(Card.COMPARE_RANK_SUIT(aceOfClubs, aceOfClubs));
        }
    }

    [TestFixture()]
    public class TestClass_CardStack
    {
        [Test()] 
        public void TestInstantiation()
        {
            CardStack stack;

            stack = CardStack.GetEmptyStack(CardStackOrientation.FaceDown);
            Assert.Zero(stack.Count);
            Assert.Zero(stack.Contents.Length);
            Assert.AreEqual(CardStackOrientation.FaceDown, stack.Orientation);

            stack = CardStack.GetSortedStandardDeck(
                    CardStackOrientation.FaceDown);
            Assert.AreEqual(52, stack.Count);
            Assert.AreEqual(52, stack.Contents.Length);
            Assert.AreEqual(CardStackOrientation.FaceDown, stack.Orientation);
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                for(int j=0; j<13; j++)
                {
                    Card card = stack.Draw(1)[0];
                    Assert.AreEqual(suit, card.Suit);
                }
            }
            Assert.IsEmpty(stack.Contents);

            stack = CardStack.GetShuffledStandardDeck(
                    CardStackOrientation.FaceDown);
            Assert.AreEqual(52, stack.Count);
            Assert.AreEqual(52, stack.Contents.Length);
            Assert.AreEqual(CardStackOrientation.FaceDown, stack.Orientation);
            foreach(CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                foreach(CardRank rank in Enum.GetValues(typeof(CardRank)))
                {
                    Assert.Contains(new Card(suit, rank), stack.Contents);
                }
            }

            List<Card> cards = new List<Card>()
            {
                new Card(CardSuit.Hearts, CardRank.Ace),
                new Card(CardSuit.Diamonds, CardRank.Ten),
                new Card(CardSuit.Spades, CardRank.Deuce)
            };
            stack = CardStack.FromList(cards, CardStackOrientation.FaceDown);
            int index = 0;
            foreach(Card card in stack.Contents)
            {
                Assert.AreEqual(card, cards[index++]);
            }

            cards.Clear();
            stack = CardStack.FromList(cards, CardStackOrientation.FaceDown);
            Assert.IsEmpty(stack.Contents);
        }

        [Test()]
        public void TestShuffle()
        {
            CardStack stack;
            int cardCount;
            Card[] unshuffled;

            //empty stack
            stack = CardStack.GetEmptyStack(CardStackOrientation.FaceDown);
            cardCount = stack.Count;
            unshuffled = stack.Contents;
            stack.Shuffle();
            Assert.AreEqual(cardCount, stack.Count);
            Assert.IsEmpty(stack.Contents);

            //only one card
            stack = CardStack.GetEmptyStack(CardStackOrientation.FaceDown);
            stack.AddCard(new Card(CardSuit.Diamonds, CardRank.King));
            cardCount = stack.Count;
            unshuffled = stack.Contents;
            stack.Shuffle();
            Assert.AreEqual(cardCount, stack.Count);
            Assert.AreEqual(1, stack.Count);
            //since shuffling one card will not make any change, we'll assert
            // that the first element is the card we added.
            Assert.True(stack.Contents[0].Equals(unshuffled[0]));

            //standard (sorted) deck
            stack = CardStack.GetSortedStandardDeck(
                    CardStackOrientation.FaceDown);
            cardCount = stack.Count;
            unshuffled = stack.Contents;
            stack.Shuffle();
            Assert.AreEqual(cardCount, stack.Count);
            Assert.AreEqual(52, stack.Count);
            //We can be reasonably certain that we won't end up with the same
            // order we started with (at least with this many cards). This
            // does NOT prove we have a good shuffle, but it does prove we 
            // did shuffle in some way.
            bool foundChange = false;
            for (int i=0; i<unshuffled.Length; i++)
            {
                if (!unshuffled[i].Equals(stack.Draw(1)[0]))
                {
                    foundChange = true;
                    break;
                }
            }
            Assert.True(foundChange);
        }

        [Test()]
        public void TestFlip()
        {
            CardStack stack = CardStack.GetShuffledStandardDeck(
                    CardStackOrientation.FaceDown);
            Card[] before = stack.Contents;
            stack.Flip();
            Assert.AreEqual(CardStackOrientation.FaceUp, stack.Orientation);
            Card[] after = stack.Contents;

            Array.Reverse(before);
            int i = 0;
            foreach (Card card in after)
            {
                Assert.True(card.Equals(before[i++]));
            }

            stack.Flip();
            Assert.AreEqual(CardStackOrientation.FaceDown, stack.Orientation);
        }

        [Test()]
        public void TestDraw()
        {
            CardStack stack = CardStack.GetSortedStandardDeck(
                    CardStackOrientation.FaceDown);
            List<Card> drawn;
            Card card;
            CardRank rankIndex;

            //draw a card (face down) and verify 
            Assert.AreEqual(52, stack.Count); //starting with the expected count
            drawn = stack.Draw(1);
            Assert.AreEqual(51, stack.Count); //drew exactly one card
            Assert.AreEqual(1, drawn.Count); //drawn cards only contains one
            card = drawn[0];
            //drew the expected card (Ace of Hearts)
            Assert.True(card.Equals(new Card(CardSuit.Hearts, CardRank.Ace)));
            //drawn card is no longer in stack
            Assert.False(Array.Exists<Card>(stack.Contents, (Card c) 
                    => (c.Suit == CardSuit.Hearts) && 
                        (c.Rank == CardRank.Ace)));

            //draw a card (face up) and verify 
            stack.Flip();
            Assert.AreEqual(51, stack.Count); //starting with the expected count
            drawn = stack.Draw(1);
            Assert.AreEqual(50, stack.Count); //drew exactly one card
            Assert.AreEqual(1, drawn.Count); //drawn cards only contains one
            card = drawn[0];
            //drew the expected card (King of Clubs)
            Assert.True(card.Equals(new Card(CardSuit.Clubs, CardRank.King)));
            //drawn card is no longer in the stack
            Assert.False(Array.Exists<Card>(stack.Contents, (Card c)
                    => (c.Suit == CardSuit.Clubs) &&
                        (c.Rank == CardRank.King)));

            //draw 5 cards (face down) and verify
            stack.Flip();
            Assert.AreEqual(50, stack.Count); //starting with the expected count
            drawn = stack.Draw(5); 
            Assert.AreEqual(45, stack.Count); //drew exactly five cards
            Assert.AreEqual(5, drawn.Count); //drawn cards contains exactly five
            //drew the expected cards in order (Deuce, 3, 4, 5, and 6 of Hearts)
            rankIndex = CardRank.Ace;
            foreach (Card c in drawn)
            {
                rankIndex++;
                //is this the card we expected to be here?
                Assert.AreEqual(rankIndex, c.Rank);
                //were there cards we didn't expect?
                Assert.LessOrEqual(rankIndex, CardRank.Six);
            }
            //drawn cards are no longer in the stack
            foreach(Card c in drawn)
            {
                Assert.False(Array.Exists<Card>(
                        stack.Contents, (Card sc) => sc.Equals(c)));
            }

            //draw 5 cards (face up) and verify
            stack.Flip();
            Assert.AreEqual(45, stack.Count); //starting with the expected count
            drawn = stack.Draw(5);
            Assert.AreEqual(40, stack.Count); //drew exactly five cards
            Assert.AreEqual(5, drawn.Count); //drawn cards contains exactly five
            //drew the expected cards in order (Queen, Jack, 10, 9, and 8 of 
            // Clubs)
            rankIndex = CardRank.King;
            foreach (Card c in drawn)
            {
                rankIndex--;
                //is this the card we expected to be here?
                Assert.AreEqual(rankIndex, c.Rank);
                //were there cards we didn't expect?
                Assert.GreaterOrEqual(rankIndex, CardRank.Eight);
            }
            //drawn cards are no longer in the stack
            foreach (Card c in drawn)
            {
                Assert.False(Array.Exists<Card>(
                        stack.Contents, (Card sc) => sc.Equals(c)));
            }

            //try to draw more than the remaining cards (face down)
            stack.Flip();
            Assert.AreEqual(40, stack.Count); //starting with the expected count
            //draw shouldn't throw any exceptions or errors
            Assert.DoesNotThrow(() => drawn = stack.Draw(52));
            Assert.Zero(stack.Count); //stack should be empty now
            Assert.AreEqual(40, drawn.Count); //drawn cards should be 40

            //try to draw a card from the empty stack (face down)
            Assert.DoesNotThrow(() => drawn = stack.Draw(1));
            Assert.Zero(stack.Count); //stack should still be empty
            Assert.Zero(drawn.Count); //drawn should be empty
        }

        [Test()]
        public void TestAdd()
        {
            CardStack stack = 
                    CardStack.GetEmptyStack(CardStackOrientation.FaceDown);
            Card card;

            card = new Card(CardSuit.Hearts, CardRank.Ace);
            stack.AddCard(card);
            Assert.AreEqual(stack.Count, 1);
            Assert.AreEqual(stack.Contents[0], 
                    new Card(CardSuit.Hearts, CardRank.Ace));

            stack.Flip();
            card = new Card(CardSuit.Diamonds, CardRank.King);
            stack.AddCard(card);
            Assert.AreEqual(2, stack.Count);
            Assert.AreEqual(new Card(CardSuit.Diamonds, CardRank.King), 
                    stack.Contents[0]);
            Assert.AreEqual(new Card(CardSuit.Hearts, CardRank.Ace), 
                    stack.Contents[1]);

            //Add a duplicated card
            stack.AddCard(new Card(CardSuit.Diamonds, CardRank.King));
            Assert.AreEqual(3, stack.Count);
            Assert.AreEqual(new Card(CardSuit.Diamonds, CardRank.King), 
                    stack.Contents[0]);
            Assert.AreEqual(new Card(CardSuit.Diamonds, CardRank.King), 
                    stack.Contents[1]);
            Assert.AreEqual(new Card(CardSuit.Hearts, CardRank.Ace), 
                    stack.Contents[2]);
            Assert.AreEqual(stack.Contents[0], stack.Contents[1]);

            stack.Flip();
            Assert.AreEqual(stack.Contents[1], stack.Contents[2]);
        }

        [Test()]
        public void TestSort()
        {
            Card kingOfDiamonds = new Card(CardSuit.Diamonds, CardRank.King);
            Card deuceOfDiamonds = new Card(CardSuit.Diamonds, CardRank.Deuce);
            Card fiveOfHearts = new Card(CardSuit.Hearts, CardRank.Five);
            Card sixOfSpades = new Card(CardSuit.Spades, CardRank.Six);

            CardStack stack =
                    CardStack.GetEmptyStack(CardStackOrientation.FaceDown);
            stack.AddCard(kingOfDiamonds);
            stack.AddCard(deuceOfDiamonds);
            stack.AddCard(fiveOfHearts);
            stack.AddCard(sixOfSpades);

            //facedown sort by suit/rank
            stack.Sort(Card.COMPARE_SUIT_RANK);
            Assert.AreEqual(fiveOfHearts, stack.Contents[0]);
            Assert.AreEqual(sixOfSpades, stack.Contents[1]);
            Assert.AreEqual(deuceOfDiamonds, stack.Contents[2]);
            Assert.AreEqual(kingOfDiamonds, stack.Contents[3]);

            //face up sort by suit/rank
            stack.Flip();
            stack.Sort(Card.COMPARE_SUIT_RANK);
            Assert.AreEqual(fiveOfHearts, stack.Contents[0]);
            Assert.AreEqual(sixOfSpades, stack.Contents[1]);
            Assert.AreEqual(deuceOfDiamonds, stack.Contents[2]);
            Assert.AreEqual(kingOfDiamonds, stack.Contents[3]);

            //face down sort by rank/suit
            stack.Flip();
            stack.Sort(Card.COMPARE_RANK_SUIT);
            Assert.AreEqual(deuceOfDiamonds, stack.Contents[0]);
            Assert.AreEqual(fiveOfHearts, stack.Contents[1]);
            Assert.AreEqual(sixOfSpades, stack.Contents[2]);
            Assert.AreEqual(kingOfDiamonds, stack.Contents[3]);

            //face up sort by rank/suit
            stack.Flip();
            stack.Sort(Card.COMPARE_RANK_SUIT);
            Assert.AreEqual(deuceOfDiamonds, stack.Contents[0]);
            Assert.AreEqual(fiveOfHearts, stack.Contents[1]);
            Assert.AreEqual(sixOfSpades, stack.Contents[2]);
            Assert.AreEqual(kingOfDiamonds, stack.Contents[3]);
        }
    }
}
