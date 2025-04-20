import { Entity, PrimaryGeneratedColumn, Column } from 'typeorm';

@Entity()
export class OutboxMessage {
  @PrimaryGeneratedColumn()
  id: string;

  @Column('text')
  type: string;

  @Column('text')
  content: string;

  @Column({ type: 'timestamp', default: () => 'CURRENT_TIMESTAMP' })
  occuredOn: Date;

  @Column({ type: 'timestamp', nullable: true, default: null})
  processedOn: Date;

  @Column('text')
  error: string;
}