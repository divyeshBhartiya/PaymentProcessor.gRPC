# PaymentProcessor.gRPC
This repository is created for knowing and learning the capabilites of gRPC.

The idea is to implement a Payment Processing Environment with gRPC Microservices which would trigger events and whenever step is completed.
In this scenario we have 3 services: Payment Service, Shipment Service and Notification Service.

Once the payment is processed an event will asynchronously fire shipment service, which in turn would trigger notification service once the order is dispatched.
Overall process is the combination of Streams of responses and Fire-Forget mechanisms. 

## PAYMENT >>>> SHIPMENT >>>> NOTIFICATION

### References:
https://medium.com/@letienthanh0212/what-is-grpc-and-how-to-implement-grpc-with-asp-net-core-3-x-affe83686123

https://grpc.io/

https://www.infoworld.com/article/3534690/how-to-send-emails-in-aspnet-core.html
