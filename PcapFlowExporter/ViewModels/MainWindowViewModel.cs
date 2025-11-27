using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using PcapFlowExporter.Services;
using Avalonia.Controls.ApplicationLifetimes;

namespace PcapFlowExporter;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly PcapFlowService _service = new();

    private string _selectedPcapPath = "";
    private string _outputCsvPath = "";
    private string _statusMessage = "Select a PCAP file to begin.";

    public string SelectedPcapPath
    {
        get => _selectedPcapPath;
        set { _selectedPcapPath = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanConvert)); }
    }

    public string OutputCsvPath
    {
        get => _outputCsvPath;
        set { _outputCsvPath = value; OnPropertyChanged(); }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set { _statusMessage = value; OnPropertyChanged(); }
    }

    public bool CanConvert => !string.IsNullOrWhiteSpace(SelectedPcapPath) && File.Exists(SelectedPcapPath);

    public ICommand BrowsePcapCommand { get; }
    public ICommand ConvertCommand { get; }

    public MainWindowViewModel()
    {
        BrowsePcapCommand = new AsyncCommand(BrowsePcapAsync);
        ConvertCommand = new AsyncCommand(ConvertAsync);
    }

    private async Task BrowsePcapAsync()
    {
        var ofd = new OpenFileDialog
        {
            Title = "Select PCAP file",
            AllowMultiple = false,
            Filters = { new FileDialogFilter { Name = "PCAP files", Extensions = { "pcap", "pcapng" } } }
        };

        var window = App.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;

        if (window == null) return;

        var result = await ofd.ShowAsync(window);
        if (result is { Length: > 0 })
        {
            SelectedPcapPath = result[0];
            OutputCsvPath = Path.ChangeExtension(SelectedPcapPath, ".flows.csv");
            StatusMessage = "PCAP selected. Ready to convert.";
        }
    }

    private async Task ConvertAsync()
    {
        try
        {
            StatusMessage = "Processing PCAP, please wait...";
            var flows = await Task.Run(() => _service.ProcessPcapToFlows(SelectedPcapPath));

            _service.ExportFlowsToCsv(flows, OutputCsvPath);
            StatusMessage = $"Done. Exported {flows.Count} flows to:\n{OutputCsvPath}";
        }
        catch (Exception ex)
        {
            StatusMessage = "Error: " + ex.Message;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
