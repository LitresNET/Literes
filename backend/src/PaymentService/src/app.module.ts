import { Module } from '@nestjs/common';
import { PaymentController } from './Controllers/PaymentController';
import { PaymentService } from './Services/PaymentService';

@Module({
  imports: [],
  controllers: [PaymentController],
  providers: [PaymentService],
})
export class AppModule {}
