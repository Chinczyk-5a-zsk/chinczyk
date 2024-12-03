class Program {
    static void Main() {
        // for (int i = 0; i < 11; i++) {
        //     for (int j = 0; j < 11; j++) {

        //         Console.ForegroundColor = ConsoleColor.Green;
        //         Console.SetCursorPosition(i * 4, j * 2);
        //         Console.Write("-=-");

        //     }
        // }

        var game = new Game();
        game.Run();
    }
}