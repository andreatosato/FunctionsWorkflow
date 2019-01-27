using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionWorkflow
{
    public static class Workflow
    {
        public const string RegisterUserOrchestrator = "RegisterUserOrchestrator";


        public const string SendMail = "SendMail";
        public const string SendSMS = "SendSMS";
        public const string WaitEvent = "WaitEvent";
        public const string NotifyRegistration = "NotifyRegistration";
        public const string ConfirmRegistration = "ConfirmRegistration";
    }
}
