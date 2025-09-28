using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using poliklinikaDemo.Models;

namespace poliklinikaDemo;

public partial class AddRoleWindow : Window
{
    private List<TextBox> _roleTextBoxes = new List<TextBox>();
    private int _fieldCounter = 0;
    private int Last_RoleID = DBHelper.context.Roles.OrderBy(x => x.RoleId).Select(x => x.RoleId).Last();

    public AddRoleWindow()
    {
        InitializeComponent();
        AddFieldClick(null!, null!); // Добавляем первое поле автоматически
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
            Watermark = $"Должность {_fieldCounter + 1}",
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

        RoleFieldsPanel.Children.Add(fieldContainer);
        _roleTextBoxes.Add(textBox);
        _fieldCounter++;
    }

    private void RemoveField(StackPanel fieldContainer, TextBox textBox)
    {
        RoleFieldsPanel.Children.Remove(fieldContainer);
        _roleTextBoxes.Remove(textBox);
    }

    private void SaveRolesClick(object? sender, RoutedEventArgs e)
    {
        var roles = GetRolesFromFields();
        AddRolesToDatabase(roles);
        this.Close();
    }

    private List<Role> GetRolesFromFields()
    {
        var roles = new List<Role>();
        
        foreach (var textBox in _roleTextBoxes)
        {
            Last_RoleID += 1;
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                roles.Add(new Role
                {
                    RoleId = Last_RoleID,
                    RoleName = textBox.Text
                });
            }
        }
        
        return roles;
    }

    private void AddRolesToDatabase(List<Role> roles)
    {
        foreach (var role in roles)
        {
            DBHelper.context.Roles.Add(role);
        }
        DBHelper.context.SaveChanges();
    }

    private void CancelClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
