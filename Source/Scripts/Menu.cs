using Godot;
using System;

public class Menu : Control
{
    private Button PlayBtn;
    private Button SettingsBtn;
    private Button CreditsBtn;

    private Panel MenuPanel;
    private Level Level;
    private DifficultySettings Dset;

    private int boardHeight;
    private int boardWidth;
    private float mineRatio;

    public override void _Ready()
    {

        boardHeight = 5;
        boardWidth = 5;
        mineRatio = 0.2f;
        
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
        Level.SetLevel(boardHeight, boardWidth, mineRatio);
    }

    private void TogglePanels()
    {
        MenuPanel.Visible = !MenuPanel.Visible;
        Dset.Visible = !Dset.Visible;
    }

}
