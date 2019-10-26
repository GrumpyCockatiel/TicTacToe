using System;

namespace TicTacToe
{
	/// <summary>Encapsulates a game piece.</summary>
	public class Piece
	{
		/// <summary>Enumerates the possible face value of a piece (e.g. red/black, X/O, etc.).</summary>
		public enum PieceValue : byte
		{
			O = 0,
			X = 1,
		}

		/// <summary>The value of the piece object.</summary>
		private PieceValue pv;

		/// <summary>Constructor.</summary>
		/// <param name="v">Piece Value to initialize the game piece with.</param>
		public Piece(PieceValue v)
		{ pv = v; }

		/// <summary>Returns the value of the piece object.</summary>
		public PieceValue Value
		{
			get { return this.pv; }
		}
	}

	/// <summary>Base class for square playing board.</summary>
	public class Board
	{
		/// <summary>Dimensions of the board.</summary>
		protected byte		dim;
		/// <summary>The board iteself, an array of pieces.</summary>
		protected Piece[]	pieces;
		/// <summary>The minimum dimensions of any playing board.</summary>
		public static readonly byte MinBoardDim = 2;
		/// <summary>The maximum dimensions of any playing board.</summary>
		public static readonly byte MaxBoardDim = 15;

		/// <summary>Clone a new board from the input board.</summary>
		public Board(Board b)
		{
			// make a copy of the input board
			this.dim = b.Dimension;
			this.pieces = new Piece[this.Dimension * this.Dimension];
			b.pieces.CopyTo(this.pieces, 0);
		}

		/// <summary>Initializes a new instance of the board with the specified dimensions.</summary>
		public Board(byte d)
		{
			// MinBoardDim < board dimensions <= MaxBoardDim
			this.dim = ( d > Board.MinBoardDim ) ? d : (byte)3;
			this.dim = ( d > Board.MaxBoardDim ) ? (byte)15 : dim;
        
			// create new pieces array
			this.pieces = new Piece[dim * dim];
		}

		#region Accessors

		/// <summary>Total number of squares on the board.</summary>
		public int Length
		{
			get { return this.pieces.Length; }
		}
    
		/// <summary>Gets the dimensions (rank) of the board.</summary>
		public byte Dimension
		{
			get { return this.dim; }
		}

		/// <summary>Total number number of pieces that have been placed on the board.</summary>
		public ushort Moves
		{
			get
			{
				ushort	p = 0;
        
				for (ushort i = 0; i < this.pieces.Length; ++i)
					if ( this.At(i) )
						++p;

				return p;
			}
		}

		#endregion Accessors

		/// <summary>Determines if there is a piece placed on the specified square.</summary>
		public bool At(ushort i)
		{
			// if the index is null, no piece has been placed there yet
			if ( this.pieces[i] == null )
				return false;
			else
				return true;
		}

		/// <summary>Gets the piece at the specified location.</summary>
		/// <param name="i">Index of the square from which to get piece.</param>
		/// <returns>The requested piece.</returns>
		public Piece GetAt(ushort i)
		{
			if ( this.At(i) )
				return this.pieces[i];
			else
				return null;
		}

		/// <summary>Place the specified piece at the specified location.</summary>
		/// <param name="a">The piece to place on the board.</param>
		/// <param name="i">The index of the square to place the piece.</param>
		/// <returns>Returns true if the piece was placed successfully, otherwise false.</returns>
		public bool Place(Piece a, ushort i)
		{
			// position already filled or larger than board size
			if ( this.At(i) || !(this.pieces.Length > i) )
				return false;

			// put a new piece on the board
			this.pieces[i] = a;
        
			return true;
		}
    
		/// <summary>Removes the piece at the specified square.</summary>
		/// <param name="i">Index of sqaure to remove piece from.</param>
		/// <returns>Returns true if there was a piece at the specified index to remove, otherwise false.</returns>
		public bool Remove(ushort i)
		{
			if ( this.pieces[i] != null )
			{
				// does this clear memory effectively?
				Array.Clear(this.pieces, i, 1);
				return true;
			}

			return false;
		}
   
		/// <summary>Removes all pieces from the board.</summary>
		public void Clear()
		{
			Array.Clear(this.pieces, 0, this.pieces.Length);
		}

		/// <summary>Is the board full.</summary>
		public bool Full()
		{
			// iterate through the board
//			for ( ushort i = 0; i < this.pieces.Length; ++i )
//				if ( !this.At(i) )
//					return false;
//
//			return true;

			// if the number of moves >= the length of the board, it is full
			return !( this.Moves < this.Length );
		}

	}
}
