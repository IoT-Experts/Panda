
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UnityPlayer;
using Microsoft.WindowsAzure.MobileServices;
using System;
using Windows.ApplicationModel;
using Windows.UI.ViewManagement;
using System.Reflection;
using Windows.UI.Core;
using System.Threading.Tasks;
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Panda_Fruit_Farm
{
    sealed partial class App : Application
    {
        private AppCallbacks appCallbacks;
        public SplashScreen splashScreen;
        public static MobileServiceClient MobileService = new MobileServiceClient("https://jetsoftgame.azure-mobile.net/", "afaWTcNSVpvjIOHmWpDazXgLsiTmcM51");
        public App()
        {
            this.InitializeComponent();
            appCallbacks = new AppCallbacks();
          //  TryEnterFullScreenMode();


        }
        public static async  void TryEnterFullScreenMode()
        {
            Task.Delay(2000);
            try
            {
                var view = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView();
                MethodInfo methodInfo = view.GetType().GetTypeInfo().GetDeclaredMethod("TryEnterFullScreenMode");
                if (methodInfo != null)
                {
                    methodInfo.Invoke(view, null);
                }
            }
            catch (Exception)
            { 
            }
               
            
        }
        protected override void OnActivated(IActivatedEventArgs args)
        {
            string appArgs = "";

            switch (args.Kind)
            {
                case ActivationKind.Protocol:
                    ProtocolActivatedEventArgs eventArgs = args as ProtocolActivatedEventArgs;
                    splashScreen = eventArgs.SplashScreen;
                    appArgs += string.Format("Uri={0}", eventArgs.Uri.AbsoluteUri);
                    break;
            }
            InitializeUnity(appArgs);
        }

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            string appArgs = "";

            splashScreen = args.SplashScreen;
            appArgs += "File=";
            bool firstFileAdded = false;
            foreach (var file in args.Files)
            {
                if (firstFileAdded) appArgs += ";";
                appArgs += file.Path;
                firstFileAdded = true;
            }

            InitializeUnity(appArgs);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.CacheSize = 1;
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    //throw new Exception("Failed to create initial page");
                }
            }
            Window.Current.Activate();
            splashScreen = args.SplashScreen;
            InitializeUnity(args.Arguments);
        }

        private void InitializeUnity(string args)
        {
#if UNITY_WP_8_1 || UNITY_UWP
			ApplicationView.GetForCurrentView().SuppressSystemOverlays = true;
#if UNITY_UWP
			if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
#endif
#pragma warning disable 4014
			{
				StatusBar.GetForCurrentView().HideAsync();
			}
#pragma warning restore 4014
#endif
            appCallbacks.SetAppArguments(args);
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null && !appCallbacks.IsInitialized())
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
                Window.Current.Activate();

                rootFrame.Navigate(typeof(MainPage));
            }

            Window.Current.Activate();
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            deferral.Complete();
        }
    }
}
