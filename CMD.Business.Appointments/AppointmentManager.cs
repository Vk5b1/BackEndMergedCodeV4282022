using CMD.Business.Appointments.CustomExceptions;
using CMD.DTO.Appointments;
using CMD.Repository.Appointments;
using System.Collections.Generic;
using System.Linq;
using System;
using CMD.Model;

namespace CMD.Business.Appointments
{
    public class AppointmentManager : IAppointmentManager
    {
        protected IAppointmentRepository repo;

        public AppointmentManager(IAppointmentRepository repo)
        {
            this.repo = repo;
        }

        public AppointmentConfirmationDTO AddAppointment(AppointmentFormDTO appointmentForm)
        {

                if (appointmentForm.AppointmentDate == default)
                    throw new DateNullException();

                if (appointmentForm.AppointmentDate < DateTime.Now || (appointmentForm.AppointmentDate == DateTime.Now.Date && appointmentForm.AppointmentTime < DateTime.Now.TimeOfDay))
                    throw new PastDateException();

                if(appointmentForm.PatientId == null || appointmentForm.DoctorId == null || appointmentForm.Issue == "") 
                    throw new NullReferenceException();

                if (repo.CheckDate(appointmentForm.AppointmentDate, appointmentForm.AppointmentTime, (int)appointmentForm.DoctorId))
                    throw new SameAppointmentTimingException();

                Appointment appointment = new Appointment()
                {
                    PatientDetail = repo.CreatePatientDetial((int)appointmentForm.PatientId),
                    AppointmentDate = appointmentForm.AppointmentDate,
                    AppointmentTime = appointmentForm.AppointmentTime,
                    Issue = repo.GetIssue(appointmentForm.Issue),
                    Reason = appointmentForm.Reason,
                    Doctor = repo.GetDoctor((int)appointmentForm.DoctorId),
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
                    PatientName = a.PatientDetail.Patient.Name,
                    PatientDOB = a.PatientDetail.Patient.DOB,
                    DoctorName = a.Doctor.Name,
                    DoctorSpeciality = a.Doctor.Speciality,
                };
                return aform;
            
        }


        public ICollection<string> GetIssues()
        {
            return repo.GetIssues();
        }

        public ICollection<PatientDTOForPatientSearch> GetPatients(int doctorId)
        {
            ICollection<Patient> patients = repo.GetPatients(doctorId);

            if(patients == null)
            {
                return null;
            }

            ICollection<PatientDTOForPatientSearch> result = new List<PatientDTOForPatientSearch>();
            foreach(Patient patient in patients)
            {
                result.Add(new PatientDTOForPatientSearch
                {
                    Id = patient.Id,
                    Name = patient.Name,
                    PhoneNumber = patient.ContactDetail.PhoneNumber
                });
            }
            return result;
        }

        public ICollection<AppointmentBasicInfoDTO> GetAllAppointment(int doctorId, string status, PaginationParams pagination)
        {
            ICollection<Appointment> appointments = repo.GetAllAppointment(doctorId).Where(a => a.Status.ToString().ToLower().Equals(status.ToLower())).Skip((pagination.Page - 1) * pagination.ItemsPerPage).Take(pagination.ItemsPerPage).ToList();

            if(appointments == null) return null;

            ICollection<AppointmentBasicInfoDTO> result = new List<AppointmentBasicInfoDTO>();
            foreach (var appointment in appointments)
            {
                result.Add(new AppointmentBasicInfoDTO()
                {
                    AppointmentId = appointment.Id,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    AppointmentStatus = appointment.Status.ToString(),
                    PatientName = appointment.PatientDetail.Patient.Name,
                    PatientPicture = appointment.PatientDetail.Patient.PatientPicture,
                    PatientDOB = appointment.PatientDetail.Patient.DOB,
                    Issue = appointment.Issue.Name
                });
            }
            return result;
        }

        public ICollection<AppointmentBasicInfoDTO> GetAllAppointment(int doctorId, PaginationParams pagination)
        {
            ICollection<Appointment> appointments = repo.GetAllAppointment(doctorId).Skip((pagination.Page - 1) * pagination.ItemsPerPage).Take(pagination.ItemsPerPage).ToList();
            ICollection<AppointmentBasicInfoDTO> result = new List<AppointmentBasicInfoDTO>();
            foreach (var appointment in appointments)
            {
                result.Add(new AppointmentBasicInfoDTO()
                {
                    AppointmentId = appointment.Id,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentTime = appointment.AppointmentTime,
                    AppointmentStatus = appointment.Status.ToString(),
                    PatientName = appointment.PatientDetail.Patient.Name ,
                    PatientPicture = appointment.PatientDetail.Patient.PatientPicture,
                    PatientDOB = appointment.PatientDetail.Patient.DOB,
                    Issue = appointment.Issue.Name
                });
            }
            return result;
        }
        public int GetAppointmentCount(int doctorId)
        {
            return repo.AppointmentCount(doctorId);
        }
        public int GetAppointmentCountBasedOnStatus(int doctorId, string status)
        {
            return repo.AppointmentCount(doctorId, status);
        }

        public bool ChangeAppointmentStatus(AppointmentStatusDTO statusDTO, int doctorId)
        {
            var result = false;
            if(statusDTO.Status == "Confirmed")
            {
                result = repo.AcceptApppointment(statusDTO.AppointmentId);
            }
            else if(statusDTO.Status == "Cancelled")
            {
                result = repo.RejectApppointment(statusDTO.AppointmentId);
            }
            return result;
        }

        #region kishore cards patients doctors

        public IdsListViewDetailsDTO GetIdsAssociatedWithAppointment(int appointmentId)
        {
            IdsListViewDetailsDTO idsListViewDetailsDTO = new IdsListViewDetailsDTO();
            var temp = repo.GetIdsAssociatedWithAppointment(appointmentId);
            idsListViewDetailsDTO.AppointmentId = temp[0];
            idsListViewDetailsDTO.PatientId = temp[1];
            idsListViewDetailsDTO.DoctorId = temp[2];
            return idsListViewDetailsDTO;
        }
        #endregion

        #region Praveen Code

        public AppointmentCommentDTO GetAppointmentComment(int appointmentId)
        {
            return new AppointmentCommentDTO { Id = appointmentId, Comment = repo.GetComment(appointmentId) };
        }

        public bool UpdateAppointmentComment(int appointmentId, AppointmentCommentDTO appointmentComment)
        {
            return repo.EditComment(appointmentId, appointmentComment.Comment);
        }

        #endregion

    }
}
