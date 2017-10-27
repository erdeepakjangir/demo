using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Entities.Enums
{
    /// <summary>
    /// User roles in CU system, A User can have only one role except Student and UG Student Role.
    /// </summary>
    public enum UserRole
    {
        Unauthorized,
        Admin,
        User,
        Student,
        UGStudent,
        FacultyStaff,
        Planner
    }
}
