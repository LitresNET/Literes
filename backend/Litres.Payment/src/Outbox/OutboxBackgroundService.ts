import { OutboxProcessor } from './OutboxProcessor';
import { IPublishEndpoint } from './IPublishEndpoint';
import { DataSource } from 'typeorm';
import { RabbitMQPublishEndpoint } from './RabbitMQPublishEndpoint';

export class OutboxBackgroundService {
    private readonly serviceScopeFactory: any;
    private readonly logger: any;
    private readonly outboxProcessorFrequency = 15000; // 15 seconds in milliseconds
    private readonly dataSource: DataSource;

    constructor(serviceScopeFactory: any, logger: any, dataSource: DataSource) {
        this.serviceScopeFactory = serviceScopeFactory;
        this.logger = logger;
        this.dataSource = dataSource;
    }

    public async start(ctx: any = {}): Promise<void> {
        try {
            this.logger.log('Starting OutboxBackgroundService...');

            while (!ctx.isCancellationRequested) {
                const scope = this.serviceScopeFactory.createScope();
                const dbContext = this.dataSource;
                const publishEndpoint: IPublishEndpoint = new RabbitMQPublishEndpoint('amqp://localhost', 'your_queue_name');
                const outboxProcessor = new OutboxProcessor(dbContext, publishEndpoint);

                await outboxProcessor.execute(ctx);

                await new Promise(resolve => setTimeout(resolve, this.outboxProcessorFrequency));
            }
        } catch (ex) {
            if (ex instanceof Error && ex.name === 'OperationCanceledException') {
                this.logger.log('OutboxBackgroundService cancelled.');
            } else {
                this.logger.error(ex, 'An error occurred in OutboxBackgroundService');
            }
        } finally {
            this.logger.log('OutboxBackgroundService finished...');
        }
    }
}