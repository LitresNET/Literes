import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import { PaymentController } from "./Controllers/PaymentController";
import { PaymentService } from "./Services/PaymentService";
import {ListController} from "./Controllers/ListController";
import {ListService} from "./Services/ListService";

@Module({
  imports: [],
  controllers: [ListController, PaymentController],
  providers: [ListService, PaymentService],
})
export class AppModule {}
