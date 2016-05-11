using Ninject;
using System;

namespace ConwaysGameOfLife
{
	public class Program
	{
	    private readonly Game game;
	    private readonly ConsoleUi ui;

	    public Program(Game game, ConsoleUi ui)
	    {
	        this.game = game;
	        this.ui = ui;
	    }

	    private static void Main()
	    {
	        new Program(new Game(new Size(60, 20)), new ConsoleUi()).Run();
	    }

	    private void Run()
	    {
	        game.Revive(Patterns.GetGlider(new Point(25, 8)));
            ui.UpdateAll(game);

	        while (true)
	        {
	            var key = Console.ReadKey(intercept: true);
	            if (key.Key == ConsoleKey.Escape) break;

	            foreach (var changedCell in game.Step().ChangedCells)
	                ui.UpdateCell(changedCell.X, changedCell.Y, changedCell.IsAlive);
	        }
	    }
	}
}