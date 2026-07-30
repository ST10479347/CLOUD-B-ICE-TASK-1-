using Microsoft.AspNetCore.Mvc;
using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cldv_act_2._1.Models
{
    public class Student : ITableEntity
    {
        public string PartitionKey { get; set; } = "Student";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
        public string Email { get; set; }

        
        public string EnrolledCoursesRaw { get; set; } = string.Empty;

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public List<string> GetEnrolledCourseIds()
        {
            return string.IsNullOrWhiteSpace(EnrolledCoursesRaw)
                ? new List<string>()
                : EnrolledCoursesRaw.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public void AddCourse(string courseId)
        {
            var courses = GetEnrolledCourseIds();
            if (!courses.Contains(courseId))
            {
                courses.Add(courseId);
                EnrolledCoursesRaw = string.Join(',', courses);
            }
        }
    }
}

