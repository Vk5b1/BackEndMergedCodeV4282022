﻿using CMD.Model.Appointments;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMD.Repository.Appointments
{
    public class AppointmentRepository : IAppointmentRepository
    {
        protected CMDAppointmentContext db = new CMDAppointmentContext();

        public Appointment CreateAppointment(Appointment appointment)
        {
            db.Appointments.Add(appointment);

            db.SaveChanges();

            return appointment;
        }
        public PatientDetail CreatePatientDetial(int patientId)
        {
            PatientDetail patientDetail = new PatientDetail();
            patientDetail.Patient = db.Patients.Where(p => p.Id == patientId).First();
            
            if(patientDetail.Patient == null)
            {
                throw new KeyNotFoundException();
            }
            db.PatientDetails.Add(patientDetail);
            db.SaveChanges();
            return patientDetail;
        }
        public Doctor GetDoctor(int docId)
        {
            return db.Doctors.Find(docId);
        }

        public Issue GetIssue(string issueName)
        {
            return db.Issues.Where(i => i.Name == issueName).Any() ? db.Issues.Where(i => i.Name == issueName).First() : AddNewIssue(new Issue { Name = issueName});
        }

        private Issue AddNewIssue(Issue issue)
        {
            db.Issues.Add(issue);
            db.SaveChanges();
            return issue;   
        }

        public int AppointmentCount(int doctorId)
        {
            return db.Appointments.Where(a => a.Doctor.Id == doctorId).Count();
        }

        public ICollection<Appointment> GetAllAppointment(int doctorId)
        {
            var allAppointments = db.Appointments
                .Include(path: a => a.Doctor)
                .Include(path: a => a.PatientDetail.Patient)
                .Include(path: a => a.Issue)
                .Where(a => a.Doctor.Id == doctorId).OrderBy(a => a.AppointmentDate).ThenBy(a => a.AppointmentTime).ToList();
            return allAppointments;
        }

        public ICollection<string> GetIssues()
        {
            return db.Issues.Select(i => i.Name).ToList();
        }

        public ICollection<Patient> GetPatients(int doctorId)
        {
            return db.Patients.ToList();
        }

        public bool AcceptApppointment(int appointmentId)
        {
            var appointment = db.Appointments.Find(appointmentId);

            appointment.Status = AppointmentStatus.Confirmed;
            return db.SaveChanges() > 0;
        }

        public bool RejectApppointment(int appointmentId)
        {
            var appointment = db.Appointments.Find(appointmentId);

            appointment.Status = AppointmentStatus.Cancelled;
            return db.SaveChanges() > 0;
        }

        public bool DoctorAppointmentValidate(int appointmentId, int doctorId)
        {
            return db.Appointments.Find(appointmentId).Doctor.Id == doctorId;
        }
    }
}
