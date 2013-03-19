using System;
using Stateless;

namespace ConsoleApplication1
{
    public class Program
    {
        static void Main(string[] args)
        {
            var workflow = RequestWorkflow.ForRequest(1);

            workflow.Fire(RequestAction.Process);

            var x = 0;

        }
    }

    //valid states
    public enum RequestState
    {
        Created,
        Suspended,
        Assigned,
        Accepted,
        InProgress,
        Submitted,
        InQc,
        Billed,
        Cancelled
    }

    //valid transitions
    public enum RequestAction
    {
        Suspend,
        Cancel,
        Process,
    }

    public static class RequestWorkflow
    {
        public static StateMachine<RequestState, RequestAction> ForRequest(int requestId)
        {
            //use request id to get the current state of the request and initialize
            var persistedState = GetRequestState(requestId);

            var workflow = new StateMachine<RequestState, RequestAction>(persistedState);

            //configure requested status
            workflow.Configure(RequestState.Created)
                .Permit(RequestAction.Suspend, RequestState.Suspended)
                .Permit(RequestAction.Cancel, RequestState.Cancelled)
                .Permit(RequestAction.Process, RequestState.Assigned);

            workflow.Configure(RequestState.Assigned)
                    .OnEntry(OnBeginHandleAssigned)
                    .OnExit(OnExitHandleAssigned);

            return workflow;
        }

        private static void OnBeginHandleAssigned()
        {
            Console.WriteLine("Assigning Request");
        }

        private static void OnExitHandleAssigned()
        {
            Console.WriteLine("Request Is Assigned.");
        }

        //Get current state from DB or other source
        private static RequestState GetRequestState(int requestId)
        {
            return RequestState.Created;
        }
    }
}
