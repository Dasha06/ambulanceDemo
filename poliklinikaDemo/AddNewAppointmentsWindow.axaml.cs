using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using poliklinikaDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace poliklinikaDemo;

public partial class AddNewAppointmentsWindow : Window
{
    private List<Doctor> doctors =  DBHelper.context.Doctors.ToList();
    public AddNewAppointmentsWindow()
    {
        InitializeComponent();
        var doctorList = doctors.Select(d => new 
        { 
            DocId = d.DocId, 
            FullName = $"{d.DocSname} {d.DocFname} {d.Doclname}".Trim() 
        }).ToList();
        DoctorComboBox.ItemsSource = doctorList;
    }

    private async void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var selectedDate = DatePicker.SelectedDate!.Value;
        var startTime = TimePicker.SelectedTime!.Value;
        var doctorId = (int)DoctorComboBox.SelectedValue!;
        var appointmentCount = (int)AppointmentCountPicker.Value!;
        var intervalMinutes = (int)IntervalPicker.Value!;

        // Создаем записи в расписании
        for (int i = 0; i < appointmentCount; i++)
        {
            var appointmentTime = startTime.Add(TimeSpan.FromMinutes(i * intervalMinutes));
            
            var schedule = new Schedule
            {
                DocId = doctorId,
                SchedDate = DateOnly.FromDateTime(selectedDate),
                SchedTime = TimeOnly.FromTimeSpan(appointmentTime),
                SchedIsClosed = false
            };

            DBHelper.context.Schedules.Add(schedule);
        }

        // Сохраняем изменения в базе данных
        await Task.Run(() => DBHelper.context.SaveChanges());

        await ShowMessage("Успех", $"Создано {appointmentCount} записей в расписании");
        
        // Очищаем форму
        ClearForm();
    }

    private void ClearForm()
    {
        DatePicker.SelectedDate = null;
        TimePicker.SelectedTime = null;
        DoctorComboBox.SelectedItem = null;
        AppointmentCountPicker.Value = 1;
        IntervalPicker.Value = 30;
    }

    private async Task ShowMessage(string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 300,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        
        var panel = new StackPanel
        {
            Margin = new Thickness(20)
        };
        
        panel.Children.Add(new TextBlock 
        { 
            Text = message, 
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Margin = new Thickness(0, 0, 0, 20)
        });
        
        var okButton = new Button 
        { 
            Content = "OK", 
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            Width = 100
        };
        okButton.Click += (s, e) => dialog.Close();
        panel.Children.Add(okButton);
        
        dialog.Content = panel;
        await dialog.ShowDialog(this);
    }
}