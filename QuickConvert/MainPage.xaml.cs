﻿namespace QuickConvert
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            MainViewModel viewModel = new ();
            BindingContext = viewModel;
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}