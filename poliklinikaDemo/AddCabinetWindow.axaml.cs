using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using poliklinikaDemo.Models;

namespace poliklinikaDemo;

public partial class AddCabinetWindow : Window
{
    private List<TextBox> _cabinetTextBoxes = new List<TextBox>();
    private int _fieldCounter = 0;
    private int Last_CabID = DBHelper.context.Cabinets.OrderBy(x => x.CabId).Select(x => x.CabId).Last();

    public AddCabinetWindow()
    {
        InitializeComponent();
        AddFieldClick(null!, null!); 
    }

    private void AddFieldClick(object? sender, RoutedEventArgs e)
    {
        var fieldContainer = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            Spacing = 10
        };

        var textBox = new TextBox
        {
            FontSize = 14,
            Padding = new Thickness(10),
            Watermark = $"Номер кабинета {_fieldCounter + 1}",
            Width = 300
        };

        var deleteButton = new Button
        {
            Content = "×",
            Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.Red),
            Foreground = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.White),
            Width = 30,
            Height = 30,
            FontSize = 16
        };

        deleteButton.Click += (s, args) => RemoveField(fieldContainer, textBox);

        fieldContainer.Children.Add(textBox);
        fieldContainer.Children.Add(deleteButton);

        CabinetFieldsPanel.Children.Add(fieldContainer);
        _cabinetTextBoxes.Add(textBox);
        _fieldCounter++;
    }

    private void RemoveField(StackPanel fieldContainer, TextBox textBox)
    {
        CabinetFieldsPanel.Children.Remove(fieldContainer);
        _cabinetTextBoxes.Remove(textBox);
    }

    private void SaveCabinetsClick(object? sender, RoutedEventArgs e)
    {
        var cabinets = GetCabinetsFromFields();
        AddCabinetsToDatabase(cabinets);
        Close();
    }

    private List<Cabinet> GetCabinetsFromFields()
    {
        var cabinets = new List<Cabinet>();
        
        foreach (var textBox in _cabinetTextBoxes)
        {
            Last_CabID += 1;
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                cabinets.Add(new Cabinet
                {
                    CabId = Last_CabID,
                    CabNumber = textBox.Text.Trim()
                });
            }
        }
        
        return cabinets;
    }

    private void AddCabinetsToDatabase(List<Cabinet> cabinets)
    {
        foreach (var cabinet in cabinets)
        {
            DBHelper.context.Cabinets.Add(cabinet);
        }
        DBHelper.context.SaveChanges();
    }

    private void CancelClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
