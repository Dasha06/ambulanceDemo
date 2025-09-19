using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using poliklinikaDemo.Models;

namespace poliklinikaDemo;

public partial class AddPatientWindow : Window
{
    public Schedule selectedSchedule = new Schedule();
    public AddPatientWindow(Schedule schedule)
    {
        selectedSchedule =  schedule;
        InitializeComponent();
        FioDoctorsTextBlock.Text = schedule.Doc.DocSname + " " + schedule.Doc.Doclname;
        DateAndTimeTextBlock.Text = schedule.SchedDate.ToString() + " " + schedule.SchedTime.ToString();
    }

    private void CreateAnAppointmentButton(object? sender, RoutedEventArgs e)
    {
        var findPatient = DBHelper.context.Patients.FirstOrDefault(x => x.PatinFname == FirstNamePatientTextBox.Text
                                                                        && x.PatinSname == SecondNamePatientTextBox.Text
                                                                        && x.PatinLname == LastNamePatientTextBox.Text
                                                                        && x.PatinBirthday ==
                                                                        DateOnly.FromDateTime(
                                                                            (DateTime)BirthdayPatientTextBox
                                                                                .SelectedDate));

        if (findPatient != null)
        {
            var appoin =  new Appointment
            {
                SchedId = selectedSchedule.SchedId,
                AppointReason = ReasonPatientTextBox.Text
            };
            DBHelper.context.Appointments.Add(appoin);
            DBHelper.context.SaveChanges();
            DBHelper.context.AppointmentsAndPatients.Add(new  AppointmentsAndPatient{PatinId = findPatient.PatinId, AppointId = 
                DBHelper.context.Appointments.Where(x => x.AppointReason == ReasonPatientTextBox.Text).Select(x => x.AppointId).First()});
            selectedSchedule.SchedIsClosed = true;
            DBHelper.context.SaveChanges();
            Close(true);
        }
        else
        {
            Patient newPatient = new Patient
            {
                PatinFname = FirstNamePatientTextBox.Text,
                PatinSname = SecondNamePatientTextBox.Text,
                PatinLname = LastNamePatientTextBox.Text,
                PatinBirthday = DateOnly.FromDateTime(
                    (DateTime)BirthdayPatientTextBox
                        .SelectedDate)
            };
            
            var appoin =  new Appointment
            {
                SchedId = selectedSchedule.SchedId,
                AppointReason = ReasonPatientTextBox.Text
            };
            
            DBHelper.context.Appointments.Add(appoin);
            DBHelper.context.Patients.Add(newPatient);
            DBHelper.context.SaveChanges();
            var savedPatient = DBHelper.context.Patients.FirstOrDefault(x => x.PatinFname == FirstNamePatientTextBox.Text
                                                                             && x.PatinSname == SecondNamePatientTextBox.Text
                                                                             && x.PatinLname == LastNamePatientTextBox.Text
                                                                             && x.PatinBirthday ==
                                                                             DateOnly.FromDateTime(
                                                                                 (DateTime)BirthdayPatientTextBox
                                                                                     .SelectedDate));
            DBHelper.context.AppointmentsAndPatients.Add(new  AppointmentsAndPatient{PatinId = savedPatient.PatinId, 
                AppointId = DBHelper.context.Appointments
                    .Where(x => x.AppointReason == ReasonPatientTextBox.Text && x.SchedId == selectedSchedule.SchedId)
                    .Select(x => x.AppointId).First()});
            selectedSchedule.SchedIsClosed = true;
            DBHelper.context.SaveChanges();
            
            Close(true);
        }
    }
    
}