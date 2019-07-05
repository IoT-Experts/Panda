using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.Windows;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using UnityPlayer;
using Microsoft.WindowsAzure.MobileServices;
using Windows.Security.Authentication.Web;
using System.Threading.Tasks;
using Facebook;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.ApplicationModel.DataTransfer;
using Microsoft.Advertising.WinRT.UI;
using Windows.UI.ViewManagement;
using Windows.Graphics.Display;

namespace Panda_Fruit_Farm
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WinRTBridge.WinRTBridge _bridge;
        private SplashScreen splash;
        private Rect splashImageRect;
        private WindowSizeChangedEventHandler onResizeHandler;
        InterstitialAd MyVideoAd = new InterstitialAd();
        string MyAppId = "5b17e07e-fd7a-46a8-9f37-35188cbe64e9";
        string MyAdUnitId = "11615668";
        private DispatcherTimer dispatcherTimer;
        //Add them 
        private MobileServiceCollection<pandafruitfarm, pandafruitfarm> items;
        private IMobileServiceTable<pandafruitfarm> Pandafruitfarm = App.MobileService.GetTable<pandafruitfarm>();
        public string AccessToken;
        public DateTime TokenExpiry;
        public string ClientId = "889744884476873";

        public MainPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;

            AppCallbacks appCallbacks = AppCallbacks.Instance;
            // Setup scripting bridge
            _bridge = new WinRTBridge.WinRTBridge();
            appCallbacks.SetBridge(_bridge);
            timer();
            hidebanner();
            appCallbacks.RenderingStarted += () => { RemoveSplashScreen(); };           
            ManagerDelegate.LoginFacebook += LoginFacebook;
            ManagerDelegate.ShowData += QueryData;
            ManagerDelegate.QueryIdFb += Query_IdFB;
            //  ManagerDelegate.ShareFB += ShareFB;
           

#if WINDOWS10

#endif

#if !UNITY_WP_8_1
            appCallbacks.SetKeyboardTriggerControl(this);
#endif
            appCallbacks.SetSwapChainPanel(GetSwapChainPanel());
            appCallbacks.SetCoreWindowEvents(Window.Current.CoreWindow);
            appCallbacks.InitializeD3DXAML();

            splash = ((App)App.Current).splashScreen;
            GetSplashBackgroundColor();
            OnResize();
            onResizeHandler = new WindowSizeChangedEventHandler((o, e) => OnResize());
            Window.Current.SizeChanged += onResizeHandler;

#if UNITY_WP_8_1
			SetupLocationService();
