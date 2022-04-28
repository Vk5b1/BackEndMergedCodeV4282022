using CMD.Model;
using System;
using System.Collections.Generic;

namespace CMD.Repository.Appointments
{
    public interface IAppointmentRepository
    {
        ICollection<Appointment> GetAllAppointment(int doctorId);
        int AppointmentCount(int doctorId);
        int AppointmentCount(int doctorId, string status);
        Appointment CreateAppointment(Appointment appointment);
        bool CheckDate(DateTime date, TimeSpan time, int doctorId);
        ICollection<string> GetIssues();
        ICollection<Patient> GetPatients(int doctorId);
        PatientDetail CreatePatientDetial(int patientId);
        Doctor GetDoctor(int docId);
        Issue GetIssue(string issueName);
        bool DoctorAppointmentValidate(int appointmentId, int doctorId);
        bool AcceptApppointment(int appointmentId);
        bool RejectApppointment(int appointmentId);
        List<int> GetIdsAssociatedWithAppointment(int appointmentId);
        string GetComment(int appointmentId);
        bool EditComment(int appointmentId, string comment);
    }
}
