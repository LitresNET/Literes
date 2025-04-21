import {
  Body,
  Controller,
  Get,
  Post,
  Query,
  Render,
  Res
} from '@nestjs/common';
import { PaymentService } from '../Services/PaymentService';
import { Response } from 'express';

@Controller('pay')
export class PaymentController {
  constructor(private readonly paymentService: PaymentService) {}

  @Get()
  @Render('paymentPage')
  async getPaymentPage(@Query() query) {
    const orderId = query.orderId;

    return { orderId };
  }

  @Post()
  async pay(@Body() body: any, @Res() res: Response) {
    const amount = body.totalPrice;
    if (await this.paymentService.tryPay()) {
      return res.redirect(
        `http://localhost:5225/api/user/deposit?amount=${amount}`,
      );
    }

    // Пока решили, что оплата всегда проходит
  }
}
