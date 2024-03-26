import {
  Body,
  Controller,
  Get,
  Post,
  Redirect,
  Render,
  Req,
} from '@nestjs/common';
import { PaymentService } from '../Services/PaymentService';

@Controller('pay')
export class PaymentController {
  constructor(private readonly paymentService: PaymentService) {}

  @Get()
  @Render('paymentPage')
  getPaymentPage(@Body() goodsList: ProductDto[]) {
    let totalPrice: number = 0;

    goodsList.forEach(function (good) {
      totalPrice += good.price * good.amount;
    });

    return { totalPrice };
  }

  @Post()
  @Redirect()
  async pay(@Req() req: Request) {
    if (await this.paymentService.tryPay()) {
      return { url: req.referrer };
    } else {
      return;
    }
  }
}
