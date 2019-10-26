using System;

namespace TicTacToe
{
	/// <summary>A TicTacToeBoard.</summary>
	public class TicTacToeBoard : Board
	{
		/// <summary>Clone a TicTacToe board from another instance of a board.</summary>
		/// <param name="b">Board to create a deep copy of.</param>
		public TicTacToeBoard(TicTacToeBoard b) : base(b)
		{}

		/// <summary>Initializes a new instance of a TictacToe board with a dimension of 3..</summary>
		public TicTacToeBoard() : base(3)
		{}

		/// <summary>The indices of the corner squares.</summary>
		public static ushort[] Corners
		{
			get { return new ushort[] {0, 2, 6, 8}; }
		}

		/// <summary>Determines if the specified piece has won.</summary>
		/// <param name="p"></param>
		/// <returns>Returns true if the piece has won, otherwise false.</returns>
		public bool Win(Piece.PieceValue p)
		{
			int[]	a = new int[9];
			int[]	b = new int[8];
			byte	i = 0;		// iterator
        
			// values to determine winner
			int[]	magic = {6,1,8,7,5,3,2,9,4};
        
			for ( i = 0; i < this.Length; ++i )
			{
				if ( this.At(i) && this.GetAt(i).Value == p )
					a[i] = magic[i];
			}
	
			// sum all the rows, columns, and diagnols
			b[0] = a[0] + a[1] + a[2];
			b[1] = a[3] + a[4] + a[5];
			b[2] = a[6] + a[7] + a[8];
			b[3] = a[0] + a[3] + a[6];
			b[4] = a[1] + a[4] + a[7];
			b[5] = a[2] + a[5] + a[8];
			b[6] = a[0] + a[4] + a[8];
			b[7] = a[2] + a[4] + a[6];

			// now see if input player won
			for ( i = 0; i < 8; ++i )
				if ( b[i] == 15 )
					return true;
        
			return false;
		}
	}
}
