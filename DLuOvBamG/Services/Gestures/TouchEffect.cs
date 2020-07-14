using Xamarin.Forms;

/**
 from https://docs.microsoft.com/en-us/samples/xamarin/xamarin-forms-samples/effects-touchtrackingeffect/
 */
namespace DLuOvBamG.Services.Gestures
{
    public class TouchEffect : RoutingEffect
    {
        public event TouchActionEventHandler TouchAction;

        public TouchEffect() : base("XamarinDocs.TouchEffect")
        {
        }

        public bool Capture { set; get; }

        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
        }
    }
}
