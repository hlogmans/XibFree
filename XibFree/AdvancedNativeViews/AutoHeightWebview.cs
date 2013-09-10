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
    /// WARNING: to prevent a KVO problems, set the View to null in ViewWillDisappear.
    /// </summary>
    /// 
    /// 
    public class AutoHeightWebview<T> : NativeView<T> where T : UIWebView
    {

        

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

      

        

        public new T View
        {
            get
            {
                return base.View;
            }
            set
            {
                RemoveKVOHandler(base.View);

                base.View = value;
                if (value != null) AttachKVOHandler(base.View);
            }
        }

        private LocalObjectForKVO kvohandler = new LocalObjectForKVO();
        private Boolean _handlerAttached = false;

        private Boolean ObserverAttached;

        protected void AttachKVOHandler(T view)
        {
            if (view != null)
            {
                if (!_handlerAttached) {
                    kvohandler.OnSizeUpdate += OnSizeUpdate;
                    _handlerAttached = true;
                }
                view.ScrollView.AddObserver(kvohandler, LocalObjectForKVO.KVOID, NSKeyValueObservingOptions.New, IntPtr.Zero);
                
                ObserverAttached = true;

            }
            
        }

        private void OnSizeUpdate()
        {
            LayoutParameters.Height = View.ScrollView.ContentSize.Height;
            DoVisibilityChanged();

        }

        protected void RemoveKVOHandler(T view)
        {
            if (view != null && ObserverAttached)
            {
                try
                {
                    view.InvokeOnMainThread( () => view.ScrollView.RemoveObserver(kvohandler, LocalObjectForKVO.KVOID));
                } catch {}
                
                ObserverAttached = false;

            }
        }


    }

    internal class LocalObjectForKVO : NSObject
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

    
}
