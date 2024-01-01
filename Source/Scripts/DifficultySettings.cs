using Godot;
using System;

public class DifficultySettings : Control
{

    private Label ChoiceLabel;
    private Button EasyBtn;
    private Button NormalBtn;
    private Button HardBtn;
    private Button NuclearBtn;

    private Button BackBtn;

    private TextureRect TRect;
    private Label TextLabel;

    [Signal] public delegate void BackToMainSignal();
    [Signal] public delegate void SettingsSignal(int h, int w, float r);

    public override void _Ready()
    {
        TRect = GetNode<TextureRect>("TRect");
        TextLabel = GetNode<Label>("TextLabel");

        ChoiceLabel = GetNode<Label>("PresetPanel/ChoiceLabel");
        EasyBtn = GetNode<Button>("PresetPanel/VBoxContainer/DifficultyBox/Easy");
        NormalBtn = GetNode<Button>("PresetPanel/VBoxContainer/DifficultyBox/Normal");
        HardBtn = GetNode<Button>("PresetPanel/VBoxContainer/DifficultyBox/Hard");
        NuclearBtn = GetNode<Button>("PresetPanel/VBoxContainer/DifficultyBox/Nuclear");
        BackBtn = GetNode<Button>("PresetPanel/OptionsBtns/BackBtn");
        
        EasyBtn.Connect("pressed", this, "UpdateLabel", new Godot.Collections.Array {"Easy"});
        NormalBtn.Connect("pressed", this, "UpdateLabel", new Godot.Collections.Array {"Normal"});
        HardBtn.Connect("pressed", this, "UpdateLabel", new Godot.Collections.Array {"Hard"});
        NuclearBtn.Connect("pressed", this, "UpdateLabel", new Godot.Collections.Array {"Nuclear"});

        BackBtn.Connect("pressed", this, "GoBackToMain");
        TRect.Hide();
    }

    private void UpdateTweaks(int height, int width, float mRatio)
    {
        EmitSignal("SettingsSignal", height, width, mRatio);
    }

    private void UpdateLabel(string choice)
    {
        TRect.Show();
        ChoiceLabel.Text = choice;

        switch (choice)
        {
            case "Easy":
                UpdateTweaks(5,5,0.2f);
                UpdateRect(choice);
                break;
            case "Normal":
                UpdateTweaks(7,7,0.2f);
                UpdateRect(choice);
                break;
            case "Hard":
                UpdateTweaks(8,8,0.25f);
                UpdateRect(choice);
                break;
            case "Nuclear":
                UpdateTweaks(10,10,0.4f);
                UpdateRect(choice);
                break;
        }
    }

    private void UpdateRect(string choice)
    {
        if (TRect.Texture is AtlasTexture img)
        {
            switch (choice)
            {
                case "Easy":
                    img.Region = new Rect2(0,0,128,128);
                    TextLabel.Text = "You want a quick and easy win?\nPick this!";
                    break;
                case "Normal":
                    img.Region = new Rect2(128,0,128,128);
                    TextLabel.Text = "Looking for a nice and casual game?\nTry this one!";
                    break;
                case "Hard":
                    img.Region = new Rect2(256,0,128,128);
                    TextLabel.Text = "Oh, someone's feeling brave today!\nSure you don't wanna try the easy mode?";
                    break;
                case "Nuclear":
                    img.Region = new Rect2(384,0,128,128);
                    TextLabel.Text = "Wait! What are you doing?!\nYou can't beat this!";
                    break;
            }
        }

    }

    private void GoBackToMain()
    {
        EmitSignal("BackToMainSignal");
    }   
}
