using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Layouts;

namespace WhackAMoleAssignment;

public partial class MainPage : ContentPage
{
    Random rand;
    private int _score;
    private float _timer;
    private float move;

	public MainPage()
	{
		InitializeComponent();
        rand = new Random();
	}

   //timer function
    private void timer()
    {
        _timer = 15f;
        move = 1f;
        //main timer, counts down from 15 and stops at 0. Calls game over function when timer is 0
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(100), () =>  
        {
            _timer -= 0.1f;
            countDownlbl.Text = "Time Remainging: " + _timer.ToString("0.0");
            if (_timer <= 0)
            {
                gameOver();
                return false;
            }
            return true;
        });

        //timer for the mole to move on its own if not clicked in time every 1s
        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(100), () =>
        {
            move -= 0.1f;
            if (move <= 0)
            {
                move = 1f;
                MoveTheMole();
            }

            if (_timer <= 0)
            {
                return false;
            }
            return true;
        });
    }

    //clears the board once the timer is up
    private void gameOver()
    {
        MoleImgBtn4.IsVisible = false;
        MoleImgBtn4.IsEnabled=false;

        MoleImgBtn5.IsEnabled=false;
        MoleImgBtn5.IsVisible = false;

        buttons.IsVisible = false;
        buttons.IsEnabled = false;

        Grids.IsVisible = false;    
        Grids.IsEnabled = false;
        countDownlbl.Text = "Time Remainging: 0.0";

        resetBtn.IsEnabled = true;
        resetBtn.IsVisible = true;
    }

    //button for switching between the 4x4 grid and 5x5 grid
    private void grdSwitchBtn_Clicked(object sender, EventArgs e)
    {
        switch (gridSwitchBtn.Text)
        {
            //if text says 4x4 then it changes the grid to the 4x4 grid
            case "4x4":
                {
                    Grid5.IsVisible = false;
                    Grid5.IsEnabled = false;
                    Grid4.IsVisible = true;
                    Grid4.IsEnabled = true;
                    gridSwitchBtn.Text = "5x5";
                    break;
                }
            //if text says 5x5 then it changes the grid to the 5x5 grid
            case "5x5":
                {
                    Grid5.IsVisible = true;
                    Grid5.IsEnabled = true;
                    Grid4.IsVisible = false;
                    Grid4.IsEnabled = false;
                    gridSwitchBtn.Text = "4x4";
                    break;
                }
            default:
                break;
        }
    }

    //function for turning off the mole when game is not running/over
    private void ResetMole()
    { 
        MoleImgBtn4.IsVisible = false;
        MoleImgBtn4.IsEnabled = false;

        MoleImgBtn5.IsVisible = false;
        MoleImgBtn5.IsEnabled = false;
    }

    //moves the mole
    private void MoveTheMole()
    {                                                       //one line if
        Grid grid = (Grid)FindByName(gridSwitchBtn.Text == "4x4" ? "Grid5" : "Grid4");

        int r1 = 0, c1 = 0;
        r1 = rand.Next(0, grid.RowDefinitions.Count);
        c1 = rand.Next(0, grid.ColumnDefinitions.Count);

        //debugging
        //Trace.WriteLine(grid.RowDefinitions.Count.ToString() + " " + grid.ColumnDefinitions.Count.ToString());

        MoleImgBtn4.SetValue(Grid.RowProperty, r1);
        MoleImgBtn4.SetValue(Grid.ColumnProperty, c1);
        MoleImgBtn4.IsVisible = true;
        MoleImgBtn4.IsEnabled = true;

        MoleImgBtn5.SetValue(Grid.RowProperty, r1);
        MoleImgBtn5.SetValue(Grid.ColumnProperty, c1);
        MoleImgBtn5.IsVisible = true;
        MoleImgBtn5.IsEnabled = true;
    }

    //main play button when app is launched
    private void PlayBtn_clicked(object sender, EventArgs e)
    {
        game.IsEnabled = true;
        game.IsVisible = true;
        mainMenu.IsEnabled = false;
        mainMenu.IsVisible = false;
        ResetMole();
    }

    //starts the game, resets the score and starts the timer
    //moves the mole each time 
    private void startBtn_Clicked(object sender, EventArgs e)
    {
        _score = 0;
        playerScoreLbl.Text = "Your Score: " + _score.ToString();
        timer();
        MoveTheMole();

        buttons.IsVisible = false;
        buttons.IsEnabled = false;
    }

    //adds to the score whenever the mole is clicked
    private void MoleImgBtn_Clicked(object sender, EventArgs e)
    {
        MoveTheMole();
        _score++;
        move = 1f;
        playerScoreLbl.Text = "Your Score: " + _score.ToString();
    }

    //button to return to the main menu
    private void rtrnMenuBtn_Clicked(object sender, EventArgs e)
    {
        game.IsEnabled = false;
        game.IsVisible = false;
        mainMenu.IsEnabled = true;
        mainMenu.IsVisible = true;
        ResetMole();
    }

    //play again button, returns to choice menu
    private void resetBtn_Clicked(object sender, EventArgs e)
    {
        MoleImgBtn4.IsVisible = true;
        MoleImgBtn4.IsEnabled = true;

        MoleImgBtn5.IsEnabled = true;
        MoleImgBtn5.IsVisible = true;

        buttons.IsVisible = true;
        buttons.IsEnabled = true;

        Grids.IsVisible = true;
        Grids.IsEnabled = true;  
        
        resetBtn.IsEnabled = false;
        resetBtn.IsVisible = false;
        ResetMole();
    }
}

