import { Injectable } from '@nestjs/common';

@Injectable()
export class PaymentService {
  async tryPay(): Promise<boolean> {
    const sleep = (ms: number) =>
      new Promise((resolve) => setTimeout(resolve, ms));
    await sleep(3000);
    return Math.random() > 0.4;
  }
}
