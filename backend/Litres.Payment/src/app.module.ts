import { Module } from '@nestjs/common';
import { PaymentController } from './Controllers/PaymentController';
import { PaymentService } from './Services/PaymentService';
import { GetDataService } from './Services/GetDataService';

@Module({
  imports: [],
  controllers: [PaymentController],
  providers: [PaymentService, GetDataService],
})
export class AppModule {}
