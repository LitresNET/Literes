import { Entity, PrimaryGeneratedColumn, Column } from 'typeorm';

@Entity()
export class Payment {
  @PrimaryGeneratedColumn()
  id: number;

  @Column()
  sum: number;

  @Column()
  status: string;

  @Column()
  payment_method: string;

  @Column()
  order_id: number;

  @Column({ type: 'timestamp' })
  date_time: Date;
}
