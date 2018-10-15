using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using CompanySystem.Entities;
using CompanySystem.Web.Models;
using Elmah;
using CompanySystem.Web.Filters;
using System.Diagnostics;
//using System.Web.Mvc;
//using System.Web.Mvc;

namespace CompanySystem.Web.Controllers
{
    [ElmahError]
    public class CompaniesController : BaseApiController
    {
        public CompaniesController(ICompanySystemRepository repo) : base(repo)
        {

        }
        // GET: Companies
        [HttpGet]
        [ElmahError]        
        public IEnumerable<CompanyModel> Companies()
        {
            Trace.WriteLine("Invoking respository to get all acompanies");

            IQueryable<Company> query;
            query = TheRepository.GetAllCompanies();
            var result = query
                .ToList()
                .Select(s => TheModelFactory.Create(s));
            return result;
        }

        // GET: Company
        [HttpGet]
        [ElmahError]
        public HttpResponseMessage Companies(int id)
        {
            
                var company = TheRepository.GetCompany(id);
                if (company != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(company));
                }
                else
                {
                    ErrorLog.GetDefault(HttpContext.Current).Log(new Error() {
                        HostName   = "CompaniesController",
                        Message    = string.Format("Company not found with Id {0}", id.ToString()),
                        StatusCode = (int)HttpStatusCode.NotFound                        
                    });

                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

           
        }

        [HttpPost]
        public HttpResponseMessage Company([FromBody] CompanyModel companyModel)
        {
            try
            {
                var entity = TheModelFactory.Parse(companyModel);

                if (entity == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read company details from body");

                if (TheRepository.Insert(entity) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(entity));
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not save to the database.");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPut]
        public HttpResponseMessage Company(int id, [FromBody] CompanyModel companyModel)
        {
            try
            {

                var updatedCompany = TheModelFactory.Parse(companyModel);

                if (updatedCompany == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read company details from body");

                var originalCompany = TheRepository.GetCompany(id);

                if (originalCompany == null || originalCompany.Id != id)
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Course is not found");
                }
                else
                {
                    updatedCompany.Id = id;
                }

                if (TheRepository.Update(originalCompany, updatedCompany) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(updatedCompany));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified);
                }

            }
            catch (Exception ex)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        public HttpResponseMessage Company(int id)
        {
            try
            {
                var company = TheRepository.GetCompany(id);

                if (company == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                if (TheRepository.DeleteCompany(id) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage Employee([FromBody] EmployeeModel employeeModel)
        {
            try
            {
                var entity = TheModelFactory.Parse(employeeModel);

                if (entity == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read employee details from body");

                if (TheRepository.Insert(entity) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(entity));
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not save to the database.");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        // GET: Companies
        [HttpGet]
        public IEnumerable<EmployeeModel> Employees()
        {
            IQueryable<Employee> query;
            query = TheRepository.GetAllEmployees();
            var result = query
                .ToList()
                .Select(s =>
                {
                    s.EmployeeCompany = TheRepository.GetCompany(s.Company_Id);
                    return TheModelFactory.Create(s);
                });
            return result;
        }

        // GET: Company
        [HttpGet]
        public HttpResponseMessage Employees(int id)
        {
            try
            {
                var employee = TheRepository.GetEmployee(id);
                employee.EmployeeCompany = TheRepository.GetCompany(employee.Company_Id);

                if (employee != null)
                {                    
                    return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(employee));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        [HttpPut]
        public HttpResponseMessage Employee(int id, [FromBody] EmployeeModel employeeModel)
        {
            try
            {

                var updatedEmployee = TheModelFactory.Parse(employeeModel);

                if (updatedEmployee == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read Employee details from body");

                var originalEmployee = TheRepository.GetEmployee(id);

                if (originalEmployee == null || originalEmployee.Id != id)
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Employee is not found");
                }
                else
                {
                    updatedEmployee.Id = id;
                }

                if (TheRepository.Update(originalEmployee, updatedEmployee) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(updatedEmployee));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        public HttpResponseMessage Employee(int id)
        {
            try
            {
                var employee = TheRepository.GetEmployee(id);

                if (employee == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                if (TheRepository.DeleteEmployee(id) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}