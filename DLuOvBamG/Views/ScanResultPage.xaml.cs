using DLuOvBamG.Models;
using DLuOvBamG.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DLuOvBamG.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScanResultPage : ContentPage
	{
		private ScanResultViewModel VM;
        private List<Picture> similarPictures;
        private List<Picture> blurryPictures;
        private List<Picture> darkPictures;

        public ScanResultPage(Dictionary<ScanOptionsEnum, double> options)
		{
			Title = "Scanergebnisse";
			InitializeComponent();
			VM = BindingContext as ScanResultViewModel;
			VM.Navigation = Navigation;
			VM.Options = options;
            FillPictureLists();

            foreach(ScanOptionsEnum option in options.Keys)
            {
                ShowImageGroups(option);
            }
        }

        private void FillPictureLists()
        {
            similarPictures = new List<Picture>();
            blurryPictures = new List<Picture>();
            darkPictures = new List<Picture>();

            Picture picture1 = new Picture("https://farm5.staticflickr.com/4011/4308181244_5ac3f8239b.jpg", "1");
            
            Picture picture2 = new Picture("https://farm9.staticflickr.com/8351/8299022203_de0cb894b0.jpg", "2");
            
            Picture picture3 = new Picture("https://farm6.staticflickr.com/5117/14045101350_113edbe20b.jpg", "3");
            
            Picture picture4 = new Picture("https://farm8.staticflickr.com/7423/8729135907_79599de8d8.jpg", "4");
            
            similarPictures.Add(picture1);

            blurryPictures.Add(picture2);
            blurryPictures.Add(picture3);

            darkPictures.Add(picture4);
            darkPictures.Add(picture2);
            darkPictures.Add(picture3);
        }

        private void ShowImageGroups(ScanOptionsEnum option)
        {
            List<Picture> displayImages = GetListOfDisplayImages(option);
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


            Grid imageGrid = CreateImageGrid(displayImages);
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


            Label setsAndPics = new Label
            {
                //TODO: richtiger Text
                Text = "3 Sets, 19 Bilder"
            };
            grid.Children.Add(setsAndPics);
            Grid.SetRow(setsAndPics, 1);
            Grid.SetColumn(setsAndPics, 1);

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
                RowDefinitions = { new RowDefinition { Height = GridLength.Star }}
            };

            foreach (int dist in spaceDistribution)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(dist, GridUnitType.Star) });
            }
            
            for (int columnNumber = displayImages.Count - 1; columnNumber >= 0; --columnNumber)
            {
                Image image = new Image
                {
                    Source = displayImages[columnNumber].Uri,
                    Aspect = Aspect.AspectFill
                };
                grid.Children.Add(image);
                Grid.SetRow(image, 0);
                Grid.SetColumn(image, columnNumber);
                Grid.SetRowSpan(image, 3);
                Grid.SetColumnSpan(image, 2);

            }

            return grid;
        }

        private List<Picture> GetListOfDisplayImages(ScanOptionsEnum option)
        {
            List<Picture> displayImages = null;
            switch (option)
            {
                case ScanOptionsEnum.blurryPics:
                    displayImages = blurryPictures;
                    break;
                case ScanOptionsEnum.darkPics:
                    displayImages = darkPictures;
                    break;
                case ScanOptionsEnum.similarPics:
                    displayImages = similarPictures;
                    break;
            }

            return displayImages;
        }
	}
}