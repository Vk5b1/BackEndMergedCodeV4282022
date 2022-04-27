using CMD.API.Appointments.Controllers;
using CMD.Business.Appointments;
using CMD.DTO.Appointments;
using CMD.Repository.Appointments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Web.Http.Results;

namespace CMD.API.Appointments.Tests
{
    [TestClass]
    public class AppointmentControllerUnitTest
    {
        private IAppointmentManager appointmentManager;

        [TestInitialize]
        public void Init()
        {
            appointmentManager = new AppointmentManager(new AppointmentRepository());
        }

        [TestCleanup]
        public void Clean()
        {
            if (appointmentManager != null) 
                appointmentManager = null;
        }

        [TestMethod]
        public void GetAllAppointmentById()
        {
            var controller = new AppointmentController(appointmentManager);
            var paginationParams = new PaginationParams
            {
                ItemsPerPage = 8,
                Page = 1
            };
            var response = controller.GetAllAppointment(2, paginationParams);
            var contentResult = response as OkNegotiatedContentResult<PaginationMetaData>;
            Assert.IsNotNull(contentResult);
        }
    }
}
