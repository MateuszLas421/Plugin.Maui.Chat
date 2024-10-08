namespace Plugin.Maui.Chat.Controls;

public partial class AudioPlayer : ContentView
{
    #region Fields
    Color isStoppedColor;
    readonly Color isPlayingColor = Colors.Green;
    #endregion

    #region Constructor
    public AudioPlayer()
	{
		InitializeComponent();

        StartStopCommand ??= new Command(async () => await StartStopAudioAsync());

        isStoppedColor = Color;
        IsVisible = false;
    }
    #endregion

    #region Bindable properties
    /// <summary>
    /// Holds audio service instance.
    /// </summary>
    internal static readonly BindableProperty AudioServiceProperty =
        BindableProperty.Create(nameof(AudioService), typeof(AudioService), typeof(AudioService));

    public AudioService AudioService
    {
        get => (AudioService)GetValue(AudioServiceProperty);
        set => SetValue(AudioServiceProperty, value);
    }

    /// <summary>
    /// Audio content in message.
    /// </summary>
    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(IAudioSource), typeof(AudioPlayer), defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnSourceChanged);

    private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (AudioPlayer)bindable;
        control.OnSourceChanged(newValue);
    }

    private void OnSourceChanged(object newValue) => IsVisible = newValue != null;

    public IAudioSource Source
    {
        get => (IAudioSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    /// <summary>
    /// Play audio message.
    /// </summary>
    public static readonly BindableProperty StartStopCommandProperty =
        BindableProperty.Create(nameof(StartStopCommand), typeof(ICommand), typeof(AudioPlayer), propertyChanged: OnStartStopCommandChanged);

    /// <summary>
    /// Necessary to expose StartStopCommand as a bindable property
    /// </summary>
    private static void OnStartStopCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (AudioPlayer)bindable;
        control.OnStartStopCommandChanged(newValue);
    }

    private void OnStartStopCommandChanged(object newValue)
    {
        if (newValue is null) 
            StartStopCommand = new Command(async () => await StartStopAudioAsync());
    }

    public ICommand StartStopCommand
    {
        get => (ICommand)GetValue(StartStopCommandProperty);
        set => SetValue(StartStopCommandProperty, value);
    }

    /// <summary>
    /// Audio content button icon.
    /// </summary>
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(AudioPlayer), ImageSource.FromFile(Maui.Chat.Resources.Icons.Waveform));

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Audio content button color.
    /// </summary>
    public static readonly BindableProperty ColorProperty =
        BindableProperty.Create(nameof(Color), typeof(Color), typeof(AudioPlayer), default, propertyChanged: OnColorChanged);

    private static void OnColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (AudioPlayer)bindable;

        if (oldValue == default)
            control.isStoppedColor = (Color)newValue;
    }

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }
    #endregion

    #region Private methods
    async Task StartStopAudioAsync()
    {
        Color = isPlayingColor;

        await AudioService.StartStopPlayerAsync(Source);

        Color = isStoppedColor;
    }
    #endregion
}