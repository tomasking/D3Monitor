using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace D3Monitor.Tests
{
    [TestFixture]
    public class MsmqTest
    {
        [Test]
        public void GetStats()
        {
            var queues = System.Messaging.MessageQueue.GetPrivateQueuesByMachine("localhost");
            

            foreach (var messageQueue in queues)
            {
                var queueCounter = new PerformanceCounter(
                    "MSMQ Queue",
                    "Messages in Queue",
                    string.Format("{0}\\{1}", messageQueue.MachineName, messageQueue.QueueName), messageQueue.MachineName);
                
                try
                {
                    Console.WriteLine("{0}\\{1} {2}", messageQueue.MachineName, messageQueue.QueueName, queueCounter.RawValue);
                }
                catch (Exception e)
                {
                    Console.WriteLine(messageQueue.QueueName + " Length: ?");
                }
            }
            Assert.That(queues, Is.Not.Null);
        }


    }
}
