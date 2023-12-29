using Godot;
using System;
using System.Collections.Generic;

public class Level : Node2D
{
    private PackedScene boardscene;
    private Board board;

    private float MineRatio;
    private int TotalMines;
    private int Rows;
    private int Columns;
    private int NumberOfCells;

    private List<Cell> CellList;
    private List<Cell> RemainingCells;

    private int[,] Distance = new int[8,2]
    {
        {-1,-1},
        {-1,0},
        {-1,1},

        {0,-1},
        {0,1},

        {1,-1},
        {1,0},
        {1,1},
    };

    public override void _Ready()
    {
        boardscene = ResourceLoader.Load<PackedScene>("res://Scenes/Board.tscn");
        
        Rows = 10;
        Columns = 10;       
        MineRatio = 0.20f;

        // SetLevel(Rows,Columns,MineRatio);
    }

    public void SetLevel(int width, int height, float mineRatio)
    {
        NumberOfCells = height * width;
        TotalMines = (int)Math.Floor(NumberOfCells * mineRatio);
        board = (Board)boardscene.Instance();
        AddChild(board);

        board.SetBoard(height,width);
        CellList = new List<Cell>();
        RemainingCells = new List<Cell>();
        foreach (Cell cell in board.GetChildren())
        {
            CellList.Add(cell);
            RemainingCells.Add(cell);
            cell.Connect("GameOverSignal", this, "GameOver");
            cell.Connect("OpenSpaceSignal", this, "SpreadOpenSpaces");
            cell.Connect("UpdateSignal", this, "CheckForWin");
        }

        AssignMines();
        AssignCount();
    }

    private void SpreadOpenSpaces(Cell origin)
    {
        RemainingCells.Remove(origin);

        int r = origin.row;
        int c = origin.col;

        for (int i = 0; i < 8; i++)
        {
            Cell cell = new Cell();
            cell = GetCell(r + Distance[i,0], c + Distance[i,1]);
            if (cell != null)
            {
                if (RemainingCells.Contains(cell) && cell.hidden && !cell.hasFlag)
                {
                    cell.hidden = false;
                    cell.OpenCell();
                }
            }
        }
    }

    private void GameOver()
    {
        GD.Print("Gameover");
        foreach (Cell cell in board.GetChildren())
        {
            cell.Disabled = true;
            if (cell.hasMine) 
            {
                if (cell.hasFlag)
                {
                    cell.SetImage("wrongflag");
                }
                else
                {
                    if (cell.hidden)
                    {
                        cell.SetImage("mine");
                    }
                    else
                    {
                        cell.SetImage("explode");
                    }
                }
            }

        }
    }

    private void AssignMines()
    {
        int mines = TotalMines;
        while (mines != 0)
        {
            Random random = new Random();
            Cell _cell = CellList[random.Next(NumberOfCells)];
            if (!_cell.hasMine)
            {
                _cell.hasMine = true;
                mines--;
            }
        }
    }

    private void AssignCount()
    {
        foreach (Cell cell in CellList)
        {
            cell.mineCount = CountMines(cell);
        }
    }

    private int CountMines(Cell origin)
    {
        int mineTotal = 0;
        int r = origin.row;
        int c = origin.col;

        for (int i = 0; i < 8; i++)
        {
            Cell cell = new Cell();
            cell = GetCell(r + Distance[i,0], c + Distance[i,1]);
            if (cell != null)
            {
                if (cell.hasMine) mineTotal++;
            }
        }
        
        return mineTotal;
    }

    private Cell GetCell(float row, float col)
    {        
        return CellList.Find(cell => cell.row == row && cell.col == col);
    }

    private void CheckForWin()
    {
        GD.Print("Checking for win");
        int target = NumberOfCells - TotalMines;
        int count = 0;
        foreach (Cell cell in CellList)
        {
            if (!cell.hidden && !cell.hasMine) count++;
        }
        if (count == target) DeclareWin();
        GD.Print(count);
    }

    private void DeclareWin()
    {
        foreach (Cell cell in CellList)
        {
            if (cell.hasFlag)
            {
                cell.SetImage("win");
            }
        }
   
    }
}
