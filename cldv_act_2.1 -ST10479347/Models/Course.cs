
    using Azure;
    using Azure.Data.Tables;
    using System;
    using Microsoft.AspNetCore.Mvc;

    namespace cldv_act_2._1.Models
    {
        public class Course : ITableEntity
        {
            // PartitionKey groups all courses together
            public string PartitionKey { get; set; } = "Course";

            // RowKey is the unique Course ID
            public string RowKey { get; set; } = Guid.NewGuid().ToString();

            public string CourseCode { get; set; }
            public string CourseName { get; set; }
            public string Instructor { get; set; }
            public int Capacity { get; set; }

            // Required by ITableEntity
            public DateTimeOffset? Timestamp { get; set; }
            public ETag ETag { get; set; }
        }
    }

