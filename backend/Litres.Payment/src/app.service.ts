import { Injectable, OnModuleInit } from '@nestjs/common';
import { PaymentCompletedEvent, PaymentEventServiceClient, PaymentFailedEvent, PaymentResponse } from './Generated/payment';
import { Client, ClientGrpc, Transport } from '@nestjs/microservices';
import { join } from 'path';
import { Observable } from 'rxjs';

@Injectable()
export class AppService implements OnModuleInit {
  @Client({
    transport: Transport.GRPC,
    options: {
      package: 'payment',
      protoPath: join(__dirname, './Protos/payment.proto')
    }
  })
  client: ClientGrpc;

  private paymentService: PaymentEventServiceClient;

  onModuleInit() {
    this.paymentService = this.client.getService<PaymentEventServiceClient>('PaymentService');
  }

  ProcessPaymentCompletion(event: PaymentCompletedEvent) : Observable<PaymentResponse> {
    return this.paymentService.processPaymentCompletion(event)
  }

  ProcessPaymentFailure(event: PaymentFailedEvent) : Observable<PaymentResponse> {
    return this.paymentService.processPaymentFailure(event)
  }
}
