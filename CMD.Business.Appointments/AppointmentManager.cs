using CMD.DTO.Appointments;
using CMD.Model.Appointments;
using CMD.Repository.Appointments;
using System.Collections.Generic;
using System.Linq;

namespace CMD.Business.Appointments
{
    public class AppointmentManager : IAppointmentManager
    {
        protected IAppointmentRepository repo;

        public AppointmentManager(AppointmentRepository repo)
        {
            this.repo = repo;
        }

        public AppointmentFormDTO AddAppointment(AppointmentFormDTO appointmentForm)
        {
            Appointment appointment = new Appointment()
            {
                PatientDetail = new PatientDetail { PatientId = appointmentForm.Patient.Id },
                AppointmentDate = appointmentForm.AppointmentDate,
                AppointmentTime = appointmentForm.AppointmentTime,
                Status = AppointmentStatus.Open,
                Issue = new Issue { Name = appointmentForm.Issue },
                Reason = appointmentForm.Reason,
                Doctor = new Doctor { Id = appointmentForm.Doctor.Id },
            };

            Appointment a = repo.CreateAppointment(appointment);

            var aform =  new AppointmentFormDTO()
            {
                AppointmentId = a.Id,
                AppointmentDate = a.AppointmentDate,
                AppointmentTime = a.AppointmentTime,
                AppointmentStatus = a.Status.ToString(),
                Issue = a.Issue.Name,
                Reason = a.Reason,
                Doctor = new DoctorDTO
                {
                    Name = a.Doctor.FirstName + " " + a.Doctor.LastName,
                    Specialities = a.Doctor.Specialities.Select(x => x.SpecialityName).ToList(),
                    DoctorPicture = a.Doctor.DoctorPicture,
                    DOB = a.Doctor.DOB
                },
                Patient = new PatientDTO
                {
                    Name = a.PatientDetail.Patient.FirstName + " " + a.PatientDetail.Patient.LastName,
                    DOB = a.PatientDetail.Patient.DOB,
                    PatientPicture = a.PatientDetail.Patient.PatientPicture
                }
            };
            return aform;
        }

        public ICollection<IssueDTO> GetIssues()
        {
            ICollection<Issue> issues = repo.GetIssues();
            ICollection<IssueDTO> result = new List<IssueDTO>();
            foreach(Issue issue in issues)
            {
                result.Add(new IssueDTO
                {
                    Id = issue.Id,
                    Name = issue.Name,
                });
            }
            return result;
        }

        public ICollection<PatientDTOForPatientSearch> GetRecommendedPatients(int doctorId)
        {
            ICollection<Patient> recommededPatients = repo.GetRecommededPatients(doctorId);
            ICollection<PatientDTOForPatientSearch> result = new List<PatientDTOForPatientSearch>();
            foreach(Patient patient in recommededPatients)
            {
                result.Add(new PatientDTOForPatientSearch
                {
                    Id = patient.Id,
                    DOB = patient.DOB,
                    Name = patient.FirstName + " " + patient.LastName,
                    PatientPicture = patient.PatientPicture,
                    PhoneNumber = patient.ContactDetail.PhoneNumber
                });
            }
            return result;
        }

        ICollection<AppointmentBasicInfoDTO> IAppointmentManager.GetAllAppointment(int doctorId)
        {
            ICollection<Appointment> appointments = repo.GetAllAppointment(doctorId);
            ICollection<AppointmentBasicInfoDTO> result = new List<AppointmentBasicInfoDTO>();
            foreach (var appointment in appointments)
            {
                result.Add(new AppointmentBasicInfoDTO()
                {
                    AppointmentId = appointment.Id,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    PatientName = appointment.PatientDetail.Patient.FirstName + " " + appointment.PatientDetail.Patient.LastName,
                    PatientPicture = appointment.PatientDetail.Patient.PatientPicture,
                    PatientDOB = appointment.PatientDetail.Patient.DOB,
                    Issue = appointment.Issue.Name
                });
            }
            return result;
        }
    }
}
