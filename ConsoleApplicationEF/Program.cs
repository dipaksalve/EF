using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplicationEF
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new TestDBEntities();

            SaveEmployeeDetails(101, 102, "Dipak Salve", "SR");
            Console.WriteLine("After Create");
            ReadData(string.Empty);
            UpdateEmployeeDetails(101, 103, "Dipak Y Salve", "SR");
            Console.WriteLine("After Update");
            ReadData(string.Empty);
            DeleteEmployee(101);
            Console.WriteLine("After Delete");
            ReadData(string.Empty);
            
            var emp = (from empl in context.EmployeeDetails
                       group empl by empl.Emp_MgrId into grp_ems
                       where grp_ems.Count() > 1
                       select grp_ems);

            //var emp = context.EmployeeDetails.
            //                GroupBy(item => item.Emp_MgrId).
            //                Where(group => group.Count() > 1).
            //                Select(group => group);

            foreach (var groupItem in emp)
            {
                Console.WriteLine(groupItem.Key.HasValue ? groupItem.Key.Value : 0);

                foreach (var e in groupItem)
                {
                    Console.WriteLine(e.Emp_Name+" : "+e.Emp_Designation);
                }

            }

            Console.ReadLine();
        }

        private static void SaveEmployeeDetails(int empId, int mgrId, string name, string designation)
        {
            var context = new TestDBEntities();

            var emp = new EmployeeDetail()
            {
                Emp_Id = empId,
                Emp_MgrId = mgrId,
                Emp_Name = name,
                Emp_Designation = designation
            };

            context.EmployeeDetails.Add(emp);
            context.SaveChanges();
        }

        private static void UpdateEmployeeDetails(int empId, int mgrId, string name, string designation)
        {
            var context = new TestDBEntities();

            var emp = context.EmployeeDetails.Find(empId);

            emp.Emp_MgrId = mgrId;
            emp.Emp_Name = name;
            emp.Emp_Designation = designation;
            
            context.SaveChanges();
        }

        private static void DeleteEmployee(int empId)
        {
            var context = new TestDBEntities();

            var emp = context.EmployeeDetails.SingleOrDefault(e => e.Emp_Id == empId);
            context.EmployeeDetails.Remove(emp);
            context.SaveChanges();
        }

        private static void ReadData(string message)
        {
            var context = new TestDBEntities();

            var empSal = (from det in context.EmployeeDetails
                        join
                        sal in context.Emp_Salary
                        on det.Emp_Id equals sal.Emp_Id
                        select new { det.Emp_Name, sal.Emp_Salary1 }).ToList();

            foreach (var e in context.EmployeeDetails)
            {
                Console.WriteLine(message + " - Read " + e.Emp_Name + " : " + e.Emp_Designation);
            }
        }
    }
}
