using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using poliklinikaDemo.Models;

namespace poliklinikaDemo;

public partial class MainWindow : Window
{
    private List<string> Roles = DBHelper.context.Roles.Select(x => x.RoleName).ToList();
    public List<Schedule> Schedules = DBHelper.context.Schedules
        .Include(x => x.Doc)
        .ThenInclude(x => x.Roles)
        .Where(x => x.SchedIsClosed == false)
        .ToList();

    public MainWindow()
    {
        InitializeComponent();
        DoctorsRoleComboBox.ItemsSource = Roles;
    }
    private void SelectedDateClendarPicker(object? sender, SelectionChangedEventArgs e)
    {
        var selectedDoc = DoctorsRoleComboBox.SelectionBoxItem as string;
        var selectDate = SelectedDate.SelectedDate;
        var sortedSchedules = Schedules;
        if (selectDate != null)
        {
            FreAppointmentsItemControl.ItemsSource = sortedSchedules.Where(x => x.SchedDate == DateOnly.FromDateTime((DateTime)selectDate) 
                                                                          && x.Doc.Roles.Any(y => y.RoleName == selectedDoc)).OrderBy(x => x.SchedTime).ToList();
        }
    }

    private async void AppointmentButton(object? sender, RoutedEventArgs e)
    {
        int id = (int)(sender as Button).Tag;
        var selectedAppointment = Schedules.FirstOrDefault(x => x.SchedId == id);
        AddPatientWindow addPatientWindow = new AddPatientWindow(selectedAppointment);
        var result = await addPatientWindow.ShowDialog<bool>(this);
        if (result == true)
        {
            Schedules = DBHelper.context.Schedules
                .Include(x => x.Doc)
                .ThenInclude(x => x.Roles)
                .Where(x => x.SchedIsClosed == false)
                .ToList();

            var selectedDoc = DoctorsRoleComboBox.SelectionBoxItem as string;
            var selectDate = SelectedDate.SelectedDate;

            if (selectDate != null && selectedDoc != null)
            {
                FreAppointmentsItemControl.ItemsSource = Schedules.Where(x =>
                    x.SchedDate == DateOnly.FromDateTime((DateTime)selectDate) &&
                    x.Doc.Roles.Any(y => y.RoleName == selectedDoc)).ToList();
            }
            else
            {
                FreAppointmentsItemControl.ItemsSource = Schedules;
            }
        }
    }

    private void SelectedDocChanged(object? sender, SelectionChangedEventArgs e)
    {
        var selectedDoc = DoctorsRoleComboBox.SelectionBoxItem as string;
        var sortedSchedules = Schedules;

        if (selectedDoc != null && SelectedDate.SelectedDate != null)
        {
            FreAppointmentsItemControl.ItemsSource = sortedSchedules.FindAll(x => x.Doc.Roles.Any(y => y.RoleName == selectedDoc));
        }
    }

    private void AddNewDataClick(object? sender, RoutedEventArgs e)
    {
        AdapterAddNewDataWindow adaptNewData =  new AdapterAddNewDataWindow();
        adaptNewData.Show();
        Close();
    }

    private void SeeAllDataClick(object? sender, RoutedEventArgs e)
    {
        ViewDataWindow viewDataWindow = new ViewDataWindow();
        viewDataWindow.Show();
    }
}