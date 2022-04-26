using CMD.Business.Appointments;
using CMD.DTO.Appointments;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using System.Net.Http;

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
        public IHttpActionResult GetAllAppointment(int doctorId, [FromUri]PaginationParams parameters)
        {
            ICollection<AppointmentBasicInfoDTO> appointments = appointmentManager.GetAllAppointment(doctorId, parameters);

            if(appointments.Count() == 0)
            {
                return Ok("No appointment");
            }

            var totalAppointmentCount = appointmentManager.GetAppointmentCount();

            var paginationMetaData = new PaginationMetaData(totalAppointmentCount, parameters.Page, parameters.ItemsPerPage);
            var responseData = new
            {
                paginationMetaData,
                appointments
            };
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK, responseData);
            return ResponseMessage(response);
        }

        // GET: api/appointment/recommendedpatient/{doctorId}
        [HttpGet]
        [Route("patients/{doctorId}")]
        [ResponseType(typeof(PatientDTOForPatientSearch))]
        public IHttpActionResult GetAllPatient(int doctorId)
        {
            ICollection<PatientDTOForPatientSearch> patientsDTO = appointmentManager.GetPatients(doctorId);
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
            var isssues = appointmentManager.GetIssues();
            if(isssues.Count() == 0)
            {
                return NotFound();
            }
            return Ok(isssues);
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

        // POST: api/appointment/create
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
