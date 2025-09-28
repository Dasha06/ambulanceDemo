using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using poliklinikaDemo.Models;

namespace poliklinikaDemo;

public partial class ViewDataWindow : Window
{
    public ViewDataWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            // Загружаем врачей с их должностями и кабинетами
            var doctors = DBHelper.context.Doctors
                .Include(d => d.Roles)
                .Include(d => d.Cab)
                .ToList();
            DoctorsListBox.ItemsSource = doctors;

            // Загружаем занятые записи с врачами и пациентами
            var appointments = DBHelper.context.AppointmentsAndPatients
                .Include(a => a.Patin)
                .Include(a => a.Appoint)
                .ThenInclude(a => a.Sched)
                .ThenInclude(s => s.Doc)
                .Where(a => a.Patin != null) // Только занятые записи
                .ToList();
            AppointmentsListBox.ItemsSource = appointments;

            // Загружаем должности
            var roles = DBHelper.context.Roles.ToList();
            RolesListBox.ItemsSource = roles;

            // Загружаем кабинеты
            var cabinets = DBHelper.context.Cabinets.ToList();
            CabinetsListBox.ItemsSource = cabinets;
        }
        catch (Exception ex)
        {
            // В случае ошибки показываем пустые списки
            DoctorsListBox.ItemsSource = new List<Doctor>();
            AppointmentsListBox.ItemsSource = new List<AppointmentsAndPatient>();
            RolesListBox.ItemsSource = new List<Role>();
            CabinetsListBox.ItemsSource = new List<Cabinet>();
        }
    }

    private void BackClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
