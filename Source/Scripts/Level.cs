using Godot;
using System;
using System.Collections.Generic;

public class Level : Node2D
{
    private Escape Options;
    private Button EscapeBtn;
    private PackedScene boardscene;
    private Board board;
    private Timer Clock;
    private Label TimeCount;
    private Label FlagCount;
    private Label MineCount;
    private int TotalMines;
    private int NumberOfCells;
    private int RemainingFlags;
    private List<Cell> CellList;
    private List<Cell> RemainingCells;
    private int ClockTime;
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
        FlagCount = GetNode<Label>("FlagCount");
        MineCount = GetNode<Label>("MineCount");
        ClockTime = 0;
        Clock = GetNode<Timer>("Timer");
        TimeCount = GetNode<Label>("TimeCount");
        EscapeBtn = GetNode<Button>("EscapeBtn");
        Options = GetNode<Escape>("Options");
        Options.Hide();
        boardscene = ResourceLoader.Load<PackedScene>("res://Scenes/Board.tscn");

        EscapeBtn.Connect("pressed", this, "ToggleOptions");
        Clock.Connect("timeout", this, "UpdateClock");
        Options.Connect("ResumeSignal", this, "ResumeGame");
        TimeCount.Text = "Time: 0";
        FlagCount.Text = "Flags: 0";
        MineCount.Text = "Mines: 0";
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (Input.IsActionJustPressed("esc") && Visible) ToggleOptions();
    }

    private void UpdateFlagCount()
    {
        FlagCount.Text = "Flags: " + RemainingFlags.ToString();
    }

    private void FlagUpdate(bool flagdelta)
    {
        if (flagdelta) 
        {
            RemainingFlags--;
        }
        else
        {
            RemainingFlags++;
        }
        UpdateFlagCount();
    }
   
    public void ResumeGame()
    {
        ToggleOptions();
        StartClock();
    }

    public void StartClock()
    {
        Clock.Start();
    }

    public void StopClock()
    {
        Clock.Stop();
    }

    private void UpdateClock()
    {
        ClockTime++;
        TimeCount.Text = "Time: " + ClockTime.ToString();
    }

    private void ToggleOptions()
    {
        StopClock();
        Options.Visible = !Options.Visible;
        EscapeBtn.Visible = !EscapeBtn.Visible;
    }

    public void SetLevel(int width, int height, float mineRatio)
    {
        NumberOfCells = height * width;
        TotalMines = (int)Math.Floor(NumberOfCells * mineRatio);
        MineCount.Text = "Mines: " + TotalMines.ToString(); 
        FlagCount.Text = "Flags: " + TotalMines.ToString(); 
        RemainingFlags = TotalMines;
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
            cell.Connect("FlagSignal", this, "FlagUpdate");
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
        StopClock();
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
        Options.Show();
        Options.Lose();
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
        int target = NumberOfCells - TotalMines;
        int count = 0;
        foreach (Cell cell in CellList)
        {
            if (!cell.hidden && !cell.hasMine) count++;
        }
        if (count == target) DeclareWin();
    }

    private void DeclareWin()
    {
        StopClock();
        foreach (Cell cell in CellList)
        {
            if (cell.hasFlag)
            {
                cell.SetImage("win");
            }
        }
        Options.Show();
        Options.Win();
   
    }
}
