using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace XibFree.AdvancedNativeViews
{
    /// <summary>
    /// The attached Webview class will be autogrowing in height for the set width.
    /// This class notifies the nativeView of height changes.
    /// </summary>
    public class AutoHeightWebview<T> : NativeView<T> where T : UIWebView
    {

        private class LocalObjectForKVO  : NSObject
        {
            public static NSString KVOID = new NSString("contentSize");
            public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
            {
                if (keyPath == KVOID) DoOnSizeUpdate();
            }

            private void DoOnSizeUpdate()
            {
                if (OnSizeUpdate != null) OnSizeUpdate();
            }
            internal Action OnSizeUpdate;
        }

        public AutoHeightWebview()
        {
            Initialize();
        }

        public AutoHeightWebview(T view, LayoutParameters lp)
            : base(view, lp)
        {
            Initialize();
            AttachKVOHandler(view);
        }

        private void Initialize()
        {
            AutoLayout = true;
        }

        public T View
        {
            get
            {
                return base.View;
            }
            set
            {
                RemoveKVOHandler(base.View);
                base.View = value;
                AttachKVOHandler(base.View);
            }
        }

        private LocalObjectForKVO kvohandler = new LocalObjectForKVO();
        private Boolean _handlerAttached = false;

        protected void AttachKVOHandler(T view)
        {
            if (view != null)
            {
                if (!_handlerAttached) {
                    kvohandler.OnSizeUpdate += OnSizeUpdate;
                    _handlerAttached = true;
                }
                view.ScrollView.AddObserver(kvohandler, LocalObjectForKVO.KVOID, NSKeyValueObservingOptions.New, IntPtr.Zero);

            }
            
        }

        private void OnSizeUpdate()
        {
            LayoutParameters.Height = View.ScrollView.ContentSize.Height;
            DoVisibilityChanged();

        }

        protected void RemoveKVOHandler(T view)
        {
            if (view != null)
            {
                view.ScrollView.RemoveObserver(kvohandler, LocalObjectForKVO.KVOID);
            }
        }


    }
}
