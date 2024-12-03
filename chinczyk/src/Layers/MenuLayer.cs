class MenuLayer : Layer {
    public override void Display(Game game) {
        Console.Clear();
        Console.WriteLine("\tChińczyk");
        Console.WriteLine("1. Nowa gra");
        Console.WriteLine("2. Zakończ");
    }

    public override void HandleInput(Game game) {
        var info = Console.ReadKey();
        switch (info.Key) {
            case ConsoleKey.D1:
                game.AddLayer(new GameLayer());
                break;

            case ConsoleKey.D2:
                game.Close();
                break;
        }
    }
}
