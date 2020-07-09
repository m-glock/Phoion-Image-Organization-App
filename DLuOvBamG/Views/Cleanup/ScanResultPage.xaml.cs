using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
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

        public ScanResultPage(Dictionary<ScanOptionsEnum, double> optionValues)
		{
			Title = "Scanergebnisse";
			InitializeComponent();
			VM = BindingContext as ScanResultViewModel;
			VM.Navigation = Navigation;
			VM.OptionValues = optionValues;
            VM.FillPictureListsTF();


            foreach(ScanOptionsEnum option in optionValues.Keys)
            {
                ShowImageGroups(option);
            }
        }

        private void ShowImageGroups(ScanOptionsEnum option)
        {
            Picture[] displayImages = App.tf.GetImagesForDisplay(option);
            if (displayImages == null) { /*TODO: handle*/ }
            
           
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
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer()
            {
                Command = VM.OpenScanOptionDisplayPage,
                CommandParameter = option,
            };
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


            Grid imageGrid = CreateImageGrid(displayImages.ToList());
            grid.Children.Add(imageGrid);
            Grid.SetRow(imageGrid, 0);
            Grid.SetColumn(imageGrid, 1);
            Grid.SetColumnSpan(imageGrid, 1);

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

            Image arrow = new Image { Source = "arrow.png"};
            imageFrame.Content = arrow;

            grid.Children.Add(imageFrame);
            Grid.SetRow(imageFrame, 0);
            Grid.SetColumn(imageFrame, 3);


            int setAmount = App.tf.GetAmountOfSetsForOption(option);
            int pictureAmount = App.tf.GetAmountOfPicturesForOption(option);

            Label setsAndPics = new Label();
            if (pictureAmount > 0)
            {
                setsAndPics.Text = setAmount + " sets, " + pictureAmount + " pictures";
            } else
            {
                setsAndPics.Text = "No pictures found";
            }


            grid.Children.Add(setsAndPics);
            Grid.SetRow(setsAndPics, 1);
            Grid.SetColumn(setsAndPics, 1);

            StackLayout.Children.Add(grid);
        }


        private Grid CreateImageGrid(List<Picture> displayImages)
        {
            int[] spaceDistribution;

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
                RowDefinitions = { new RowDefinition { Height = GridLength.Star }}
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
	}
}