#endif
            try
            {
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
                App.TryEnterFullScreenMode();
            }
            catch (Exception)
            { 
            }
            
        }
       
        void timer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }
       async void hidebanner()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                admediator.Visibility = Visibility.Collapsed;
                adduplexcontrols.Visibility = Visibility.Collapsed;
            });
        }
        private void DispatcherTimer_Tick(object sender, object e)
        {
            if (admodads.viewadsvideo == true)
            {
                MyVideoAd.AdReady += MyVideoAd_AdReady;
                //MyVideoAd.ErrorOccurred += MyVideoAd_ErrorOccurred;
                MyVideoAd.Completed += MyVideoAd_Completed;
                //MyVideoAd.Cancelled += MyVideoAd_Cancelled;


                // pre-fetch an ad 30-60 seconds before you need it 

                MyVideoAd.RequestAd(AdType.Video, MyAppId, MyAdUnitId);
                admodads.viewadsvideo = false;
            }
            if (ChooseLevel.bannercontrol==true)
            {
                admediator.Visibility = Visibility.Visible;
                adduplexcontrols.Visibility = Visibility.Visible;
            }
            else
            {
                admediator.Visibility = Visibility.Collapsed;
                adduplexcontrols.Visibility = Visibility.Collapsed;
            }
            if (GP_ControllerButtonLayer.sharebuton==true)
            {
                GP_ControllerButtonLayer.sharebuton = false;
                ShareFB();
            }

        }

        private void MyVideoAd_Completed(object sender, object e)
        {
            admodads.congtrathuong = true;
        }

        private void MyVideoAd_AdReady(object sender, object e)
        {
            if ((InterstitialAdState.Ready) == (MyVideoAd.State))
            {
                MyVideoAd.Show();
            }
        }



        #region WinStore
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //splash = (SplashScreen)e.Parameter;
            OnResize();
        }

        private void OnResize()
        {
            if (splash != null)
            {
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }
        }

        private void PositionImage()
        {
            var inverseScaleX = 1.0f;
            var inverseScaleY = 1.0f;
#if UNITY_WP_8_1
			inverseScaleX = inverseScaleX / DXSwapChainPanel.CompositionScaleX;
			inverseScaleY = inverseScaleY / DXSwapChainPanel.CompositionScaleY;
#endif

            ExtendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X * inverseScaleX);
            ExtendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y * inverseScaleY);
            ExtendedSplashImage.Height = splashImageRect.Height * inverseScaleY;
            ExtendedSplashImage.Width = splashImageRect.Width * inverseScaleX;
        }

        private async void GetSplashBackgroundColor()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///AppxManifest.xml"));
                string manifest = await FileIO.ReadTextAsync(file);
                int idx = manifest.IndexOf("SplashScreen");
                manifest = manifest.Substring(idx);
                idx = manifest.IndexOf("BackgroundColor");
                if (idx < 0)  // background is optional
                    return;
                manifest = manifest.Substring(idx);
                idx = manifest.IndexOf("\"");
                manifest = manifest.Substring(idx + 1);
                idx = manifest.IndexOf("\"");
                manifest = manifest.Substring(0, idx);
                int value = 0;
                bool transparent = false;
                if (manifest.Equals("transparent"))
                    transparent = true;
                else if (manifest[0] == '#') // color value starts with #
                    value = Convert.ToInt32(manifest, 16) & 0x00FFFFFF;
                else
                    return; // at this point the value is 'red', 'blue' or similar, Unity does not set such, so it's up to user to fix here as well
                byte r = (byte)(value >> 16);
                byte g = (byte)((value & 0x0000FF00) >> 8);
                byte b = (byte)(value & 0x000000FF);

                await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.High, delegate ()
                {
                    byte a = (byte)(transparent ? 0x00 : 0xFF);
                    ExtendedSplashGrid.Background = new SolidColorBrush(Color.FromArgb(a, r, g, b));
                });
            }
            catch (Exception)
            { }
        }

        public SwapChainPanel GetSwapChainPanel()
        {
            return DXSwapChainPanel;
        }

        public void RemoveSplashScreen()
        {
            DXSwapChainPanel.Children.Remove(ExtendedSplashGrid);
            if (onResizeHandler != null)
            {
                Window.Current.SizeChanged -= onResizeHandler;
                onResizeHandler = null;
            }
        }
        #endregion

        #region Facebook
        private async void LoginFacebook()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Login();
            });

        }
        private async Task Login()
        {
            var scope = "public_profile, email, publish_actions, user_friends";
            var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ClientId,
                redirect_uri = redirectUri,
                response_type = "token",
                scope = scope
            });
            Uri startUri = loginUrl;
            Uri endUri = new Uri(redirectUri, UriKind.Absolute);


            //#if WINDOWS_APP
            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);
            await ParseAuthenticationResult(result);
            //#endif
            var postParams = new
            {
                name = "This is best game on IOS, Android, Windows.",
                caption = "Download and play now",
                link = "https://apps.facebook.com/889744884476873",
                description = "Great games",
                picture = "https://fbcdn-photos-c-a.akamaihd.net/hphotos-ak-xta1/v/t1.0-0/s480x480/13087472_701657546644106_270901025500941675_n.jpg?oh=ab7134b258b44fc260cc4af65198fbde&oe=579BBED2&__gda__=1471625511_f48a8b7807ab45493a31b3ecf3e5c236"
            };
            try
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    FacebookClient client = new FacebookClient(AccessToken);
                    dynamic fbPostTaskResult = client.PostTaskAsync("/me/feed", postParams);
                    var responseresult = (IDictionary<string, object>)fbPostTaskResult;

                });
                admodads.congtrathuong = true;
            }
            catch (Exception ex)
            {


            }


        }

        public async Task ParseAuthenticationResult(WebAuthenticationResult result)
        {
            switch (result.ResponseStatus)
            {
                //connection error
                case WebAuthenticationStatus.ErrorHttp:
                    break;
                //authentication successfull
                case WebAuthenticationStatus.Success:
                    var pattern = string.Format("{0}#access_token={1}&expires_in={2}",
                        WebAuthenticationBroker.GetCurrentApplicationCallbackUri(), "(?<access_token>.+)",
                        "(?<expires_in>.+)");
                    var match = System.Text.RegularExpressions.Regex.Match(result.ResponseData, pattern);

                    var access_token = match.Groups["access_token"];
                    var expires_in = match.Groups["expires_in"];

                    AccessToken = access_token.Value;
                    TokenExpiry = DateTime.Now.AddSeconds(double.Parse(expires_in.Value));

                    await ShowUserInfo();
                    break;
                //operation aborted by the user
                case WebAuthenticationStatus.UserCancel:
                    break;
                default:
                    break;
            }

        }
        private async Task ShowUserInfo()
        {
            Debug.WriteLine("nhay vao day");
            FacebookClient client = new FacebookClient(AccessToken);
            dynamic user = await client.GetTaskAsync("me");
            ManagerDelegate.IdFacebook = user.id;
            ManagerDelegate.UsernameFacebook = user.name;
        }
        #endregion

        #region Azure
        public async void QueryData()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                string level = ManagerDelegate.parameterAzure;
                if (string.IsNullOrEmpty(level))
                {
                    level = "Level1";
                }
                Querry(level);
            });
        }

        private async Task Querry(string level)
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                items = await Pandafruitfarm
                    .Where(todoItem => todoItem.Level == level)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                List<pandafruitfarm> list = new List<pandafruitfarm>();
                foreach (var item in items)
                {
                    list.Add(item);
                }
                ManagerDelegate.lstDataAzure = list;
                Debug.WriteLine(list.Count);
                ManagerDelegate.showDataQuery = true;
            }
        }

        public async void Query_IdFB()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                string idFB = ManagerDelegate.IdFacebook;
                string level = ManagerDelegate.parameterAzure;
                QuerryIdFb(level);
            });
        }

        private async Task QuerryIdFb(string level)
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                items = await Pandafruitfarm
                    .Where(todoItem => todoItem.Level == level)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                List<pandafruitfarm> list = new List<pandafruitfarm>();

                foreach (var item in items)
                {
                    list.Add(item);
                }
                foreach (var item in list)
                {
                    if (ManagerDelegate.scoreAzure > item.Score)
                    {
                        int index = -1;
                        index = list.FindIndex(p => p.IdFacebook == ManagerDelegate.IdFacebook);
                        if (index == -1)
                        {

                            //Insert
                            var todoItem = new pandafruitfarm
                            {
                                IdFacebook = ManagerDelegate.IdFacebook,
                                Level = ManagerDelegate.parameterAzure,
                                Score = ManagerDelegate.scoreAzure,
                                Username = ManagerDelegate.UsernameFacebook
                            };
                            await InsertTodoItem(todoItem);
                        }
                        else
                        {
                            //Update
                            list[index].Score = ManagerDelegate.scoreAzure;
                            list[index].Level = item.Level;
                            list[index].Username = item.Username;
                            list[index].IdFacebook = item.IdFacebook;
                            await UpdateCheckedTodoItem(list[index]);
                        }
                        break;
                    }

                }



                // ManagerDelegate.lstDataAzure = list;
                Debug.WriteLine(list.Count);
                // ManagerDelegate.showDataQuery = true;
            }
        }

        private async Task InsertTodoItem(pandafruitfarm todoItem)
        {

            await Pandafruitfarm.InsertAsync(todoItem);
            items.Add(todoItem);
        }

        private async Task UpdateCheckedTodoItem(pandafruitfarm item)
        {
            await Pandafruitfarm.UpdateAsync(item);
        }

        #endregion

        #region Share
        private async void ShareFB()
        {
            if (ManagerDelegate.IdFacebook == null)
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Login();
                });
              
            }
            var postParams = new
            {
                name = "This is best game on IOS, Android, Windows.",
                caption = "Download and play now",
                link = "https://apps.facebook.com/889744884476873",
                description = "Great games",
                picture = "https://fbcdn-photos-c-a.akamaihd.net/hphotos-ak-xta1/v/t1.0-0/s480x480/13087472_701657546644106_270901025500941675_n.jpg?oh=ab7134b258b44fc260cc4af65198fbde&oe=579BBED2&__gda__=1471625511_f48a8b7807ab45493a31b3ecf3e5c236"
            };
            try
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    FacebookClient client = new FacebookClient(AccessToken);
                    dynamic fbPostTaskResult = client.PostTaskAsync("/me/feed", postParams);
                    var responseresult = (IDictionary<string, object>)fbPostTaskResult;

                });
                admodads.congtrathuong = true;
            }
            catch (Exception ex)
            {


            }

        }

        private async void dtManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            e.Request.Data.Properties.Title = "Code Samples";
            e.Request.Data.Properties.Description = "Here are some great code samples for Windows Phone.";
            e.Request.Data.SetWebLink(new Uri("http://code.msdn.com/wpapps"));
        }
        #endregion

