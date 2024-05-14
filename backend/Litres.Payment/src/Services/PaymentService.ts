import { Injectable } from '@nestjs/common';

@Injectable()
export class PaymentService {
  async tryPay(): Promise<boolean> {
    return true;
  }
}
