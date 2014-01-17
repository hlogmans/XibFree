using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;

namespace XibFree.AdvancedNativeViews
{
    public class ToggleButton : NativeView<UIButton>
    {
        public String _imagenameForPressed { get; private set; }
        public String _imagenameForUnpressed { get; private set; }

        public ToggleButton(String imageNamePressed, String imageNameNormal, float width = AutoSize.WrapContent, float height = AutoSize.WrapContent)
            : base()
        {
            _imagenameForPressed = imageNamePressed;
            _imagenameForUnpressed = imageNameNormal;

            var button = new UIButton();
            button.SetImage(new UIImage(imageNameNormal), UIControlState.Normal);
			button.ContentMode = UIViewContentMode.Center;
            base.LayoutParameters = new LayoutParameters(width, height);
            View = button;

            View.TouchUpInside += ViewOnTouchUpInside;
            RaisePropertyChanged();
        }

        private void ViewOnTouchUpInside(object sender, EventArgs eventArgs)
        {
            Value = !Value;
            ValueChanged();
        }

        public UIColor BackgroundColor {get { return View.BackgroundColor; } set { View.BackgroundColor = value; }}

        private Boolean _value;

        public Boolean Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }

        public event Action ValueChanged;

        protected void CallValueChanged()
        {
            if (ValueChanged != null) ValueChanged();
        }

        protected void RaisePropertyChanged()
        {
            var img = new UIImage(Value ? _imagenameForPressed : _imagenameForUnpressed);
            View.SetImage(img, UIControlState.Normal);
        }

        public void SetBackgroundImage(UIImage image)
        {
            View.SetBackgroundImage(image, UIControlState.Normal);
        }
    }
}
