using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.ActionFilters
{
    public class ExistsEmployeeForCompanyAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public ExistsEmployeeForCompanyAttribute(ILoggerManager logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var companyId = (Guid)context.ActionArguments["companyId"];
            var company = await _repository.Company.GetCompanyAsync(companyId, false);

            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                context.Result = new NotFoundResult();
                return;
            }

            var employees = await _repository.Employee.GetEmployeesAsync(companyId, false);

            if (employees == null || employees.Count() == 0)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't have employees");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("employees", employees);
                await next();
            }
        }
    }
}
