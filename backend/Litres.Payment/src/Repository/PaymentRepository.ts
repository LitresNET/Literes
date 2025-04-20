import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Payment } from '../Models/Payment';
import { Repository, DataSource } from 'typeorm';
import { OutboxRepository } from './OutboxRepository';

@Injectable()
export class PaymentRepository {
  constructor(
    @InjectRepository(Payment)
    private readonly paymentRepo: Repository<Payment>,
    private readonly outboxRepo: OutboxRepository,
    private readonly dataSource: DataSource,
  ) {}

  async createPayment(
    sum: number,
    paymentMethod: PaymentMethod,
    orderId: number,
    status: Status = Status.InProgress,
  ): Promise<Payment> {
    return this.dataSource.transaction(async (transactionManager) => {
      const payment = transactionManager.create(Payment, {
        sum,
        payment_method: paymentMethod,
        order_id: orderId,
        status,
        date_time: Date.now(),
      });

      const savedPayment = await transactionManager.save(payment);
      await this.outboxRepo.createOutboxMessage(
        JSON.stringify({ event: 'PaymentCreated', data: savedPayment }), 
        transactionManager
      );

      return savedPayment;
    });
  }

  async updateStatus(id: number, status: Status): Promise<void> {
    return this.dataSource.transaction(async (transactionalEntityManager) => {
      await transactionalEntityManager.update(Payment, id, { status });
      await this.outboxRepo.createOutboxMessage(
        JSON.stringify({ event: 'PaymentStatusUpdated', data: { id, status } }), 
        transactionalEntityManager
      );
    });
  }

  async findByOrderId(orderId: number): Promise<Payment[]> {
    return this.paymentRepo.find({ where: { order_id: orderId } });
  }
}