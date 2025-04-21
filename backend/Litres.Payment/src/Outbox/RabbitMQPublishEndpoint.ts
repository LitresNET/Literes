import { IPublishEndpoint } from './IPublishEndpoint';
import * as amqp from 'amqplib';

export class RabbitMQPublishEndpoint implements IPublishEndpoint {
    private readonly connectionString: string;
    private readonly queueName: string;

    constructor(connectionString: string, queueName: string) {
        this.connectionString = connectionString;
        this.queueName = queueName;
    }

    public async publish(message: any): Promise<void> {
        const connection = await amqp.connect(this.connectionString);
        const channel = await connection.createChannel();
        await channel.assertQueue(this.queueName, { durable: true });
        channel.sendToQueue(this.queueName, Buffer.from(JSON.stringify(message)), { persistent: true });
        await channel.close();
        await connection.close();
    }
}