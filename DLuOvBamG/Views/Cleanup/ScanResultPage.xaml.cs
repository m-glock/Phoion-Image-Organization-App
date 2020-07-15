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

            Title = "Scan results";
            InitializeComponent();
            VM = BindingContext as ScanResultViewModel;
            VM.Navigation = Navigation;
            VM.OptionValues = optionValues;

            foreach (ScanOptionsEnum option in optionValues.Keys)
            {
                ShowImageGroups(option);
            }

            App.tf.ScanWasFinished += UpdateGrid;
            //VM.FillPictureListsTF();

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
                    new ColumnDefinition{ Width = new GridLength(41, GridUnitType.Star)},
                    new ColumnDefinition{ Width = new GridLength(4, GridUnitType.Star)},
                    new ColumnDefinition{ Width = new GridLength(5, GridUnitType.Star)},
                }
            };
            grid.Margin = new Thickness(0, 0, 0, 20);
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, option.GetNameForGalleryPage());
            grid.GestureRecognizers.Add(tapGestureRecognizer);


            BoxView bv = new BoxView
            {
                BackgroundColor = Color.AntiqueWhite,
                Opacity = 0.8
            };
            grid.Children.Add(bv);
            Grid.SetRow(bv, 0);
            Grid.SetColumn(bv, 1);
            Grid.SetColumnSpan(bv, 3);


            ActivityIndicator indicator = new ActivityIndicator();
            indicator.IsRunning = true;
            indicator.Color = Color.FromHex("#00695C");
            grid.Children.Add(indicator);
            Grid.SetRow(indicator, 0);
            Grid.SetColumn(indicator, 1);
            Grid.SetColumnSpan(indicator, 1);

            IndicatorDict.Add(option, Tuple.Create(grid, indicator));


            Label optionName = new Label
            {
                Text = option.GetTextForDisplay(),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                FontAttributes = FontAttributes.Bold
            };
            grid.Children.Add(optionName);
            Grid.SetRow(optionName, 0);
            Grid.SetColumn(optionName, 2);
            Grid.SetColumnSpan(optionName, 1);


            Frame imageFrame = new Frame
            {
                Padding = new Thickness(5, 5, 5, 5),
                BackgroundColor = Color.AntiqueWhite,
                Opacity = 0.8,
                HasShadow = false
            };

            Image arrow = new Image { Source = "arrow.png" };
            imageFrame.Content = arrow;

            grid.Children.Add(imageFrame);
            Grid.SetRow(imageFrame, 0);
            Grid.SetColumn(imageFrame, 3);




            StackLayout.Children.Add(grid);
        }


        private Grid CreateImageGrid(List<Picture> displayImages)
        {
            int[] spaceDistribution;

            //TODO: empty list? Default
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
            Console.WriteLine(e.Option.ToString() + " super fancy");
            Picture[] displayImages = App.tf.GetImagesForDisplay(e.Option);
            if (displayImages == null) { /*TODO: handle*/ }

            Tuple<Grid, ActivityIndicator> tuple = IndicatorDict[e.Option];

            tuple.Item1.Children.Remove(tuple.Item2);

            Grid imageGrid = CreateImageGrid(displayImages.ToList());
            tuple.Item1.Children.Add(imageGrid);
            Grid.SetRow(imageGrid, 0);
            Grid.SetColumn(imageGrid, 1);
            Grid.SetColumnSpan(imageGrid, 1);

            int setAmount = App.tf.GetAmountOfSetsForOption(e.Option);
            int pictureAmount = App.tf.GetAmountOfPicturesForOption(e.Option);
            Label setsAndPics = new Label
            {
                Text = setAmount + " Sets, " + pictureAmount + " Images"
            };
            tuple.Item1.Children.Add(setsAndPics);
            Grid.SetRow(setsAndPics, 1);
            Grid.SetColumn(setsAndPics, 1);
        }

    }
}