#if !UNITY_WP_8_1
        protected override Windows.UI.Xaml.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
        {
            return new UnityPlayer.XamlPageAutomationPeer(this);
        }
#else
		// This is the default setup to show location consent message box to the user
		// You can customize it to your needs, but do not remove it completely if your application
		// uses location services, as it is a requirement in Windows Store certification process
		private async void SetupLocationService()
		{
			AppCallbacks appCallbacks = AppCallbacks.Instance;
			if (!appCallbacks.IsLocationCapabilitySet())
			{
				return;
			}

			const string settingName = "LocationContent";
			bool userGaveConsent = false;

			object consent;
			var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
			var userWasAskedBefore = settings.Values.TryGetValue(settingName, out consent);

			if (!userWasAskedBefore)
			{
				var messageDialog = new Windows.UI.Popups.MessageDialog("Can this application use your location?", "Location services");

				var acceptCommand = new Windows.UI.Popups.UICommand("Yes");
				var declineCommand = new Windows.UI.Popups.UICommand("No");

				messageDialog.Commands.Add(acceptCommand);
				messageDialog.Commands.Add(declineCommand);

				userGaveConsent = (await messageDialog.ShowAsync()) == acceptCommand;
				settings.Values.Add(settingName, userGaveConsent);
			}
			else
			{
				userGaveConsent = (bool)consent;
			}

			if (userGaveConsent)
			{	// Must be called from UI thread
				appCallbacks.SetupGeolocator();
			}
		}
#endif
    }
}
