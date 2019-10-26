using System;
using System.Security.Cryptography;

namespace TicTacToe
{
	public class Computer : Player
	{
		public enum SkillLevel : byte
		{
			Easy = 0,
			Difficult = 1,
			Impossible = 9
		}

		protected enum MoveStrategy : sbyte
		{ OWin = -2, OBlock = -1, Draw = 0, XBlock = 1, XWin = 2}

		/// <summary>Method the computer will use to determine its move.  Set at runtime.</summary>
		private ChooseMove moveMethod;

        /// <summary>Random number generator for the computer player.</summary>
        private static readonly Random randNum = null;

        static Computer()
        {
            // generate a random byte array
            byte[] seed = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(seed);

            // convert to an integer value
            randNum = new Random(BitConverter.ToInt32(seed, 0));
        }

        /// <summary>Instantiates a new computer player.</summary>
        /// <param name="t">The piece the computer will use.</param>
        public Computer(Piece.PieceValue t) : base(t)
		{
			// set how the computer chooses a move based on skill level
			moveMethod = new Player.ChooseMove(Computer.MinMaxMove);
		}

		public override ushort Move(TicTacToeBoard currentBoard)
		{
			// computer's algorithm will modify the board so create a copy to work on
			// TicTacToeBoard tempBoard = new TicTacToeBoard(currentBoard);

			// call the method used to determine the move
			ushort chosenMove = this.moveMethod(currentBoard, this.Token);

			while ( currentBoard.At(chosenMove) )
				chosenMove = this.moveMethod(currentBoard, this.Token);

			return chosenMove;
		}

		/// <summary>Computer picks a board move randomly.</summary>
		/// <returns>Index of board location to move to.</returns>
		public static ushort RandomMove(TicTacToeBoard board, Piece.PieceValue token)
		{
			return Convert.ToUInt16( Computer.randNum.Next(0, 8) );
		}

		public static ushort MinMaxMove(TicTacToeBoard board, Piece.PieceValue token)
		{
			// computer always plays the center square if it is available
			if ( !board.At(4) )
				return 4;

			// computer's 2nd move should be a corner if computer went first
			// computer's 1st move should be a corner if the center is taken
			// really not the best strategy
			if ( (board.Moves == 1 && board.At(4)) || board.Moves == 2 )
				return (TicTacToeBoard.Corners)[Computer.randNum.Next(0, TicTacToeBoard.Corners.Length-1)];

			// start with recommended move
			ushort	aMove = 4;
			
			// use MinMax strategy to find the next move
			MoveStrategy strategy = Computer.LookAhead(new Piece(token), new TicTacToeBoard(board), ref aMove);
	
			return aMove;
		}

		protected static MoveStrategy LookAhead(Piece inPiece, TicTacToeBoard inBoard, ref ushort bestMove)
		{
			// unused dummy value to seend the algorithm
			ushort			aDummy = 0;
			// temporary strategy
			MoveStrategy	aResponse;
			// the strategy to return
			MoveStrategy	outValue = ( inPiece.Value == Piece.PieceValue.X ) ? MoveStrategy.OWin - 1 : MoveStrategy.XWin + 1;			

			// first check to see if the game is a draw
			if ( inBoard.Full() )
				outValue = MoveStrategy.Draw;
			// return any move that results in an immediate win
			else if ( Computer.ImmediateWin(inPiece, inBoard, ref bestMove) )
				outValue = ( inPiece.Value == Piece.PieceValue.X ) ? MoveStrategy.XWin : MoveStrategy.OWin;
			// return any move that will result in blocking the other player from an immediate win on his next turn
			else if ( Computer.ImmediateBlock(inPiece, inBoard, ref bestMove) )
				outValue = ( inPiece.Value == Piece.PieceValue.X ) ? MoveStrategy.XBlock : MoveStrategy.OBlock;
			// no immediate wins so look for a promising move
			else
			{		
				for ( ushort i = 0; i < inBoard.Length; ++i )
				{
					if ( !inBoard.At(i) )
					{
						inBoard.Place(inPiece, i);
							
						// swap pieces and go down a move
						Piece aPiece = ( inPiece.Value == Piece.PieceValue.X )  ? new Piece(Piece.PieceValue.O) : new Piece(Piece.PieceValue.X);
						
						// call look ahead recursively with a copy of the board each time
						aResponse = LookAhead(aPiece, new TicTacToeBoard(inBoard), ref aDummy);
						
						// determine the best move
						switch ( inPiece.Value )
						{
							case Piece.PieceValue.X:
								if ( aResponse > outValue )
								{
									outValue = aResponse;
									bestMove = i;
								}
								break;
									
							case Piece.PieceValue.O:
								if ( aResponse < outValue )
								{
									outValue = aResponse;
									bestMove = i;
								}
								break;

							default:
								break;
						} // end switch
			
					} // end if
						
				} // end for loop
				
			} // end else

			// return the best move strategy
			return outValue;
		}

		protected static bool ImmediateWin(Piece inPiece, TicTacToeBoard inBoard, ref ushort bestMove)
		{	
			for ( ushort i = 0; i < inBoard.Length; ++i )
			{
				// create a clone of the current board
				TicTacToeBoard tempBoard =  new TicTacToeBoard(inBoard);
				
				// place a piece on each sqaure in the new board
				tempBoard.Place(inPiece, i);
				
				// and see if it results in an immediate win
				if ( tempBoard.Win(inPiece.Value) )
				{
					bestMove = i;
					return true;
				}
			}
				
			return false;
		}

		protected static bool ImmediateBlock(Piece inPiece, TicTacToeBoard inBoard, ref ushort bestMove)
		{
			Piece			p = ( inPiece.Value == Piece.PieceValue.X ) ? new Piece(Piece.PieceValue.X) : new Piece(Piece.PieceValue.O);
				
			for ( ushort i = 0; i < inBoard.Length; ++i )
			{
				// create a clone of the current board
				TicTacToeBoard tempBoard =  new TicTacToeBoard(inBoard);
						
				tempBoard.Place(p, i);
						
				if ( tempBoard.Win(p.Value) )
				{
					bestMove = i;
					return true;
				}
			}
				
			return false;
		}

	}
}
