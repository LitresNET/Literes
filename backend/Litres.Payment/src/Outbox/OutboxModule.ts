import { Module } from '@nestjs/common';
import { ScheduleModule } from '@nestjs/schedule';
import { OutboxBackgroundService } from './OutboxBackgroundService';

@Module({
  imports: [ScheduleModule.forRoot()],
  providers: [OutboxBackgroundService],
})
export class OutboxModule {}