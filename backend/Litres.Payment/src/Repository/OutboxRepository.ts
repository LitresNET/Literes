import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { OutboxMessage } from '../Models/OutboxMessage';
import { Repository, EntityManager } from 'typeorm';

@Injectable()
export class OutboxRepository {
  constructor(
    @InjectRepository(OutboxMessage)
    private readonly outboxRepo: Repository<OutboxMessage>,
  ) {}

  async createOutboxMessage(content: string, transactionManager?: EntityManager): Promise<OutboxMessage> {
    const outboxMessage = transactionManager
      ? transactionManager.create(OutboxMessage, { content, occuredOn: new Date() })
      : this.outboxRepo.create({ content, occuredOn: new Date() });

    return transactionManager
      ? transactionManager.save(outboxMessage)
      : this.outboxRepo.save(outboxMessage);
  }

  async getUnprocessedMessages(transactionManager?: EntityManager): Promise<OutboxMessage[]> {
    return transactionManager
      ? transactionManager.find(OutboxMessage)
      : this.outboxRepo.find();
  }

  async markMessageAsProcessed(id: number, transactionManager?: EntityManager): Promise<void> {
    if (transactionManager) {
      await transactionManager.delete(OutboxMessage, id);
    } else {
      await this.outboxRepo.delete(id);
    }
  }
}