using CMD.DTO.Appointments;
using System.Collections.Generic;

namespace CMD.Business.Appointments
{
    public interface IAppointmentManager
    {
        ICollection<AppointmentBasicInfoDTO> GetAllAppointment(int doctorId);
        AppointmentFormDTO AddAppointment(AppointmentFormDTO appointmentForm);
    }
}
