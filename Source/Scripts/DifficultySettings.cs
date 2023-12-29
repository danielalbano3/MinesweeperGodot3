using Godot;
using System;

public class DifficultySettings : Control
{
    private int RowChoice;
    private int ColChoice;

    private Label ChoiceLabel;
    private Button EasyBtn;
    private Button NormalBtn;
    private Button HardBtn;
    private Button NuclearBtn;

    private Button PlayBtn;
    private Button BackToMainBtn;

    private Panel PresetPanel;

    
    [Signal] public delegate void BackToMainSignal();
    [Signal] public delegate void PlaySignal();
    [Signal] public delegate void SettingsSignal(int h, int w, float r);

    public override void _Ready()
    {
        RowChoice = 5;
        ColChoice = 5;

        ChoiceLabel = GetNode<Label>("PresetPanel/ChoiceLabel");
        PresetPanel = GetNode<Panel>("PresetPanel");
        EasyBtn = GetNode<Button>("PresetPanel/VBoxContainer/DifficultyBox/Easy");
        NormalBtn = GetNode<Button>("PresetPanel/VBoxContainer/DifficultyBox/Normal");
        HardBtn = GetNode<Button>("PresetPanel/VBoxContainer/DifficultyBox/Hard");
        NuclearBtn = GetNode<Button>("PresetPanel/VBoxContainer/DifficultyBox/Nuclear");
        
        PlayBtn = GetNode<Button>("PresetPanel/OptionsBtns/PlayBtn");
        BackToMainBtn = GetNode<Button>("PresetPanel/OptionsBtns/BackToMainBtn");

        BackToMainBtn.Connect("pressed", this, "GoBackToMain");
        PlayBtn.Connect("pressed", this, "GoPlay");

        EasyBtn.Connect("pressed", this, "UpdateLabel", new Godot.Collections.Array {"Easy"});
        NormalBtn.Connect("pressed", this, "UpdateLabel", new Godot.Collections.Array {"Normal"});
        HardBtn.Connect("pressed", this, "UpdateLabel", new Godot.Collections.Array {"Hard"});
        NuclearBtn.Connect("pressed", this, "UpdateLabel", new Godot.Collections.Array {"Nuclear"});

    }

    private void UpdateTweaks(int height, int width, float mRatio)
    {
        EmitSignal("SettingsSignal", height, width, mRatio);
    }

    private void UpdateLabel(string choice)
    {
        ChoiceLabel.Text = choice;

        switch (choice)
        {
            case "Easy":
                UpdateTweaks(5,3,0.2f);
                break;
            case "Normal":
                UpdateTweaks(5,5,0.2f);
                break;
            case "Hard":
                UpdateTweaks(10,10,0.25f);
                break;
            case "Nuclear":
                UpdateTweaks(10,20,0.3f);
                break;
        }
    }

    private void GoPlay()
    {
        EmitSignal("PlaySignal");
    }

    private void GoBackToMain()
    {
        EmitSignal("BackToMainSignal");
    }



    
}
