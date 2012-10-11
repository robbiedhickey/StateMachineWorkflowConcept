using Stateless;

namespace ConsoleApplication1
{
    public class Program
    {
        static void Main(string[] args)
        {
            var workflow = RequestWorkflow.ForRequest(1);
        }
    }

    //valid states
    public enum RequestState
    {
        AwaitingProcessing,
        Requested,
        Suspended,
        Cancelled,
        Processed,
        AwaitingReview,
        UnderReview,
        AwaitingQuickReview,
        UnderQuickReview,
        AwaitingManagerApproval,
        DeliveredAwaitingPDFGeneration,
        Delivered,
        AwaitingReconciliation,
        ReconciledAwaitingPDFGeneration,
        Reconciled,
        Billed,
        Paid
    }

    //valid transitions
    public enum Action
    {
        Suspend,
        Cancel,
        Process,
    }

    public static class RequestWorkflow
    {
        public static StateMachine<RequestState, Action> ForRequest(int requestId)
        {
            //use request id to get the current state of the request and initialize
            var persistedState = GetRequestState(requestId);

            var workflow = new StateMachine<RequestState, Action>(persistedState);

            //configure requested status
            workflow.Configure(RequestState.Requested)
                .Permit(Action.Suspend, RequestState.Suspended)
                .Permit(Action.Cancel, RequestState.Cancelled)
                .Permit(Action.Process, RequestState.Processed);

            return workflow;
        }

        private static RequestState GetRequestState(int requestId)
        {
            return RequestState.Requested;
        }
    }
}
