using StreamApp;
using StreamApp.UWP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Media.Streaming.Adaptive;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(VideoPlayer), typeof(VideoPlayerRenderer))]
namespace StreamApp.UWP
{
    public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, MediaPlayerElement>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<VideoPlayer> args)
        {
            base.OnElementChanged(args);
            if (Control == null)
            {
                var mediaPlayerElement = new MediaPlayerElement();
                mediaPlayerElement.AreTransportControlsEnabled = true;
                SetNativeControl(mediaPlayerElement);
                if (args.NewElement != null)
                {
                    args.NewElement.Played += NewElement_Played;
                    args.NewElement.Stopped += NewElement_Stopped;
                    if (!string.IsNullOrEmpty(args.NewElement.Url))
                    {
                        InitializeAdaptiveMediaSource(new Uri(args.NewElement.Url));
                    }
                }
            }
        }

        private void NewElement_Stopped()
        {
            Control?.MediaPlayer?.Pause();
        }

        private void NewElement_Played(string url)
        {
            Control?.MediaPlayer?.Play();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VideoPlayer.UrlProperty.PropertyName)
            {
                 InitializeAdaptiveMediaSource(new Uri(Element.Url));
            }
        }


        async private void InitializeAdaptiveMediaSource(System.Uri uri)
        {
                AdaptiveMediaSourceCreationResult result = await AdaptiveMediaSource.CreateFromUriAsync(uri);

                if (result.Status == AdaptiveMediaSourceCreationStatus.Success)
                {
                    var ams = result.MediaSource;
                    Control.SetMediaPlayer(new MediaPlayer());
                    Control.MediaPlayer.Source = MediaSource.CreateFromAdaptiveMediaSource(ams);
                    Control.MediaPlayer.Play();
                    ams.DownloadRequested += Ams_DownloadRequested; ;
                    ams.DownloadCompleted += Ams_DownloadCompleted; ;
                    ams.DownloadFailed += Ams_DownloadFailed; ;
                    ams.DownloadBitrateChanged += Ams_DownloadBitrateChanged; ;
                    ams.PlaybackBitrateChanged += Ams_PlaybackBitrateChanged; ;
                    ams.Diagnostics.DiagnosticAvailable += Diagnostics_DiagnosticAvailable; ;
                }
                else
                {
                    throw new ArgumentException();
                }
            
        }

        private void Diagnostics_DiagnosticAvailable(AdaptiveMediaSourceDiagnostics sender, AdaptiveMediaSourceDiagnosticAvailableEventArgs args)
        {
        }

        private void Ams_PlaybackBitrateChanged(AdaptiveMediaSource sender, AdaptiveMediaSourcePlaybackBitrateChangedEventArgs args)
        {
        }

        private void Ams_DownloadBitrateChanged(AdaptiveMediaSource sender, AdaptiveMediaSourceDownloadBitrateChangedEventArgs args)
        {
        }

        private void Ams_DownloadFailed(AdaptiveMediaSource sender, AdaptiveMediaSourceDownloadFailedEventArgs args)
        {
            Debug.WriteLine("Stream failed: " + args.ExtendedError);
        }

        private void Ams_DownloadCompleted(AdaptiveMediaSource sender, AdaptiveMediaSourceDownloadCompletedEventArgs args)
        {
        }

        private async void Ams_DownloadRequested(AdaptiveMediaSource sender, AdaptiveMediaSourceDownloadRequestedEventArgs args)
        {

        }
    }
}
