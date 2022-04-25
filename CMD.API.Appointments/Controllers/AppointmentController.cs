using CMD.Business.Appointments;
using CMD.DTO.Appointments;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace CMD.API.Appointments.Controllers
{
    [RoutePrefix("api/appointment")]
    public class AppointmentController : ApiController
    {
        protected IAppointmentManager appointmentManager;
        
        public AppointmentController(IAppointmentManager appointmentManager) 
        {
            this.appointmentManager = appointmentManager;
        }


        // GET: api/appointment/allappointments/{doctorId}
        [HttpGet]
        [Route("allappointments/{doctorId}")]
        [ResponseType(typeof(AppointmentBasicInfoDTO))]
        public IHttpActionResult GetAllAppointment(int doctorId)
        {
            ICollection<AppointmentBasicInfoDTO> appointment = appointmentManager.GetAllAppointment(doctorId);

            if(appointment.Count() == 0)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        // GET: api/appointment/recommendedpatient/{doctorId}
        [HttpGet]
        [Route("recommededpatient/{doctorId}")]
        [ResponseType(typeof(PatientDTOForPatientSearch))]
        public IHttpActionResult GetAllRecommendedPatient(int doctorId)
        {
            ICollection<PatientDTOForPatientSearch> patientsDTO = appointmentManager.GetRecommendedPatients(doctorId);
            if(patientsDTO.Count() == 0)
            {
                return NotFound();
            }
            return Ok(patientsDTO);
        }


        // GET: api/appointment/issues
        [HttpGet]
        [Route("issues")]
        [ResponseType(typeof(IssueDTO))]
        public IHttpActionResult GetAllIssues()
        {
            ICollection<IssueDTO> issuesDTO = appointmentManager.GetIssues();
            if(issuesDTO.Count() == 0)
            {
                return NotFound();
            }
            return Ok(issuesDTO);
        }

        #region Praveen Code

        // GET: api/appointment/comment/{appointmentId}
        [HttpGet]
        [Route("comment/{appointmentId}")]
        [ResponseType(typeof(AppointmentCommentDTO))]
        public IHttpActionResult GetComment(int appointmentId)
        {
            var a = appointmentManager.GetAppointmentComment(appointmentId);
            return Ok(a);
        }

        // PUT: api/appointment/comment/{appointmentId}
        [HttpPut]
        [Route("comment/{appointmentId}")]
        [ResponseType(typeof (AppointmentCommentDTO))]
        public IHttpActionResult EditComment(int appointmentId, AppointmentCommentDTO comment)
        {
            var a = appointmentManager.UpdateAppointmentComment(appointmentId, comment);
            if(!a)
            {
                return BadRequest();
            }
            return Ok();
        }

        #endregion

        // POST: api/appointment/add
        [HttpPost]
        [Route("create")]
        [ResponseType(typeof(AppointmentFormDTO))]
        public IHttpActionResult AddAppointment(AppointmentFormDTO jsonData)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AppointmentFormDTO appointmentForm = jsonData;
            var result = appointmentManager.AddAppointment(appointmentForm);
            return Created($"api/appointment/{result.AppointmentId}", result);
        }
        
    }
}
