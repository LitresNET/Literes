import { DataSource } from 'typeorm';
import { OutboxMessage } from '../Models/OutboxMessage';

export class OutboxExtensions {
    private static dataSource: DataSource;

    public static setDataSource(dataSource: DataSource): void {
        OutboxExtensions.dataSource = dataSource;
    }
    
    public static async insertOutboxMessage<T>(message: T): Promise<void> {
        if (!OutboxExtensions.dataSource) {
            throw new Error('DataSource is not set');
        }

        const outboxMessageRepository = OutboxExtensions.dataSource.getRepository(OutboxMessage);

        const outboxMessage = new OutboxMessage();
        outboxMessage.content = JSON.stringify(message);
        outboxMessage.occuredOn = new Date();

        await outboxMessageRepository.save(outboxMessage);
    }

    private static generateGuid(): string {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            const r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
}