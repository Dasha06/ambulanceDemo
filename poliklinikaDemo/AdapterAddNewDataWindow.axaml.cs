using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace poliklinikaDemo;

public partial class AdapterAddNewDataWindow : Window
{
    public AdapterAddNewDataWindow()
    {
        InitializeComponent();
    }

    private void AddNewDocClick(object? sender, RoutedEventArgs e)
    {
        AddDocWindow addDocWindow = new AddDocWindow();
        addDocWindow.Show();
    }

    private void AddNewAppointmentsClick(object? sender, RoutedEventArgs e)
    {
        AddNewAppointmentsWindow addAppointmentsWindow = new AddNewAppointmentsWindow();
        addAppointmentsWindow.Show();
    }

    private void AddNewCaminetsClick(object? sender, RoutedEventArgs e)
    {
        AddCabinetWindow addCabinetWindow = new AddCabinetWindow();
        addCabinetWindow.Show();
    }

    private void AddNewRolesClick(object? sender, RoutedEventArgs e)
    {
        AddRoleWindow addRoleWindow = new AddRoleWindow();
        addRoleWindow.Show();
    }

    private void BackToMainClick(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }
}