﻿using RLNET;
using SalemLib;
using System.Diagnostics;

namespace MinesweeperRLNetPT.Core
{
    public class Tile
    {
        public Point Position { get; set; }
        public bool IsMine { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsHighlighted { get; set; }
        public int AdjacentMines { get; set; }

        private RLColor Color;
        private RLColor BgColor;

        public Tile(Point position)
        {
            Position = position;
            IsRevealed = false;
            IsMine = false;
            UpdateColor();
            UpdateBgColor();
        }

        public int CountAdjacentMines(Map map)
        {
            var adjacentTiles = map.GetAdjacentTiles(this);
            int mineCount = 0;

            foreach (var tile in adjacentTiles)
            {
                if (tile.IsMine)
                {
                    mineCount++;
                }
            }

            AdjacentMines = mineCount;
            return mineCount;
        }

        public void Draw(RLConsole console)
        {
            if (IsFlagged)
            {
                SetConsoleSymbol(console, 'F');
            }
            else if (!IsRevealed)
            {
                SetConsoleSymbol(console, 'X');
            }
            else if (IsMine)
            {
                SetConsoleSymbol(console, 'M');
            }
            else if (AdjacentMines != 0)
            {
                SetConsoleSymbolFromAdjMines(console);
            }
            else
            {
                SetConsoleSymbol(console, 0);
            }
        }

        public void Reveal()
        {
            IsRevealed = true;
            UpdateColor();
        }

        public void ToggleFlag()
        {
            IsFlagged = !IsFlagged;
            UpdateColor();
        }

        public void ToggleHighlight()
        {
            IsHighlighted = !IsHighlighted;
            UpdateBgColor();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Tile t = (Tile)obj;
                return t.Position.Equals(Position)
                    && t.IsMine == IsMine
                    && t.IsRevealed == IsRevealed
                    && t.IsFlagged == IsFlagged
                    && t.AdjacentMines == AdjacentMines;
            }
        }

        // generated GetHashCode override
        public override int GetHashCode()
        {
            int hashCode = -1020351265;
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + IsMine.GetHashCode();
            hashCode = hashCode * -1521134295 + IsRevealed.GetHashCode();
            hashCode = hashCode * -1521134295 + IsFlagged.GetHashCode();
            hashCode = hashCode * -1521134295 + AdjacentMines.GetHashCode();
            return hashCode;
        }

        // PRIVATE METHODS
        private void SetConsoleSymbol(RLConsole console, int symbol)
        {
            console.Set(Position.X, Position.Y, Color, BgColor, symbol);
        }

        private void SetConsoleSymbolFromAdjMines(RLConsole console)
        {
            int charIndex = AdjacentMines + 48;
            SetConsoleSymbol(console, charIndex);
        }

        private void UpdateColor()
        {
            if (IsFlagged)
            {
                Color = Colors.Flag;
            }
            else if (!IsRevealed)
            {
                Color = Colors.Undiscovered;
            }
            else if (IsMine)
            {
                Color = Colors.Mine;
            }
            else
            {
                Color = Colors.Number;
            }
        }

        private void UpdateBgColor()
        {
            if (IsHighlighted)
            {
                BgColor = Colors.HBackground;
            }
            else
            {
                BgColor = Colors.Background;
            }
        }
    }
}