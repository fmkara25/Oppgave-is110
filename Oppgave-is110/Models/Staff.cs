using System;

namespace UniversitySystem.Models
{
    public class Staff : User
    {
        public string StaffId { get; private set; } = string.Empty;
        public string Position { get; private set; } = string.Empty;
        public string Department { get; private set; } = string.Empty;

        public Staff(string staffId, string name, string email, string position, string department)
            : base(name, email)
        {
            SetStaffId(staffId);
            SetPosition(position);
            SetDepartment(department);
        }

        public void SetStaffId(string staffId)
        {
            if (string.IsNullOrWhiteSpace(staffId))
                throw new ArgumentException("StaffId cannot be empty.");

            StaffId = staffId.Trim();
        }

        public void SetPosition(string position)
        {
            if (string.IsNullOrWhiteSpace(position))
                throw new ArgumentException("Position cannot be empty.");

            Position = position.Trim();
        }

        public void SetDepartment(string department)
        {
            if (string.IsNullOrWhiteSpace(department))
                throw new ArgumentException("Department cannot be empty.");

            Department = department.Trim();
        }

        public override string ToString()
        {
            return $"Staff {StaffId} - {base.ToString()} - {Position}, {Department}";
        }
    }
}
