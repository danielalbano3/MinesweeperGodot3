using Godot;
using System;
using System.Collections.Generic;

public class Cell : TextureButton
{
    public bool hasMine;
    public bool hasFlag;
    public bool hidden;

    public int mineCount;
    public int row;
    public int col;

    private Texture texture;
    private AtlasTexture img;
    private AnimatedSprite Boom;
    private AudioStreamPlayer Sfx;
    private AudioStreamPlayer Flagsfx;

    [Signal] public delegate void GameOverSignal();
    [Signal] public delegate void OpenSpaceSignal(Cell cell);
    [Signal] public delegate void UpdateSignal();

    public override void _Ready()
    {
        Boom = GetNode<AnimatedSprite>("Boom");
        Sfx = GetNode<AudioStreamPlayer>("Sfx");
        Flagsfx = GetNode<AudioStreamPlayer>("Flagsfx");
        texture = ResourceLoader.Load<Texture>("res://Assets/Art/50x50.png");
        img = new AtlasTexture();
        img.Atlas = texture;

        SetImage("Cell");
        hasMine = false;
        hasFlag = false;
        hidden = true;
        mineCount = 0;
        row = 0;
        col = 0;

        Boom.Visible = false;
        Connect("pressed", this, "OnClick");
    }

    public void OnClick()
    {
        if (!hasFlag && hidden)
        {
            hidden = false;
            OpenCell();
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("flag"))
        {
            if (IsHovered() && !Disabled)
            {
                FlagToggle();
            }
        }
    }   

    private void FlagToggle()
    {
        if (hidden)
        {
            if (hasFlag)
            {
                SetImage("cell");
            }
            else
            {
                SetImage("flag");
                Flagsfx.Play();
            }
            hasFlag = !hasFlag;
        }
    }

    public void OpenCell()
    {
        Boom.Visible = true;
        Boom.Play();
        Sfx.Play();

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
                EmitSignal("OpenSpaceSignal", this);
            }
            
        }
        EmitSignal("UpdateSignal");
    }

    public void SetImage(object imageName)
    {
        //0-8, wrongflag, cell, mine, explode, flag
        string input = imageName.ToString().ToUpper();
        float sidesize = 50f;

        switch(input)
        {
            case "1":
                img.Region = new Rect2(0f,0f,sidesize,sidesize);
                break;
            case "2":
                img.Region = new Rect2(sidesize,0f,sidesize,sidesize);
                break;
            case "3":
                img.Region = new Rect2(2f * sidesize,0f,sidesize,sidesize);
                break;
            case "4":
                img.Region = new Rect2(3f * sidesize,0f,sidesize,sidesize);
                break;
            case "5":
                img.Region = new Rect2(4f * sidesize,0f,sidesize,sidesize);
                break;
            case "6":
                img.Region = new Rect2(0f,sidesize,sidesize,sidesize);
                break;
            case "7":
                img.Region = new Rect2(sidesize,sidesize,sidesize,sidesize);
                break;
            case "8":
                img.Region = new Rect2(2f * sidesize,sidesize,sidesize,sidesize);
                break;
            case "0":
                img.Region = new Rect2(3f * sidesize,sidesize,sidesize,sidesize);
                break;
            case "WRONGFLAG":
                img.Region = new Rect2(4f * sidesize,sidesize,sidesize,sidesize);
                break;
            case "CELL":
                img.Region = new Rect2(0f,2f * sidesize,sidesize,sidesize);
                break;
            case "MINE":
                img.Region = new Rect2(sidesize,2f * sidesize,sidesize,sidesize);
                break;
            case "EXPLODE":
                img.Region = new Rect2(2f * sidesize,2f * sidesize,sidesize,sidesize);
                break;
            case "FLAG":
                img.Region = new Rect2(3f * sidesize,2f * sidesize,sidesize,sidesize);
                break;
            case "WIN":
                img.Region = new Rect2(4f * sidesize,2f * sidesize,sidesize,sidesize);
                break;
        }
        TextureNormal = img;
    }
}