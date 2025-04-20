import { DataSource } from 'typeorm';
import { OutboxMessage } from '../Models/OutboxMessage';
import { IPublishEndpoint } from './IPublishEndpoint';

export class OutboxProcessor {
    private readonly publishEndpoint: IPublishEndpoint;
    private readonly batchSize = 10;
    private readonly dataSource: DataSource;

    constructor(dataSource: DataSource, publishEndpoint: IPublishEndpoint) {
        this.dataSource = dataSource;
        this.publishEndpoint = publishEndpoint;
    }

    public async execute(ctx: any = {}): Promise<number> {
        const outboxMessageRepository = this.dataSource.getRepository(OutboxMessage);

        const outboxMessages = await outboxMessageRepository
            .createQueryBuilder('outboxMessage')
            .where('outboxMessage.processedOnUtc IS NULL')
            .orderBy('outboxMessage.occuredOnUtc', 'ASC')
            .take(this.batchSize)
            .getMany();

        for (const outboxMessage of outboxMessages) {
            try {
                const messageType = require(outboxMessage.type).default;
                const deserializedMessage = JSON.parse(outboxMessage.content);

                await this.publishEndpoint.publish(deserializedMessage, messageType, ctx);

                outboxMessage.processedOn = new Date();
                await outboxMessageRepository.save(outboxMessage);
            } catch (ex) {
                outboxMessage.processedOn = new Date();
                outboxMessage.error = ex.toString();
                await outboxMessageRepository.save(outboxMessage);
            }
        }

        return outboxMessages.length;
    }
}