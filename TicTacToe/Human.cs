using System;

namespace TicTacToe
{
	/// <summary>
	/// Summary description for Human.
	/// </summary>
	public class Human : Player
	{
		/// <summary>Method human player will use to select their move.  Specific to game interface.</summary>
		private ChooseMove moveMethod;

		public Human(Piece.PieceValue t) : base(t)
		{}

		public ChooseMove MoveMethod
		{
			set
			{
				this.moveMethod = value;
			}
		}

		public override ushort Move(TicTacToeBoard currentBoard)
		{
			ushort chosenMove = 0;

			do
			{
				// call the method used to determine the move
				chosenMove = this.moveMethod(currentBoard, this.Token);
			} while ( currentBoard.At(chosenMove) );

			return chosenMove;
		}

	}
}
