import { Injectable } from '@nestjs/common';

@Injectable()
export class PaymentService {
  tryPay(): boolean {
    return Math.random() > 0.4;
  }
}
