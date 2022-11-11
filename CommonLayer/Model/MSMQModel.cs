using Experimental.System.Messaging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Model
{
    public class MSMQModel
    {
        MessageQueue messageQueue = new MessageQueue();


        public void sendData2Queue(string token)
        {
            messageQueue.Path = @".\private$\token";
            if(!MessageQueue.Exists(messageQueue.Path))
            {
               MessageQueue.Create(messageQueue.Path);
            }
            
            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted; 
            messageQueue.Send(token);
            messageQueue.BeginReceive();
            messageQueue.Close();
        }

        private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var message = messageQueue.EndReceive(e.AsyncResult);
            string token = message.Body.ToString();
            string subject = "Fundoo App Reset Link";
            string Body = "Reset Password Token: " + token;
            var SMTP = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("helpindia82@gmail.com", "gfpowcsgavumyxbj"),
                EnableSsl = true,
            };
            SMTP.Send("helpindia82@gmail.com", "helpindia82@gmail.com", subject, Body);
            messageQueue.BeginReceive();
        }
    }
}
