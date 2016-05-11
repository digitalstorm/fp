﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ConwaysGameOfLife
{
	public class Game : IReadonlyField
	{
		public int Width { get; }
		public int Height { get; }

		private bool[,] isAlive;

		public Game(Size size)
		{
			Width = size.Width;
			Height = size.Height;
			isAlive = new bool[Width, Height];
		}

		public void Revive(params Point[] cells)
		{
			foreach (var pos in cells)
				isAlive[(pos.X + Width) % Width, (pos.Y + Height) % Height] = true;
		}

		public StepResult Step()
		{
		    var changedCells = new List<ChangedCell>();
			var willBeAlive = new bool[Width, Height];
		    for (int y = 0; y < Height; y++)
		    {
		        for (int x = 0; x < Width; x++)
		        {
		            willBeAlive[x, y] = WillBeAlive(x, y);
		            if (willBeAlive[x, y] != isAlive[x, y])
		            {
		                changedCells.Add(new ChangedCell(x, y, willBeAlive[x, y]));
		            }
		        }
		    }
		    isAlive = willBeAlive;
		    return new StepResult
		    {
		        ChangedCells = changedCells
		    };
		}

		private bool WillBeAlive(int x, int y)
		{
			var aliveCount = GetNeighbours(x, y).Count(IsAlive);
			return WillBeAlive(isAlive[x, y], aliveCount);
		}

		private bool WillBeAlive(bool alive, int aliveNeighbours)
		{
			return aliveNeighbours == 3 || aliveNeighbours == 2 && alive;
		}

		private IEnumerable<Point> GetNeighbours(int x, int y)
		{
			return
				from nx in new[] { x - 1, x, x + 1 }
				from ny in new[] { y - 1, y, y + 1 }
				where nx != x || ny != y
				select new Point(nx, ny);
		}

		public bool IsAlive(Point pos)
		{
			return IsAlive(pos.X, pos.Y);
		}

		public bool IsAlive(int x, int y)
		{
			return isAlive[(x + Width) % Width, (y + Height) % Height];
		}

		public override string ToString()
		{

			var rows = Enumerable.Range(0, Height)
				.Select(y =>
					string.Join("",
						Enumerable.Range(0, Width).Select(x => isAlive[x, y] ? "#" : " ")
				));
			return string.Join("\n", rows);
		}
	}

    public class StepResult
    {
        public List<ChangedCell> ChangedCells { get; set; }
    }

    public class ChangedCell
    {
        public ChangedCell(int x, int y, bool isAlive)
        {
            X = x;
            Y = y;
            IsAlive = isAlive;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }
    }
}