class GameLayer : Layer {
    public GameLayer() {
        // plansza ma 48 pól w pętli
        board = new PlayerColor?[48];

        // każdy gracz ma 4 pola startowe z pionkami
        start = [];
        start[PlayerColor.Red] = [ true, true, true, true ];
        start[PlayerColor.Green] = [ true, true, true, true ];
        start[PlayerColor.Cyan] = [ true, true, true, true ];
        start[PlayerColor.Yellow] = [ true, true, true, true ];

        // każdy gracz ma 4 pola puste pola docelowe
        finish = [];
        finish[PlayerColor.Red] = [ false, false, false, false ];
        finish[PlayerColor.Green] = [ false, false, false, false ];
        finish[PlayerColor.Cyan] = [ false, false, false, false ];
        finish[PlayerColor.Yellow] = [ false, false, false, false ];
    }

    public override void Display(Game game) {
        Console.Clear();
        
        // pola startowe gracza czerwonego
        var color = PlayerColor.Red;
        for (int i = 0; i < 4; i++) {
            // dwa pierwsze parametry wyznaczaję współrzędne pola (10 oraz 1 to przesunięcie stałe dla koloru, i % 2 oraz i / 2 to zamiana liczb 0-3 na współrzędne [0, 0], [0, 1], [1, 0], [1, 1])
            // trzeci parametr to kolor pola
            // czwarty parametr to kolor pionka, instrukcja `start[color][i]` sprawdza czy na tym polu jest pionek, jeżeli tak brana jest wartość po znaku zapytania (czyli kolor gracza), jeżeli nie brana jest wartość po dwukropku (czyli null reprezentujący puste pole)
            DisplayField(10 + i % 2, 1 + i / 2, color, start[color][i] ? color : null);
        }
        // pionek na planszy


        // czerowne domki
        for (int i = 0; i < 4; i++)
        {
            DisplayField(6, 1 + i, color, finish[color][i] ? color : null);
        }

        // pola startowe gracza zielonego
        color = PlayerColor.Green;
        for (int i = 0; i < 4; i++) {
            DisplayField(10 + i % 2, 10 + i / 2, color, start[color][i] ? color : null);
        }
        // zielone domki
        for (int i = 0; i < 4; i++)
        {
            DisplayField(8 + i, 6, color, finish[color][i] ? color : null);
        }

        // pola startowe gracza niebieskiego
        color = PlayerColor.Cyan;
        for (int i = 0; i < 4; i++) {
            DisplayField(1 + i % 2, 10 + i / 2, color, start[color][i] ? color : null);
        }
        // niebieskie domki
        for (int i = 0; i < 4; i++)
        {
            DisplayField(6, 8 + i, color, finish[color][i] ? color : null);
        }

        // pola startowe gracza żółtego
        color = PlayerColor.Yellow;
        for (int i = 0; i < 4; i++) {
            DisplayField(1 + i % 2, 1 + i / 2, color, start[color][i] ? color : null);
        }
        // zolte domki
        for (int i = 0; i < 4; i++)
        {
            DisplayField(1 + i, 6, color, finish[color][i] ? color : null);
        }

        // wyświetlenie pętli planszy
        for (int i = 0; i < 48; i++) {
            var (x, y) = boardCoordinates(i); // współrzędne pola na podstawie indeksu
            var c = boardColor(i);
            DisplayField(x, y, c, board[i]); // wyświetlenie pola na wskazanych współrzędnych
        }

        // przesunięcie kursora pod planszę
        Console.SetCursorPosition(0, 40);

    }

    //powrót do bazy 
    private void ReturnToBase(PlayerColor player)
    {
        for (int i = 0; i < 4; i++) {
            if (!start[player][i])
            {
                start[player][i] = true;
                Console.WriteLine("Pionek wrócił do bazy");
                return;
            }
        }
    }

    private void MovePawn(PlayerColor player, int steps)
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == player)
            {
                board[i] = null;
                int NewPosition = (i + steps) % board.Length;
                if (NewPosition != null)
                {
                    PlayerColor opponent = board[NewPosition];
                    Console.WriteLine("pionek zbity");
                    ReturnToBase(opponent);
                }
                board[NewPosition] = player;
                Console.WriteLine("Gracz {player} przesuną się na pole{newposition}");
                return;
            }
        }
        Console.WriteLine("nie masz pionków na polu");
        
    }

    // sesja gry
    private void SaveGameState()
    {
        using (StreamWriter writer = new StreamWriter("gamestate.txt"))
        {
            // writer.WriteLine($"CurrentPlayer: {currentPlayer}"); <- przypisanie gracza
            writer.WriteLine("Board:");
            for (int i = 0; i < board.Length; i++)
            {
                writer.WriteLine($"{i}:{board[i]}");
            }
        }

        // kostka
    }

    private void EnterBoard(PlayerColor player)
    {
        for (int i = 0; i < 4; i++)
        {
            if (start[player][i])
            { // Sprawdza, czy pionek jest w domku
                start[player][i] = false; // Usuwa pionek z domku
                board[GetStartPosition(player)] = player; // Umieszcza pionek na polu startowym
                Console.WriteLine($"Gracz {player} wprowadził pionek na planszę.");
                return;
            }
        }
        Console.WriteLine("Brak pionków w domku!");
    }

    //Ustawianie pozycji pionków , na sztywno przypisanie "Koordynatów pionków na planszy"

    private int GetStartPosition(PlayerColor player)
    {
        return player switch
        {
            PlayerColor.Red => 0,
            PlayerColor.Green => 12,
            PlayerColor.Cyan => 24,
            PlayerColor.Yellow => 36,
            _ => throw new ArgumentOutOfRangeException("Nieprawidłowy gracz."),
        };
    }


        //podstawowe akcje dla pionka narazie bez rotacji kolorów
    public override void HandleInput(Game game)
    {
        int diceRoll = RollDice(); // Rzut kostką
        Console.WriteLine("Wybierz akcję: (1) Wprowadź pionek na planszę, (2) Przesuń pionek.");
        var action = Console.ReadKey().KeyChar;
        Console.WriteLine();

        PlayerColor currentPlayer = PlayerColor.Red; // Na razie ustawiamy gracza "na sztywno"

        if (action == '1')
        {
            EnterBoard(currentPlayer);
        }
        else if (action == '2')
        {
            MovePawn(currentPlayer, diceRoll);
        }
        else
        {
            Console.WriteLine("Nieprawidłowa akcja!");
        }

        game.Removelayer(this);
    }

