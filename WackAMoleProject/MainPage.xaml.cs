using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace WackAMoleProject;

public partial class MainPage : ContentPage
{
    Random _random;
    System.Timers.Timer _timer;
    int _MyInterval = 500;
    int _CountdownStart = 40;
    int time;
    int miss;

    public MainPage()
    {
        InitializeComponent();
        _random = new Random();
        SetUpTimers();
    }
    private void BtnChangeGrid_Clicked(object sender, EventArgs e)
    {
        switch (BtnChangeGrid.Text)
        {
            //Displays 5 x 5 grid
            case "5 x 5":
                {
                    GridGame3x3.IsVisible = false;
                    GridGame5x5.IsVisible = true;
                    BtnChangeGrid.Text = "3 x 3";
                    break;
                }
            //Displays 3 x 3 grid
            case "3 x 3":
                {
                    GridGame3x3.IsVisible = true;
                    GridGame5x5.IsVisible = false;
                    BtnChangeGrid.Text = "5 x 5";
                    break;
                }
        }
    }
    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        int score = Convert.ToInt32(LblScore.Text);

        //Increments the score
        score += 10;
        LblScore.Text = score.ToString();

        //Moves the mole
        MoveTheMole();
        //Checks the score and time
        CheckTimeAndScore();
    }
    private void MoveTheMole()
    {
        int MaxRow, MaxCol;
        ImageButton currentImageButton;

        int score = Convert.ToInt32(LblScore.Text);

        //takes time each time mole is moved
        time = Convert.ToInt32(LblTimer.Text);

        if (GridGame3x3.IsVisible == true)
        {
            MaxRow = MaxCol = 3;
            currentImageButton = mole3;
        }
        else
        {
            MaxRow = MaxCol = 5;
            currentImageButton = mole5;
        }
        //generates random numbers for rows and cols
        int r = _random.Next(0, MaxRow);
        int c = _random.Next(0, MaxCol);

        //sets the values of rows and cols
        currentImageButton.SetValue(Grid.RowProperty, r);
        currentImageButton.SetValue(Grid.ColumnProperty, c);
        
    }
    private void CheckTimeAndScore()
    {
        int score = Convert.ToInt32(LblScore.Text);
        int counter = Convert.ToInt32(LblTimer.Text);

        //Checks the time, the score, and the number of misses after each timer update
        if (score >= 200 || counter < 1 || miss == 10)
        {
            //Display Game Over elements
            BtnChangeGrid.IsVisible = false;
            mole3.IsVisible = false;
            mole5.IsVisible = false;
            LblTimer.IsVisible = false;

            GameOver.IsVisible = true;
            BtnRestart.IsVisible = true;

            //Displays Victory Image if score is bigger than 120
            if (score > 120 && miss < 10)
            {   
                VictoryImg.IsVisible = true;
            }   
        }              
    }
    private void BtnRestart_Clicked(object sender, EventArgs e)
    {
        int counter = Convert.ToInt32(LblTimer.Text);
        int score = Convert.ToInt32(LblScore.Text);

        if (counter != 0 && score < 200 && miss < 10)
        {
            //Display "Do you want to countinue" stack
            ContinueStack.IsVisible = true;

            BtnRestart.IsVisible = false;
            mole3.IsVisible = false;
            mole5.IsVisible = false;
        }
        else if (counter == 0 || score >= 200 || miss == 10)
        {
            //Brings user to the start
            ContinueStack.IsVisible = false;
            BtnRestart.IsVisible = false;
            LblScore.IsVisible = false;
            ScoreImg.IsVisible = false;
            LblTimer.IsVisible = false;
            LblMiss.IsVisible = false;  
            MainStack.IsVisible = false;
            GameOver.IsVisible = false;
            VictoryImg.IsVisible = false;
            WarningImg.IsVisible = false;

            BtnChangeGrid.IsVisible = true;            
            BtnStart.IsVisible = true;
            ResetTimers();
        }
    }
    private void SetUpTimers()
    {
        //Initialization of timer
        _timer = new System.Timers.Timer();
        _timer.Interval = _MyInterval;

        _timer.Elapsed += _timer_Elapsed;

        LblTimer.Text = _CountdownStart.ToString();
    }
    private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        // This method is on the timer thread
        Dispatcher.Dispatch(
            () => { // code goes here
                    // call another method to update the UI 
                UpdateTimer();
            }
            );
    }
    private void UpdateTimer()
    {
        int counter = Convert.ToInt32(LblTimer.Text);
        int score = Convert.ToInt32(LblScore.Text);

        //timer decrement
        counter--;
        LblTimer.Text = counter.ToString();

        if (counter < 6)
        {
            LblTimer.TextColor = Colors.Red;
        }
        if (counter == 0 || score == 200 || miss == 10)
        {
            _timer.Stop();
            CheckTimeAndScore();
            ContinueStack.IsVisible = false;
        }
      
        if (counter == time - 2)
        {   
            //after first click move the mole every 2 seconds
            MoveTheMole();

            //increment number of misses
            miss++;
            LblMiss.Text = miss.ToString();
        }
    }
    private void ResetTimers()
    {
        LblTimer.Text = _CountdownStart.ToString();
        LblTimer.TextColor = Colors.Black;
    }
    private void BtnStart_Clicked(object sender, EventArgs e)
    {
        //reset timer
        ResetTimers();
        //start timer
        _timer.Start();
        //move the mole
        MoveTheMole();

        //visability
        MainStack.IsVisible = true;
        BtnRestart.IsVisible = true;
        LblScore.IsVisible = true;
        mole3.IsVisible = true;
        mole5.IsVisible = true;
        ScoreImg.IsVisible = true;
        LblTimer.IsVisible = true;
        LblMiss.IsVisible = true;
        WarningImg.IsVisible = true;

        BtnStart.IsVisible = false;
        BtnChangeGrid.IsVisible = false;

        //Sets number of misses on 0
        miss = 0;
        //Reset score and number of misses
        LblScore.Text = "0";
        LblMiss.Text = "0";
    }
    private void BtnYes_Clicked(object sender, EventArgs e)
    {
        //visible elements if Yes
        ContinueStack.IsVisible = false;
        BtnRestart.IsVisible = true;
        mole3.IsVisible = true;
        mole5.IsVisible = true;
    }
    private void BtnNo_Clicked(object sender, EventArgs e)
    {
        //visible elements if No
        ContinueStack.IsVisible = false;
        LblScore.IsVisible = false;
        LblTimer.IsVisible = false;
        ScoreImg.IsVisible = false;
        MainStack.IsVisible = false;
        LblMiss.IsVisible = false;
        WarningImg.IsVisible = false;

        BtnStart.IsVisible = true;
        BtnChangeGrid.IsVisible = true;

        //stop the timer
        _timer.Stop();
    }
}



