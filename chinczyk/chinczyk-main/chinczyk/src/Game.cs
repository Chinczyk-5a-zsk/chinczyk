sealed class Game {
    public Game() {
        running = true;
        layers = [];
        layers.Add(new MenuLayer());
    }

    public void Run() {
        Console.SetWindowSize(77, 42);

        while (running) {
            var last = layers.LastOrDefault();
            if (last == null) {
                running = false;
                break;
            }

            last.Display(this);
            last.HandleInput(this);
        }
    }

    public void AddLayer(Layer layer) {
        layers.Add(layer);
    }

    public void Removelayer(Layer layer) {
        layers.Remove(layer);
    }

    public void Close() {
        running = false;
    }

    private List<Layer> layers;
    private bool running;
}
