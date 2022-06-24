using System.Collections.Generic;

namespace Entities.DataTransferObjects
{
    public class CompanyForUpdateDto: CompanyForManipulation
    {
        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }
    }
}
