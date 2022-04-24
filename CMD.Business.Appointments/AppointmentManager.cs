using CMD.DTO.Appointments;
using CMD.Model.Appointments.Entities;
using CMD.Repository.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD.Business.Appointments
{
    public class AppointmentManager : IAppointmentManager
    {
        protected IAppointmentRepository repo;

        public AppointmentManager(IAppointmentRepository repo)
        {
            this.repo = repo;
        }

        public AppointmentFormDTO AddAppointment(AppointmentFormDTO appointmentForm)
        {
            throw new NotImplementedException();
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
