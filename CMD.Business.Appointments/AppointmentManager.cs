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
            throw new NotImplementedException();
        }
    }
}
