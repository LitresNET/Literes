import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { Transaction } from '../Models/Transaction';

@Injectable()
export class TransactionRepository {
  constructor(
    @InjectRepository(Transaction)
    private readonly transactionRepo: Repository<Transaction>,
  ) {}

  async createTransaction(
    type: string,
    status: Status,
    paymentId: number,
  ): Promise<Transaction> {
    const transaction = this.transactionRepo.create({
      type,
      status,
      payment_id: paymentId,
      date_time: new Date(),
    });

    return this.transactionRepo.save(transaction);
  }

  async getAll(): Promise<Transaction[]> {
    return this.transactionRepo.find();
  }

  async findByPaymentId(paymentId: number): Promise<Transaction[]> {
    return this.transactionRepo.find({
      where: { payment_id: paymentId },
    });
  }

  async updateStatus(id: number, status: Status): Promise<void> {
    await this.transactionRepo.update(id, { status });
  }
}
