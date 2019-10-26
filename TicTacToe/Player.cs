using System;

namespace TicTacToe
{
	/// <summary>Game player.</summary>
	abstract public class Player
	{
		/// <summary>This player's token.</summary>
		private Piece.PieceValue	token;
		/// <summary>This player's current score.</summary>
		private int					score = 0;

		/// <summary>Initializes a new instance of a player with the specified token value.</summary>
		/// <param name="t"></param>
		public Player(Piece.PieceValue t)
		{
			this.token = t;
		}

		/// <summary>Delegate function that defines how a player chooses their move.</summary>
		public delegate ushort ChooseMove(TicTacToeBoard board, Piece.PieceValue token);

		/// <summary>Gets or set player's token.</summary>
		public Piece.PieceValue Token
		{
			get { return this.token; }
			set { this.token = value; }
		}

		/// <summary>Get player's current score.</summary>
		public int Score
		{
			get { return this.score; }
		}

		/// <summary>Adds the specified number of points to the player's current score.</summary>
		/// <returns>The player's new score.</returns>
		public int AddPoints(int pts)
		{
			this.score += pts;
			return this.Score;
		}

		/// <summary>Method called by the game when it is a player's turn to move.</summary>
		/// <param name="currentBoard">The current game before the player moves.</param>
		/// <returns>Index of the board position the player chooses to move to.</returns>
		public abstract ushort Move(TicTacToeBoard currentBoard);
	}

}
