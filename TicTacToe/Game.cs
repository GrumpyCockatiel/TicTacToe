using System;

namespace TicTacToe
{
	/// <summary>Main game wrapper class created for each type of interface.</summary>
	public class Game
	{
		/// <summary>Whether the game is over or not.</summary>
		bool						over = false;
		/// <summary>Current state of the game board.</summary>
		private	TicTacToeBoard		theBoard;
		/// <summary>Array of players.  Replace with a circular queue.</summary>
		private CircularList		players = new CircularList();
		/// <summary>Player's whose turn it current is.</summary>
		private Player				currentPlayer = null;

		/// <summary>Console based Tic Tac Toe game.</summary>
		[STAThread]
		public static void Main(string[] args)
		{
			// number of games played
			uint games = 0;

			// print the game intro
			Game.PrintIntro();

			// create a new game
			Game thisGame = new Game();

			// play the game
			do
			{
				// play the current game
				Console.WriteLine("Game {0}", ++games);
				thisGame.Play();

				// swap the player's tokens
				thisGame.SwapTokens();

				// clear the board
				thisGame.Reset();

			} while ( Game.PlayAgain() );

			// print final score and exit message
			Game.PrintDone();
		}

		public Game()
		{
			// initialize a new game board
			theBoard = new TicTacToeBoard();

			// choose the type of players
			this.InitPlayers(this.players);
		}

		public void Play()
		{
			// set starting player to the player who is Os
			// this.currentPlayer = (this.players[0].Token == Piece.PieceValue.O) ? (byte)0 : (byte)1;

			foreach (Player p in this.players)
			{
				this.currentPlayer = p;
				if (p.Token == Piece.PieceValue.O)
					break;
			}

			// draw the empty board
			this.DrawBoard();
			Console.WriteLine();

			// player this game until there is a winner
			while (!this.Over)
			{
				// get a reference to the current player
				this.currentPlayer = (Player)players.Current;

				// player selects a move
				ushort move = this.currentPlayer.Move(this.theBoard);

				// mark move on the board
				this.theBoard.Place( new Piece(this.currentPlayer.Token), move );

				// draw the board on the screen
				Console.WriteLine("Round {0}: {1} chooses square {2}.", this.theBoard.Moves, this.currentPlayer.Token.ToString(), move);
				this.DrawBoard();
				Console.WriteLine();

				// check to see if the most recent player won
				if ( this.Over = theBoard.Win(this.currentPlayer.Token) )
				{
					this.currentPlayer.AddPoints(1);
					this.PrintWinner();
				}
				// check to see if the game is a draw
				else if ( this.Over = theBoard.Full() )
					this.PrintDraw();
				// otherwise, move to next player
				else
					players.MoveNext();
			}
		}

		/// <summary>Swaps the players token values.</summary>
		public void SwapTokens()
		{
			// reset the list
			this.players.Reset();

			// put players in a temporary list
			// which should be a feature of the list itself
			Player[] tempList = new Player[2];

			tempList[0] = (Player)this.players.Current;
			this.players.MoveNext();
			tempList[1] = (Player)this.players.Current;

			// swap player's tokens
			Piece.PieceValue temp = tempList[1].Token;
			tempList[1].Token = tempList[0].Token;
			tempList[0].Token = temp;
		}

		public void Reset()
		{
			this.theBoard.Clear();
			this.Over = false;
		}

		/// <summary>Get or set the state of the current game.</summary>
		public bool Over
		{
			get { return this.over; }
			set { this.over = value; }
		}

		private static void PrintIntro()
		{
			Console.WriteLine("TIC TAC TOE");
			Console.WriteLine("O always goes first.");
			Console.WriteLine("Players swap after each game.");
			Console.WriteLine();
		}

		/// <summary>Echo the winner of the current game.</summary>
		private void PrintWinner()
		{
			Console.WriteLine("{0} Wins!", this.currentPlayer.Token.ToString());
			this.PrintScore();
		}

		/// <summary>Display draw game message.</summary>
		private void PrintDraw()
		{
			Console.WriteLine("The game is a draw.");
			this.PrintScore();
		}

