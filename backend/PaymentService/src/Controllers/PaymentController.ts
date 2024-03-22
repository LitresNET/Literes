import {
  Body,
  Controller,
  Get,
  HttpStatus,
  Post,
  Render,
  Res,
} from '@nestjs/common';
import { Response } from 'express';
import { PaymentService } from '../Services/PaymentService';

@Controller('pay')
export class PaymentController {
  constructor(private readonly paymentService: PaymentService) {}

  @Get()
  @Render('paymentPage')
  getPaymentPage() {
    return;
  }

  @Post()
  pay(@Res() response: Response): Response {
    if (this.paymentService.tryPay()) {
      return response.status(HttpStatus.OK).send({
        status: 'paid',
      });
    }
    return response.status(HttpStatus.BAD_REQUEST).send({
      status: 'not paid',
    });
  }
}
