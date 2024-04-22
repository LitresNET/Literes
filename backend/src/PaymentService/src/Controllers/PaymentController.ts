import {
  Body,
  Controller,
  Get,
  Post,
  Query,
  Render,
  Res,
} from '@nestjs/common';
import { PaymentService } from '../Services/PaymentService';
import { GetDataService } from 'src/Services/GetDataService';
import { Response } from 'express';

@Controller('pay')
export class PaymentController {
  constructor(private readonly paymentService: PaymentService, private readonly getDataService: GetDataService) {}

  @Get()
  @Render('paymentPage')
  async getPaymentPage(@Query() query) {
    let userId = query.userId;
    let totalPrice = query.amount;
    if (userId !== null)
      return { userId, totalPrice }

    let goods = await this.getDataService.getOrderData(query.orderId);
    let orderId = query.orderId;

    goods.products.forEach(function (good) {
      totalPrice += good.price * good.amount;
    });

    return { totalPrice, orderId };
  }

  @Post()
  async pay(@Body() body: any, @Res() res: Response) {
    let userId = body.userId;
    let amount = body.totalPrice;
    fetch(`http://localhost:5032/api/order/refill?userId=${userId}&amount=${amount}`, {
      method: "PATCH"
    })
      .then(r => r.ok ? window.close() : window.alert("Что-то пошло не так!"))
      .catch(e => console.log(e));
    return;

   /* if (await this.paymentService.tryPay()) {
      return res.redirect(`http://localhost:5032/api/order/${orderId}/finish?isSuccess=true`);
    } else {
      return res.redirect(`http://localhost:5032/api/order/${orderId}/finish?isSuccess=false`);
    }*/
  }
}
