using Godot;
using System;

public class Escape : CanvasLayer
{
    private Label StateLabel;
    private Button BackBtn;
    private Button ResumeBtn;

    [Signal] public delegate void ResumeSignal();

    public override void _Ready()
    {
        StateLabel = GetNode<Label>("Control/VBoxContainer/StateLabel");
        BackBtn = GetNode<Button>("Control/VBoxContainer/BtnBox/BackBtn");
        ResumeBtn = GetNode<Button>("Control/VBoxContainer/BtnBox/ResumeBtn");

        ResumeBtn.Connect("pressed", this, "ResumeEmit");
        BackBtn.Connect("pressed", this, "ReloadGame");
    }

    private void ResumeEmit()
    {
        EmitSignal("ResumeSignal");
    }

    public void ReloadGame()
    {
        GetTree().ReloadCurrentScene();
    }

    public void Lose()
    {
        StateLabel.Text = "GAME OVER!\nLuck is not on your side today it seems...";
        ResumeBtn.Hide();
    }

    public void Win()
    {
        StateLabel.Text = "YOU WON!\nSkill is your secret sauce it seems...";
        ResumeBtn.Hide();
    }
}
