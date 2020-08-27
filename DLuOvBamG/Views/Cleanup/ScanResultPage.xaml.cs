using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanResultPage : ContentPage
    {
        private ScanResultViewModel VM;
        private Dictionary<ScanOptionsEnum, Tuple<Grid, ActivityIndicator>> IndicatorDict;

        public ScanResultPage(Dictionary<ScanOptionsEnum, double> optionValues)
        {
            IndicatorDict = new Dictionary<ScanOptionsEnum, Tuple<Grid, ActivityIndicator>>();

            // initialize page content and ViewModel
            Title = "Scan results";
            InitializeComponent();
            VM = new ScanResultViewModel();
            VM.Navigation = Navigation;
            VM.OptionValues = optionValues;
            BindingContext = VM;

            // show grid with images and label for each chosen scan option
            foreach (ScanOptionsEnum option in optionValues.Keys)
            {
                ShowImageGroups(option);
            }

            // subscribe to the event that is thrown when the scan is finished
            App.tf.ScanWasFinished += UpdateGrid;
        }

        private void ShowImageGroups(ScanOptionsEnum option)
        {
            Grid grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(90) },
                    new RowDefinition { Height = GridLength.Auto }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition{ Width = new GridLength(5, GridUnitType.Star)},
                    new ColumnDefinition{ Width = new GridLength(45, GridUnitType.Star)},
                    new ColumnDefinition{ Width = new GridLength(45, GridUnitType.Star)},
                    new ColumnDefinition{ Width = new GridLength(5, GridUnitType.Star)},
                }
            };
            grid.Margin = new Thickness(0, 0, 0, 20);
            
            // slightly transparent box view
            BoxView bv = new BoxView
            {
                 Opacity = 0.8
            };
            grid.Children.Add(bv);
            Grid.SetRow(bv, 0);
            Grid.SetColumn(bv, 1);
            Grid.SetColumnSpan(bv, 2);

            // loading spinner that is shown until scan for this option is finished
            ActivityIndicator indicator = new ActivityIndicator();
            indicator.IsRunning = true;
            indicator.Color = Color.FromHex("#FFFFFF");
            grid.Children.Add(indicator);
            Grid.SetRow(indicator, 0);
            Grid.SetColumn(indicator, 1);
            Grid.SetColumnSpan(indicator, 1);
            IndicatorDict.Add(option, Tuple.Create(grid, indicator));

            // labe that displays the option name
            Label optionName = new Label
            {
                Text = option.GetTextForDisplay(),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 0, 20, 0)
            };
            grid.Children.Add(optionName);
            Grid.SetRow(optionName, 0);
            Grid.SetColumn(optionName, 2);
            Grid.SetColumnSpan(optionName, 1);

            StackLayout.Children.Add(grid);
        }

        private Grid CreateImageGrid(List<Picture> displayImages)
        {
            int[] spaceDistribution;

            // find correct grid row and column sizes depending on how many sample pictures are shown
            switch (displayImages.Count)
            {
                case 1:
                    spaceDistribution = new int[] { 100 };
                    break;
                case 2:
                    spaceDistribution = new int[] { 34, 33, 33 };
                    break;
                case 3:
                    spaceDistribution = new int[] { 25, 25, 25, 25 };
                    break;
                default:
                    spaceDistribution = new int[] { };
                    break;
            }

            Grid grid = new Grid
            {
                RowDefinitions = { new RowDefinition { Height = GridLength.Star } }
            };

            foreach (int dist in spaceDistribution)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(dist, GridUnitType.Star) });
            }

            // display sample images for this option
            for (int columnNumber = displayImages.Count - 1; columnNumber >= 0; --columnNumber)
            {
                Frame frame = new Frame();
                frame.Padding = new Thickness(2);

                Image image = new Image
                {
                    Source = displayImages[columnNumber].Uri,
                    Aspect = Aspect.AspectFill
                };

                frame.Content = image;


                grid.Children.Add(frame);
                Grid.SetRow(frame, 0);
                Grid.SetColumn(frame, columnNumber);
                Grid.SetRowSpan(frame, 3);
                Grid.SetColumnSpan(frame, 2);

            }

            return grid;
        }

        public void UpdateGrid(object sender, ScanEventArgs e)
        {
            if (VM.OpenedNewPage) return;
            Console.WriteLine("tapped");
            // when scan is finished, get all pictures from the TensorFlowExecutor
            Picture[] displayImages = App.tf.GetImagesForDisplay(e.Option);
            if (displayImages == null) displayImages = new Picture[] { };
            if (!IndicatorDict.ContainsKey(e.Option)) return;
            Tuple<Grid, ActivityIndicator> tuple = IndicatorDict[e.Option];

            // remove the activity indicator from the grid
            tuple.Item1.Children.Remove(tuple.Item2);

            // create image grid with sample pictures
            Grid imageGrid = CreateImageGrid(displayImages.ToList());
            tuple.Item1.Children.Add(imageGrid);
            Grid.SetRow(imageGrid, 0);
            Grid.SetColumn(imageGrid, 1);
            Grid.SetColumnSpan(imageGrid, 1);

            // display amount if sets and pictures for the option
            int setAmount = App.tf.GetAmountOfSetsForOption(e.Option);
            int pictureAmount = App.tf.GetAmountOfPicturesForOption(e.Option);
            Label setsAndPics = new Label
            {
                Text = setAmount + " Sets, " + pictureAmount + " Images"
            };
            setsAndPics.TextColor = Color.Black;
            tuple.Item1.Children.Add(setsAndPics);
            Grid.SetRow(setsAndPics, 1);
            Grid.SetColumn(setsAndPics, 1);

            // add click event for grid to get to scan option page
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer()
            {
                Command = VM.OpenScanOptionDisplayPage,
                CommandParameter = e.Option,
            };
            Console.WriteLine("event finished");
            tuple.Item1.GestureRecognizers.Add(tapGestureRecognizer);
        }

    }
}