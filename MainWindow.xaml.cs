using System;
using Microsoft.UI.Xaml;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinUIEx;

namespace FFmpegInteropX_WinUI_UnpackagedDemo;

public sealed partial class MainWindow : Window
{
    readonly Windows.Media.Playback.MediaPlayer mediaPlayer = new();
    FFmpegInteropX.FFmpegMediaSource mediaSource;
    Windows.Storage.Streams.IRandomAccessStream stream;

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        var picker = new FileOpenPicker();
        picker.ViewMode = PickerViewMode.Thumbnail;
        picker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
        picker.FileTypeFilter.Add("*");

        WinRT.Interop.InitializeWithWindow.Initialize(picker, this.GetWindowHandle());

        var file = await picker.PickSingleFileAsync();

        if (file != null)
        {
            stream = await file.OpenAsync(FileAccessMode.Read);

            mediaSource = await FFmpegInteropX.FFmpegMediaSource.CreateFromStreamAsync(stream);
            mediaPlayer.Source = mediaSource.CreateMediaPlaybackItem();

            mp.SetMediaPlayer(mediaPlayer);

            mediaPlayer.Play();
        }
    }
}