		/// <summary>Print both player's current number of wins.</summary>
		private void PrintScore()
		{
			// reset the list
			this.players.Reset();

			// put players in a temporary list
			// which should be a feature of the list itself
			Player[] tempList = new Player[2];

			tempList[0] = (Player)this.players.Current;
			this.players.MoveNext();
			tempList[1] = (Player)this.players.Current;

			Console.WriteLine("Player 1: {0} Wins | Player 2: {1} Wins", tempList[0].Score, tempList[1].Score);
		}

		/// <summary>Print game over message.</summary>
		private static void PrintDone()
		{
			Console.WriteLine("Game over!");
		}

		/// <summary>Command line method used by a human player to select a move.</summary>
		/// <returns>Board index to move to.</returns>
		public static ushort PromptForMove(TicTacToeBoard board, Piece.PieceValue token)
		{
			ushort	i = 0;

			do
			{
				Console.Write("Enter the location of your move: ");
				i = Convert.ToUInt16( Console.ReadLine() );
			} while ( i < 0 || i > 9 );

			return i;
		}

		/// <summary>Prompt for another game.</summary>
		private static bool PlayAgain()
		{
			Console.Write("Continue playing [y | n]? ");
			char c = Char.ToLower( Convert.ToChar( Console.ReadLine()) );
			
			return (c == 'y');
		}

		/// <summary>Initializes the players array.</summary>
		/// <param name="a"></param>
		private void InitPlayers(CircularList a)
		{
			byte	n = 0;
	
			do
			{
				Console.Write("How many human players [0|1|2]? ");
				n = Convert.ToByte( Console.ReadLine() );
			} while ( n < 0 || n > 2 );

			switch (n)
			{
				case 1:
					this.ChooseSides(a);
					break;
	
				case 2:
					// create human players
					Human human1 = new Human(Piece.PieceValue.O);
					human1.MoveMethod = new Player.ChooseMove(Game.PromptForMove);
					Human human2 = new Human(Piece.PieceValue.X);
					human2.MoveMethod = new Player.ChooseMove(Game.PromptForMove);
					
					// insert into the player's list
					a.Add(human1);
					a.Add(human2);
					break;
			
				case 0:
				default:
					// create computer players
					Computer comp1 = new Computer(Piece.PieceValue.O);
					Computer comp2 = new Computer(Piece.PieceValue.X);
					// set their skill level
					// insert into the player's list
					a.Add(comp1);
					a.Add(comp2);
					break;
			}
		}

		/// <summary>Prompts a human player to be Xs or Os.</summary>
		/// <param name="a"></param>
		private void ChooseSides(CircularList a)
		{
			char	c;
	
			do
			{
				Console.Write("Do you want to start as Xs or Os? ");
				c = Char.ToLower( Convert.ToChar( Console.ReadLine() ) );
			} while ( c != 'x' && c != 'o' );
	
			if ( c == 'o' )
			{
				Human hum = new Human(Piece.PieceValue.O);
				hum.MoveMethod = new Player.ChooseMove(Game.PromptForMove);
				Computer comp = new Computer(Piece.PieceValue.X);
				// insert into the player's list
				a.Add(hum);
				a.Add(comp);
			}
			else
			{
				Computer comp = new Computer(Piece.PieceValue.O);
				Human hum = new Human(Piece.PieceValue.X);
				hum.MoveMethod = new Player.ChooseMove(Game.PromptForMove);
				// insert into the player's list
				a.Add(comp);
				a.Add(hum);
			}
		}

		/// <summary>Draws the current board for a command line interface.</summary>
		private void DrawBoard()
		{
			byte	i = 0;	// iterator

			// iterate through the board
			for ( i = 0; i < theBoard.Length; ++i )
			{
				if ( theBoard.At(i) )
				{
					switch ( theBoard.GetAt(i).Value )
					{
						case Piece.PieceValue.X:
							Console.Write("X ");
							break;
						case Piece.PieceValue.O:
							Console.Write("O ");
							break;
						default:
							// some kind of error has occured
							throw new System.Exception();
					} // end switch
				}
				else
					Console.Write("{0} ", i);

				// start a new line after mDim squares
				if ( i % theBoard.Dimension == theBoard.Dimension - 1 )
					Console.WriteLine();
			} // end for loop
		}

	}
}
