using Azure.Storage.Queues;
using cldv_act_2._1.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading.Tasks;

namespace cldv_act_2._1.Services
{
    public class QueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("AzureStorage");
            _queueClient = new QueueClient(connectionString, "CourseEnrollmentQueue");
            _queueClient.CreateIfNotExists();
        }

        // Called when a student enrolls: pushes a message onto the queue
        public async Task SendEnrollmentMessageAsync(EnrollmentRequest request)
        {
            string json = JsonSerializer.Serialize(request);
            await _queueClient.SendMessageAsync(json);
        }

        // Pulls messages off the queue and processes each one
        public async Task<int> ProcessQueueAsync(TableStorageService tableStorageService)
        {
            int processedCount = 0;
            var messages = await _queueClient.ReceiveMessagesAsync(maxMessages: 32);

            foreach (var message in messages.Value)
            {
                var request = JsonSerializer.Deserialize<EnrollmentRequest>(message.MessageText);

                if (request != null)
                {
                    var student = await tableStorageService.GetStudentAsync(request.StudentId);
                    if (student != null)
                    {
                        student.AddCourse(request.CourseId);
                        await tableStorageService.UpdateStudentAsync(student);
                        processedCount++;
                    }
                }

                // Removes the message from the queue once processed
                await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            }

            return processedCount;
        }
    }
}