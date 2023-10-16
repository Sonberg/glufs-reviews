using Azure.Messaging.ServiceBus;
using Glufs.Reviews.Domain.Events;

namespace Glufs.Reviews.Jobs.SendReviewEmail;

public class SendReviewEmailTrigger : Trigger<SendReviewEmailEvent>
{
	public SendReviewEmailTrigger(IServiceProvider serviceProvider, ServiceBusClient client) : base(serviceProvider, client)
	{
	}

    public override string Queue => "send-review-email";
}

