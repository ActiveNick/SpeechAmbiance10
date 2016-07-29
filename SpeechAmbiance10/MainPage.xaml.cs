//==========================================================================
//
// Author:  Nick Landry
// Title:   Senior Technical Evangelist - Microsoft US DX - NY Metro
// Twitter: @ActiveNick
// Blog:    www.AgeofMobility.com
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Disclaimer: Portions of this code may been simplified to demonstrate
// useful application development techniques and enhance readability.
// As such they may not necessarily reflect best practices in enterprise 
// development, and/or may not include all required safeguards.
// 
// This code and information are provided "as is" without warranty of any
// kind, either expressed or implied, including but not limited to the
// implied warranties of merchantability and/or fitness for a particular
// purpose.
//
// To learn more about Universal Windows app development using Cortana
// and the Speech SDK, watch the full-day course for free on
// Microsoft Virtual Acdemy (MVA) at http://aka.ms/cortanamva
//
//==========================================================================using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SpeechAmbiance10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Reminder: You need to enable the Microphone capabilitiy in Windows Phone projects
        //Reminder: Add this namespace in your using statements
        //using Windows.Media.SpeechSynthesis;

        // The object for controlling the speech synthesis engine (voice).
        SpeechSynthesizer speech;

        // The media object for controlling and playing audio.
        //MediaElement mediaplayer;
        MediaElement fx1player;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            speech = new SpeechSynthesizer();
            //mediaplayer = new MediaElement();

            //mediaplayer.AudioCategory = AudioCategory.BackgroundCapableMedia;

            lstVoices.ItemsSource = SpeechSynthesizer.AllVoices;
            lstVoices.SelectedValuePath = "DisplayName";
            lstVoices.SelectedValue = SpeechSynthesizer.DefaultVoice.DisplayName;

            lstPitch.ItemsSource = new string[] { "default", "x-low", "low", "medium", "high", "x-high" };
            lstPitch.SelectedValue = "default";

            fx1player = new MediaElement();
            fx1player.Source = new Uri("ms-appx:///Assets/177958__sclolex__water-dripping-in-cave.wav");

            //mediaplayer.MediaEnded += Mediaplayer_MediaEnded;

        }

        private void Mediaplayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            //ReadBoringText(txtInput.Text);
            ReadSsmlText(txtInput.Text);

            
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaplayer.Pause();
        }

        private async void BackAudio_Click(object sender, RoutedEventArgs e)
        {
            var var_assets = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            // Audio file courtesy of Sclolex and freesound.org
            // http://www.freesound.org/people/Sclolex/sounds/177958/
            // Crative Commons 0 License - CC0 1.0 Universal (CC0 1.0) Public Domain Dedication
            var var_file = await var_assets.GetFileAsync("177958__sclolex__water-dripping-in-cave.wav");
            var var_stream = await var_file.OpenAsync(FileAccessMode.Read);

            fx1player.SetSource(var_stream, var_file.ContentType);
            fx1player.IsLooping = true;
            fx1player.Play();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            // Nothing for now
        }


        private async void ReadBoringText(string mytext)
        {
            //Retrieve the first female voice
            speech.Voice = SpeechSynthesizer.AllVoices
                .First(i => (i.Gender == VoiceGender.Female && i.Description.Contains("United States")));
            //VoiceInformation currentVoice = (VoiceInformation)lstVoices.SelectedItem;
            //speech.Voice = currentVoice;

            // Generate the audio stream from plain text.
            SpeechSynthesisStream stream = await speech.SynthesizeTextToStreamAsync(mytext);

            // Send the stream to the media object.
            mediaplayer.SetSource(stream, stream.ContentType);
            mediaplayer.Play();
        }

        private async void ReadSsmlText(string mytext)
        {
            VoiceInformation currentVoice = (VoiceInformation)lstVoices.SelectedItem;

            // The object for controlling the speech synthesis engine (voice).
            string Ssml =
                @"<speak version='1.0' " +
                "xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='" + currentVoice.Language + "'>" +
                //                "<voice name='" + currentVoice.DisplayName + "' gender='" + currentVoice.Gender.ToString() + "' xml:lang='" + currentVoice.Language + "'>" +
                "<voice name='" + currentVoice.DisplayName + "'>" +
                "<prosody pitch='" + lstPitch.SelectedItem.ToString() + "' rate='" + sldRate.Value.ToString() + "'>" + mytext + "</prosody>" +
                "</voice></speak>";

            // Generate the audio stream from plain text.
            SpeechSynthesisStream stream = await speech.SynthesizeSsmlToStreamAsync(Ssml);

            // Send the stream to the media object.
            mediaplayer.SetSource(stream, stream.ContentType);
            mediaplayer.Play();
        }
    }
}
