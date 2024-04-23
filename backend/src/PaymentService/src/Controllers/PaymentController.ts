import {
  Body,
  Controller,
  Get,
  Post,
  Query,
  Redirect,
  Render,
  Req,
  Res,
} from '@nestjs/common';
import { PaymentService } from '../Services/PaymentService';
import { GetDataService } from 'src/Services/GetDataService';
import { Response } from 'express';
import * as bodyParser from 'body-parser';

@Controller('pay')
export class PaymentController {
  constructor(private readonly paymentService: PaymentService, private readonly getDataService: GetDataService) {}

  @Get()
  @Render('paymentPage')
  async getPaymentPage(@Query() query) {
    let goods = await this.getDataService.getOrderData(query.orderId);
    let totalPrice: number = 0;
    let orderId:number = goods.orderId

    goods.products.forEach(function (good) {
      totalPrice += good.price * good.amount;
    });

    return { totalPrice, orderId};
  }

  @Post()
  async pay(@Body() body: any, @Res() res: Response) {
    const orderId = body.orderId;
    if (await this.paymentService.tryPay()) {
      return res.redirect(`http://localhost:5032/api/order/${orderId}/finish?isSuccess=true`);
    } else {
      return res.redirect(`http://localhost:5032/api/order/${orderId}/finish?isSuccess=false`);
    }
  }
}
