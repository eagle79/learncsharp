using System;
using System.Collections.Generic;

namespace Cards
{
    /// <summary>
    /// Representation of a card suit.
    /// </summary>
    public enum CardSuit
    {
        Hearts,
        Spades,
        Diamonds,
        Clubs
    }

    /// <summary>
    /// Representation of a card rank or value; progression of values runs from
    /// Ace through King, with King having a higher value than Queen, etc. Games
    /// where cards may have different definitions (e.g. blackjack) will need
    /// to allow for this separately.
    /// </summary>
    public enum CardRank
    {
        Ace = 1,
        Deuce = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }

    /// <summary>
    /// Representation of the orientation of a stack of cards.
    /// </summary>
    public enum CardStackOrientation
    {
        FaceUp,
        FaceDown
    }

    /// <summary>
    /// Extension methods for Card Deck types.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Generates the appropriate printable symbol for a card suit.
        /// </summary>
        /// <returns>The symbol for the suit.</returns>
        /// <param name="suit">The suit to generate the symbol for.</param>
        public static string Symbol(this CardSuit suit)
        {
            switch (suit)
            {
                case CardSuit.Spades:
                    return "\u2664";
                case CardSuit.Clubs:
                    return "\u2667";
                case CardSuit.Diamonds:
                    return "\u2666";
                case CardSuit.Hearts:
                    return "\u2665";
                default:
                    return "!"; //this is an error and should not be possible
            }
        }

        /// <summary>
        /// Generates the appropriate printable symbol for a card rank.
        /// </summary>
        /// <returns>The symbol for the rank.</returns>
        /// <param name="rank">The rank to generate the symbol for.</param>
        public static string Symbol(this CardRank rank)
        {
            switch (rank)
            {
                case CardRank.Ace:
                    return "A";
                case CardRank.Jack:
                    return "J";
                case CardRank.Queen:
                    return "Q";
                case CardRank.King:
                    return "K";
                case CardRank.Ten:
                    return "T";
                default:
                    return $"{(int)rank}";
            }
        }

        /// <summary>
        /// Extends generic List type to include a Shuffle function that
        /// randomly sorts the contents of the list.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        /// <typeparam name="T">The type of the list being shuffled.</typeparam>
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    /// <summary>
    /// Representation of a modern French-suited playing card. This 
    /// representation currently does not allow for Joker cards. Card is an
    /// immutable type.
    /// </summary>
    public class Card : IComparable<Card>
    {
        //properties

        /// <summary>
        /// Returns the card's suit.
        /// </summary>
        /// <value>The card's suit.</value>
        public CardSuit Suit { get; }

        /// <summary>
        /// Returns the card's rank.
        /// </summary>
        /// <value>The card's rank.</value>
        public CardRank Rank { get; }

        /// <summary>
        /// Returns the card's name as a long string in the format 
        /// "{Rank} of {Suit}" (e.g. "Jack of Spades").
        /// </summary>
        /// <value>The long name of the card.</value>
        public string LongName => $"{Rank} of {Suit}";

        /// <summary>
        /// Returns the card's name as a short string in the format 
        /// "{RankSymbol}{SuitSymbol} " (e.g. "J♤").
        /// </summary>
        /// <value>The short name.</value>
        public string ShortName => $"{Rank.Symbol()}{Suit.Symbol()} ";

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:GinRummy.Card"/> class with the provided values.
        /// </summary>
        /// <param name="suit">The card's suit.</param>
        /// <param name="rank">The card's rank.</param>
        public Card(CardSuit suit, CardRank rank)
        {
            if (!Enum.IsDefined(typeof(CardSuit), suit))
            {
                throw new ArgumentOutOfRangeException(
                        nameof(suit), suit, "Invalid card suit.");
            }

            if (!Enum.IsDefined(typeof(CardRank), rank))
            {
                throw new ArgumentOutOfRangeException(
                        nameof(rank), rank, "Invalid card rank.");
            }

            Suit = suit;
            Rank = rank;
        }

        /// <summary>
        /// Returns a string that represents the current Card.
        /// </summary>
        /// <returns>A string that represents the current Card.</returns>
        public override string ToString() => ShortName;

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to 
        /// the current <see cref="T:Cards.Card"/>.
        /// </summary>
        /// <param name="obj">
        ///     The <see cref="object"/> to compare with the current
        ///     <see cref="T:Cards.Card"/>.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="object"/> is equal 
        ///     to the current <see cref="T:Cards.Card"/>;
        ///     otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Card))
            {
                return false;
            }