//    public override void HandleInput(Game game) {
//        var info = Console.ReadKey();
//        game.Removelayer(this);
 //   }

    private Random dice = new Random();
    private int diceResult;

    //rzut kostka !!!!
    private int RollDice()
    {
        diceResult = dice.Next(1,7); //1-6 
        Console.Write($"rzut kostką to : {diceResult}");
        return diceResult;
    }

    public void StartTurn(PlayerColor player)
    {
        Console.WriteLine("");
        int Result = RollDice();
        if (Result == 6)
        {
            //wyjście z bazy
        }
        else
        {
            MovePawn(player, Result);
        }
    }



    private void DisplayField(int x, int y, PlayerColor? fieldColor, PlayerColor? pawnColor) {
        if (pawnColor != null) {
            var pawnConsoleColor = pawnColor switch {
                PlayerColor.Red => ConsoleColor.DarkRed,
                PlayerColor.Green => ConsoleColor.DarkGreen,
                PlayerColor.Cyan => ConsoleColor.DarkCyan,
                PlayerColor.Yellow => ConsoleColor.DarkYellow,
                _ => ConsoleColor.Black, // ten warunek nigdy nie zajdzie, ale bez tego podkreśla kod na żółto :)
            };
            Console.SetCursorPosition(x * 6 + 1, y * 3);
            Console.ForegroundColor = pawnConsoleColor;
            Console.Write("X");
        }

        var fieldConsoleColor = fieldColor switch {
            PlayerColor.Red => ConsoleColor.DarkRed,
            PlayerColor.Green => ConsoleColor.DarkGreen,
            PlayerColor.Cyan => ConsoleColor.DarkCyan,
            PlayerColor.Yellow => ConsoleColor.DarkYellow,
            _ => ConsoleColor.Gray,
        };
        Console.SetCursorPosition(x * 6, y * 3 + 1);
        Console.ForegroundColor = fieldConsoleColor;
        Console.Write("-=-");

        // wyczyszczenie koloru
        Console.ForegroundColor = ConsoleColor.White;
    }

    // zamiana numeru pola na jego współrzędne, funkcja zwraca rekord (dwie zmienne)
    static private (int x, int y) boardCoordinates(int index) {
        return index switch {
            // sprawdzenie poprawności indexu, index poza przedziałem <0, 47> wyrzuci błąd
            < 0 or > 47 => throw new IndexOutOfRangeException( $"field index must be between <0, 47>, but {index} was given."),

            < 6 => (7, index), // pola od 0 do 5 maję współrzędne od [7, 0] do [7, 6]
            < 11 => (2 + index, 5), // pola od 0 do 5 maję współrzędne od [8, 5] do [12, 5]
            11 => (12, 6), // pole 11 ma współrzędne [12, 6]
            < 18 => (24 - index, 7), // pola od 0 do 5 maję współrzędne od [12, 7] do [7, 7]
            < 23 => (7, index - 10), // pola od 0 do 5 maję współrzędne od [7, 8] do [7, 12]
            23 => (6, 12), // pole 11 ma współrzędne [6, 12]
            < 30 => (5, 36 - index), // pola od 0 do 5 maję współrzędne od [5, 12] do [5, 7]
            < 35 => (34 - index, 7), // pola od 0 do 5 maję współrzędne od [4, 7] do [0, 7]
            35 => (0, 6), // pole 11 ma współrzędne [0, 6]
            < 42 => (index - 36, 5), // pola od 0 do 5 maję współrzędne od [0, 5] do [5, 5]
            < 47 => (5, 46 - index), // pola od 0 do 5 maję współrzędne od [5, 4] do [5, 0]
            47 => (6, 0), // pole 11 ma współrzędne [6, 0]
        };
    }

    // zwraca kolor danego pola (pola startowe zwracają kolor danego gracza pozostałe zwracają null)
    static private PlayerColor? boardColor(int index) {
        return index switch {
            0 => PlayerColor.Red,
            12 => PlayerColor.Green,
            24 => PlayerColor.Cyan,
            36 => PlayerColor.Yellow,
            _ => null,
        };
    }

    private PlayerColor?[] board;
    private Dictionary<PlayerColor, bool[]> start;
    private Dictionary<PlayerColor, bool[]> finish;
}