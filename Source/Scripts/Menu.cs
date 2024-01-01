using Godot;
using System;

public class Menu : Control
{
    private Button PlayBtn;
    private Button SettingsBtn;
    private Button CreditsBtn;
    private Button EscapeCreditsBtn;
    private VBoxContainer BtnBox;

    private Panel MenuPanel;
    private Level Level;
    private DifficultySettings Dset;
    private Label CreditsLabel;

    private int boardHeight;
    private int boardWidth;
    private float mineRatio;

    public override void _Ready()
    {

        boardHeight = 5;
        boardWidth = 5;
        mineRatio = 0.2f;

        BtnBox = GetNode<VBoxContainer>("Panel/ButtonsBox");
        EscapeCreditsBtn = GetNode<Button>("EscapeCredits");

        CreditsLabel = GetNode<Label>("CreditsLabel");
        Dset = GetNode<DifficultySettings>("DifficultySettings");
        MenuPanel = GetNode<Panel>("Panel");
        Level = GetNode<Level>("Level");
        PlayBtn = GetNode<Button>("Panel/ButtonsBox/PlayBtn");
        SettingsBtn = GetNode<Button>("Panel/ButtonsBox/SettingsBtn");
        CreditsBtn = GetNode<Button>("Panel/ButtonsBox/CreditsBtn");

        PlayBtn.Connect("pressed", this, "PlayGame");
        SettingsBtn.Connect("pressed", this, "TogglePanels");
        Dset.Connect("BackToMainSignal", this, "TogglePanels");
        Dset.Connect("SettingsSignal", this, "UpdateBoardSize");
        CreditsBtn.Connect("pressed", this, "ToggleCredits");
        EscapeCreditsBtn.Connect("pressed", this, "ToggleCredits");
    }

    private void ToggleCredits()
    {
        CreditsLabel.Visible = !CreditsLabel.Visible;
        BtnBox.Visible = !BtnBox.Visible;
        EscapeCreditsBtn.Visible = !EscapeCreditsBtn.Visible;
    }

    private void UpdateBoardSize(int height, int width, float ratio)
    {
        boardHeight = height;
        boardWidth = width;
        mineRatio = ratio;
    }

    private void PlayGame()
    {
        MenuPanel.Hide();
        Level.Show();
        Level.StartClock();
        Level.SetLevel(boardHeight, boardWidth, mineRatio);
    }

    private void TogglePanels()
    {
        MenuPanel.Visible = !MenuPanel.Visible;
        Dset.Visible = !Dset.Visible;
    }

}
