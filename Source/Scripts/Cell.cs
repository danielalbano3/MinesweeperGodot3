using Godot;
using System;
using System.Collections.Generic;

public class Cell : TextureButton
{
    public bool hasMine;
    public bool hasFlag;
    public bool isOpen;
    public int mineCount;
    public Vector2 rowcol;

    private Texture texture;
    private AtlasTexture img;

    [Signal] public delegate void GameOverSignal();
    [Signal] public delegate void OpenSpaceSignal(Vector2 pos);

    public override void _Ready()
    {
        texture = ResourceLoader.Load<Texture>("res://Assets/Art/mstiles.png");
        img = new AtlasTexture();
        img.Atlas = texture;

        SetImage("Cell");
        hasMine = false;
        hasFlag = false;
        isOpen = false;
        mineCount = 5;
        rowcol = new Vector2(0,0);

        Connect("pressed", this, "OnClick");
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseButton mouseBtn)
        {
            if (mouseBtn.ButtonIndex == (int)ButtonList.Right && mouseBtn.Pressed)
            {
                FlagToggle();
            }
        }
    }

    private void FlagToggle()
    {
        if (!isOpen)
        {
            if (hasFlag)
            {
                SetImage("cell");
            }
            else
            {
                SetImage("flag");
            }
            hasFlag = !hasFlag;
        }
    }

    private void Reveal()
    {
        if (hasMine && !isOpen)
        {
            if (hasFlag)
            {
                SetImage("wrongflag");
            }
            else
            {
                SetImage("mine");
            }
        }
    }

    private void OpenCell()
    {
        if (!isOpen)
        {
            if (hasMine)
            {
                SetImage("explode");
                EmitSignal("GameOverSignal");
            }
            else
            {
                SetImage(mineCount);
                if (mineCount == 0)
                {
                    EmitSignal("OpenSpaceSignal", rowcol);
                }
            }
            isOpen = true;
        }
    }

    private void SetImage(object imageName)
    {
        //0-8, wrongflag, cell, mine, explode, flag
        string input = imageName.ToString().ToUpper();
        
        switch(input)
        {
            case "1":
                img.Region = new Rect2(0f,0f,100f,100f);
                break;
            case "2":
                img.Region = new Rect2(100f,0f,100f,100f);
                break;
            case "3":
                img.Region = new Rect2(200f,0f,100f,100f);
                break;
            case "4":
                img.Region = new Rect2(300f,0f,100f,100f);
                break;
            case "5":
                img.Region = new Rect2(400f,0f,100f,100f);
                break;
            case "6":
                img.Region = new Rect2(0f,100f,100f,100f);
                break;
            case "7":
                img.Region = new Rect2(100f,100f,100f,100f);
                break;
            case "8":
                img.Region = new Rect2(200f,100f,100f,100f);
                break;
            case "0":
                img.Region = new Rect2(300f,100f,100f,100f);
                break;
            case "WRONGFLAG":
                img.Region = new Rect2(400f,100f,100f,100f);
                break;
            case "CELL":
                img.Region = new Rect2(0f,200f,100f,100f);
                break;
            case "MINE":
                img.Region = new Rect2(100f,200f,100f,100f);
                break;
            case "EXPLODE":
                img.Region = new Rect2(200f,200f,100f,100f);
                break;
            case "FLAG":
                img.Region = new Rect2(300f,200f,100f,100f);
                break;
        }
        TextureNormal = img;
    }

    public void OnClick()
    {
        if (!hasFlag)
        {
            OpenCell();
        }
    }
}