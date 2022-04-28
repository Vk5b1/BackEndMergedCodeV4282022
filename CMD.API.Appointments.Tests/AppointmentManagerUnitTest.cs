using CMD.Business.Appointments;
using CMD.Business.Appointments.CustomExceptions;
using CMD.DTO.Appointments;
using CMD.Repository.Appointments;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMD.API.Appointments.Tests
{
    [TestClass]
    public class AppointmentManagerUnitTest
    {
        private IAppointmentRepository repo;
        private AppointmentManager manager;

        [TestInitialize]
        public void Init()
        {
            repo = new AppointmentRepository();
            manager = new AppointmentManager(repo);
        }

        [TestCleanup]
        public void Clean()
        {
            if(manager != null) manager = null; 
            if(repo != null) repo = null;
        }

        [TestMethod]
        public void AddAppointment_Should_Return_AppointmentConfirmationDTOObject_Which_Is_Not_Null()
        {
            var appointment = new AppointmentFormDTO
            {
                AppointmentDate = new DateTime(day: 26, month: 10, year: 2023),
                AppointmentTime = TimeSpan.Parse("11:00"),
                Issue = "Leg Injury",
                Reason = "Paitent met with accident",
                PatientId = 1,
                DoctorId = 2
            };

            var result = manager.AddAppointment(appointment);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(DateNullException))]
        public void AddAppointment_Should_Throw_DateNullException()
        {
            var appointment = new AppointmentFormDTO
            {
                AppointmentDate = default,
                AppointmentTime = TimeSpan.Parse("11:00"),
                Issue = "Leg Injury",
                Reason = "Paitent met with accident",
                PatientId = 1,
                DoctorId = 2
            };
            _ = manager.AddAppointment(appointment);
        }

        [TestMethod]
        [ExpectedException(typeof(SameAppointmentTimingException))]
        public void AddAppointment_Should_Throw_SameAppointmentTimingException()
        {
            var appointment = new AppointmentFormDTO
            {
                AppointmentDate = new DateTime(day: 24, month: 08, year: 2023),
                AppointmentTime = TimeSpan.Parse("11:00"),
                Issue = "Leg Injury",
                Reason = "Paitent met with accident",
                PatientId = 1,
                DoctorId = 2
            };
            _ = manager.AddAppointment(appointment);
        }

        [TestMethod]
        [ExpectedException(typeof(PastDateException))]
        public void AddAppointment_Should_Throw_PastDateException()
        {
            var appointment = new AppointmentFormDTO
            {
                AppointmentDate = new DateTime(day: 24, month: 08, year: 2020),
                AppointmentTime = TimeSpan.Parse("11:00"),
                Issue = "Leg Injury",
                Reason = "Paitent met with accident",
                PatientId = 1,
                DoctorId = 2
            };
            _ = manager.AddAppointment(appointment);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddAppointment_Should_Throw_NullReferenceException()
        {
            var appointment = new AppointmentFormDTO
            {
                AppointmentDate = new DateTime(day: 24, month: 08, year: 2023),
                AppointmentTime = TimeSpan.Parse("11:00"),
                Issue = "Leg Injury",
                Reason = "Paitent met with accident",
                PatientId = null,
                DoctorId = 2
            };
            _ = manager.AddAppointment(appointment);
        }

        [TestMethod]
        public void GetIssue_Should_Return_Collection_Of_Issue()
        {
            var issues = manager.GetIssues();

            Assert.IsNotNull(issues);
            Assert.IsTrue(issues.Count > 0);
            Assert.IsInstanceOfType(issues, typeof(ICollection<string>));
        }

        [TestMethod]
        public void GetPatients_Should_Return_Collection_Of_PatientDTOForPatientSearch()
        {
            var patients = manager.GetPatients(2);

            Assert.IsNotNull(patients);
            Assert.IsTrue(patients.Count > 0);
            Assert.IsInstanceOfType(patients, typeof(ICollection<PatientDTOForPatientSearch>));
        }

        [TestMethod]
        public void GetAllAppointments_Should_Return_Collection_Of_AppointmentBasicInfoDTO()
        {
            var paginationparams = new PaginationParams { ItemsPerPage = 10, Page = 1 };
            var appointments = manager.GetAllAppointment(2, paginationparams);
            Assert.IsNotNull(appointments);
            Assert.IsTrue(appointments.Count > 0);
            Assert.IsInstanceOfType(appointments, typeof(ICollection<AppointmentBasicInfoDTO>));
        }
        
        [TestMethod]
        public void GetAllAppointments_Should_Return_Collection_Of_AppointmentBasicInfoDTO_With_Desired_Number_Of_Items_Using_Pagination()
        {
            var paginationparams = new PaginationParams { ItemsPerPage = 4, Page = 1 };
            var appointments = manager.GetAllAppointment(2, paginationparams);
            Assert.IsTrue(appointments.Count == 4);
            Assert.IsInstanceOfType(appointments, typeof(ICollection<AppointmentBasicInfoDTO>));
        }

        [TestMethod]
        public void GetAllAppointments_Based_On_Status_OPEN_Should_Return_Collection_Of_AppointmentBasicInfoDTO_And_Desired_Number_Of_Items_Using_Pagination()
        {
            var paginationparams = new PaginationParams { ItemsPerPage = 4, Page = 1 };
            var appointments = manager.GetAllAppointment(2,"Open", paginationparams);
            Assert.IsTrue(appointments.Count == 4);
            Assert.IsInstanceOfType(appointments, typeof(ICollection<AppointmentBasicInfoDTO>));
            Assert.IsTrue(appointments.ToList().FirstOrDefault().AppointmentStatus == "Open");
        }

        [TestMethod]
        public void GetAllAppointments_Based_On_Status_CLOSED_Should_Return_Collection_Of_AppointmentBasicInfoDTO_And_Desired_Number_Of_Items_Using_Pagination()
        {
            var paginationparams = new PaginationParams { ItemsPerPage = 4, Page = 1 };
            var appointments = manager.GetAllAppointment(2, "Closed", paginationparams);
            Assert.IsTrue(appointments.Count == 4);
            Assert.IsInstanceOfType(appointments, typeof(ICollection<AppointmentBasicInfoDTO>));
            Assert.IsTrue(appointments.ToList().FirstOrDefault().AppointmentStatus == "Closed");
        }

        [TestMethod]
        public void GetAllAppointments_Based_On_Status_CANCELLED_Should_Return_Collection_Of_AppointmentBasicInfoDTO_And_Desired_Number_Of_Items_Using_Pagination()
        {
            var paginationparams = new PaginationParams { ItemsPerPage = 4, Page = 1 };
            var appointments = manager.GetAllAppointment(2, "Cancelled", paginationparams);
            Assert.IsTrue(appointments.Count == 4);
            Assert.IsInstanceOfType(appointments, typeof(ICollection<AppointmentBasicInfoDTO>));
            Assert.IsTrue(appointments.ToList().FirstOrDefault().AppointmentStatus == "Cancelled");
        }

        [TestMethod]
        public void GetAllAppointments_Based_On_Status_CONFIRMED_Should_Return_Collection_Of_AppointmentBasicInfoDTO_And_Desired_Number_Of_Items_Using_Pagination()
        {
            var paginationparams = new PaginationParams { ItemsPerPage = 4, Page = 1 };
            var appointments = manager.GetAllAppointment(2, "Confirmed", paginationparams);
            Assert.IsTrue(appointments.Count == 4);
            Assert.IsInstanceOfType(appointments, typeof(ICollection<AppointmentBasicInfoDTO>));
            Assert.IsTrue(appointments.ToList().FirstOrDefault().AppointmentStatus == "Confirmed");
        }

        [TestMethod]
        public void ChangeAppointmentStatus_If_Status_Is_Open_Based_On_Keyword_Confirmed_Should_Return_True()
        {
            var result = manager.ChangeAppointmentStatus(new AppointmentStatusDTO { AppointmentId = 20, Status = "Confirmed" }, 2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ChangeAppointmentStatus_If_Status_Is_Open_Based_On_Keyword_Cancelled_Should_Return_True()
        {
            var result = manager.ChangeAppointmentStatus(new AppointmentStatusDTO { AppointmentId = 21, Status = "Cancelled" }, 2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ChangeAppointmentStatus_If_Status_Is_Open_Based_On_Keyword_Cancelled_Should_Return_False()
        {
            var result = manager.ChangeAppointmentStatus(new AppointmentStatusDTO { AppointmentId = 21, Status = "Cancelled" }, 2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ChangeAppointmentStatus_If_Status_Is_Open_Based_On_Keyword_Confirmed_Should_Return_False()
        {
            var result = manager.ChangeAppointmentStatus(new AppointmentStatusDTO { AppointmentId = 20, Status = "Confirmed" }, 2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetAppointmentCount_For_A_Doctor_Should_Return_Count()
        {
            var result = manager.GetAppointmentCount(2);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void GetAppointmentCountBasedOnStatus_Cancelled_For_A_Doctor_Should_Return_Count()
        {
            var result = manager.GetAppointmentCountBasedOnStatus(2,"Cancelled");
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void GetAppointmentCountBasedOnStatus_Open_For_A_Doctor_Should_Return_Count()
        {
            var result = manager.GetAppointmentCountBasedOnStatus(2, "Open");
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void GetAppointmentCountBasedOnStatus_Closed_For_A_Doctor_Should_Return_Count()
        {
            var result = manager.GetAppointmentCountBasedOnStatus(2, "Closed");
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void GetAppointmentCountBasedOnStatus_Confirmed_For_A_Doctor_Should_Return_Count()
        {
            var result = manager.GetAppointmentCountBasedOnStatus(2, "Confirmed");
            Assert.IsTrue(result > 0);
        }
    }
}
