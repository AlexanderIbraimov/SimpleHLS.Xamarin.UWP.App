using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StreamApp
{
    public class VideoPlayer : View
    {
        public event Action<string> Played;
        public event Action Stopped;
        public VideoPlayer()
        {

        }
        public static readonly BindableProperty UrlProperty = BindableProperty.Create(nameof(Url), typeof(string), typeof(VideoPlayer), string.Empty);
        public string Url
        {
            set
            {
                SetValue(UrlProperty, value);
            }
            get
            {
                return (string)GetValue(UrlProperty);
            }
        }

        public void Play()
        {
            Played?.Invoke(Url);
        }

        public void Pause()
        {
            Stopped?.Invoke();
        }
    }
}
