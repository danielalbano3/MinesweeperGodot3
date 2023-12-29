using Godot;
using System;
using System.Runtime.CompilerServices;

public class Board : GridContainer
{
    private PackedScene cellscene;
    public float boardsizex;
    public float boardsizey;
    private float imgsize;

    public override void _Ready()
    {
        imgsize = 50f;
        cellscene = ResourceLoader.Load<PackedScene>("res://Scenes/Cell.tscn");
    }

    public void SetBoard(int row, int col)
    {
        Columns = col;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Cell cell = (Cell)cellscene.Instance();
                AddChild(cell);
                cell.row = i;
                cell.col = j;
            }
        }

        boardsizex = (float)(col * imgsize);
        boardsizey = (float)(row * imgsize);
        CenterBoard(boardsizex,boardsizey);
    }   

    public void CenterBoard(float sizeX, float sizeY)
    {
        Vector2 screensize = GetViewportRect().Size;
        float deltaX = sizeX / 2f;
        float deltaY = sizeY / 2f;

        float deltaSx = screensize.x / 2f;
        float deltaSy = screensize.y / 2f;

        float offsetX = deltaSx - deltaX;
        float offsetY = deltaSy - deltaY;

        SetPosition(new Vector2(offsetX, offsetY));
    }
}
