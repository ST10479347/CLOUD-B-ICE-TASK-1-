using Azure.Data.Tables;
using cldv_act_2._1.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cldv_act_2._1.Services
{
    public class TableStorageService
    {
        private readonly TableClient _coursesTable;
        private readonly TableClient _studentsTable;

        public TableStorageService(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("AzureStorage");
            var serviceClient = new TableServiceClient(connectionString);

            _coursesTable = serviceClient.GetTableClient("Courses");
            _studentsTable = serviceClient.GetTableClient("Students");

            _coursesTable.CreateIfNotExists();
            _studentsTable.CreateIfNotExists();
        }

        // ---------- COURSES ----------

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            var results = new List<Course>();
            await foreach (var course in _coursesTable.QueryAsync<Course>(c => c.PartitionKey == "Course"))
                results.Add(course);
            return results;
        }

        public async Task<Course> GetCourseAsync(string rowKey)
        {
            try
            {
                var response = await _coursesTable.GetEntityAsync<Course>("Course", rowKey);
                return response.Value;
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        public async Task AddCourseAsync(Course course)
        {
            course.PartitionKey = "Course";
            await _coursesTable.AddEntityAsync(course);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            await _coursesTable.UpdateEntityAsync(course, course.ETag, TableUpdateMode.Replace);
        }

        public async Task DeleteCourseAsync(string rowKey)
        {
            await _coursesTable.DeleteEntityAsync("Course", rowKey);
        }

        // ---------- STUDENTS ----------

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var results = new List<Student>();
            await foreach (var student in _studentsTable.QueryAsync<Student>(s => s.PartitionKey == "Student"))
                results.Add(student);
            return results;
        }

        public async Task<Student> GetStudentAsync(string rowKey)
        {
            try
            {
                var response = await _studentsTable.GetEntityAsync<Student>("Student", rowKey);
                return response.Value;
            }
            catch (Azure.RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        public async Task AddStudentAsync(Student student)
        {
            student.PartitionKey = "Student";
            await _studentsTable.AddEntityAsync(student);
        }

        public async Task UpdateStudentAsync(Student student)
        {
            await _studentsTable.UpdateEntityAsync(student, student.ETag, TableUpdateMode.Replace);
        }

        public async Task DeleteStudentAsync(string rowKey)
        {
            await _studentsTable.DeleteEntityAsync("Student", rowKey);
        }
    }
}