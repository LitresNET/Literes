import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Payment } from '../Models/Payment';
import { Repository } from 'typeorm';

@Injectable()
export class PaymentRepository {
  constructor(
    @InjectRepository(Payment)
    private readonly paymentRepo: Repository<Payment>,
  ) {}

  async createPayment(
    sum: number,
    paymentMethod: PaymentMethod,
    orderId: number,
    status: Status = Status.InProgress,
  ): Promise<Payment> {
    const payment = this.paymentRepo.create({
      sum,
      payment_method: paymentMethod,
      order_id: orderId,
      status,
      date_time: new Date(),
    });

    return this.paymentRepo.save(payment);
  }

  async updateStatus(id: number, status: Status): Promise<void> {
    await this.paymentRepo.update(id, { status });
  }

  async findByOrderId(orderId: number): Promise<Payment[]> {
    return this.paymentRepo.find({ where: { order_id: orderId } });
  }

  async getAll(): Promise<Payment[]> {
    return this.paymentRepo.find();
  }
}