            Card c = (Card)obj;
            return this.Suit == c.Suit && this.Rank == c.Rank;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="T:Cards.Card"/> object.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance that is suitable for use in 
        ///     hashing algorithms and data structures such as a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return Suit.GetHashCode() ^ Rank.GetHashCode();
        }

        /// <summary>
        /// Defines the default comparison for a Card.
        /// </summary>
        /// <returns>A value representing the relative order of the two Cards 
        /// being compared. (see System.Collections.Generic.IComparable 
        /// interface documentation for more information)</returns>
        /// <param name="otherCard">Other card.</param>
        public int CompareTo(Card otherCard)
        {
            return COMPARE_SUIT_RANK(this, otherCard);
        }

        /// <summary>
        /// Custom comparision to define sort order of suit, then rank, with
        /// rank value defined by the <see cref="CardRank"/> enumeration and 
        /// suit value defined by the <see cref="CardSuit"/> enumeration.
        /// </summary>
        public static Comparison<Card> COMPARE_SUIT_RANK =
            (e1, e2) =>
                e1.Suit.CompareTo(e2.Suit) != 0 ?
                e1.Suit.CompareTo(e2.Suit) : e1.Rank.CompareTo(e2.Rank);

        /// <summary>
        /// Custom comparision to define sort order of rank, then suit, with
        /// rank value defined by the <see cref="CardRank"/> enumeration and 
        /// suit value defined by the <see cref="CardSuit"/> enumeration.
        /// </summary>
        public static Comparison<Card> COMPARE_RANK_SUIT =
            (e1, e2) =>
                e1.Rank.CompareTo(e2.Rank) != 0 ?
                e1.Rank.CompareTo(e2.Rank) : e1.Suit.CompareTo(e2.Suit);
    }

    /// <summary>
    /// Represents a stack of Cards. A CardStack may be empty or contain any 
    /// number of cards, including duplicates. As such, it may represent a 
    /// standard deck, or any other group of cards. 
    /// </summary>
    public class CardStack
    {
        //private fields
        private List<Card> _cards = new List<Card>();

        //properties
        /// <summary>
        /// Returns the contents of the stack as an array. Cards are sorted
        /// according to the stack orientation.
        /// </summary>
        /// <value>The contents of the card stack.</value>
        public Card[] Contents
        {
            get
            {
                Card[] cards = _cards.ToArray();
                if (Orientation == CardStackOrientation.FaceUp)
                {
                    Array.Reverse(cards);
                }
                return cards;
            }
        }

        /// <summary>
        /// Returns the number of cards in the stack.
        /// </summary>
        /// <value>The number of cards in the stack.</value>
        public int Count => _cards.Count;

        /// <summary>
        /// Gets or sets the orientation of the card stack.
        /// </summary>
        /// <value>The orientation of the stack.</value>
        public CardStackOrientation Orientation { get; set; }

        /// <summary>
        /// Initializes an empty card stack with default (face down) 
        /// orientation.
        /// </summary>
        private CardStack(CardStackOrientation orientation) 
        {
            Orientation = orientation;
        }

        /// <summary>
        /// Shuffles the stack of cards.
        /// </summary>
        public void Shuffle()
        {
            //we'll shuffle up to 5 times
            Random r = new Random();
            int shuffleCount = r.Next() % 5 + 1;

            for (int i = 0; i < shuffleCount; i++)
            {
                _cards.Shuffle();
            }
        }

        /// <summary>
        /// Flips the orientation of the stack.
        /// </summary>
        public void Flip()
        {
            Orientation = (Orientation == CardStackOrientation.FaceDown) ?
                    CardStackOrientation.FaceUp : CardStackOrientation.FaceDown;
        }

        /// <summary>
        /// Draws a specified number of cards from the "top" of the stack, where
        /// "top" depends on Orientation. If face down, the "top" is the cards
        /// with the face not visible. If face up, the "top" is the cards with
        /// the face visible. If the stack contains less than n cards, this 
        /// method returns the remainder of cards in the stack. If the stack is
        /// empty, this method returns an empty list.
        /// </summary>
        /// <returns>The drawn cards.</returns>
        /// <param name="n">The number of cards to draw.</param>
        public List<Card> Draw(int n)
        {
            if (Orientation == CardStackOrientation.FaceDown)
            {
                return DrawFromFront(n);
            }
            else
            {
                return DrawFromEnd(n);
            }
        }

        /// <summary>
        /// Draws a specified number of cards from the front of the stack and 
        /// returns it as a list. This method pulls from the front of the 
        /// underlying list without respect to stack orientation.
        /// </summary>
        /// <returns>The drawn cards.</returns>
        /// <param name="n">The number of cards to draw.</param>
        private List<Card> DrawFromFront(int n)
        {
            List<Card> returnCards = new List<Card>();
            while ((returnCards.Count < n) && (_cards.Count != 0))
            {
                returnCards.Add(_cards[0]);
                _cards.RemoveAt(0);
            }

            return returnCards;
        }

        /// <summary>
        /// Draws a specified number of cards from the end of the stack and 
        /// returns it as a list. This method pulls from the end of the 
        /// underlying list without respect to stack orientation.
        /// </summary>
        /// <returns>The drawn cards.</returns>
        /// <param name="n">The number of cards to draw.</param>
        private List<Card> DrawFromEnd(int n)
        {
            List<Card> returnCards = new List<Card>();
            while ((returnCards.Count < n) && (_cards.Count != 0)) 
            {
                returnCards.Add(_cards[_cards.Count - 1]);
                _cards.RemoveAt(_cards.Count - 1);
            }

            return returnCards;
        }

        /// <summary>
        /// Adds the specified card to the "top" of the stack, where
        /// "top" depends on Orientation. If face down, the "top" is the cards
        /// with the face not visible. If face up, the "top" is the cards with
        /// the face visible.
        /// </summary>
        /// <param name="card">The card to add.</param>
        public void AddCard(Card card)
        {
            if (Orientation == CardStackOrientation.FaceDown)
            {
                AddCardToFront(card);
            } 
            else
            {
                AddCardToEnd(card);
            }
        }

        /// <summary>
        /// Adds the specified card to the beginning of the stack. This method 
        /// adds to the front of the underlying list without respect to stack 
        /// orientation. 
        /// </summary>
        /// <param name="card">The card to add.</param>
        private void AddCardToFront(Card card)
        {
            _cards.Insert(0, card);
        }

        /// <summary>
        /// Adds the specified card to the end of the stack. This method 
        /// adds to the front of the underlying list without respect to stack 
        /// orientation. 
        /// </summary>
        /// <param name="card">The card to add.</param>
        private void AddCardToEnd(Card card)
        {
            _cards.Add(card);
        }

        /// <summary>
        /// Sorts the card stack using the specified comparison. Built-in 
        /// comparisons are available on the interface of <see cref="Card"/>.
        /// The re-sort respects the orientation of the stack.
        /// </summary>
        /// <param name="comparison">The Comparison to use for the sort</param>
        public void Sort(Comparison<Card> comparison)
        {
            _cards.Sort(comparison);
            if (Orientation == CardStackOrientation.FaceUp)
            {
                _cards.Reverse();
            }
        }

        /// <summary>
        /// Returns a new empty card stack with the specified orientation.
        /// </summary>
        /// <returns>The empty stack.</returns>
        /// <param name="orientation">The orientation for the new stack</param>
        public static CardStack GetEmptyStack(CardStackOrientation orientation)
        {
            return new CardStack(orientation);
        }

        /// <summary>
        /// Gets a standard 52-card stack, sorted by suit, then by rank.
        /// </summary>
        /// <returns>The sorted standard deck.</returns>
        /// <param name="orientation">The orientation for the new stack</param>
        public static CardStack GetSortedStandardDeck(
                CardStackOrientation orientation)
        {
            CardStack stack = new CardStack(orientation);
            foreach (CardSuit s in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardRank v in Enum.GetValues(typeof(CardRank)))
                {
                    stack._cards.Add(new Card(s, v));
                }
            }

            return stack;
        }

        /// <summary>
        /// Gets a standard 52-card stack that has been shuffled.
        /// </summary>
        /// <returns>The shuffled standard deck.</returns>
        /// <param name="orientation">The orientation for the new stack</param>
        public static CardStack GetShuffledStandardDeck(
                CardStackOrientation orientation)
        {
            CardStack stack = CardStack.GetSortedStandardDeck(orientation);
            stack.Shuffle();
            return stack;
        }

        /// <summary>
        /// Creates a new card stack in the specified orientation from the 
        /// supplied list. The first card in the list will be the "top" card in
        /// the new CardStack.
        /// </summary>
        /// <returns>A new card stack from the provided list.</returns>
        /// <param name="cards">The cards to create the stack from</param>
        /// <param name="orientation">
        ///     The orientation of the new card stack
        /// </param>
        public static CardStack FromList(
                List<Card> cards, CardStackOrientation orientation)
        {
            CardStack stack = CardStack.GetEmptyStack(orientation);

            //we'll temporarily flip the stack so that we can add the cards
            // in the correct order
            stack.Flip();

            //add the cards
            foreach (Card card in cards)
            {
                stack.AddCard(card);
            }

            //flip it back and return it
            stack.Flip();

            return stack;
        }
    }
}
