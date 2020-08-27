using Xamarin.Forms;

namespace DLuOvBamG.Models
{
    class ScanOptionViewGroup
    {
        public Switch OptionSwitch { get; }
        public Expander OptionExpander { get; }
        public Slider OptionSlider { get; }
        public ScanOptionsEnum Option { get; }

        public ScanOptionViewGroup(ScanOptionsEnum option, Switch optionSwitch, Expander optionExpander, Slider optionSlider)
        {
            Option = option;
            OptionExpander = optionExpander;
            OptionSwitch = optionSwitch;
            OptionSlider = optionSlider;
        }
    }
}
