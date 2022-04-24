using CMD.Business.Appointments;
using CMD.DTO.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CMD.API.Appointments.Controllers
{
    public class AppointmentController : ApiController
    {
        protected IAppointmentManager appointmentManager;
        
        public AppointmentController(IAppointmentManager appointmentManager) 
        {
            this.appointmentManager = appointmentManager;
        }

        // GET: api/Appointment/{doctorId}
        public IHttpActionResult GetAllAppointment(int doctorId)
        {
            ICollection<AppointmentBasicInfoDTO> appointment = appointmentManager.GetAllAppointment(doctorId);
            return Ok(appointment);
        }

        // POST: api/Appointment/add

        public IHttpActionResult AddAppointment([FromBody]AppointmentFormDTO jsonData)
        {
            AppointmentFormDTO appointmentForm = jsonData;
            var result = appointmentManager.AddAppointment(appointmentForm);
            
        }       
    }
}
