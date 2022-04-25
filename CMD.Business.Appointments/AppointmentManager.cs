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

        public AppointmentConfirmationDTO AddAppointment(AppointmentFormDTO appointmentForm)
        {
            Appointment appointment = new Appointment()
            {
                PatientDetail = repo.CreatePatientDetial(appointmentForm.PatientId),
                AppointmentDate = appointmentForm.AppointmentDate,
                AppointmentTime = appointmentForm.AppointmentTime,
                Issue = repo.GetIssue(appointmentForm.Issue),
                Status = AppointmentStatus.Open,
                Reason = appointmentForm.Reason,
                Doctor = repo.GetDoctor(appointmentForm.DoctorId),
            };

            Appointment a = repo.CreateAppointment(appointment);

            var aform = new AppointmentConfirmationDTO()
            {
                AppointmentId = a.Id,
                AppointmentDate = a.AppointmentDate,
                AppointmentTime = a.AppointmentTime,
                Status = a.Status.ToString(),
                Reason = a.Reason,
                IssueName = a.Issue.Name,
                PatientName = a.PatientDetail.Patient.FirstName + " " + a.PatientDetail.Patient.LastName,
                PatientDOB = a.PatientDetail.Patient.DOB,
                DoctorName = a.Doctor.FirstName + " " + a.Doctor.LastName,
                DoctorDOB = a.Doctor.DOB,
                DoctorSpecialities = a.Doctor.Specialities.Select(s => s.SpecialityName).ToList(),
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
                    Name = patient.FirstName + " " + patient.LastName,
                    PhoneNumber = patient.ContactDetail.PhoneNumber
                });
            }
            return result;
        }

        #region Praveen Code

        public AppointmentCommentDTO GetAppointmentComment(int appointmentId)
        {
            return new AppointmentCommentDTO { Comment = repo.GetComment(appointmentId) };
        }

        public bool UpdateAppointmentComment(int appointmentId, AppointmentCommentDTO appointmentComment)
        {
            return repo.EditComment(appointmentId, appointmentComment.Comment);
        }

        #endregion

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
                    AppointmentStatus = appointment.Status.ToString(),
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
