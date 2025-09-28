using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System.Linq;
using System.Threading.Tasks;
using poliklinikaDemo.Models;

namespace poliklinikaDemo;

public partial class AddDocWindow : Window
{
    public AddDocWindow()
    {
        InitializeComponent();
        CabinetComboBox.ItemsSource = DBHelper.context.Cabinets.Select(x => x.CabNumber).ToList();
        RoleComboBox.ItemsSource = DBHelper.context.Roles.Select(x => x.RoleName).ToList();
    }

    private void CancelClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void SaveClick(object? sender, RoutedEventArgs e)
    {

        var selectedCabNumber = CabinetComboBox.SelectedItem as string;
        int? cabId = null;
        if (!string.IsNullOrWhiteSpace(selectedCabNumber))
        {
            cabId = DBHelper.context.Cabinets
                .Where(x => x.CabNumber == selectedCabNumber)
                .Select(x => (int?)x.CabId)
                .FirstOrDefault();
        }

        var selectedRoleName = RoleComboBox.SelectedItem as string;
        Role? selectedRole = null;
        if (!string.IsNullOrWhiteSpace(selectedRoleName))
        {
            selectedRole = DBHelper.context.Roles
                .FirstOrDefault(x => x.RoleName == selectedRoleName);
        }

        var newDoctor = new Doctor
        {
            DocFname = FirstNameTextBox.Text,
            DocSname = SecondNameTextBox.Text,
            Doclname = string.IsNullOrWhiteSpace(LastNameTextBox.Text) ? null : LastNameTextBox.Text,
            CabId = cabId,
            Roles = new List<Role> { selectedRole }
        };

        DBHelper.context.Doctors.Add(newDoctor);
        DBHelper.context.SaveChanges();

        Close();
    }
}